using FitCore.Application.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FitCore.Application.Services.Dashboard
{
    public class SuperAdminDashboardDto
    {
        public int TotalGyms { get; set; }
        public int ActiveGyms { get; set; }
        public int TotalMembers { get; set; }
        public int TotalNutritionPrograms { get; set; }
        public int TotalTrainingPrograms { get; set; }
        public List<GymRowDto> Gyms { get; set; }
    }

    public class GymRowDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string SubscriptionExpireDate { get; set; }
        public int MembersCount { get; set; }
        public int NutritionCount { get; set; }
        public int TrainingCount { get; set; }
        public int ExercisesCount { get; set; }
    }

    public interface ISuperAdminDashboardService
    {
        Task<SuperAdminDashboardDto> Execute();
    }

    public class SuperAdminDashboardService : ISuperAdminDashboardService
    {
        private readonly IDataBaseContext _context;
        public SuperAdminDashboardService(IDataBaseContext context) => _context = context;

        public async Task<SuperAdminDashboardDto> Execute()
        {
            var gyms = await _context.Gyms.AsNoTracking().ToListAsync();

            var rows = new List<GymRowDto>();
            foreach (var g in gyms)
            {
                rows.Add(new GymRowDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Code = g.Code,
                    IsActive = g.IsActive,
                    SubscriptionExpireDate = g.SubscriptionExpireDate,
                    MembersCount = await _context.Users.CountAsync(u => u.GymId == g.Id),
                    NutritionCount = await _context.NutritionPrograms.CountAsync(p => p.GymId == g.Id && !p.IsRemoved),
                    TrainingCount = await _context.TrainingPrograms.CountAsync(p => p.GymId == g.Id && !p.IsRemoved),
                    ExercisesCount = await _context.Exercises.CountAsync(e => e.GymId == g.Id && !e.IsRemoved)
                });
            }

            return new SuperAdminDashboardDto
            {
                TotalGyms = gyms.Count,
                ActiveGyms = gyms.Count(g => g.IsActive),
                TotalMembers = rows.Sum(r => r.MembersCount),
                TotalNutritionPrograms = rows.Sum(r => r.NutritionCount),
                TotalTrainingPrograms = rows.Sum(r => r.TrainingCount),
                Gyms = rows.OrderByDescending(r => r.MembersCount).ToList()
            };
        }
    }
}
