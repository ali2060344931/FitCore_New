using FitCore.Application.Interfaces.ISms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.SmsService.Commands
{
    public class SmsService : ISmsService
    {
        public Task SendAsync(string phoneNumber, string message)
        {
            // اینجا API پیامک خودت را قرار بده
            Console.WriteLine($"SMS To: {phoneNumber} | {message}");
            return Task.CompletedTask;
        }
    }
}
