using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class WarehouseRequestValidator : AbstractValidator<WarehouseRequestInputDTO>
    {
        public WarehouseRequestValidator()
        {
            RuleFor(inputDTO => inputDTO.Name).NotNull().WithMessage("Name is required");
            RuleFor(inputDTO => inputDTO.Name).MaximumLength(100).WithMessage("Name can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Name).Matches(@"^[a-zA-Z0-9À-ỹ\s-()]+$").WithMessage("Name can not contains special character");

            RuleFor(inputDTO => inputDTO.Description).NotNull().WithMessage("Description is required");
            RuleFor(inputDTO => inputDTO.Description).MaximumLength(500).WithMessage("Description can not longer than 500 characters");
            RuleFor(inputDTO => inputDTO.Description).Matches(@"^[a-zA-Z0-9À-ỹ\s-()]+$").WithMessage("Description can not contains special character");

            RuleFor(inputDTO => inputDTO.WarehouseRequestRequirements).NotNull().WithMessage("Warehouse Request Requirement is required");
        }
    }
}
