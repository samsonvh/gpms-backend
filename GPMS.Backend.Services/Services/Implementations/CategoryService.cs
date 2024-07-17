using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IValidator<CategoryInputDTO> _categoryValidator;
        private readonly IMapper _mapper;
        public CategoryService(IGenericRepository<Category> categoryRepository,
        IValidator<CategoryInputDTO> categoryValidator,
        IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _categoryValidator = categoryValidator;
            _mapper = mapper;
        }
        public async Task<CreateUpdateResponseDTO<Category>> Add(CategoryInputDTO inputDTO)
        {
            ValidationResult validationResult = _categoryValidator.Validate(inputDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Category Invalid", validationResult.Errors);
            }
            Category category = _mapper.Map<Category>(inputDTO);
            _categoryRepository.Add(category);
            return new CreateUpdateResponseDTO<Category>
            {
                Id = category.Id
            };
        }

        public async Task<CategoryDTO> DetailsByName(string name)
        {
            return _mapper.Map<CategoryDTO>
            (await _categoryRepository.Search(category => category.Name.Equals(name))
                                        .FirstOrDefaultAsync());
        }
    }
}