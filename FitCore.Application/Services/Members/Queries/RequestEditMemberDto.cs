using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries
{
    public class RequestEditMemberDto
    {
        public long Id { get; set; }

        public long AppUserId { get; set; }

        public string Gender { get; set; }

        public string BirthDate { get; set; }
        public string MembershipStartDate { get; set; }
        public string MembershipEndDate { get; set; }

        public int? ActivityLevelId { get; set; }
        public int? ExperienceLevelId { get; set; }

        [MaxLength(500)]
        public string FoodAllergies { get; set; }

        [MaxLength(500)]
        public string MedicalConditions { get; set; }

        [MaxLength(500)]
        public string Injuries { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }





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
