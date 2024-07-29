using FluentValidation;
using GPMS.Backend.Data.Enums.Times;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class ProductionEstimationValidator : AbstractValidator<ProductionEstimationInputDTO>
    {
        public ProductionEstimationValidator()
        {
            RuleFor(inputDTO => inputDTO.Quantity)
                .GreaterThan(0).WithMessage("Quantity must greater than 0");

            RuleFor(inputDTO => inputDTO.OverTimeQuantity)
                .GreaterThan(0).WithMessage("Over Time Quantity must greater than 0");
            RuleFor(inputDTO => inputDTO.OverTimeQuantity)
                .GreaterThan(inputDTO => inputDTO.Quantity)
                .WithMessage("Over Time Quantity must greater than Quantity");

        }
    }
}
