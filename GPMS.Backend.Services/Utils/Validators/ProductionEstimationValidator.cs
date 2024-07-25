using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
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
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to zero.")
                .NotEmpty().WithMessage("Quantity is required.");

            RuleFor(x => x.OverTimeQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Over time Quantity must be greater than or equal to zero.")
                .NotEmpty().WithMessage("Quantity is required.");
        }
    }
}
