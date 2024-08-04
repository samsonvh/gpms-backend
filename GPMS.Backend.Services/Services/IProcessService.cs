using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;

namespace GPMS.Backend.Services.Services
{
    public interface IProcessService : IBaseService<ProcessInputDTO, CreateUpdateResponseDTO<ProductProductionProcess>,
    ProcessListingDTO,ProcessDTO,ProcessFilterModel>
    {
        Task AddList(List<ProcessInputDTO> inputDTOs, Guid productId,
        List<Guid> materialIds, 
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinishedProductCodes);
        Task<DefaultPageResponseListingDTO<ProcessListingDTO>> GetAllProcessOfProduct(Guid productId, ProcessFilterModel processFilterModel);
    }
}