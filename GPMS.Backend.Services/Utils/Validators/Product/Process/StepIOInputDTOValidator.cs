using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class StepIOInputDTOValidator : AbstractValidator<StepIOInputDTO>
    {
        public StepIOInputDTOValidator()
        {
            //Not a Material/ Semi Finished Product/ Product
            RuleFor(inputDTO => inputDTO).Must(inputDTO => !(inputDTO.MaterialId != null && inputDTO.SemiFinishedProductCode != null))
            .WithMessage("This step input output can not be both material and semi finished product")
            .OverridePropertyName("MaterialId and SemiFinishedProductCode");

            //Material
            RuleFor(inputDTO => inputDTO.Consumption).NotNull()
                .When(inputDTO => !inputDTO.MaterialId.IsNullOrEmpty() && inputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                .WithMessage("Consumption must not be null when this step input output is material");
            RuleFor(inputDTO => inputDTO.Quantity).Null()
                .When(inputDTO => !inputDTO.MaterialId.IsNullOrEmpty() && inputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                .WithMessage("Quantity must be null when this step input output is material");
            RuleFor(inputDTO => inputDTO.IsProduct).Must(IsProduct => !IsProduct)
                .When(inputDTO => !inputDTO.MaterialId.IsNullOrEmpty() && inputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                .WithMessage("Is Product must be false when this step input output is material");

            //Semi Finished Product
            RuleFor(inputDTO => inputDTO.Consumption).Null()
                .When(inputDTO => inputDTO.MaterialId.IsNullOrEmpty() && !inputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                .WithMessage("Consumption must be null when this step input output is semi finished product");
            RuleFor(inputDTO => inputDTO.Quantity).NotNull()
                .When(inputDTO => inputDTO.MaterialId.IsNullOrEmpty() && !inputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                .WithMessage("Quantity must be not null when this step input output is semi finished product");
            RuleFor(inputDTO => inputDTO.IsProduct).Must(IsProduct => !IsProduct)
                .When(inputDTO => inputDTO.MaterialId.IsNullOrEmpty() && !inputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                .WithMessage("Is Product must be false when this step input output is semi finished product");

            //Product
            RuleFor(inputDTO => inputDTO.Consumption).Null()
                .When(inputDTO => inputDTO.MaterialId.IsNullOrEmpty() && inputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                .WithMessage("Consumption must be null when this step input output is product");
            RuleFor(inputDTO => inputDTO.Quantity).NotNull()
                .When(inputDTO => inputDTO.MaterialId.IsNullOrEmpty() && inputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                .WithMessage("Quantity must be not null when this step input output is product");
            RuleFor(inputDTO => inputDTO.IsProduct).Must(IsProduct => IsProduct)
                .When(inputDTO => inputDTO.MaterialId.IsNullOrEmpty() && inputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                .WithMessage("Is Product must be true when this step input output is product");

            RuleFor(inputDTO => inputDTO.Quantity).GreaterThan(0).When(inputDTO => inputDTO.Quantity.HasValue)
                .WithMessage("Quantity must greater than 0");
            RuleFor(inputDTO => inputDTO.Consumption).GreaterThan(0).When(inputDTO => inputDTO.Consumption.HasValue)
                .WithMessage("Consumption must greater than 0");
            RuleFor(inputDTO => inputDTO.Type).NotNull()
                .WithMessage("Type is required");
            RuleFor(inputDTO => inputDTO).Must(inputDTO => inputDTO.Quantity != null || inputDTO.Consumption != null)
                .WithMessage("Quantity and Consumption must not be both required or not required")
                .OverridePropertyName("MaterialId and SemiFinishedProductCode");
        }
    }
}