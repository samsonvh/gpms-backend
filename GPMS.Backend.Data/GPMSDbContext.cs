using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Models.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data
{
    public class GPMSDbContext : DbContext
    {
        public GPMSDbContext( ) {
           
        }
        public GPMSDbContext(DbContextOptions<GPMSDbContext> options) : base(options) {}

        public DbSet<Department> Departments { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<ProductionEstimation> ProductionEstimation { get; set; }
        public DbSet<ProductionPlan> ProductionPlan { get; set; }
        public DbSet<ProductionRequirement> ProductionRequirement { get; set; }
        public DbSet<ProductionSeries> ProductionSerie { get; set; }
        public DbSet<ProductionProcessStep> ProductionProcessStep { get; set; }
        public DbSet<ProductionProcessStepIO> ProductionProcessStepIO { get; set; }
        public DbSet<ProductProductionProcess> ProductProductionProcess { get; set; }
        public DbSet<BillOfMaterial> BillOfMaterial { get; set; }
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<ProductSpecification> ProductSpecification { get; set; }
        public DbSet<QualityStandard> QualityStandard { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<SemiFinishedProduct> SemiFinishedProduct { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<InspectionRequest> InspectionRequest { get; set; }
        public DbSet<WarehouseRequest> WarehouseRequest { get; set; }
        public DbSet<FaultyProduct> FaultyProduct { get; set; }
        public DbSet<InspectionRequestResult> InspectionRequestResult { get; set; }
        public DbSet<ProductFault> ProductFault { get; set; }
        public DbSet<ProductionProcessStepIOResult> ProductionProcessStepIOResult { get; set; }
        public DbSet<ProductionProcessStepResult> ProductionProcessStepResult { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<WarehouseTicket> WarehouseTicket { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=.\\SQLSERVER_22;Database=GPMS;Trusted_Connection=True;");
            // optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=GPMS;Trusted_Connection=True;TrustServerCertificate=True");
            // optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
