using FitCore.Application.Services.Announcements.Commands.AddAnnouncement;
using FitCore.Application.Services.Announcements.Commands.DeleteAnnouncement;
using FitCore.Application.Services.Announcements.Commands.EditAnnouncement;
using FitCore.Application.Services.Announcements.Dashboard.DismissAnnouncement;
using FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementClick;
using FitCore.Application.Services.Announcements.Dashboard.RegisterAnnouncementView;
using FitCore.Application.Services.Announcements.Queries;
using FitCore.Application.Services.Announcements.Queries.GetAnnouncementById;
using FitCore.Application.Services.Announcements.Queries.GetAnnouncements;
using FitCore.Application.Services.Announcements.Queries.GetGyms;
using FitCore.Application.Services.Announcements.Queries.GetRoles;

namespace FitCore.Application.Services.Announcements.Facade
{
    public class AnnouncementFacade : IAnnouncementFacade
    {
        public AnnouncementFacade(

            IAddAnnouncementService addAnnouncementService,

            IEditAnnouncementService editAnnouncementService,

            IDeleteAnnouncementService deleteAnnouncementService,

            IGetAnnouncementsService getAnnouncementsService,

            IGetAnnouncementByIdService getAnnouncementByIdService,

            IGetRolesService getRolesService,

            IGetGymsService getGymsService,

            IRegisterAnnouncementViewService registerAnnouncementViewService,

            IDismissAnnouncementService dismissAnnouncementService,

            IRegisterAnnouncementClickService registerAnnouncementClickService

            )
        {
            AddAnnouncementService = addAnnouncementService;

            EditAnnouncementService = editAnnouncementService;

            DeleteAnnouncementService = deleteAnnouncementService;

            GetAnnouncementsService = getAnnouncementsService;

            GetAnnouncementByIdService = getAnnouncementByIdService;

            GetRolesService = getRolesService;

            GetGymsService = getGymsService;

            RegisterAnnouncementViewService = registerAnnouncementViewService;

            DismissAnnouncementService = dismissAnnouncementService;

            RegisterAnnouncementClickService =    registerAnnouncementClickService;

        }

        #region Commands

        public IAddAnnouncementService AddAnnouncementService { get; }

        public IEditAnnouncementService EditAnnouncementService { get; }

        public IDeleteAnnouncementService DeleteAnnouncementService { get; }

        #endregion

        #region Queries

        public IGetAnnouncementsService GetAnnouncementsService { get; }

        public IGetAnnouncementByIdService GetAnnouncementByIdService { get; }

        public IGetRolesService GetRolesService { get; }

        public IGetGymsService GetGymsService { get; }

        public IRegisterAnnouncementViewService RegisterAnnouncementViewService { get; }

        public IDismissAnnouncementService DismissAnnouncementService { get; }
        public IRegisterAnnouncementClickService RegisterAnnouncementClickService { get; }

        #endregion
    }
}