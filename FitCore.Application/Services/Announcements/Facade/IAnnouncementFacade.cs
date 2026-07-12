using FitCore.Application.Services.Announcements.Commands.AddAnnouncement;
using FitCore.Application.Services.Announcements.Commands.DeleteAnnouncement;
using FitCore.Application.Services.Announcements.Commands.EditAnnouncement;
using FitCore.Application.Services.Announcements.Dashboard.DismissAnnouncement;
using FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementClick;
using FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementView;
using FitCore.Application.Services.Announcements.Queries;
using FitCore.Application.Services.Announcements.Queries.GetGyms;
using FitCore.Application.Services.Announcements.Queries.GetRoles;

namespace FitCore.Application.Services.Announcements.Facade
{
    public interface IAnnouncementFacade
    {
        #region Commands

        IAddAnnouncementService AddAnnouncementService { get; }

        IEditAnnouncementService EditAnnouncementService { get; }

        IDeleteAnnouncementService DeleteAnnouncementService { get; }

        #endregion

        #region Queries

        IGetAnnouncementsService GetAnnouncementsService { get; }

        IGetAnnouncementByIdService GetAnnouncementByIdService { get; }

        IGetRolesService GetRolesService { get; }

        IGetGymsService GetGymsService { get; }

        IRegisterAnnouncementViewService RegisterAnnouncementViewService { get; }
        IDismissAnnouncementService DismissAnnouncementService { get; }

        IRegisterAnnouncementClickService RegisterAnnouncementClickService { get; }

        #endregion
    }
}