using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class MaterialInputDTOValidator : AbstractValidator<MaterialInputDTO>
    {
        public MaterialInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Code).NotNull().NotEmpty().WithMessage("Code is required")
                                            .MaximumLength(20).WithMessage("Code can not longer than 20 characters");

            RuleFor(inputDTO => inputDTO.Name).NotNull().NotEmpty()
                .When(inputDTO => inputDTO.IsNew)
                .WithMessage("Name is required");
            RuleFor(inputDTO => inputDTO.Name).MaximumLength(100)
                .When(inputDTO => inputDTO.IsNew && !inputDTO.Name.IsNullOrEmpty())
                .WithMessage("Name can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Name).Matches(@"^[a-zA-Z0-9À-ỹ\s]+$")
                .When(inputDTO => inputDTO.IsNew && !inputDTO.Name.IsNullOrEmpty())
                .WithMessage("Name can not contains special character");

            RuleFor(inputDTO => inputDTO.ConsumptionUnit).NotNull().NotEmpty()
                .When(inputDTO => inputDTO.IsNew)
                .WithMessage("ConsumptionUnit is required");
            RuleFor(inputDTO => inputDTO.ConsumptionUnit).MaximumLength(20)
                .When(inputDTO => inputDTO.IsNew && !inputDTO.ConsumptionUnit.IsNullOrEmpty())
                .WithMessage("ConsumptionUnit can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.ConsumptionUnit).Matches(@"^[a-zA-Z0-9À-ỹ\s]+$")
                .When(inputDTO => inputDTO.IsNew && !inputDTO.ConsumptionUnit.IsNullOrEmpty())
                .WithMessage("ConsumptionUnit can not contains special character");

            RuleFor(inputDTO => inputDTO.SizeWidthUnit).NotNull().NotEmpty()
                .When(inputDTO => inputDTO.IsNew)
                .WithMessage("Size width unit is required");
            RuleFor(inputDTO => inputDTO.SizeWidthUnit).MaximumLength(20)
                .When(inputDTO => inputDTO.IsNew && !inputDTO.SizeWidthUnit.IsNullOrEmpty())
                .WithMessage("Size width unit can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.SizeWidthUnit).Matches(@"^[a-zA-Z0-9À-ỹ\s]+$")
                .When(inputDTO => inputDTO.IsNew && !inputDTO.SizeWidthUnit.IsNullOrEmpty())
                .WithMessage("Size width unit can not contains special character");

            RuleFor(inputDTO => inputDTO.ColorCode).NotNull().NotEmpty()
                .When(inputDTO => inputDTO.IsNew)
                .WithMessage("Color code is required");
            RuleFor(inputDTO => inputDTO.ColorCode).MaximumLength(20)
                .When(inputDTO => inputDTO.IsNew && !inputDTO.ColorCode.IsNullOrEmpty())
                .WithMessage("Color code can not longer than 20 characters");

            RuleFor(inputDTO => inputDTO.ColorName).NotNull().NotEmpty()
                .When(inputDTO => inputDTO.IsNew)
                .WithMessage("Color name is required");
            RuleFor(inputDTO => inputDTO.ColorName).MaximumLength(20)
                .When(inputDTO => inputDTO.IsNew && !inputDTO.ColorName.IsNullOrEmpty())
                .WithMessage("Color name can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.ColorName).Matches(@"^[a-zA-Z0-9À-ỹ\s]+$")
                .When(inputDTO => inputDTO.IsNew && !inputDTO.ColorName.IsNullOrEmpty())
                .WithMessage("Color name can not contains special character");

            RuleFor(inputDTO => inputDTO.Description).MaximumLength(500)
                .When(inputDTO => inputDTO.IsNew && !inputDTO.Description.IsNullOrEmpty())
                .WithMessage("Description can not longer than 500 characters");


            RuleFor(inputDTO => inputDTO.Name).Null()
                .When(inputDTO => !inputDTO.IsNew)
                .WithMessage("Name must be null when this is not a new material");
            RuleFor(inputDTO => inputDTO.ConsumptionUnit).Null()
                .When(inputDTO => !inputDTO.IsNew)
                .WithMessage("Consumption unit must be null when this is not a new material");
            RuleFor(inputDTO => inputDTO.SizeWidthUnit).Null()
                .When(inputDTO => !inputDTO.IsNew)
                .WithMessage("Size width unit must be null when this is not a new material");
            RuleFor(inputDTO => inputDTO.ColorCode).Null()
                .When(inputDTO => !inputDTO.IsNew)
                .WithMessage("Color code must be null when this is not a new material");
            RuleFor(inputDTO => inputDTO.ColorName).Null()
                .When(inputDTO => !inputDTO.IsNew)
                .WithMessage("Color name must be null when this is not a new material");
            RuleFor(inputDTO => inputDTO.Description).Null()
                .When(inputDTO => !inputDTO.IsNew)
                .WithMessage("Description must be null when this is not a new material");

        }
    }
}