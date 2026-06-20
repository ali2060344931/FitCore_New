using FitCore.Application.Contexts;
using FitCore.Common.Roles;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Tickets
{
    public class GetTicketsService : IGetTicketsService
    {
        private readonly IDataBaseContext _context;
        public GetTicketsService(IDataBaseContext context) => _context = context;

        private IQueryable<TicketListItemDto> BaseQuery()
        {
            return _context.Tickets
                .Where(t => !t.IsRemoved)
                .Include(t => t.SenderUser)
                .Include(t => t.Gym)
                .Select(t => new TicketListItemDto
                {
                    Id = t.Id,
                    Subject = t.Subject,
                    SenderName = t.SenderUser.FullName,
                    GymName = t.Gym != null ? t.Gym.Name : "-",
                    Status = t.Status,
                    Priority = t.Priority,
                    MessagesCount = t.Messages.Count(m => !m.IsRemoved),
                    LastActivity = t.Messages.Any() ? t.Messages.Max(m => m.InsertTime) : t.InsertTime
                });
        }

        public async Task<List<TicketListItemDto>> ExecuteForMember(long senderUserId)
        {
            return await _context.Tickets
                .Where(t => !t.IsRemoved && t.SenderUserId == senderUserId && t.SenderRole == UserRoles.Member)
                .Include(t => t.SenderUser).Include(t => t.Gym)
                .Select(t => new TicketListItemDto
                {
                    Id = t.Id,
                    Subject = t.Subject,
                    SenderName = t.SenderUser.FullName,
                    GymName = t.Gym != null ? t.Gym.Name : "-",
                    Status = t.Status,
                    Priority = t.Priority,
                    MessagesCount = t.Messages.Count(m => !m.IsRemoved),
                    LastActivity = t.Messages.Any() ? t.Messages.Max(m => m.InsertTime) : t.InsertTime
                })
                .OrderByDescending(t => t.LastActivity).ToListAsync();
        }

        public async Task<List<TicketListItemDto>> ExecuteForAdmin(long gymId)
        {
            return await _context.Tickets
                .Where(t => !t.IsRemoved && t.GymId == gymId && t.SenderRole == UserRoles.Member)
                .Include(t => t.SenderUser).Include(t => t.Gym)
                .Select(t => new TicketListItemDto
                {
                    Id = t.Id,
                    Subject = t.Subject,
                    SenderName = t.SenderUser.FullName,
                    GymName = t.Gym != null ? t.Gym.Name : "-",
                    Status = t.Status,
                    Priority = t.Priority,
                    MessagesCount = t.Messages.Count(m => !m.IsRemoved),
                    LastActivity = t.Messages.Any() ? t.Messages.Max(m => m.InsertTime) : t.InsertTime
                })
                .OrderByDescending(t => t.LastActivity).ToListAsync();
        }

        public async Task<List<TicketListItemDto>> ExecuteForSuperAdmin()
        {
            return await _context.Tickets
                .Where(t => !t.IsRemoved && t.SenderRole == UserRoles.Admin)
                .Include(t => t.SenderUser).Include(t => t.Gym)
                .Select(t => new TicketListItemDto
                {
                    Id = t.Id,
                    Subject = t.Subject,
                    SenderName = t.SenderUser.FullName,
                    GymName = t.Gym != null ? t.Gym.Name : "-",
                    Status = t.Status,
                    Priority = t.Priority,
                    MessagesCount = t.Messages.Count(m => !m.IsRemoved),
                    LastActivity = t.Messages.Any() ? t.Messages.Max(m => m.InsertTime) : t.InsertTime
                })
                .OrderByDescending(t => t.LastActivity).ToListAsync();
        }
    }
}