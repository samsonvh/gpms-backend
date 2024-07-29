using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils.Validators.ProductionPlan
{
    public class ProductionSeriesValidator : AbstractValidator<ProductionSeriesInputDTO>
    {
        public ProductionSeriesValidator()
        {
            RuleFor(inputDTO => inputDTO.Code).NotNull().NotEmpty()
                .WithMessage("Code is required.");
            RuleFor(inputDTO => inputDTO.Code).MaximumLength(20)
                .When(inputDTO => !inputDTO.Code.IsNullOrEmpty())
                .WithMessage("Code length can not longer than 20 characters.");

            RuleFor(inputDTO => inputDTO.Quantity).GreaterThan(0)
                .WithMessage("Quantity must greater than 0");
        }
    }
}