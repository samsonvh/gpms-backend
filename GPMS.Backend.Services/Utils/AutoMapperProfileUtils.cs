using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;

namespace GPMS.Backend.Services.Utils
{
    public class AutoMapperProfileUtils : Profile
    {
        public AutoMapperProfileUtils()
        {
            //account
            CreateMap<Account, AccountListingDTO>();
            //category
            CreateMap<CategoryInputDTO, Category>();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            //Product 
            CreateMap<ProductDefinitionInputDTO, Product>()
            .ForMember(product => product.Category, options => options.Ignore())
            .ForMember(product => product.SemiFinishedProducts, options => options.Ignore());
            //SemiFinishedProduct
            CreateMap<SemiFinishedProductInputDTO, SemiFinishedProduct>();
            //Material
            CreateMap<MaterialInputDTO,Material>();
            //Specification
            CreateMap<SpecificationInputDTO, ProductSpecification>()
            .ForMember(specification => specification.Measurements, options => options.Ignore())
            .ForMember(specification => specification.QualityStandards, options => options.Ignore());
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
            //Step IO
            CreateMap<StepIOInputDTO,ProductionProcessStepIO>();
        }
        
    }
}