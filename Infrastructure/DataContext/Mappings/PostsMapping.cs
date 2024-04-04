using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataContext.Mappings
{
    public class PostsMapping : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Body).HasMaxLength(120).IsRequired();

            builder
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.UserId);

            builder
                .HasMany<Post>(x => x.Replies)
                .WithOne(x => x.ParentPost)
                .HasForeignKey(x => x.ParentPostId);

            builder
               .HasMany<Category>(x => x.Categories)
               .WithMany(x => x.Posts);
        }
    }
}
