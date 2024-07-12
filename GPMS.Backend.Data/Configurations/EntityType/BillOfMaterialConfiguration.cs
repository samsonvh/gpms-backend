﻿using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    public class BillOfMaterialConfiguration : IEntityTypeConfiguration<BillOfMaterial>
    {
        public void Configure(EntityTypeBuilder<BillOfMaterial> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.SizeWidth);
            builder.Property(e => e.Consumption);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);

            builder.HasOne<Material>().WithMany().HasForeignKey(e => e.MaterialId);
            builder.HasOne<ProductSpecification>().WithMany().HasForeignKey(e => e.ProductSpecificationId);
        }
    }
}
