using FluentValidation;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using Microsoft.IdentityModel.Tokens;
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
            RuleFor(inputDTO => inputDTO.ParentProductionPlanId).Null()
                .When(inputDTO => inputDTO.Type.ToLower().Equals(ProductionPlanType.Year.ToString().ToLower()))
                .WithMessage("Parent Production Plan Id must null when Production Plan Type is Year");

            RuleFor(inputDTO => inputDTO.ParentProductionPlanId).NotNull().NotEmpty()
                .When(inputDTO => !inputDTO.Type.Equals(ProductionPlanType.Year.ToString()))
                .WithMessage("Parent Production Plan Id is required when Production Plan Type is not Year");

            RuleFor(inputDTO => inputDTO.Code).NotNull().NotEmpty()
                .WithMessage("Code is required.");
            RuleFor(inputDTO => inputDTO.Code).MaximumLength(20)
                .When(inputDTO => !inputDTO.Code.IsNullOrEmpty())
                .WithMessage("Code length can not longer than 20 characters.");

            RuleFor(inputDTO => inputDTO.Name).NotNull().NotEmpty()
                .WithMessage("Name is required.");
            RuleFor(inputDTO => inputDTO.Name).MaximumLength(100)
                .When(inputDTO => !inputDTO.Name.IsNullOrEmpty())
                .WithMessage("Name length can not longer than 100 characters.");
            RuleFor(inputDTO => inputDTO.Name).Matches(@"^[a-zA-Z0-9À-ỹ\s]+$")
                .When(inputDTO => !inputDTO.Name.IsNullOrEmpty())
                .WithMessage("Name can not contains special character");

            RuleFor(inputDTO => inputDTO.StartingDate).NotNull().NotEmpty()
                .WithMessage("Starting Date is required");

            RuleFor(inputDTO => inputDTO.DueDate).NotNull().NotEmpty()
                .WithMessage("Due Date is required");
            RuleFor(inputDTO => inputDTO.DueDate).GreaterThan(inputDTO => inputDTO.StartingDate)
                .WithMessage("Due Date must greater than Starting Date");

            RuleFor(inputDTO => inputDTO.Type).IsEnumName(typeof(ProductionPlanType), false)
                .When(inputDTO => !inputDTO.Type.IsNullOrEmpty())
                .WithMessage("Invalid Type");

            RuleFor(inputDTO => inputDTO.Description).MaximumLength(500)
                .When(inputDTO => !inputDTO.Description.IsNullOrEmpty())
                .WithMessage("Description length can not longer than 500 characters");

            RuleFor(inputDTO => inputDTO.ProductionRequirements.Count)
                .GreaterThan(0).WithMessage("Production Requirements is required");
        }
    }
}
