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
                .ForMember(dto => dto.FullName, opt => opt.MapFrom(account => account.Staff.FullName))
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
            CreateMap<Product, ProductDTO>()
                .ForMember(productDTO => productDTO.Category, opt => opt.MapFrom(product => product.Category))
                .ForMember(productDTO => productDTO.SemiFinishedProducts, opt => opt.MapFrom(product => product.SemiFinishedProducts))
                .ForMember(productDTO => productDTO.Processes, opt => opt.MapFrom(product => product.ProductionProcesses))
                .ForMember(productDTO => productDTO.Specifications, opt => opt.MapFrom(product => product.Specifications));

            CreateMap<Product, ChangeStatusResponseDTO<Product, ProductStatus>>();
            CreateMap<Product, ProductListingDTO>()
            .ForMember(productListingDTO => productListingDTO.ImageURLs, options => options.Ignore())
            .ForMember(productListingDTO => productListingDTO.Sizes, options => options.Ignore())
            .ForMember(productListingDTO => productListingDTO.Colors, options => options.Ignore());

            //SemiFinishedProduct
            CreateMap<SemiFinishedProductInputDTO, SemiFinishedProduct>();
            CreateMap<SemiFinishedProduct, SemiFinishedProductDTO>();
            //Material
            CreateMap<MaterialInputDTO, Material>();
            CreateMap<Material, MaterialDTO>().ReverseMap();
            CreateMap<Material, MaterialListingDTO>().ReverseMap();
            //Specification
            CreateMap<SpecificationInputDTO, ProductSpecification>()
            .ForMember(specification => specification.Measurements, options => options.Ignore())
            .ForMember(specification => specification.QualityStandards, options => options.Ignore());
            CreateMap<ProductSpecification, SpecificationDTO>()
                .ForMember(specificationDTO => specificationDTO.Measurements, opt => opt.MapFrom(productSpecification => productSpecification.Measurements))
                .ForMember(specificationDTO => specificationDTO.QualityStandards, opt => opt.MapFrom(productSpecification => productSpecification.QualityStandards))
                .ForMember(specificationDTO => specificationDTO.BillOfMaterials, opt => opt.MapFrom(productSpecification => productSpecification.BillOfMaterials));
            //Measurement
            CreateMap<MeasurementInputDTO, Measurement>();
            CreateMap<Measurement, MeasurementDTO>();
            //Bill Of Material
            CreateMap<BOMInputDTO, BillOfMaterial>();
            CreateMap<BillOfMaterial, BOMDTO>()
                .ForMember(bomDTO => bomDTO.Material, opt => opt.MapFrom(bom => bom.Material));
            //Quality Standard
            CreateMap<QualityStandardInputDTO, QualityStandard>();
            CreateMap<QualityStandard, QualityStandardDTO>()
                .ForMember(qualityStandardDTO => qualityStandardDTO.ImageURL, options => options.Ignore());
            //Process 
            CreateMap<ProcessInputDTO, ProductProductionProcess>()
            .ForMember(process => process.Steps, options => options.Ignore());
            CreateMap<ProductProductionProcess, ProcessDTO>()
                .ForMember(dest => dest.ProductionProcessSteps, opt => opt.MapFrom(src => src.Steps));
            //Step
            CreateMap<StepInputDTO, Data.Models.Products.ProductionProcesses.ProductionProcessStep>();
            CreateMap<ProductionProcessStep, ProductionProcessStepDTO>()
                .ForMember(productionProcessStepDTO => productionProcessStepDTO.ProductionProcessStepIOs, opt =>
                                opt.MapFrom(productionProcessstep => productionProcessstep.ProductionProcessStepIOs));

            //ProductionPlan
            CreateMap<ProductionPlanInputDTO, ProductionPlan>();
            CreateMap<ProductionPlan, ProductionPlanDTO>()
                .ForMember(dest => dest.ProductionRequirementDTOs, opt => opt.MapFrom(src => src.ProductionRequirements)); ;

            CreateMap<ProductionProcessStepIO, ProductionProcessStepIODTO>();

            //Produciton Requirement
            CreateMap<ProductionRequirementInputDTO, ProductionRequirement>();
            CreateMap<ProductionRequirement, ProductionRequirementDTO>()
                .ForMember(dest => dest.ProductSpecificationDTO, opt => opt.MapFrom(src => src.ProductSpecification))
                .ForMember(dest => dest.ProductionEstimationDTOs, opt => opt.MapFrom(src => src.ProductionEstimations));
            //Production Estimation
            CreateMap<ProductionEstimation, ProductionEstimationDTO>()
                .ForMember(dest => dest.ProductionSeriesDTOs, opt => opt.MapFrom(src => src.ProductionSeries));

            //Production Series
            CreateMap<ProductionSeriesInputDTO, ProductionSeries>();
            CreateMap<ProductionSeries, ProductionSeriesDTO>();
            CreateMap<StepIOInputDTO, ProductionProcessStepIO>().ReverseMap();
        }
    }
}