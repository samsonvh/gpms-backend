using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class ProductionRequirementInputDTOValidator : AbstractValidator<ProductionRequirementInputDTO>
    {
        public ProductionRequirementInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.ProductionSpecificationId).NotNull().NotEmpty()
                .WithMessage("Production Specification Id is required");

            RuleFor(inputDTO => inputDTO.Quantity).GreaterThan(0)
                .WithMessage("Quantity must greater than 0");

            RuleFor(inputDTO => inputDTO.ProductionEstimations.Count).GreaterThan(0)
                .WithMessage("Production Estimation List is required");
        }
    }
}