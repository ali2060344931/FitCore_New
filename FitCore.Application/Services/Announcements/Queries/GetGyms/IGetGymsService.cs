using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Queries.GetGyms
{
    public interface IGetGymsService
    {
        Task<ResultDto<List<ResultGetGymsDto>>> Execute();
    }
}
