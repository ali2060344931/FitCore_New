using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitCore.Domain.Entities.Announcements;

namespace FitCore.Application.Services.Announcements.Dashboard.GetDashboardAnnouncements
{
    public class GetDashboardAnnouncementsService :
        IGetDashboardAnnouncementsService
    {
        private readonly IDataBaseContext _context;

        public GetDashboardAnnouncementsService(
            IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<ResultGetDashboardAnnouncementDto>> Execute(
            RequestGetDashboardAnnouncementsDto request)
        {
            var now = DateTime.Now;

            var query =
                _context.Announcements
                .AsNoTracking()
                .Where(x =>
                    !x.IsRemoved &&
                    x.IsActive &&
                    (!x.StartDate.HasValue || x.StartDate <= now) &&
                    (!x.EndDate.HasValue || x.EndDate >= now));

            //------------------------------------
            // Gym
            //------------------------------------

            query = query.Where(x =>
                    x.IsForAllGyms ||

                    x.Gyms.Any(g =>
                        g.GymId == request.GymId));

            //------------------------------------
            // Role
            //------------------------------------

            query = query.Where(x =>
                x.IsForAllRoles ||

                x.Roles.Any(r =>
                    request.RoleIds.Contains(r.RoleId)));

            //-------------------------------------------------
            // ShowOnce
            //-------------------------------------------------

            query = query.Where(a =>

                !a.ShowOnce ||

                !a.Views.Any(v =>
                    v.UserId == request.UserId)
            );


            //-------------------------------------------------
            // Dismiss
            //-------------------------------------------------

            query = query.Where(a =>

                !a.Views.Any(v =>

                    v.UserId == request.UserId &&

                    v.DismissedAt != null &&

                    (

                        a.RepeatAfterDays == null ||

                        v.DismissedAt.Value.AddDays(a.RepeatAfterDays.Value)
                            > now

                    )
                )
            );
            //------------------------------------
            // مرتب سازی
            //------------------------------------

            var data =
                await query

                    .OrderByDescending(x => x.IsPinned)

                    .ThenByDescending(x => x.Priority)

                    .ThenBy(x => x.DisplayOrder)

                    .ThenByDescending(x => x.InsertTime)

                    .Select(x => new ResultGetDashboardAnnouncementDto
                    {
                        Id = x.Id,

                        Title = x.Title,

                        Message = x.Message,

                        ImageUrl = x.ImageUrl,

                        ButtonText = x.ButtonText,

                        ButtonUrl = x.ButtonUrl,

                        Type = x.Type,

                        Priority = x.Priority,

                        CanDismiss = x.CanDismiss,

                        IsPinned = x.IsPinned,

                        ShowOnce = x.ShowOnce,

                        RepeatAfterDays = x.RepeatAfterDays,

                        DisplayOrder = x.DisplayOrder,
                    })

                    .FirstOrDefaultAsync();

            return new ResultDto<ResultGetDashboardAnnouncementDto>
            {
                IsSuccess = true,
                Data = data
            };
        }
    }
}