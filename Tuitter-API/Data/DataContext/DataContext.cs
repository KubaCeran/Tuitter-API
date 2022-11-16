using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tuitter_API.Data.DataContext.Mappings;
using Tuitter_API.Data.Entities;

namespace Tuitter_API.Data.DataContext
{
    public class DataContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UsersMapping());
            builder.ApplyConfiguration(new PostsMapping());
            builder.ApplyConfiguration(new CategoriesMapping());
            builder.ApplyConfiguration(new PhotosMapping());

            base.OnModelCreating(builder);
        }
    }
}
