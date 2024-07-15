using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class StepInputDTOValidator : AbstractValidator<StepInputDTO>
    {
        public StepInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Code).NotNull().WithMessage("Code is required");
            RuleFor(inputDTO => inputDTO.Code).MaximumLength(20).WithMessage("Code can not longer than 20 characters");
            RuleFor(inputDTO => inputDTO.Name).NotNull().WithMessage("Name is required");
            RuleFor(inputDTO => inputDTO.Name).MaximumLength(100).WithMessage("Name can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Name).Matches(@"^[a-zA-Z0-9 ]*$").WithMessage("Name can not contains special character");
            RuleFor(inputDTO => inputDTO.OrderNumber).GreaterThan(0).WithMessage("Order number must greater than 0");
            RuleFor(inputDTO => inputDTO.StandardTime).GreaterThan(0).WithMessage("Standard time must greater than 0");
            RuleFor(inputDTO => inputDTO.OutputPerHour).GreaterThan(0).WithMessage("Output per hour must greater than 0");
            RuleFor(inputDTO => inputDTO.Description).MaximumLength(50).WithMessage("Description can not longer than 500 characters");
            RuleFor(inputDTO => inputDTO.StepIOs).NotNull().WithMessage("Step input output list is required");
        }
    }
}