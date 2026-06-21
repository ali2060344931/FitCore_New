using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public interface IBaleBotService
    {
        Task<bool> SendMessageAsync(long chatId, string text);
    }
}
