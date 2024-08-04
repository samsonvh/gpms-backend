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
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Enums.Statuses.Requests;
using GPMS.Backend.Data.Models.Warehouses;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;

namespace GPMS.Backend.Services.Utils
{
    public class AutoMapperProfileUtils : Profile
    {
        public AutoMapperProfileUtils()
        {
            //account
            CreateMap<Account, AccountListingDTO>()
                .ForMember(accountListingDTO => accountListingDTO.Position, options => options.MapFrom(account => account.Staff.Position));
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
            CreateMap<Product,CreateProductListingDTO>();
            //SemiFinishedProduct
            CreateMap<SemiFinishedProduct, SemiFinishedProductListingDTO>();
            CreateMap<SemiFinishedProductInputDTO, SemiFinishedProduct>();
            CreateMap<SemiFinishedProduct, SemiFinishedProductDTO>();
            //Material
            CreateMap<MaterialInputDTO, Material>()
                .ForMember(material => material.Id, options => options.Ignore());
            CreateMap<MaterialInputDTO, MaterialDTO>();
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
            CreateMap<ProductSpecification,CreateProductSpecificationListingDTO>();
            CreateMap<ProductSpecification, SpecificationListingDTO>();
            //Measurement
            CreateMap<MeasurementInputDTO, Measurement>();
            CreateMap<Measurement, MeasurementDTO>();
            CreateMap<Measurement, MeasurementListingDTO>();
            //Bill Of Material
            CreateMap<BOMInputDTO, BillOfMaterial>();
            CreateMap<BillOfMaterial, BOMDTO>()
                .ForMember(bomDTO => bomDTO.Material, opt => opt.MapFrom(bom => bom.Material));
            //Quality Standard
            CreateMap<QualityStandardInputDTO, QualityStandard>();
            CreateMap<QualityStandard, QualityStandardDTO>()
                .ForMember(qualityStandardDTO => qualityStandardDTO.ImageURL, options => options.Ignore());
            CreateMap<QualityStandard, QualityStandardListingDTO>()
                .ForMember(qualityStandardListingDTO => qualityStandardListingDTO.ImageURL, options => options.Ignore());
            //Process 
            CreateMap<ProcessInputDTO, ProductProductionProcess>()
            .ForMember(process => process.Steps, options => options.Ignore());
            CreateMap<ProductProductionProcess, ProcessDTO>()
                .ForMember(dest => dest.ProductionProcessSteps, opt => opt.MapFrom(src => src.Steps));
            CreateMap<ProductProductionProcess, ProcessListingDTO>();
            //Step
            CreateMap<StepInputDTO, Data.Models.Products.ProductionProcesses.ProductionProcessStep>();
            CreateMap<ProductionProcessStep, ProductionProcessStepDTO>()
                .ForMember(productionProcessStepDTO => productionProcessStepDTO.ProductionProcessStepIOs, opt =>
                                opt.MapFrom(productionProcessstep => productionProcessstep.ProductionProcessStepIOs));

            CreateMap<ProductionProcessStepIO, ProductionProcessStepIODTO>()
                .ForMember(productionProcessStepIODTO => productionProcessStepIODTO.SemiFinishedProductCode,
                                opt => opt.MapFrom(productionProcessStepIO => productionProcessStepIO.SemiFinishedProduct.Code))
                .ForMember(productionProcessStepIODTO => productionProcessStepIODTO.SemiFinishedProductName,
                                opt => opt.MapFrom(productionProcessStepIO => productionProcessStepIO.SemiFinishedProduct.Name));
            CreateMap<ProductionProcessStep, StepListingDTO>();
            //StepIO
            CreateMap<ProductionProcessStepIO, StepIOListingDTO>();
            //Production Plan
            CreateMap<ProductionPlanInputDTO, ProductionPlan>()
            .ForMember(productionPlan => productionPlan.Type, options => options.Ignore())
            .ForMember(productionPlan => productionPlan.ProductionRequirements, options => options.Ignore());
            CreateMap<ProductionPlan, ProductionPlanListingDTO>();
            CreateMap<ProductionPlan, ProductionPlanDTO>()

                .ForMember(dest => dest.ProductionRequirements, opt => opt.MapFrom(src => src.ProductionRequirements))

                .ForMember(productionPlanDTO => productionPlanDTO.CreatorName, opt => opt.MapFrom(productionPlan => productionPlan.Creator.FullName))
                .ForMember(productionPlanDTO => productionPlanDTO.ReviewerName, opt => opt.MapFrom(productionPlan => productionPlan.Reviewer.FullName))
                .ForMember(productionPlanDTO => productionPlanDTO.ProductionRequirements, opt => opt.MapFrom(productionPlan => productionPlan.ProductionRequirements))
                .ForMember(productionPlanDTO => productionPlanDTO.ParentProductionPlan, opt => opt.Ignore())
            .ForMember(productionPlanDTO => productionPlanDTO.ChildProductionPlans, opt => opt.Ignore());

            CreateMap<ProductionPlan, ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>>();
            //take 
            CreateMap<ProductionProcessStepIO, ProductionProcessStepIODTO>();

            //Produciton Requirement
            //CreateMap<ProductionRequirementInputDTO, ProductionRequirement>()
            //    .ForMember(productionRequirement => productionRequirement.ProductionEstimations, options => options.Ignore());
            CreateMap<ProductionRequirement, ProductionRequirementDTO>()
                //take
                .ForMember(dest => dest.ProductSpecification, opt => opt.MapFrom(src => src.ProductSpecification))
                .ForMember(dest => dest.ProductionEstimations, opt => opt.Ignore());

            //Production Estimation
            CreateMap<ProductionEstimation, ProductionEstimationDTO>()
                /*.ForMember(dest => dest.ProductionSeries, opt => opt.Ignore())*/;
            CreateMap<ProductionEstimationInputDTO, ProductionEstimation>()
                .ForMember(productionEstimation => productionEstimation.ProductionSeries, options => options.Ignore())
                .ForMember(productionEstimation => productionEstimation.Quarter, options => options.Ignore())
                .ForMember(productionEstimation => productionEstimation.Month, options => options.Ignore());
            //Production Series
            CreateMap<ProductionSeriesInputDTO, ProductionSeries>()
                .ForMember(productionSeries => productionSeries.CurrentProcess, options => options.Ignore())
                .ForMember(productionSeries => productionSeries.FaultyQuantity, options => options.Ignore());
            CreateMap<ProductionSeries, ProductionSeriesDTO>();
            CreateMap<StepIOInputDTO, ProductionProcessStepIO>().ReverseMap();

            //WarehouseRequest
            CreateMap<WarehouseRequestInputDTO, WarehouseRequest>();
            CreateMap<WarehouseRequest, CreateUpdateResponseDTO<WarehouseRequest>>();
            CreateMap<WarehouseRequest, WarehouseRequestDTO>()
                .ForMember(warehouseRequestDto => warehouseRequestDto.WarehouseTicket, options => options.MapFrom(warehouseRequest => warehouseRequest.WarehouseTicket));
            CreateMap<WarehouseRequest, WarehouseRequestListingDTO>();

            //WarehouseRequest Requirement
            CreateMap<WarehouseRequestRequirement, CreateUpdateResponseDTO<WarehouseRequestRequirement>>();
            CreateMap<WarehouseRequestRequirementInputDTO, WarehouseRequestRequirement>()
                .ForMember(warehouseRequestRequirement => warehouseRequestRequirement.ProductionRequirementId, opt => opt.MapFrom(dto => dto.ProducitonRequirementId));
            CreateMap<WarehouseRequest, ChangeStatusResponseDTO<WarehouseRequest, WarehouseRequestStatus>>();

            //warehouseTicket
            CreateMap<WarehouseTicket, WarehouseTicketDTO>()
                .ForMember(warehouseTicketDto => warehouseTicketDto.ProductSpeccificationId,
                                opt => opt.MapFrom(warehouseTicket => warehouseTicket.ProductSpecificationId));

            //inspection request
            CreateMap<InspectionRequestInputDTO, InspectionRequest>();

            CreateMap<InspectionRequest, InspectionRequestDTO>()
           .ForMember(dest => dest.ProductionSeriesCode, opt => opt.MapFrom(src => src.ProductionSeries.Code));
        }
    }
}