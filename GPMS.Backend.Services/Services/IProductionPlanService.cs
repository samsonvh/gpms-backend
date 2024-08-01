using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.PageRequests;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;

namespace GPMS.Backend.Services.Services
{
    public interface IProductionPlanService : IBaseService<ProductionPlanInputDTO, 
                                    CreateUpdateResponseDTO<ProductionPlan>, ProductionPlanListingDTO, ProductionPlanDTO, ProductionPlanFilterModel> 
    {

        Task<DefaultPageResponseListingDTO<ProductionPlanListingDTO>> GetAll(ProductionPlanFilterModel productionPlanFilterModel);


        Task<List<CreateUpdateResponseDTO<ProductionPlan>>> AddAnnualProductionPlanList(List<ProductionPlanInputDTO> inputDTOs);
        Task<List<CreateUpdateResponseDTO<ProductionPlan>>> AddChildProductionPlanList(List<ProductionPlanInputDTO> inputDTOs);
        Task<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>> ChangeStatus(Guid id, string productionPlanStatus);
        Task<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>> StartProductionPlan(Guid id, string productionPlanStatus);
    }
}
