using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuitter_API.Data.Entities;

namespace Tuitter_API.Data.DataContext.Mappings
{
    public class PhotosMapping : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn().IsRequired();

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.PhotoName).IsRequired();

            builder.Property(x => x.PhotoPath).IsRequired();

            builder.Property(x => x.IsProfilePicture).IsRequired().HasDefaultValue(false);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
