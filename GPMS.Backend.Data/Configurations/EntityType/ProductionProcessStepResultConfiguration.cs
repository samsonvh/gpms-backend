using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Models.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductionProcessStepResultConfiguration : IEntityTypeConfiguration<ProductionProcessStepResult>
    {
        public void Configure(EntityTypeBuilder<ProductionProcessStepResult> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
            builder.Property(e => e.Status);

            builder.HasOne(e => e.InspectionRequestResult)
                .WithOne(e => e.ProductionProcessStepResult)
                .HasForeignKey<ProductionProcessStepResult>(e => e.InspectionRequestResultId)
                .IsRequired(false).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Creator)
                .WithMany(e => e.productionProcessStepResults)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.ProductionSeries)
                .WithMany(e => e.ProductionProcessStepResults)
                .HasForeignKey(e => e.ProductionSeriesId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.ProductionProcessStep)
                .WithMany(e => e.ProductionProcessStepResults)
                .HasForeignKey(e => e.ProductionProcessStepId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
