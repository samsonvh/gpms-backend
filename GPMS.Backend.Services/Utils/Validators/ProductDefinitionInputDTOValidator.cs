using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class ProductDefinitionInputDTOValidator : AbstractValidator<ProductDefinitionInputDTO>
    {
        public ProductDefinitionInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Code).NotNull().WithMessage("Code is required");
            RuleFor(inputDTO => inputDTO.Code).MaximumLength(20).WithMessage("Code can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.Name).NotNull().WithMessage("Name is required");
            RuleFor(inputDTO => inputDTO.Name).MaximumLength(100).WithMessage("Name can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Name).Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("Name can not contains special character");
            RuleFor(inputDTO => inputDTO.Sizes).NotNull().WithMessage("Size is required");
            RuleFor(inputDTO => inputDTO.Sizes).MaximumLength(100).WithMessage("Size can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Colors).NotNull().WithMessage("Color is required");
            RuleFor(inputDTO => inputDTO.Colors).MaximumLength(100).WithMessage("Color can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Description).MaximumLength(500).WithMessage("Description can not longer than 500 characters");
            RuleFor(inputDTO => inputDTO.Category).NotNull().WithMessage("Category is required");
            RuleFor(inputDTO => inputDTO.SemiFinishedProducts).NotNull().WithMessage("Semi finished product list is required");
            RuleFor(inputDTO => inputDTO.Materials).NotNull().WithMessage("Material is required");
        }
    }
}