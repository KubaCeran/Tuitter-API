using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuitter_API.Data.Entities;

namespace Tuitter_API.Data.DataContext.Mappings
{
    public class PostsMapping : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn().IsRequired();

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.CategoryId).IsRequired();

            builder.Property(x => x.Headline).HasMaxLength(20).IsRequired();

            builder.Property(x => x.Body).HasMaxLength(120).IsRequired();

            builder.Property(x => x.CreationTime).HasComputedColumnSql("GetUtcDate()");

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
        }
    }
}
