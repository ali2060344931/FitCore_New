using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    // ============ Get List (برای عضو: تیکت‌های خودش / برای ادمین: تیکت‌های اعضای باشگاهش / برای سوپرادمین: تیکت‌های همه ادمین‌ها) ============

    public interface IGetTicketsService
    {
        Task<List<TicketListItemDto>> ExecuteForMember(long senderUserId);
        Task<List<TicketListItemDto>> ExecuteForAdmin(long gymId);
        Task<List<TicketListItemDto>> ExecuteForSuperAdmin();
    }
}