using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IProductionEstimationService
    {
        Task AddEstimationListForAnnualProductionPlan(List<ProductionEstimationInputDTO> inputDTOs, Guid productionRequirementId, Guid productionPlanId);
        Task AddEstimationListForChildProductionPlan(List<ProductionEstimationInputDTO> inputDTOs, Guid productionRequirementId, Guid productionPlanId);

        Task<DefaultPageResponseListingDTO<ProductionEstimationListingDTO>> GetAllEstimationOfRequirement(Guid requirementId, ProductionEstimationFilterModel productionEstimationFilterModel);

    }
}
