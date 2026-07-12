using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Commands.EditAnnouncement
{
    public interface IEditAnnouncementService
    {
        Task<ResultDto<RequestEditAnnouncementDto>> Execute(RequestEditAnnouncementDto request);
    }

}
