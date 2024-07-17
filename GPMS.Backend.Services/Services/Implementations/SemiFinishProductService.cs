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
            ValidateSemiFinishedProductInputDTOList(inputDTOs);
            await CheckSemiFinishedProductCodeDuplicate(inputDTOs);
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

        public Task<SemiFinishedProductListingDTO> GetAll()
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
        private void ValidateSemiFinishedProductInputDTOList(List<SemiFinishedProductInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (SemiFinishedProductInputDTO inputDTO in inputDTOs)
            {
                ValidationResult validationResult = _semiFinishedProductValidator.Validate(inputDTO);
                if (!validationResult.IsValid)
                {
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errors.Add(new FormError
                        {
                            ErrorMessage = validationFailure.ErrorMessage,
                            Property = validationFailure.PropertyName
                        });
                    }
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Semi finished product list invalid", errors);
            }
        }
        private async Task CheckSemiFinishedProductCodeDuplicate(List<SemiFinishedProductInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (SemiFinishedProductInputDTO semiFinishedProductInputDTO in inputDTOs)
            {
                SemiFinishedProduct codeDuplicatedSemiFinishedProduct =
                await _semiFinishedProductRepository.Search(semiFinishedProduct => semiFinishedProduct.Code.Equals(semiFinishedProductInputDTO.Code))
                                                    .FirstOrDefaultAsync();
                if (codeDuplicatedSemiFinishedProduct != null)
                {
                    errors.Add(new FormError
                    {
                        Property = typeof(SemiFinishedProductInputDTO).GetProperty("Code").Name,
                        ErrorMessage = $"Semi finished product with code : {semiFinishedProductInputDTO.Code} duplicated"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Code duplicate in semi finish product list", errors);
            }
        }
    }
}