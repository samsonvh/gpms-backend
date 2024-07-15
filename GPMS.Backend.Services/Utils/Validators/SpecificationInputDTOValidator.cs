using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class SpecificationInputDTOValidator : AbstractValidator<SpecificationInputDTO>
    {
        public SpecificationInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Size).NotNull().WithMessage("Size is required");
            RuleFor(inputDTO => inputDTO.Size).MaximumLength(100).WithMessage("Size can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Color).NotNull().WithMessage("Color is required");
            RuleFor(inputDTO => inputDTO.Color).MaximumLength(100).WithMessage("Color can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Measurements).NotNull().WithMessage("Measurement list is required");
            RuleFor(inputDTO => inputDTO.BOMs).NotNull().WithMessage("BOM list is required");
            RuleFor(inputDTO => inputDTO.QualityStandards).NotNull().WithMessage("Quality standards list is required");
        }
    }
}