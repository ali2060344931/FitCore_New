using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Queries.GetRoles
{
    public interface IGetRolesService
    {
        Task<ResultDto<List<ResultGetRolesDto>>> Execute();
    }

}
