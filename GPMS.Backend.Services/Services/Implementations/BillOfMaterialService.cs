using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.PageRequests;
using GPMS.Backend.Services.Utils;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class BillOfMaterialService : IBillOfMaterialService
    {
        private readonly IGenericRepository<BillOfMaterial> _billOfMaterialRepository;
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IValidator<BOMInputDTO> _billOfMaterialValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        public BillOfMaterialService(
            IGenericRepository<BillOfMaterial> billOfMaterialRepository,
            IGenericRepository<Material> materialRepository,
            IValidator<BOMInputDTO> billOfMaterialValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper
            )
        {
            _billOfMaterialRepository = billOfMaterialRepository;
            _materialRepository = materialRepository;
            _billOfMaterialValidator = billOfMaterialValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
        }

        public Task<BOMDTO> Add(BOMInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<BOMInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<BOMInputDTO> inputDTOs, Guid specificationId)
        {
            ServiceUtils.ValidateInputDTOList<BOMInputDTO, BillOfMaterial>
                (inputDTOs, _billOfMaterialValidator, _entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<BOMInputDTO,BillOfMaterial>
                (inputDTOs,"MaterialId",_entityListErrorWrapper);
            CheckMaterialExist(inputDTOs);
            foreach (BOMInputDTO inputDTO in inputDTOs)
            {
                BillOfMaterial billOfMaterial = _mapper.Map<BillOfMaterial>(inputDTO);
                billOfMaterial.ProductSpecificationId = specificationId;
                billOfMaterial.MaterialId = inputDTO.MaterialId;
                _billOfMaterialRepository.Add(billOfMaterial);
            }
        }

        private async void CheckMaterialExist(List<BOMInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (var inputDTO in inputDTOs)
            {
                var materialExist = _materialRepository.Details(inputDTO.MaterialId);
                if (materialExist == null)
                {
                    errors.Add
                    (
                        new FormError
                        {
                            EntityOrder = inputDTOs.IndexOf(inputDTO),
                            ErrorMessage = $"Material with MaterialId: {inputDTO.MaterialId} not exist",
                            Property = "MaterialId"
                        }
                    );
                }
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<Material>(errors,_entityListErrorWrapper);
            }
        }

        public Task<BOMDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DefaultPageResponseListingDTO<BOMListingDTO>> GetAll(BaseFilterModel filter)
        {
            throw new NotImplementedException();
        }

        public Task<BOMDTO> Update(Guid id, BOMInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        // public async Task<List<BOMListingDTO>> GetAll()
        // {
        //     var query = _billOfMaterialRepository.GetAll();
        //     query = Filters(query, accountFilterModel);
        //     query = query.SortBy<Account>(accountFilterModel);
        //     int totalItem = query.Count();
        //     int pageCount = totalItem / accountFilterModel.PageSize;
        //     if (totalItem % accountFilterModel.PageSize > 0)
        //     {
        //         pageCount += 1;
        //     }
        //     query = query.PagingEntityQuery<Account>(accountFilterModel);
        //     var accounts = await query.ProjectTo<AccountListingDTO>(_mapper.ConfigurationProvider)
        //                                 .ToListAsync();
        //     return new DefaultPageResponseListingDTO<AccountListingDTO>
        //     {
        //         Data = accounts,
        //         PageIndex = accountFilterModel.PageIndex,
        //         PageSize = accountFilterModel.PageSize,
        //         TotalItem = totalItem,
        //         PageCount = pageCount
        //     };
        // }


        public Task UpdateList(List<BOMInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
    }
}