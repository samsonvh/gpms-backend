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
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Size).HasMaxLength(100);
            builder.Property(e => e.Color).HasMaxLength(100);
            builder.Property(e => e.InventoryQuantity);

            builder.HasOne(e => e.Product).WithMany(e => e.Specifications).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Warehouse).WithMany(e => e.Specifications).HasForeignKey(e => e.WarehouseId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
