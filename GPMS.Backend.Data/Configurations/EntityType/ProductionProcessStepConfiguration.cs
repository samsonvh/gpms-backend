using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductionProcessStepConfiguration : IEntityTypeConfiguration<ProductionProcessStep>
    {
        public void Configure(EntityTypeBuilder<ProductionProcessStep> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.OutputPerHour);
            builder.Property(e => e.OrderNumber);

            builder.HasOne<ProductProductionProcess>().WithMany().HasForeignKey(e => e.ProductionProcessId);

            builder.HasMany<ProductFault>().WithOne().HasForeignKey(e => e.ProductionProcessStepId);
        }
    }
}
