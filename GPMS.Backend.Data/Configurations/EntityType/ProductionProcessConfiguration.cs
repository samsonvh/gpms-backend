using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Configurations.EntityType
{
    internal class ProductionProcessConfiguration : IEntityTypeConfiguration<ProductProductionProcess>
    {
        public void Configure(EntityTypeBuilder<ProductProductionProcess> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.OrderNumber);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);

            builder.HasOne(e => e.Product).WithMany(e => e.ProductionProcesses).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
