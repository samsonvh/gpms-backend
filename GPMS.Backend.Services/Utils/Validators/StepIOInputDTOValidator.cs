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
            RuleFor(inputDTO => inputDTO.Quantity).GreaterThan(0).WithMessage("Quantity Width must greater than 0");
            RuleFor(inputDTO => inputDTO.Consumption).GreaterThan(0).WithMessage("Consumption must greater than 0");
            RuleFor(inputDTO => inputDTO.Type).NotNull().WithMessage("Type is required");
        }
    }
}