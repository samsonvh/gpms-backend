using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend
{
    public static class ServiceExtension
    {
        public static void ConfigureService (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GPMSDbContext>(options => 
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));
        }
    }
}