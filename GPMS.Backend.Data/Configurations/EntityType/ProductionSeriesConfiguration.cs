using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductionSeriesConfiguration : IEntityTypeConfiguration<ProductionSeries>
    {
        public void Configure(EntityTypeBuilder<ProductionSeries> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.Quantity);
            builder.Property(e => e.FaultyQuantity).IsRequired(false);
            builder.Property(e => e.CurrentProcess).HasMaxLength(100).IsRequired(false);
            builder.Property(e => e.Status);

            builder.HasOne<ProductionEstimation>().WithMany().HasForeignKey(e => e.ProductionEstimationId);

            builder.HasMany<InspectionRequest>().WithOne().HasForeignKey(e => e.ProductionSeriesId);
        }
    }
}
