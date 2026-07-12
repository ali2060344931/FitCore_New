using EndPoint.Site.Areas.Admin.Models.Announcement;

using FitCore.Application.Services.Announcements.Commands.AddAnnouncement;
using FitCore.Application.Services.Announcements.Commands.EditAnnouncement;
using FitCore.Application.Services.Announcements.Queries.GetAnnouncementById;

namespace EndPoint.Site.Areas.Admin.Mappers
{
    public static class AnnouncementMapper
    {
        public static RequestAddAnnouncementDto ToAddDto(
            AnnouncementCreateEditViewModel model)
        {
            return new RequestAddAnnouncementDto
            {
                Title = model.Title,
                Message = model.Message,
                ImageUrl = model.ImageUrl,

                ButtonText = model.ButtonText,
                ButtonUrl = model.ButtonUrl,

                Type = model.Type,
                Priority = model.Priority,

                IsActive = model.IsActive,
                IsPinned = model.IsPinned,

                ShowOnce = model.ShowOnce,

                IsForAllRoles = model.IsForAllRoles,
                IsForAllGyms = model.IsForAllGyms,

                CanDismiss = model.CanDismiss,
                RepeatAfterDays = model.RepeatAfterDays,

                DisplayOrder = model.DisplayOrder,

                StartDate = model.StartDate,
                EndDate = model.EndDate,

                RoleIds = model.SelectedRoleIds,

                GymIds = model.SelectedGymIds
            };
        }

        public static RequestEditAnnouncementDto ToEditDto(
            AnnouncementCreateEditViewModel model)
        {
            return new RequestEditAnnouncementDto
            {
                Id = model.Id,

                Title = model.Title,
                Message = model.Message,
                ImageUrl = model.ImageUrl,

                ButtonText = model.ButtonText,
                ButtonUrl = model.ButtonUrl,

                Type = model.Type,
                Priority = model.Priority,

                IsActive = model.IsActive,
                IsPinned = model.IsPinned,

                ShowOnce = model.ShowOnce,

                IsForAllRoles = model.IsForAllRoles,
                IsForAllGyms = model.IsForAllGyms,

                CanDismiss = model.CanDismiss,
                RepeatAfterDays = model.RepeatAfterDays,

                DisplayOrder = model.DisplayOrder,

                StartDate = model.StartDate,
                EndDate = model.EndDate,

                RoleIds = model.SelectedRoleIds,

                GymIds = model.SelectedGymIds
            };
        }

        public static AnnouncementCreateEditViewModel ToViewModel(
            ResultGetAnnouncementByIdDto dto)
        {
            return new AnnouncementCreateEditViewModel
            {
                Id = dto.Id,

                Title = dto.Title,
                Message = dto.Message,

                ImageUrl = dto.ImageUrl,

                ButtonText = dto.ButtonText,
                ButtonUrl = dto.ButtonUrl,

                Type = dto.Type,
                Priority = dto.Priority,

                IsActive = dto.IsActive,
                IsPinned = dto.IsPinned,

                ShowOnce = dto.ShowOnce,

                IsForAllRoles = dto.IsForAllRoles,
                IsForAllGyms = dto.IsForAllGyms,

                CanDismiss = dto.CanDismiss,
                RepeatAfterDays = dto.RepeatAfterDays,

                DisplayOrder = dto.DisplayOrder,

                StartDate = dto.StartDate,
                EndDate = dto.EndDate,

                SelectedRoleIds = dto.RoleIds,

                SelectedGymIds = dto.GymIds
            };
        }
    }
}