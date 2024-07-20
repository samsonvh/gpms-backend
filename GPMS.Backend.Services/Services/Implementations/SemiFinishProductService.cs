using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class SemiFinishProductService : ISemiFinishedProductService
    {
        private readonly IGenericRepository<SemiFinishedProduct> _semiFinishedProductRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IValidator<SemiFinishedProductInputDTO> _semiFinishedProductValidator;
        private readonly IMapper _mapper;

        public SemiFinishProductService(
            IGenericRepository<SemiFinishedProduct> semiFinishedProductRepository,
            IGenericRepository<Product> productRepository,
            IValidator<SemiFinishedProductInputDTO> semiFinishedProductValidator,
            IMapper mapper
            )
        {
            _semiFinishedProductRepository = semiFinishedProductRepository;
            _semiFinishedProductValidator = semiFinishedProductValidator;
            _mapper = mapper;
        }

        public Task<CreateUpdateResponseDTO<SemiFinishedProduct>> Add(SemiFinishedProductInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<SemiFinishedProductInputDTO> inputDTOs, Guid? productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CreateUpdateResponseDTO<SemiFinishedProduct>>> AddList(List<SemiFinishedProductInputDTO> inputDTOs, Guid productId)
        {
            ServiceUtils.ValidateInputDTOList<SemiFinishedProductInputDTO, SemiFinishedProduct>
            (inputDTOs, _semiFinishedProductValidator);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<SemiFinishedProductInputDTO,SemiFinishedProduct>
            (inputDTOs,"Code");
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<SemiFinishedProductInputDTO, SemiFinishedProduct>
            (inputDTOs,_semiFinishedProductRepository,"Code","Code");
            List<CreateUpdateResponseDTO<SemiFinishedProduct>> responses = new List<CreateUpdateResponseDTO<SemiFinishedProduct>>();
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

        public Task<List<SemiFinishedProductListingDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<SemiFinishedProduct>> Update(SemiFinishedProductInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<SemiFinishedProductInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
    }
}