using FitCore.Domain.Entities.Commons;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Users;

using System;

namespace FitCore.Domain.Entities.Trainers
{
    public class Trainer : BaseEntity
    {
        public long GymId { get; set; }

        public virtual Gym Gym { get; set; }

        //------------------------------------------------

        public string AppUserId { get; set; }

        public virtual AppUser AppUser { get; set; }

        //------------------------------------------------

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? NationalCode { get; set; }

        public string? Mobile { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool IsMale { get; set; }

        //------------------------------------------------

        public string? Biography { get; set; }

        public string? Avatar { get; set; }

        //------------------------------------------------

        public bool IsActive { get; set; } = true;
    }
}