using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Sizes).HasMaxLength(100);
            builder.Property(e => e.Colors).HasMaxLength(100);
            builder.Property(e => e.ImageURLs).HasMaxLength(4000).IsRequired(false);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.Status);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

            builder.HasOne(e => e.Category).WithMany(e => e.Products).HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Creator).WithMany(e => e.CreatedProducts).HasForeignKey(e => e.CreatorId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Reviewer).WithMany(e => e.ReviewedProducts).HasForeignKey(e => e.ReviewerId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
