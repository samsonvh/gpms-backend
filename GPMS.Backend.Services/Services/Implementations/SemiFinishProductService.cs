using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.PageRequests;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class SemiFinishProductService : ISemiFinishedProductService
    {
        private readonly IGenericRepository<SemiFinishedProduct> _semiFinishedProductRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IValidator<SemiFinishedProductInputDTO> _semiFinishedProductValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;

        public SemiFinishProductService(
            IGenericRepository<SemiFinishedProduct> semiFinishedProductRepository,
            IGenericRepository<Product> productRepository,
            IValidator<SemiFinishedProductInputDTO> semiFinishedProductValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper
            )
        {
            _semiFinishedProductRepository = semiFinishedProductRepository;
            _semiFinishedProductValidator = semiFinishedProductValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
        }


        public async Task AddList(List<SemiFinishedProductInputDTO> inputDTOs, Guid? productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CreateUpdateResponseDTO<SemiFinishedProduct>>> AddList(List<SemiFinishedProductInputDTO> inputDTOs, Guid productId)
        {
            ServiceUtils.ValidateInputDTOList<SemiFinishedProductInputDTO, SemiFinishedProduct>
                (inputDTOs, _semiFinishedProductValidator,_entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<SemiFinishedProductInputDTO,SemiFinishedProduct>
                (inputDTOs,"Code",_entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<SemiFinishedProductInputDTO, SemiFinishedProduct>
                (inputDTOs,_semiFinishedProductRepository,"Code","Code",_entityListErrorWrapper);
            List<CreateUpdateResponseDTO<SemiFinishedProduct>> responses = 
                new List<CreateUpdateResponseDTO<SemiFinishedProduct>>();
            foreach (SemiFinishedProductInputDTO semiFinishedProductInputDTO in inputDTOs)
            {
                SemiFinishedProduct semiFinishedProduct = _mapper.Map<SemiFinishedProduct>(semiFinishedProductInputDTO);
                semiFinishedProduct.ProductId = productId;
                _semiFinishedProductRepository.Add(semiFinishedProduct);
                responses.Add(new CreateUpdateResponseDTO<SemiFinishedProduct>
                {
                    Code = semiFinishedProduct.Code,
                    Id = semiFinishedProduct.Id
                });
            }
            return responses;
        }

        public Task<SemiFinishedProductDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponseListingDTO<SemiFinishedProductListingDTO>> GetAll(SemiFinishedProductFilterModel semiFinishedProductFilterModel)
        {
            var query = _semiFinishedProductRepository.GetAll();
            query = Filters(query, semiFinishedProductFilterModel);
            query = query.SortBy<SemiFinishedProduct>(semiFinishedProductFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery<SemiFinishedProduct>(semiFinishedProductFilterModel);
            var semiFinishedProducts = await query.ProjectTo<SemiFinishedProductListingDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
            return new DefaultPageResponseListingDTO<SemiFinishedProductListingDTO>
            {
                Data = semiFinishedProducts,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = semiFinishedProductFilterModel.Pagination.PageIndex,
                    PageSize = semiFinishedProductFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        private IQueryable<SemiFinishedProduct> Filters(IQueryable<SemiFinishedProduct> query, SemiFinishedProductFilterModel semiFinishedProductFilterModel)
        {
            if (!semiFinishedProductFilterModel.Code.IsNullOrEmpty())
            {
                query = query.Where(account => account.Code.Contains(semiFinishedProductFilterModel.Code));
            }

            if (!semiFinishedProductFilterModel.Name.IsNullOrEmpty())
            {
                query = query
                .Where(product => product.Name.ToLower().Contains(semiFinishedProductFilterModel.Name.ToLower()));
            }
            return query;
        }


        public Task UpdateList(List<SemiFinishedProductInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponseListingDTO<SemiFinishedProductListingDTO>> GetAllSemiOfProduct(Guid productId, SemiFinishedProductFilterModel semiFinishedProductFilterModel)
        {
            IQueryable<SemiFinishedProduct> query = _semiFinishedProductRepository.GetAll();
            query = query.Where(process => process.ProductId == productId);
            query = Filters(query, semiFinishedProductFilterModel);
            int totalItem = query.Count();
            query = query.SortBy<SemiFinishedProduct>(semiFinishedProductFilterModel);
            query = query.PagingEntityQuery(semiFinishedProductFilterModel);
            var data = await query.ProjectTo<SemiFinishedProductListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new DefaultPageResponseListingDTO<SemiFinishedProductListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = semiFinishedProductFilterModel.Pagination.PageIndex,
                    PageSize = semiFinishedProductFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        public Task<SemiFinishedProductDTO> Add(SemiFinishedProductInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task<SemiFinishedProductDTO> Update(SemiFinishedProductInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }
    }
}