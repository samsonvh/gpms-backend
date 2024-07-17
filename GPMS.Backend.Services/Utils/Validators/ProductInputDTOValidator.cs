using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class ProductInputDTOValidator : AbstractValidator<ProductInputDTO>
    {
        public ProductInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Definition).NotNull().WithMessage("Definition is required");
            RuleFor(inputDTO => inputDTO.Specifications).NotNull().WithMessage("Specification list is required");
            RuleFor(inputDTO => inputDTO.Processes).NotNull().WithMessage("Process list is required");
        }
    }
}