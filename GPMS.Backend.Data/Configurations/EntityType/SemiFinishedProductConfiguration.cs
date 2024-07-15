using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class SemiFinishedProductConfiguration : IEntityTypeConfiguration<SemiFinishedProduct>
    {
        public void Configure(EntityTypeBuilder<SemiFinishedProduct> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Quantity);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);

            builder.HasOne(e => e.Product).WithMany(e => e.SemiFinishedProducts).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
