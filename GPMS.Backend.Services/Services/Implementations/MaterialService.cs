using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class MaterialService : IMaterialService
    {
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IValidator<MaterialInputDTO> _materialValidator;
        private readonly IMapper _mapper;

        public MaterialService(
            IGenericRepository<Material> materialRepository,
            IValidator<MaterialInputDTO> materialValidator,
            IMapper mapper)
        {
            _materialRepository = materialRepository;
            _materialValidator = materialValidator;
            _mapper = mapper;
        }

        public Task<CreateUpdateResponseDTO<Material>> Add(MaterialInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<MaterialInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CreateUpdateResponseDTO<Material>>> AddList(List<MaterialInputDTO> inputDTOs)
        {
            ServiceUtils.ValidateInputDTOList<MaterialInputDTO, Material>(inputDTOs, _materialValidator);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<MaterialInputDTO,Material>
            (inputDTOs,"Code");
            await CheckMaterialCodeInMaterialInInputDTOList(inputDTOs);
            List<CreateUpdateResponseDTO<Material>> responses = new List<CreateUpdateResponseDTO<Material>>();
            foreach (MaterialInputDTO materialInputDTO in inputDTOs)
            {
                Material material = _mapper.Map<Material>(materialInputDTO);
                CreateUpdateResponseDTO<Material> response = new CreateUpdateResponseDTO<Material>
                {
                    Code = materialInputDTO.Code
                };
                if (materialInputDTO.IsNew)
                {
                    _materialRepository.Add(material);
                    response.Id = material.Id;
                }
                else
                {
                    Material existedMaterial = await _materialRepository
                    .Search(material => material.Code.Equals(materialInputDTO.Code))
                    .FirstOrDefaultAsync();
                    response.Id = existedMaterial.Id;
                }

                responses.Add(response);
            }
            return responses;
        }

        private async Task CheckMaterialCodeInMaterialInInputDTOList(List<MaterialInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (MaterialInputDTO materialInputDTO in inputDTOs)
            {
                Material existedMaterial = await _materialRepository
                                            .Search(material => material.Code.Equals(materialInputDTO.Code))
                                            .FirstOrDefaultAsync();
                if (materialInputDTO.IsNew && existedMaterial != null)
                {
                    errors.Add(new FormError
                    {
                        Property = "Code",
                        ErrorMessage = $"{typeof(Material).Name} with Code : {materialInputDTO.Code} duplicated"
                    });
                }
                else if (!materialInputDTO.IsNew && existedMaterial == null)
                {
                    errors.Add(new FormError
                    {
                        Property = materialInputDTO.GetType().GetProperty("Code").Name,
                        ErrorMessage = $"{typeof(Material).Name} with Code : {materialInputDTO.Code} is not existed in system"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, $"Code invalid in {typeof(MaterialInputDTO).Name} list", errors);
            }
        }

        public async Task<MaterialDTO> Details(Guid id)
        {
            var material = await _materialRepository
                .Search(material => material.Id == id)
                .FirstOrDefaultAsync();
            if(material  == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Material not found");
            }
            return _mapper.Map<MaterialDTO>(material);  
        }

        public async Task<List<MaterialListingDTO>> GetAll()
        {
            return _mapper.Map<List<MaterialListingDTO>>(await _materialRepository.GetAll().ToListAsync());
        }

        public Task<CreateUpdateResponseDTO<Material>> Update(MaterialInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<MaterialInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
    }
}