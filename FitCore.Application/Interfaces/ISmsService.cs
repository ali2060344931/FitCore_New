using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Interfaces
{
    public interface ISmsService
    {
        Task SendAsync(string phoneNumber, string message);
    }
}
