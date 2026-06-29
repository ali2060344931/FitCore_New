using EndPoint.Site.BaleBot.Services;

using FitCore.Application.Contexts;
using FitCore.Application.Services.Tickets;
using FitCore.Common;
using FitCore.Common.Roles;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Tickets;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TicketController : Controller
    {
        private readonly IDataBaseContext _context;
        private readonly ICreateTicketService _createService;
        private readonly IReplyTicketService _replyService;
        private readonly ICloseTicketService _closeService;
        private readonly IGetTicketsService _getTicketsService;
        private readonly IGetTicketDetailService _getDetailService;
        private readonly IGetOpenTicketsCountService _getCountService;
        private readonly IBaleMenuService _baleMenuService;

        public TicketController(
            IDataBaseContext context,
            ICreateTicketService createService,
            IReplyTicketService replyService,
            ICloseTicketService closeService,
            IGetTicketsService getTicketsService,
            IGetTicketDetailService getDetailService,
            IGetOpenTicketsCountService getCountService,
            IBaleMenuService baleMenuService)
        {
            _context = context;
            _createService = createService;
            _replyService = replyService;
            _closeService = closeService;
            _getTicketsService = getTicketsService;
            _getDetailService = getDetailService;
            _getCountService = getCountService;
            _baleMenuService = baleMenuService;
        }

        private long CurrentUserId => long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        private async Task<long?> CurrentGymIdAsync() =>
            await _context.Users.Where(u => u.Id == CurrentUserId).Select(u => u.GymId).FirstOrDefaultAsync();

        // ===================== لیست =====================

        public async Task<IActionResult> Index()
        {
            ViewBag.IsSuperAdmin = User.IsInRole(UserRoles.SuperAdmin);
            ViewBag.IsAdmin = User.IsInRole(UserRoles.Admin);
            ViewBag.IsMember = User.IsInRole(UserRoles.Member);

            if (User.IsInRole(UserRoles.SuperAdmin))
            {
                ViewBag.Tickets = await _getTicketsService.ExecuteForSuperAdmin();
                ViewBag.Title = "تیکت‌های مدیران باشگاه";
            }
            else if (User.IsInRole(UserRoles.Admin))
            {
                var gymId = await CurrentGymIdAsync();
                ViewBag.Tickets = await _getTicketsService.ExecuteForAdmin(gymId ?? 0);
                ViewBag.Title = "تیکت‌های اعضای باشگاه";
            }
            else
            {
                ViewBag.Tickets = await _getTicketsService.ExecuteForMember(CurrentUserId);
                ViewBag.Title = "تیکت‌های من";
            }

            return View();
        }

        // ===================== جزئیات + پنجره چت =====================

        public async Task<IActionResult> Detail(long id)
        {
            var ticket = await _getDetailService.Execute(id);
            if (ticket == null) return NotFound();

            ViewBag.CurrentUserId = CurrentUserId;
            ViewBag.CurrentRole = User.IsInRole(UserRoles.SuperAdmin) ? UserRoles.SuperAdmin
                : User.IsInRole(UserRoles.Admin) ? UserRoles.Admin : UserRoles.Member;

            return View(ticket);
        }

        // ===================== ایجاد (عضو یا ادمین) =====================

        [HttpGet]
        public IActionResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string subject, string body, TicketPriority priority)
        {
            string senderRole = User.IsInRole(UserRoles.Admin) ? UserRoles.Admin : UserRoles.Member;
            var gymId = await CurrentGymIdAsync();
            var result = await _createService.Execute(new CreateTicketDto
            {
                SenderUserId = CurrentUserId,
                SenderRole = senderRole,
                GymId = gymId,
                Subject = subject,
                Body = body,
                Priority = priority
            });



            #region ارسال پیام به ربات جهت اطلاع رسانی

            //ToDo نمایش اطلاعات مدیر باشگاه با داشتن آی دی باشگاه
            var q = (from r in _context.UserRoles
                     join u in _context.Users on r.UserId equals u.Id
                     join g in _context.Gyms on u.GymId equals g.Id
                     //join m in _context.Members on u.Id equals m.AppUserId
                     where r.RoleId == 2 && u.GymId == gymId
                     select new
                     {
                         u,
                         g,
                         r,
                         //m
                     }).FirstOrDefault();





            string priorityTitle = "";
            if (priority == TicketPriority.Low)
            {
                priorityTitle = "کم";
            }
            else if (priority == TicketPriority.Normal)
            {
                priorityTitle = "متوسط";
            }
            else
            {
                priorityTitle = "زیاد";

            }
            var SenderUser = _context.Users.Where(c => c.Id == CurrentUserId).First();

            long BaleChatId = 0;
            string msg = "📨تیکت برای شما ارسال گردید.\n➖نام و نام خانوادگی ارسال کننده: " + SenderUser.FullName + "\n➖شماره تماس: " + SenderUser.PhoneNumber + "\n➖باشگاه: " + q.g.Name + "\n➖موضوع: " + subject + "\n➖میزان اهمیت: " + priorityTitle;

            if (senderRole == UserRoles.Member)
            {
                BaleChatId = (long)q.u.BaleChatId;//مربوط به مدیر باشگاه
            }
            else if (senderRole == UserRoles.Admin)
            {
                BaleChatId = (long)_context.Setings.Where(c => c.Code == "01").First().SuperAdminChatId;//مربوط به مدیر سایت
            }

            await _baleMenuService.EditMemberInfoSend((long)BaleChatId, msg);


            #endregion


            return Json(new { isSuccess = result.IsSuccess, message = result.Message, ticketId = result.Data });
        }

        // ===================== پاسخ =====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(long ticketId, string body, bool closeTicket = false)
        {
            string senderRole = User.IsInRole(UserRoles.SuperAdmin) ? UserRoles.SuperAdmin
                : User.IsInRole(UserRoles.Admin) ? UserRoles.Admin : UserRoles.Member;

            var result = await _replyService.Execute(new ReplyTicketDto
            {
                TicketId = ticketId,
                SenderUserId = CurrentUserId,
                SenderRole = senderRole,
                Body = body,
                CloseTicket = closeTicket
            });


            #region ارسال پیام به ربات جهت اطلاع رسانی
            
                var Ticket = _context.Tickets.Where(c => c.Id == ticketId).FirstOrDefault();
        
            var q = (from r in _context.UserRoles
                     join u in _context.Users on r.UserId equals u.Id
                     join g in _context.Gyms on u.GymId equals g.Id
                     where r.RoleId == 2 && u.GymId == Ticket.GymId
                     select new
                     {
                         u,
                         g,
                         r,
                     }).FirstOrDefault();



            long BaleChatId = 0;
            var SenderUser = _context.Users.Where(c => c.Id == CurrentUserId).First();
           
            //var Resiver = _context.Tickets.Where(c => c.Id == ticketId).First();



            if (senderRole == UserRoles.Member)
            {
                BaleChatId = (long)q.u.BaleChatId;//مربوط به مدیر باشگاه
            }
            else if (senderRole == UserRoles.Admin && Ticket.SenderRole== UserRoles.Member)
            {
                BaleChatId = (long)_context.Users.Where(c => c.Id == Ticket.SenderUserId).First().BaleChatId;//برای عضو باشگاه
            }
            else 
            {
                BaleChatId = (long)_context.Setings.Where(c => c.Code == "01").First().SuperAdminChatId;//مربوط به مدیر سایت
            }




            string msg = "📩پاسخ تیکت برای شما ارسال گردید.\n➖نام و نام خانوادگی ارسال کننده: " + SenderUser.FullName + "\n➖شماره تماس: " + SenderUser.PhoneNumber + "\n➖موضوع: *" + Ticket.Subject + "*\n➖پاسخ: " + body;

            await _baleMenuService.EditMemberInfoSend((long)BaleChatId, msg);

            #endregion


            return Json(new { isSuccess = result.IsSuccess, message = result.Message });
        }

        // ===================== بستن =====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(long ticketId)
        {
            var result = await _closeService.Execute(ticketId);
            return Json(new { isSuccess = result.IsSuccess, message = result.Message });
        }

        // ===================== Badge شمارش =====================

        [HttpGet]
        public async Task<IActionResult> OpenCount()
        {
            int count;

            if (User.IsInRole(UserRoles.SuperAdmin))
            {
                count = await _getCountService.ForSuperAdmin();
            }
            else if (User.IsInRole(UserRoles.Admin))
            {
                count = await _getCountService.ForAdmin((await CurrentGymIdAsync()) ?? 0);
            }
            else if (User.IsInRole(UserRoles.Member))
            {
                count = await _getCountService.ForMember((CurrentUserId));
            }
            else
            {
                count = 0;
            }

            return Json(new { count });
        }
    }
}