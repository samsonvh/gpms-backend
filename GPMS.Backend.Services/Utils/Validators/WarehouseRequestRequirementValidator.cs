using FluentValidation;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class WarehouseRequestRequirementValidator : AbstractValidator<WarehouseRequestRequirementInputDTO>
    {
        public WarehouseRequestRequirementValidator()
        {
            RuleFor(inputDTO => inputDTO.Quantity).GreaterThan(0).WithMessage(" Quantity is required");

            RuleFor(inputDTO => inputDTO.ProducitonRequirementId).NotNull().WithMessage("Production Requirement list is required");

        }
    }
}
