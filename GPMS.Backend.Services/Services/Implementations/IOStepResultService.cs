using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Results;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class IOStepResultService : IIOStepResultService
    {
        private readonly IGenericRepository<ProductionProcessStepIOResult> _IOResultRepository;
        private readonly IGenericRepository<ProductionProcessStepIO> _stepIORepository;
        private readonly IGenericRepository<ProductionProcessStep> _stepRepository;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly IMapper _mapper;

        public IOStepResultService(
            IGenericRepository<ProductionProcessStepIOResult> IOResultRepository,
            IMapper mapper,
            IGenericRepository<ProductionProcessStepIO> stepIORepository,
            IGenericRepository<ProductionProcessStep> stepRepository,
            EntityListErrorWrapper entityListErrorWrapper)
        {
            _IOResultRepository = IOResultRepository;
            _stepRepository = stepRepository;
            _mapper = mapper;
            _stepIORepository = stepIORepository;
            _entityListErrorWrapper = entityListErrorWrapper;
        }
        #region Add List
        public async Task AddList(Guid stepId, Guid stepResultId, List<InputOutputResultInputDTO> inputDTOs)
        {
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<InputOutputResultInputDTO, ProductionProcessStepIOResult>
                (inputDTOs, "StepInputOutputId", _entityListErrorWrapper);
            CheckStepIOIdsExistInStepId(stepId, inputDTOs);
            ValidateConsumptionAndQuantity(inputDTOs);

        }

        private async void CheckStepIOIdsExistInStepId(Guid stepId, List<InputOutputResultInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (var inputDTO in inputDTOs)
            {
                var existStepIO = await _stepIORepository.Search(stepIO => stepIO.Id.Equals(inputDTO.StepInputOutputId))
                                                        .Include(stepIO => stepIO.ProductionProcessStep)
                                                        .FirstOrDefaultAsync();

                if (existStepIO.ProductionProcessStepId.Equals(stepId))
                {
                    errors.Add
                    (new FormError
                    {
                        EntityOrder = inputDTOs.IndexOf(inputDTO) + 1,
                        ErrorMessage = "Step Input Output is not in Step of current process",
                        Property = "StepInputOutputId"
                    });
                }
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionProcessStepIOResult>(errors, _entityListErrorWrapper);
            }
        }

        private async void ValidateConsumptionAndQuantity(List<InputOutputResultInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (var inputDTO in inputDTOs)
            {
                var existedstepIO = await _stepIORepository
                .Search(stepIO => stepIO.Id.Equals(inputDTO.StepInputOutputId))
                .FirstOrDefaultAsync();
                if (existedstepIO == null)
                {
                    errors.Add(new FormError
                    {
                        EntityOrder = inputDTOs.IndexOf(inputDTO) + 1,
                        ErrorMessage = "Step Input Output Not Found",
                        Property = "StepInputOutputId"
                    });
                }
                else
                {
                    if (existedstepIO.MaterialId != null)
                    {
                        if (!inputDTO.Consumption.HasValue)
                        {
                            errors.Add(new FormError
                            {
                                EntityOrder = inputDTOs.IndexOf(inputDTO) + 1,
                                ErrorMessage = "Consumption is required when step input output is material",
                                Property = "Consumption"
                            });
                        }
                        else if (inputDTO.Quantity.HasValue)
                        {
                            errors.Add(new FormError
                            {
                                EntityOrder = inputDTOs.IndexOf(inputDTO) + 1,
                                ErrorMessage = "Quantity must null when step input output is material",
                                Property = "Quantity"
                            });
                        }

                    }
                    else if (existedstepIO.SemiFinishedProductId.HasValue || existedstepIO.IsProduct)
                    {
                        if (!inputDTO.Quantity.HasValue)
                        {
                            errors.Add(new FormError
                            {
                                EntityOrder = inputDTOs.IndexOf(inputDTO) + 1,
                                ErrorMessage = "Quantity is required when step input output is semi finished product or product",
                                Property = "Quantity"
                            });
                        }
                        else if (inputDTO.Consumption.HasValue)
                        {
                            errors.Add(new FormError
                            {
                                EntityOrder = inputDTOs.IndexOf(inputDTO) + 1,
                                ErrorMessage = "Consumption must null when step input output is semi finished product or product",
                                Property = "Consumption"
                            });
                        }
                    }
                }
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionProcessStepIOResult>(errors, _entityListErrorWrapper);
            }
        }
        #endregion
        public async Task<DefaultPageResponseListingDTO<IOResultListingDTO>> GetAllIOResultByStepResult(Guid stepResultId, IOResultFilterModel resultFilterModel)
        {
            var query = _IOResultRepository.GetAll().Where(ioResult => ioResult.StepResultId == stepResultId);
            query = query.SortBy<ProductionProcessStepIOResult>(resultFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery(resultFilterModel);
            var data = await query.ProjectTo<IOResultListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new DefaultPageResponseListingDTO<IOResultListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = resultFilterModel.Pagination.PageIndex,
                    PageSize = resultFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

    }
}
