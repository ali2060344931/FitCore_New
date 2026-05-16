using FitCore.Domain.Entities.Commons;

namespace FitCore.Domain.Entities.Users
{

    /// <summary>
    /// نقش ها کاربران
    /// </summary>
    public class UserInRole:BaseEntity
    {
        //public long Id { get; set; }
        public virtual User User { get; set; }
        public long UserId { get; set; }

        public virtual Role Role { get; set; }
        public long RoleId { get; set; }
    }
}
