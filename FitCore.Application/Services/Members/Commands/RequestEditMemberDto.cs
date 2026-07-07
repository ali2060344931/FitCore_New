using FitCore.Domain.Entities.Members;

using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace FitCore.Application.Services.Members.Commands
{
    public class RequestEditMemberDto
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public string Mobile { get; set; }

        public Gender Gender { get; set; }

        public string BirthDate { get; set; }

        public string MembershipStartDate { get; set; }

        public string MembershipEndDate { get; set; }
        public bool IsActive { get; set; }



        // در DTO ها اضافه کنید:
        // فایل‌های ورودی از فرم
        public IFormFile ProfileImageFile { get; set; }
        public IFormFile VideoFile { get; set; }
        public IFormFile BodyImageFile1 { get; set; }
        public IFormFile BodyImageFile2 { get; set; }
        public IFormFile BodyImageFile3 { get; set; }

        // مسیرهایی که بعد از فشرده‌سازی پر می‌شوند
        public string ProfileImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public string BodyImageUrl1 { get; set; }
        public string BodyImageUrl2 { get; set; }
        public string BodyImageUrl3 { get; set; }
    }
}
