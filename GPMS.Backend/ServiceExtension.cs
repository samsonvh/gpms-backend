using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Google.Cloud.Storage.V1;
using GPMS.Backend.Data;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Data.Repositories.Implementation;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
using GPMS.Backend.Services.Utils;
using GPMS.Backend.Services.Utils.Validators;
using GPMS.Backend.Services.Utils.Validators.ProductionPlan;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GPMS.Backend
{
    public static class ServiceExtension
    {
        public static void ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP.Net 8.0 - GPMS", Description = "APIs Service", Version = "v1" });
                c.DescribeAllParametersInCamelCase();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                 });
            });

            //Init JWT Util 1 time for using Configuration
            JWTUtils.Initialize(configuration);
            //config DBContext
            services.AddDbContext<GPMSDbContext>(options =>
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret_Key"]))
                };
            });

            //Add Auto Mapper
            services.AddAutoMapper(typeof(AutoMapperProfileUtils).Assembly);

            //Add Error List 
            services.AddScoped<EntityListErrorWrapper>();
            var serviceProvider = services.BuildServiceProvider();
            var entityListErrorWrapperService = serviceProvider.GetRequiredService<EntityListErrorWrapper>();

            //Add current login user
            services.AddScoped<CurrentLoginUserDTO>();

            //Add Quality Standard 
            services.AddScoped<QualityStandardImagesTempWrapper>();


            //Add Service 
            services.AddSingleton<IFirebaseStorageService>(service
            => new FirebaseStorageService(configuration, entityListErrorWrapperService, StorageClient.Create()));
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
            services.AddScoped<IWarehouseRequestService, WarehouseRequestService>();
            services.AddScoped<IWarehouseRequestRequirementService, WarehouseRequestRequirementService>();
            services.AddScoped<IProductionPlanService, ProductionPlanService>();
            services.AddScoped<IProductionRequirementService, ProductionRequirementService>();
            services.AddScoped<IProductionEstimationService, ProductionEstimationService>();
            services.AddScoped<IProductionSeriesService, ProductionSeriesService>();
            services.AddScoped<ISemiFinishedProductService, SemiFinishProductService>();
            services.AddScoped<IInspectionRequestService, InspectionRequestService>();
            services.AddScoped<IProductionSeriesService, ProductionSeriesService>();
            services.AddScoped<IStepResultService, StepResultService>();
            services.AddScoped<IIOStepResultService, IOStepResultService>();
            //Add IValidator
              //account
            services.AddTransient<IValidator<LoginInputDTO>, LoginInputDTOValidator>();
            services.AddTransient<IValidator<AccountInputDTO>, AccountInputDTOValidator>();
              //product
            services.AddTransient<IValidator<ProductInputDTO>, ProductInputDTOValidator>();
            services.AddTransient<IValidator<CategoryInputDTO>, CategoryInputDTOValidator>();
            services.AddTransient<IValidator<SemiFinishedProductInputDTO>, SemiFinishedProductInputDTOValidator>();
            services.AddTransient<IValidator<MaterialInputDTO>, MaterialInputDTOValidator>();
            services.AddTransient<IValidator<SpecificationInputDTO>, SpecificationInputDTOValidator>();
            services.AddTransient<IValidator<MeasurementInputDTO>, MeasurementInputDTOValidator>();
            services.AddTransient<IValidator<BOMInputDTO>, BOMInputDTOValidator>();
            services.AddTransient<IValidator<QualityStandardInputDTO>, QualityStandardInputDTOValidator>();
            services.AddTransient<IValidator<ProcessInputDTO>, ProcessInputDTOValidator>();
            services.AddTransient<IValidator<StepInputDTO>, StepInputDTOValidator>();
            services.AddTransient<IValidator<StepIOInputDTO>, StepIOInputDTOValidator>();
              //warehouse request
            services.AddTransient<IValidator<WarehouseRequestInputDTO>, WarehouseRequestValidator>();
            services.AddTransient<IValidator<WarehouseRequestRequirementInputDTO>, WarehouseRequestRequirementValidator>();
              //production plan
            services.AddTransient<IValidator<ProductionPlanInputDTO>, ProductionPlanInputDTOValidator>();
            services.AddTransient<IValidator<ProductionRequirementInputDTO>, ProductionRequirementInputDTOValidator>();
            services.AddTransient<IValidator<ProductionEstimationInputDTO>, ProductionEstimationValidator>();
            services.AddTransient<IValidator<ProductionSeriesInputDTO>, ProductionSeriesValidator>();

            //Add Mapper
            services.AddAutoMapper(typeof(AutoMapperProfileUtils));

            //Add Error List 
            services.AddScoped<EntityListErrorWrapper>();
            services.AddScoped<WarehouseRequestRequirementInputDTOWrapper>();

            //Add StepIOInputDTO List
            services.AddScoped<StepIOInputDTOWrapper>();

        }
    }
}