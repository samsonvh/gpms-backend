using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductionProcessStepIOResultConfiguration : IEntityTypeConfiguration<ProductionProcessStepIOResult>
    {
        public void Configure(EntityTypeBuilder<ProductionProcessStepIOResult> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Consumption).IsRequired(false);
            builder.Property(e => e.Quantity).IsRequired(false);

            builder.HasOne(e => e.ProductionProcessStepResult).WithMany(e => e.StepIOResults).HasForeignKey(e => e.StepResultId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.ProductionProcessStepIO).WithMany(e => e.ProductionProcessStepIOResults).HasForeignKey(e => e.StepIOId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
