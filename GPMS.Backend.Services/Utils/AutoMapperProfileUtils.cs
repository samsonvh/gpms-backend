using AutoMapper;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;

namespace GPMS.Backend.Services.Utils
{
    public class AutoMapperProfileUtils : Profile
    {
        public AutoMapperProfileUtils()
        {
            //account
            CreateMap<Account, AccountListingDTO>()
                .ForMember(dto => dto.Poition, opt => opt.MapFrom(account => account.Staff.Position));
            CreateMap<AccountInputDTO, Account>();
            CreateMap<Account, AccountDTO>()
                .ForMember(dto => dto.FullName, opt => opt.MapFrom(account =>  account.Staff.FullName))
                .ForMember(dto => dto.Position, opt => opt.MapFrom(account => account.Staff.Position));
            CreateMap<Account, CreateUpdateResponseDTO<Account>>();
            CreateMap<Account, ChangeStatusResponseDTO<Account, AccountStatus>>();

            //staff
            CreateMap<StaffInputDTO, Staff>()
                .ForMember(staff => staff.FullName, opt => opt.MapFrom(dto => dto.FullName))
                .ForMember(staff => staff.Position, opt => opt.MapFrom(dto => dto.Position))
                .ForMember(staff => staff.DepartmentId, opt => opt.MapFrom(dto => dto.DepartmentId))
                .ForMember(staff => staff.Account, opt => opt.Ignore());
            CreateMap<Staff, StaffListingDTO>()
                .ForMember(staffListingDTO => staffListingDTO.Department, options 
                            => options.MapFrom(staff => staff.Department.Name));
            CreateMap<Staff, StaffDTO>()
                .ForMember(dto => dto.DepartmentName, opt => opt.MapFrom(staff => staff.Department.Name));

            //department
            CreateMap<Department, DepartmentListingDTO>();
            CreateMap<Department, DepartmentDTO>()
                .ForMember(dto => dto.Staffs, opt => opt.MapFrom(deparment => deparment.Staffs));

            //category
            CreateMap<CategoryInputDTO, Category>();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            //Product 
            CreateMap<ProductDefinitionInputDTO, Product>()
            .ForMember(product => product.Category, options => options.Ignore())
            .ForMember(product => product.SemiFinishedProducts, options => options.Ignore());
            CreateMap<Product, ChangeStatusResponseDTO<Product, ProductStatus>>();
            CreateMap<Product,ProductListingDTO>()
            .ForMember(productListingDTO => productListingDTO.ImageURLs, options => options.Ignore())
            .ForMember(productListingDTO => productListingDTO.Sizes, options => options.Ignore())
            .ForMember(productListingDTO => productListingDTO.Colors, options => options.Ignore());
            CreateMap<Product, ProductDTO>();
            //SemiFinishedProduct
            CreateMap<SemiFinishedProductInputDTO, SemiFinishedProduct>();
            //Material
            CreateMap<MaterialInputDTO, Material>();
            CreateMap<Material,MaterialDTO>().ReverseMap();
            CreateMap<Material,MaterialListingDTO>().ReverseMap();
            //Specification
            CreateMap<SpecificationInputDTO, ProductSpecification>()
            .ForMember(specification => specification.Measurements, options => options.Ignore())
            .ForMember(specification => specification.QualityStandards, options => options.Ignore());
            CreateMap<ProductSpecification, SpecificationDTO>()
                .ForMember(dto => dto.Product, opt => opt.MapFrom(src => src.Product));
            //Measurement
            CreateMap<MeasurementInputDTO, Measurement>();
            //Bill Of Material
            CreateMap<BOMInputDTO, BillOfMaterial>();
            //Quality Standard
            CreateMap<QualityStandardInputDTO, QualityStandard>();
            //Process 
            CreateMap<ProcessInputDTO, ProductProductionProcess>()
            .ForMember(process => process.Steps, options => options.Ignore());
            //Step
            CreateMap<StepInputDTO, ProductionProcessStep>();

            //ProductionPlan
            CreateMap<ProductionPlanInputDTO, ProductionPlan>();
            CreateMap<ProductionPlan, ProductionPlanDTO>()
                .ForMember(productionPlanDTO => productionPlanDTO.ChildProductionPlans, opt => opt.MapFrom(src => src.ChildProductionPlans))
                .ForMember(productionPlanDTO => productionPlanDTO.ParentProductionPlan, opt => opt.MapFrom(src => src.ParentProductionPlan))
                .ForMember(productionPlanDTO => productionPlanDTO.ProductionRequirements, opt => opt.MapFrom(src => src.ProductionRequirements))
                ;
            CreateMap<ProductionPlan, ChildProductionPlanDTO>();

            CreateMap<ProductionPlan, ParentProductionPlanDTO>();
            //Produciton Requirement
            CreateMap<ProductionRequirementInputDTO, ProductionRequirement>();
            CreateMap<ProductionRequirement, ProductionRequirementDTO>();
            //Production Estimation
            CreateMap<ProductionEstimation, ProductionEstimationDTO>();
            CreateMap<ProductionEstimationInputDTO, ProductionEstimation>();

            //Production Series
            CreateMap<ProductionSeriesInputDTO, ProductionSeries>();
            CreateMap<ProductionSeries, ProductionSeriesDTO>();
            CreateMap<StepIOInputDTO, ProductionProcessStepIO>().ReverseMap();
        }
    }
}