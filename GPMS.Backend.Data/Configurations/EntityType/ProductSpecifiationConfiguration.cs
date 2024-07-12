using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Models.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductSpecifiationConfiguration : IEntityTypeConfiguration<ProductSpecification>
    {
        public void Configure(EntityTypeBuilder<ProductSpecification> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Size).HasMaxLength(100);
            builder.Property(e => e.Color).HasMaxLength(100);
            builder.Property(e => e.InventoryQuantity);

            builder.HasOne<Product>().WithMany().HasForeignKey(e => e.ProductId);
            builder.HasOne<Warehouse>().WithMany().HasForeignKey(e => e.WarehouseId);

            builder.HasMany<ProductionRequirement>().WithOne().HasForeignKey(e => e.ProductSpecificationId);
            builder.HasMany<Measurement>().WithOne().HasForeignKey(e => e.ProductSpecificationId);
            builder.HasMany<FaultyProduct>().WithOne().HasForeignKey(e => e.SpecificationId);
            builder.HasMany<WarehouseTicket>().WithOne().HasForeignKey(e => e.ProductSpecificationId);
        }
    }
}
