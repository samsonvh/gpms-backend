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
    public class QualityStandardInputDTOValidator : AbstractValidator<QualityStandardInputDTO>
    {
        public QualityStandardInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Name).NotNull().WithMessage("Name is required");
            RuleFor(inputDTO => inputDTO.Name).MaximumLength(100).WithMessage("Name can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Name).Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("Name can not contains special character");
            RuleFor(inputDTO => inputDTO.Description).MaximumLength(500).WithMessage("Description can not longer than 500 characters");
            RuleFor(inputDTO => inputDTO.MaterialCode).NotNull().WithMessage("Material code is required");
            RuleFor(inputDTO => inputDTO.MaterialCode).MaximumLength(20).WithMessage("Material code can not longer than 20 characters");
        }
    }
}