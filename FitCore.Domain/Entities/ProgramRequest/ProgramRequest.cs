using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Members;
using FitCore.Domain.Entities.Users;

using System;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Domain.Entities.ProgramRequest
{
    /// <summary>
    /// درخواست برنامه توسط عضو
    /// </summary>
    public class ProgramRequest : BaseEntity
    {
        /// <summary>
        /// شناسه عضو درخواست‌دهنده
        /// </summary>
        public long MemberId { get; set; }
        public Member Member { get; set; }

        /// <summary>
        /// شناسه باشگاه
        /// </summary>
        public long GymId { get; set; }
        public Gym Gym { get; set; }

        /// <summary>
        /// نوع برنامه درخواستی
        /// </summary>
        public ProgramRequestType RequestType { get; set; }

        /// <summary>
        /// وضعیت درخواست
        /// </summary>
        public ProgramRequestStatus Status { get; set; }
            = ProgramRequestStatus.Pending;

        /// <summary>
        /// توضیح / هدف عضو از این درخواست
        /// </summary>
        [MaxLength(1000)]
        public string MemberNote { get; set; }

        /// <summary>
        /// پاسخ / یادداشت مدیر باشگاه
        /// </summary>
        [MaxLength(1000)]
        public string AdminNote { get; set; }


        /// <summary>
        /// شناسه مدیری که درخواست را پردازش کرده
        /// </summary>
        public long? ProcessedByUserId { get; set; }
        public AppUser ProcessedByUser { get; set; }
    }

    public enum ProgramRequestType
    {
        Nutrition = 1,   // برنامه غذایی
        Training  = 2,   // برنامه تمرینی
        Both      = 3    // هر دو برنامه
    }

    public enum ProgramRequestStatus
    {
        Pending    = 1,  // در انتظار بررسی
        InProgress = 2,  // در حال آماده‌سازی
        Done       = 3,  // تحویل داده شد
        Rejected   = 4   // رد شد
    }
}
