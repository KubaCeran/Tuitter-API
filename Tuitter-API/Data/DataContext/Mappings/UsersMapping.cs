using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuitter_API.Data.Entities;

namespace Tuitter_API.Data.DataContext.Mappings
{
    public class UsersMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.CreatedAt).HasComputedColumnSql("GetUtcDate()");
        }
    }
}
