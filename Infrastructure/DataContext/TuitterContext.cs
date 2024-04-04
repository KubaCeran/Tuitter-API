using Core.Entities;
using Infrastructure.DataContext.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataContext
{
    public class TuitterContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public TuitterContext(DbContextOptions<TuitterContext> options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfiguration(new UsersMapping());
            builder.ApplyConfiguration(new PostsMapping());
            builder.ApplyConfiguration(new CategoriesMapping());

            base.OnModelCreating(builder);
        }
    }
}
