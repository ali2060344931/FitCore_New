using FitCore.Domain.Entities.Announcements;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitCore.Persistence.Configurations
{
    public class AnnouncementConfiguration :
        IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Message)
                .IsRequired();

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.ButtonText)
                .HasMaxLength(100);

            builder.Property(x => x.ButtonUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Type)
                .HasConversion<int>();

            builder.Property(x => x.Priority)
                .HasConversion<int>();

            builder.HasMany(x => x.Roles)
                .WithOne(x => x.Announcement)
                .HasForeignKey(x => x.AnnouncementId);

            builder.HasMany(x => x.Gyms)
                .WithOne(x => x.Announcement)
                .HasForeignKey(x => x.AnnouncementId);

            builder.HasMany(x => x.Views)
                .WithOne(x => x.Announcement)
                .HasForeignKey(x => x.AnnouncementId);

            builder.HasIndex(x => x.IsActive);

            builder.HasIndex(x => x.IsPinned);

            builder.HasIndex(x => x.DisplayOrder);
        }
    }
}