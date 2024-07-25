using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class ProductionPlanInputDTOValidator : AbstractValidator<ProductionPlanInputDTO>
    {
        public ProductionPlanInputDTOValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .MaximumLength(20).WithMessage("Code length must be between 1 and 50 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name length must be between 1 and 100 characters.");

        }
    }
}
