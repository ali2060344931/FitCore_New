using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    // ============ Counts (badge) ============

    public interface IGetOpenTicketsCountService
    {
        Task<int> ForAdmin(long gymId);
        Task<int> ForMember(long appUserId);
        Task<int> ForSuperAdmin();

    }
}