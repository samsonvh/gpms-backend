using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class ProductionProcessStepIOConfiguration : IEntityTypeConfiguration<ProductionProcessStepIO>
    {
        public void Configure(EntityTypeBuilder<ProductionProcessStepIO> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Quantity).IsRequired(false);
            builder.Property(e => e.Consumption).IsRequired(false);
            builder.Property(e => e.IsProduct);
            builder.Property(e => e.Type);

            builder.HasOne<SemiFinishedProduct>().WithMany().HasForeignKey(e => e.SemiFinishedProductId).IsRequired(false);
            builder.HasOne<Material>().WithMany().HasForeignKey(e => e.MaterialId).IsRequired(false);
            builder.HasOne<ProductionProcessStep>().WithMany().HasForeignKey(e => e.ProductionProcessStepId);

            builder.HasMany<ProductionProcessStepIOResult>().WithOne().HasForeignKey(e => e.StepIOId);
        }
    }
}
