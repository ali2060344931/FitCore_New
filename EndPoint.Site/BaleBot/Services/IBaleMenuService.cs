using System.Threading.Tasks;

namespace EndPoint.Site.BaleBot.Services
{
    public interface IBaleMenuService
    {
        Task ShowMainMenu(long chatId, string userName);
        Task ShowErrorWithMenu(long chatId, string errorMessage);
        Task SendSurveyToBale(long chatId);
    }
}
