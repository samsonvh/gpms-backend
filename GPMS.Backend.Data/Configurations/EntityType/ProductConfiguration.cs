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
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Sizes).HasMaxLength(100);
            builder.Property(e => e.Colors).HasMaxLength(100);
            builder.Property(e => e.ImageURLs).HasMaxLength(4000).IsRequired(false);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.Status);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

            builder.HasOne<Category>().WithMany(e => e.Products).HasForeignKey(e => e.CategoryId);
            builder.HasOne<Staff>().WithMany(e => e.CreatedProducts).HasForeignKey(e => e.CreatorId);
            builder.HasOne<Staff>().WithMany(e => e.ReviewedProducts).HasForeignKey(e => e.ReviewerId).IsRequired(false);

            builder.HasMany<ProductSpecification>().WithOne(e => e.Product).HasForeignKey(e => e.ProductId);
            builder.HasMany<SemiFinishedProduct>().WithOne(e => e.Product).HasForeignKey(e => e.ProductId);
            builder.HasMany<ProductProductionProcess>().WithOne(e => e.Product).HasForeignKey(e => e.ProductId);
        }
    }
}
