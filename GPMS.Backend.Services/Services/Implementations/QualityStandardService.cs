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
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class QualityStandardService : IQualityStandardService
    {
        private readonly IGenericRepository<QualityStandard> _qualityStandardRepository;
        private readonly IValidator<QualityStandardInputDTO> _qualityStandardValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly QualityStandardImagesTempWrapper _qualityStandardImagesTempWrapper;
        public QualityStandardService(
            IGenericRepository<QualityStandard> qualityStandardRepository,
            IValidator<QualityStandardInputDTO> qualityStandardValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper,
            QualityStandardImagesTempWrapper qualityStandardImagesTempWrapper
            )
        {
            _qualityStandardRepository = qualityStandardRepository;
            _qualityStandardValidator = qualityStandardValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
            _qualityStandardImagesTempWrapper = qualityStandardImagesTempWrapper;
        }

        public Task<CreateUpdateResponseDTO<QualityStandard>> Add(QualityStandardInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task AddList(List<QualityStandardInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<QualityStandardInputDTO> inputDTOs, Guid specificationId, List<CreateUpdateResponseDTO<Material>> materialCodeList)
        {
            ServiceUtils.ValidateInputDTOList<QualityStandardInputDTO, QualityStandard>
                (inputDTOs, _qualityStandardValidator, _entityListErrorWrapper);
            ServiceUtils.CheckForeignEntityCodeInInputDTOListExistedInForeignEntityCodeList<QualityStandardInputDTO, QualityStandard, Material>
                (inputDTOs.Where(inputDTO => !inputDTO.MaterialCode.IsNullOrEmpty()).ToList(),
            materialCodeList, "MaterialCode", _entityListErrorWrapper);
            foreach (QualityStandardInputDTO qualityStandardInputDTO in inputDTOs)
            {
                QualityStandard qualityStandard = _mapper.Map<QualityStandard>(qualityStandardInputDTO);
                qualityStandard.ProductSpecificationId = specificationId;
                if (!qualityStandardInputDTO.MaterialCode.IsNullOrEmpty())
                {
                    CreateUpdateResponseDTO<Material> materialCode = materialCodeList
                    .FirstOrDefault(materialCode => materialCode.Code.Equals(qualityStandardInputDTO.MaterialCode));
                    qualityStandard.MaterialId = materialCode.Id;
                }
                else qualityStandard.MaterialId = null;
                _qualityStandardRepository.Add(qualityStandard);
                if (qualityStandardInputDTO.Images.Count > 0)
                {
                    _qualityStandardImagesTempWrapper
                    .QualityStandardImagesTemps.Add(new QualityStandardImagesTemp
                    {
                        QualityStandardId = qualityStandard.Id,
                        Files = qualityStandardInputDTO.Images
                    });
                }
            }
        }

        public Task<QualityStandardDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<QualityStandardListingDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<QualityStandard>> Update(QualityStandardInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<QualityStandardInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
    }
}