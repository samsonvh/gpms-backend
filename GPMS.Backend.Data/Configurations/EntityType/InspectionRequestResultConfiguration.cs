using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Models.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class InspectionRequestResultConfiguration : IEntityTypeConfiguration<InspectionRequestResult>
    {
        public void Configure(EntityTypeBuilder<InspectionRequestResult> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(e => e.FaultyQuantity);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);

            builder.HasOne<Staff>().WithMany().HasForeignKey(e => e.CreatorId);
            builder.HasOne<InspectionRequest>().WithOne().HasForeignKey<InspectionRequestResult>(e => e.InspectionRequestId);
            builder.HasOne<ProductionProcessStepResult>().WithOne(e => e.InspectionRequestResult).HasForeignKey<ProductionProcessStepResult>(e => e.InspectionRequestResultId);

            builder.HasMany<FaultyProduct>().WithOne().HasForeignKey(e => e.InspectionRequestResultId);
        }
    }
}
