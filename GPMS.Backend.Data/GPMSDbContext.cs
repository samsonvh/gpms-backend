using GPMS.Backend.Data.Models.Staffs;
using Microsoft.EntityFrameworkCore;
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
        public GPMSDbContext() { }
        public GPMSDbContext(DbContextOptions<GPMSDbContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.UseSqlServer("Server=.\\SQLSERVER_22;Database=GPMS;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=GPMS;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
