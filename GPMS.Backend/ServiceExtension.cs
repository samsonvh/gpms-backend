using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Data;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Data.Repositories.Implementation;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
using GPMS.Backend.Services.Utils;
using GPMS.Backend.Services.Utils.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend
{
    public static class ServiceExtension
    {
        public static void ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {
            //Init JWT Util 1 time for using Configuration
            JWTUtils.Initialize(configuration);

            //config DBContext
            services.AddDbContext<GPMSDbContext>(options =>
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));

            //Add Auto Mapper
            services.AddAutoMapper(typeof(AutoMapperProfileUtils).Assembly);

            //Config Authentication with JWT
            services.AddAuthentication(config =>
            {
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (
                        Encoding.UTF8.GetBytes(configuration["JWT:Secret_Key"])
                    )
                };
            });

            //Add Service 
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAccountService, AccountService>();

            //Add IValidator
            services.AddTransient<IValidator<LoginInputDTO>,LoginInputDTOValidator>();

            //Add Mapper
            services.AddAutoMapper(typeof(AutoMapperProfileUtils));
        }
    }
}