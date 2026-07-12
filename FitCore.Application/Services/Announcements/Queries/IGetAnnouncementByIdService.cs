using FitCore.Application.Services.Announcements.Queries.GetAnnouncementById;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Queries
{
    public interface IGetAnnouncementByIdService
    {
        Task<ResultDto<ResultGetAnnouncementByIdDto>> Execute(long id);
    }
}
