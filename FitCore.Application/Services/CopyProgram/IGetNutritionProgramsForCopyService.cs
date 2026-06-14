using FitCore.Application.Contexts;
using FitCore.Common.Dto;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitCore.Application.Services.CopyPrograms
{
    //====================================================
    // DTO های نتیجه
    //====================================================

    public class ProgramForCopyDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string MemberName { get; set; }
        public string MemberMobile { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int DaysCount { get; set; }
        public string ProgramType { get; set; }
    }

    public class MemberForCopyDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
    }

    //====================================================
    // سرویس دریافت برنامه‌های غذایی برای انتخاب مبدأ
    //====================================================

    public interface IGetNutritionProgramsForCopyService
    {
        Task<ResultDto<List<ProgramForCopyDto>>> Execute(long gymId, string searchKey = "");
    }

    public class GetNutritionProgramsForCopyService : IGetNutritionProgramsForCopyService
    {
        private readonly IDataBaseContext _context;

        public GetNutritionProgramsForCopyService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<List<ProgramForCopyDto>>> Execute(long gymId, string searchKey = "")
        {
            var query =
                _context.NutritionPrograms
                .Where(p =>
                    !p.IsRemoved &&
                    p.GymId == gymId)
                .Include(p => p.Member)
                    .ThenInclude(m => m.AppUser)
                .Include(p => p.ProgramType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                query = query.Where(p =>
                    p.Member.AppUser.FullName.Contains(searchKey) ||
                    p.Member.AppUser.PhoneNumber.Contains(searchKey) ||
                    p.ProgramType.Name.Contains(searchKey));
            }

            var result = await query
                .OrderByDescending(p => p.Id)
                .Take(50)
                .Select(p => new ProgramForCopyDto
                {
                    Id = p.Id,
                    Title = p.ProgramType.Name,
                    MemberName = p.Member.AppUser.FullName,
                    MemberMobile = p.Member.AppUser.PhoneNumber,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    ProgramType = p.ProgramType.Name,
                    DaysCount = _context.NutritionProgramDays
                                    .Count(d => d.NutritionProgramId == p.Id)
                })
                .ToListAsync();

            return new ResultDto<List<ProgramForCopyDto>>
            {
                IsSuccess = true,
                Data = result
            };
        }
    }

    //====================================================
    // سرویس دریافت برنامه‌های تمرینی برای انتخاب مبدأ
    //====================================================

    public interface IGetTrainingProgramsForCopyService
    {
        Task<ResultDto<List<ProgramForCopyDto>>> Execute(long gymId, string searchKey = "");
    }

    public class GetTrainingProgramsForCopyService : IGetTrainingProgramsForCopyService
    {
        private readonly IDataBaseContext _context;

        public GetTrainingProgramsForCopyService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<List<ProgramForCopyDto>>> Execute(long gymId, string searchKey = "")
        {
            var query =
                _context.TrainingPrograms
                .Where(p =>
                    !p.IsRemoved &&
                    p.GymId == gymId)
                .Include(p => p.Member)
                    .ThenInclude(m => m.AppUser)
                .Include(p => p.TrainingProgramType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                query = query.Where(p =>
                    p.Member.AppUser.FullName.Contains(searchKey) ||
                    p.Member.AppUser.PhoneNumber.Contains(searchKey) ||
                    p.Title.Contains(searchKey));
            }

            var result = await query
                .OrderByDescending(p => p.Id)
                .Take(50)
                .Select(p => new ProgramForCopyDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    MemberName = p.Member.AppUser.FullName,
                    MemberMobile = p.Member.AppUser.PhoneNumber,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    ProgramType = p.TrainingProgramType.Name,
                    DaysCount = _context.TrainingDays
                                    .Count(d => d.TrainingProgramId == p.Id)
                })
                .ToListAsync();

            return new ResultDto<List<ProgramForCopyDto>>
            {
                IsSuccess = true,
                Data = result
            };
        }
    }

    //====================================================
    // سرویس دریافت اعضا برای انتخاب عضو مقصد
    //====================================================

    public interface IGetMembersForCopyService
    {
        Task<ResultDto<List<MemberForCopyDto>>> Execute(long gymId, string searchKey = "");
    }

    public class GetMembersForCopyService : IGetMembersForCopyService
    {
        private readonly IDataBaseContext _context;

        public GetMembersForCopyService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<List<MemberForCopyDto>>> Execute(long gymId, string searchKey = "")
        {
            var query =
                _context.Members
                .Where(m =>
                    m.AppUser.GymId == gymId)
                .Include(m => m.AppUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                query = query.Where(m =>
                    m.AppUser.FullName.Contains(searchKey) ||
                    m.AppUser.PhoneNumber.Contains(searchKey));
            }

            var result = await query
                .OrderBy(m => m.AppUser.FullName)
                .Take(100)
                .Select(m => new MemberForCopyDto
                {
                    Id = m.Id,
                    FullName = m.AppUser.FullName,
                    Mobile = m.AppUser.PhoneNumber
                })
                .ToListAsync();

            return new ResultDto<List<MemberForCopyDto>>
            {
                IsSuccess = true,
                Data = result
            };
        }
    }
}