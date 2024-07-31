using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IProductionEstimationService
    {
        Task AddList(List<ProductionEstimationInputDTO> inputDTOs, Guid productionRequirementId, Guid productionPlanId);
    }
}
