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
    public interface IProductionSeriesService
    {
        Task AddList(List<ProductionSeriesInputDTO> inputDTOs, Guid productionEstimationId);
        Task<DefaultPageResponseListingDTO<ProductionSeriesListingDTO>> GetAllSeriesOfEstimation(Guid estimationId, ProductionSeriesFilterModel productionSeriesFilterModel);
        Task<DefaultPageResponseListingDTO<ProductionSeriesListingDTO>> GetAllSeriesByRequirementIdAndDayNumber
            (Guid requirementId,ProductionSeriesFilterModel productionSeriesFilterModel);
    }
}
