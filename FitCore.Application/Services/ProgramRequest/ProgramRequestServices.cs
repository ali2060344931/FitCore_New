using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.ProgramRequest;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.ProgramRequests
{
    // ====================================================
    // DTOs
    // ====================================================

    public class SubmitProgramRequestDto
    {
        public long MemberId { get; set; }
        public long GymId { get; set; }
        public ProgramRequestType RequestType { get; set; }
        public string MemberNote { get; set; }
    }

    public class UpdateProgramRequestDto
    {
        public long RequestId { get; set; }
        public ProgramRequestStatus NewStatus { get; set; }
        public string AdminNote { get; set; }
        public long ProcessedByUserId { get; set; }
    }

    public class ProgramRequestListDto
    {
        public long Id { get; set; }
        public string MemberName { get; set; }
        public string MemberMobile { get; set; }
        public ProgramRequestType RequestType { get; set; }
        public string RequestTypeLabel { get; set; }
        public ProgramRequestStatus Status { get; set; }
        public string StatusLabel { get; set; }
        public string MemberNote { get; set; }
        public string AdminNote { get; set; }
        public DateTime InsertTime { get; set; }
    }

    // ====================================================
    // Submit — عضو درخواست می‌دهد
    // ====================================================

    public interface ISubmitProgramRequestService
    {
        Task<ResultDto> Execute(SubmitProgramRequestDto request);
    }

    public class SubmitProgramRequestService : ISubmitProgramRequestService
    {
        private readonly IDataBaseContext _context;

        public SubmitProgramRequestService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(SubmitProgramRequestDto request)
        {
            // بررسی درخواست تکراری در انتظار بررسی
            bool hasPending = await _context.ProgramRequests
                .AnyAsync(r =>
                    r.MemberId == request.MemberId &&
                    r.RequestType == request.RequestType &&
                    r.Status == ProgramRequestStatus.Pending &&
                    !r.IsRemoved);

            if (hasPending)
            {
                return ResultDto.Failure(
                    "شما یک درخواست مشابه در انتظار بررسی دارید. لطفاً منتظر پاسخ مدیر باشگاه باشید.");
            }

            var entity = new ProgramRequest
            {
                MemberId    = request.MemberId,
                GymId       = request.GymId,
                RequestType = request.RequestType,
                MemberNote  = request.MemberNote,
                Status      = ProgramRequestStatus.Pending,
                InsertTime  = DateTime.Now
            };

            await _context.ProgramRequests.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ResultDto.Success("درخواست شما با موفقیت ارسال شد. مدیر باشگاه در اسرع وقت پاسخ خواهد داد.");
        }
    }

    // ====================================================
    // Update Status — مدیر وضعیت را تغییر می‌دهد
    // ====================================================

    public interface IUpdateProgramRequestService
    {
        Task<ResultDto> Execute(UpdateProgramRequestDto request);
    }

    public class UpdateProgramRequestService : IUpdateProgramRequestService
    {
        private readonly IDataBaseContext _context;

        public UpdateProgramRequestService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto> Execute(UpdateProgramRequestDto request)
        {
            var entity = await _context.ProgramRequests
                .FirstOrDefaultAsync(r => r.Id == request.RequestId && !r.IsRemoved);

            if (entity == null)
                return ResultDto.Failure("درخواست مورد نظر یافت نشد.");

            entity.Status             = request.NewStatus;
            entity.AdminNote          = request.AdminNote;
            entity.ProcessedByUserId  = request.ProcessedByUserId;
            entity.UpdateTime         = DateTime.Now;

            await _context.SaveChangesAsync();

            return ResultDto.Success("وضعیت درخواست با موفقیت به‌روز شد.");
        }
    }

    // ====================================================
    // Get For Admin — لیست درخواست‌های باشگاه
    // ====================================================

    public interface IGetProgramRequestsForAdminService
    {
        Task<ResultDto<List<ProgramRequestListDto>>> Execute(
            long gymId, ProgramRequestStatus? statusFilter = null, string searchKey = "");
    }

    public class GetProgramRequestsForAdminService : IGetProgramRequestsForAdminService
    {
        private readonly IDataBaseContext _context;

        public GetProgramRequestsForAdminService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<List<ProgramRequestListDto>>> Execute(
            long gymId, ProgramRequestStatus? statusFilter = null, string searchKey = "")
        {
            var query = _context.ProgramRequests
                .Where(r => r.GymId == gymId && !r.IsRemoved)
                .Include(r => r.Member).ThenInclude(m => m.AppUser)
                .AsQueryable();

            if (statusFilter.HasValue)
                query = query.Where(r => r.Status == statusFilter.Value);

            if (!string.IsNullOrWhiteSpace(searchKey))
                query = query.Where(r =>
                    r.Member.AppUser.FullName.Contains(searchKey) ||
                    r.Member.AppUser.PhoneNumber.Contains(searchKey));

            var list = await query
                .OrderByDescending(r => r.InsertTime)
                .Select(r => new ProgramRequestListDto
                {
                    Id              = r.Id,
                    MemberName      = r.Member.AppUser.FullName,
                    MemberMobile    = r.Member.AppUser.PhoneNumber,
                    RequestType     = r.RequestType,
                    RequestTypeLabel = r.RequestType == ProgramRequestType.Nutrition ? "برنامه غذایی"
                                    : r.RequestType == ProgramRequestType.Training  ? "برنامه تمرینی"
                                    : "هر دو برنامه",
                    Status          = r.Status,
                    StatusLabel     = r.Status == ProgramRequestStatus.Pending    ? "در انتظار بررسی"
                                    : r.Status == ProgramRequestStatus.InProgress ? "در حال آماده‌سازی"
                                    : r.Status == ProgramRequestStatus.Done       ? "تحویل داده شد"
                                    : "رد شد",
                    MemberNote      = r.MemberNote,
                    AdminNote       = r.AdminNote,
                    InsertTime      = r.InsertTime
                })
                .ToListAsync();

            return ResultDto<List<ProgramRequestListDto>>.Success(list);
        }
    }

    // ====================================================
    // Get For Member — درخواست‌های یک عضو
    // ====================================================

    public interface IGetProgramRequestsForMemberService
    {
        Task<ResultDto<List<ProgramRequestListDto>>> Execute(long memberId);
    }

    public class GetProgramRequestsForMemberService : IGetProgramRequestsForMemberService
    {
        private readonly IDataBaseContext _context;

        public GetProgramRequestsForMemberService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<List<ProgramRequestListDto>>> Execute(long memberId)
        {
            var list = await _context.ProgramRequests
                .Where(r => r.MemberId == memberId && !r.IsRemoved)
                .OrderByDescending(r => r.InsertTime)
                .Select(r => new ProgramRequestListDto
                {
                    Id               = r.Id,
                    RequestType      = r.RequestType,
                    RequestTypeLabel = r.RequestType == ProgramRequestType.Nutrition ? "برنامه غذایی"
                                    : r.RequestType == ProgramRequestType.Training   ? "برنامه تمرینی"
                                    : "هر دو برنامه",
                    Status           = r.Status,
                    StatusLabel      = r.Status == ProgramRequestStatus.Pending    ? "در انتظار بررسی"
                                    : r.Status == ProgramRequestStatus.InProgress  ? "در حال آماده‌سازی"
                                    : r.Status == ProgramRequestStatus.Done        ? "تحویل داده شد"
                                    : "رد شد",
                    MemberNote       = r.MemberNote,
                    AdminNote        = r.AdminNote,
                    InsertTime       = r.InsertTime
                })
                .ToListAsync();

            return ResultDto<List<ProgramRequestListDto>>.Success(list);
        }
    }

    // ====================================================
    // Get Pending Count — تعداد درخواست‌های در انتظار (برای badge داشبورد)
    // ====================================================

    public interface IGetPendingRequestsCountService
    {
        Task<int> Execute(long gymId);
    }

    public class GetPendingRequestsCountService : IGetPendingRequestsCountService
    {
        private readonly IDataBaseContext _context;

        public GetPendingRequestsCountService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<int> Execute(long gymId)
        {
            return await _context.ProgramRequests
                .CountAsync(r =>
                    r.GymId == gymId &&
                    r.Status == ProgramRequestStatus.Pending &&
                    !r.IsRemoved);
        }
    }
}
