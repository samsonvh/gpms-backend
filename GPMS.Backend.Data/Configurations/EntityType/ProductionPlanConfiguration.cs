using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductionPlanConfiguration : IEntityTypeConfiguration<ProductionPlan>
    {
        public void Configure(EntityTypeBuilder<ProductionPlan> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.ExpectedStartingDate);
            builder.Property(e => e.DueDate);
            builder.Property(e => e.ActualStartingDate).IsRequired(false);
            builder.Property(e => e.CompletionDate).IsRequired(false);
            builder.Property(e => e.CreatedDate).IsRequired().HasDefaultValue(DateTime.UtcNow);
            builder.Property(e => e.Type);
            builder.Property(e => e.Status);

            builder.HasOne<Staff>().WithMany().HasForeignKey(e => e.CreatorId);
            builder.HasOne<Staff>().WithMany().HasForeignKey(e => e.ReviewerId);
            builder.HasOne<ProductionPlan>().WithMany().HasForeignKey(e => e.ParentProductionPlanId);

            builder.HasMany<ProductionPlan>().WithOne().HasForeignKey(e => e.ParentProductionPlanId);
            builder.HasMany<ProductionRequirement>().WithOne().HasForeignKey(e => e.ProductionPlanId);
        }
    }
}
