using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class MaterialInputDTOValidator : AbstractValidator<MaterialInputDTO>
    {
        public MaterialInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Code).NotNull().WithMessage("Code is required");
            RuleFor(inputDTO => inputDTO.Code).MaximumLength(20).WithMessage("Code can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.Name).NotNull().WithMessage("Name is required");
            RuleFor(inputDTO => inputDTO.Name).MaximumLength(100).WithMessage("Name can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Name).Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("Name can not contains special character");
            RuleFor(inputDTO => inputDTO.ConsumptionUnit).NotNull().WithMessage("ConsumptionUnit is required");
            RuleFor(inputDTO => inputDTO.ConsumptionUnit).MaximumLength(20).WithMessage("ConsumptionUnit can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.ConsumptionUnit).Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("ConsumptionUnit can not contains special character");
            RuleFor(inputDTO => inputDTO.SizeWidthUnit).NotNull().WithMessage("Size width unit is required");
            RuleFor(inputDTO => inputDTO.SizeWidthUnit).MaximumLength(20).WithMessage("Size width unit can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.SizeWidthUnit).Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("Size width unit can not contains special character");
            RuleFor(inputDTO => inputDTO.ColorCode).NotNull().WithMessage("Color code is required");
            RuleFor(inputDTO => inputDTO.ColorCode).MaximumLength(20).WithMessage("Color code can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.ColorName).NotNull().WithMessage("Color name is required");
            RuleFor(inputDTO => inputDTO.ColorName).MaximumLength(20).WithMessage("Color name can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.ColorName).Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("Color name can not contains special character");
            RuleFor(inputDTO => inputDTO.Description).MaximumLength(500).WithMessage("Description can not longer than 500 characters");
        }
    }
}