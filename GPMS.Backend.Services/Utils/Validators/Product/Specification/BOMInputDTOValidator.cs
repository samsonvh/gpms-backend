using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class BOMInputDTOValidator : AbstractValidator<BOMInputDTO>
    {
        public BOMInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.SizeWidth).GreaterThan(0)
                .WithMessage("Size Width must greater than 0");
            RuleFor(inputDTO => inputDTO.Consumption).GreaterThan(0)
                .WithMessage("Consumption must greater than 0");
            RuleFor(inputDTO => inputDTO.Description).MaximumLength(500)
                .When(inputDTO => !inputDTO.Description.IsNullOrEmpty())
                .WithMessage("Description can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.MaterialId).NotNull().NotEmpty()
                .WithMessage("Material Id is required");

        }
    }
}