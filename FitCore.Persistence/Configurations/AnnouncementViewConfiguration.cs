using FitCore.Domain.Entities.Announcements;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCore.Persistence.Configurations
{
    public class AnnouncementViewConfiguration :
        IEntityTypeConfiguration<AnnouncementView>
    {
        public void Configure(EntityTypeBuilder<AnnouncementView> builder)
        {
            builder.Property(x => x.UserId)
                .HasMaxLength(450);

            builder.HasIndex(x => new
            {
                x.AnnouncementId,
                x.UserId
            });

            builder.HasIndex(x => x.ViewedAt);
        }
    }
}