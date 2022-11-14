using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuitter_API.Data.Entities;

namespace Tuitter_API.Data.DataContext.Mappings
{
    public class CategoriesMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn().IsRequired();

            builder.Property(x => x.Title).HasMaxLength(20).IsRequired();

        }
    }
}
