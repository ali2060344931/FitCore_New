using FitCore.Application.Contexts;
using FitCore.Application.Services.Members.Queries.ReportMembers;
using FitCore.Common;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using Microsoft.AspNetCore.Hosting;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IAddOrUpdateMemberService
    {
        // اضافه شدن Task
        Task<ResultDto> Execute(RequestCompleteMemberInfoDto request);
        ResultDto DeleteMedia(long appUserId, string mediaType);
    }

    public class AddOrUpdateMemberService : IAddOrUpdateMemberService
    {
        private readonly IDataBaseContext _context;
        private readonly IFileCompressionService _fileService;
        private readonly IWebHostEnvironment _env;

        public AddOrUpdateMemberService(IDataBaseContext context, IFileCompressionService fileService, IWebHostEnvironment env)
        {
            _context = context;
            _fileService = fileService;
            _env = env;
        }

        // اضافه شدن async
        public async Task<ResultDto> Execute(RequestCompleteMemberInfoDto request)
        {
            var member = _context.Members.FirstOrDefault(x => x.AppUserId == request.AppUserId);
            var user = _context.Users.Where(c => c.Id == request.AppUserId).First();

            if (!CheckValidMobile.IsValidMobile(request.Mobile))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شماره موبایل وارد شده صیحیح نمی باشد."
                };
            }

            // ===== پردازش و فشرده‌سازی فایل‌ها =====
            var folderPath = "uploads/members";

            // تبدیل .Result به await (بسیار مهم برای جلوگیری از قطع سرور)
            if (request.ProfileImageFile != null)
                request.ProfileImageUrl = await _fileService.SaveAndCompressImageAsync(request.ProfileImageFile, folderPath);

            if (request.VideoFile != null)
                request.VideoUrl = await _fileService.SaveAndCompressVideoAsync(request.VideoFile, folderPath);

            if (request.BodyImageFile1 != null)
                request.BodyImageUrl1 = await _fileService.SaveAndCompressImageAsync(request.BodyImageFile1, folderPath);

            if (request.BodyImageFile2 != null)
                request.BodyImageUrl2 = await _fileService.SaveAndCompressImageAsync(request.BodyImageFile2, folderPath);

            if (request.BodyImageFile3 != null)
                request.BodyImageUrl3 = await _fileService.SaveAndCompressImageAsync(request.BodyImageFile3, folderPath);
            // =========================================================

            if (member == null)
            {
                member = new Domain.Entities.Members.Member
                {
                    AppUserId = request.AppUserId,
                    Gender = request.Gender,
                    BirthDate = request.BirthDate,
                    ActivityLevelId = request.ActivityLevelId,
                    ExperienceLevelId = request.ExperienceLevelId,
                    FoodAllergies = request.FoodAllergies,
                    MedicalConditions = request.MedicalConditions,
                    Injuries = request.Injuries,
                    IsActive = request.IsActive,
                    Description = request.Description,
                    Height = request.Height,

                    // اضافه شدن این ۵ خط (باگ منطقی رفع شد)
                    ProfileImageUrl = request.ProfileImageUrl,
                    VideoUrl = request.VideoUrl,
                    BodyImageUrl1 = request.BodyImageUrl1,
                    BodyImageUrl2 = request.BodyImageUrl2,
                    BodyImageUrl3 = request.BodyImageUrl3
                };

                user.FullName = request.FullName;
                user.PhoneNumber = request.Mobile;

                _context.Members.Add(member);
                _context.SaveChanges();

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "اطلاعات عضو با موفقیت ثبت شد."
                };
            }

            // کدهای ویرایش (بدون تغییر)
            user.FullName = request.FullName;
            user.PhoneNumber = request.Mobile;
            member.Gender = request.Gender;
            member.BirthDate = request.BirthDate;
            member.ActivityLevelId = request.ActivityLevelId;
            member.ExperienceLevelId = request.ExperienceLevelId;
            member.FoodAllergies = request.FoodAllergies;
            member.MedicalConditions = request.MedicalConditions;
            member.Injuries = request.Injuries;
            member.IsActive = request.IsActive;
            member.Description = request.Description;
            member.Height = request.Height;

            // فقط اگر فایل جدیدی آپلود شده بود، مسیر جدید را در دیتابیس ثبت کن
            if (request.ProfileImageFile != null)
                member.ProfileImageUrl = request.ProfileImageUrl;

            if (request.VideoFile != null)
                member.VideoUrl = request.VideoUrl;

            if (request.BodyImageFile1 != null)
                member.BodyImageUrl1 = request.BodyImageUrl1;

            if (request.BodyImageFile2 != null)
                member.BodyImageUrl2 = request.BodyImageUrl2;

            if (request.BodyImageFile3 != null)
                member.BodyImageUrl3 = request.BodyImageUrl3;


            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "اطلاعات عضو با موفقیت ویرایش شد."
            };
        }



        public ResultDto DeleteMedia(long appUserId, string mediaType)
        {
            var member = _context.Members.FirstOrDefault(x => x.AppUserId == appUserId);
            if (member == null) return new ResultDto { IsSuccess = false, Message = "عضو یافت نشد" };

            string filePath = null;

            // بررسی اینکه کدام فایل باید حذف شود
            switch (mediaType)
            {
                case "ProfileImage": filePath = member.ProfileImageUrl; member.ProfileImageUrl = null; break;
                case "Video": filePath = member.VideoUrl; member.VideoUrl = null; break;
                case "BodyImage1": filePath = member.BodyImageUrl1; member.BodyImageUrl1 = null; break;
                case "BodyImage2": filePath = member.BodyImageUrl2; member.BodyImageUrl2 = null; break;
                case "BodyImage3": filePath = member.BodyImageUrl3; member.BodyImageUrl3 = null; break;
                default: return new ResultDto { IsSuccess = false, Message = "نوع فایل نامعتبر است" };
            }

            // حذف فیزیکی فایل از روی هارد سرور
            if (!string.IsNullOrEmpty(filePath))
            {
                var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            _context.SaveChanges();
            return new ResultDto { IsSuccess = true, Message = "فایل با موفقیت حذف شد" };
        }
    }
}