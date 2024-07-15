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
    public class MeasurementInputDTOValidator : AbstractValidator<MeasurementInputDTO>
    {
        public MeasurementInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Name).NotNull().WithMessage("Name is required");
            RuleFor(inputDTO => inputDTO.Name).MaximumLength(100).WithMessage("Name can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Name).Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("Name can not contains special character");
            RuleFor(inputDTO => inputDTO.Unit).NotNull().WithMessage("Unit is required");
            RuleFor(inputDTO => inputDTO.Unit).MaximumLength(100).WithMessage("Unit can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Unit).Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("Unit can not contains special character");
            RuleFor(inputDTO => inputDTO.Measure).GreaterThan(0).WithMessage("Measurement must greater than 0");
        }
    }
}