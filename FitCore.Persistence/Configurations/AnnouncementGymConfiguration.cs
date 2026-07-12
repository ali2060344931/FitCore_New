using FitCore.Domain.Entities.Announcements;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCore.Persistence.Configurations
{
    public class AnnouncementGymConfiguration :
        IEntityTypeConfiguration<AnnouncementGym>
    {
        public void Configure(EntityTypeBuilder<AnnouncementGym> builder)
        {
            builder.HasOne(x => x.Gym)
                .WithMany()
                .HasForeignKey(x => x.GymId);

            builder.HasIndex(x => new
            {
                x.AnnouncementId,
                x.GymId
            }).IsUnique();
        }
    }
}