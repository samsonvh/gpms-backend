using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Data;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Data.Repositories.Implementation;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
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
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IStaffService, StaffService>();  
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISemiFinishedProductService, SemiFinishProductService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ISpecificationService, SpecificationService>();
            services.AddScoped<IQualityStandardService, QualityStandardService>();
            services.AddScoped<IMeasurementService, MeasurementService>();
            services.AddScoped<IBillOfMaterialService, BillOfMaterialService>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddScoped<IStepService, StepService>();
            services.AddScoped<IStepIOService, StepIOService>();

            //Add IValidator
            services.AddTransient<IValidator<LoginInputDTO>,LoginInputDTOValidator>();
            services.AddTransient<IValidator<AccountInputDTO>, AccountInputDTOValidator>();
            services.AddTransient<IValidator<ProductInputDTO>,ProductInputDTOValidator>();
            services.AddTransient<IValidator<ProductDefinitionInputDTO>,ProductDefinitionInputDTOValidator>();
            services.AddTransient<IValidator<CategoryInputDTO>,CategoryInputDTOValidator>();
            services.AddTransient<IValidator<SemiFinishedProductInputDTO>,SemiFinishedProductInputDTOValidator>();
            services.AddTransient<IValidator<MaterialInputDTO>,MaterialInputDTOValidator>();
            services.AddTransient<IValidator<SpecificationInputDTO>,SpecificationInputDTOValidator>();
            services.AddTransient<IValidator<MeasurementInputDTO>,MeasurementInputDTOValidator>();
            services.AddTransient<IValidator<BOMInputDTO>,BOMInputDTOValidator>();
            services.AddTransient<IValidator<QualityStandardInputDTO>,QualityStandardInputDTOValidator>();
            services.AddTransient<IValidator<ProcessInputDTO>,ProcessInputDTOValidator>();
            services.AddTransient<IValidator<StepInputDTO>,StepInputDTOValidator>();
            services.AddTransient<IValidator<StepIOInputDTO>,StepIOInputDTOValidator>();

            //Add Mapper
            services.AddAutoMapper(typeof(AutoMapperProfileUtils));
        }
    }
}