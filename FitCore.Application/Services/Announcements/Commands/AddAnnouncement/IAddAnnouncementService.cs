using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Announcements.Commands.AddAnnouncement
{
    public interface IAddAnnouncementService
    {
        Task<ResultDto<RequestAddAnnouncementDto>> Execute(RequestAddAnnouncementDto request);
    }
}
