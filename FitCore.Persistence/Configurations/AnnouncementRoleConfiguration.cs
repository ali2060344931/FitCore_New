using FitCore.Domain.Entities.Announcements;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCore.Persistence.Configurations
{
    public class AnnouncementRoleConfiguration :
        IEntityTypeConfiguration<AnnouncementRole>
    {
        public void Configure(EntityTypeBuilder<AnnouncementRole> builder)
        {
            builder.HasIndex(x => new
            {
                x.AnnouncementId,
                x.RoleId
            }).IsUnique();
        }
    }
}