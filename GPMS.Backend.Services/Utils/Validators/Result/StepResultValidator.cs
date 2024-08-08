using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Services.DTOs.InputDTOs.Results;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils.Validators.Result
{
    public class StepResultValidator : AbstractValidator<StepResultInputDTO>
    {
        public StepResultValidator()
        {
            RuleFor(inputDTO => inputDTO.Description).MaximumLength(500)
                .When(inputDTO => !inputDTO.Description.IsNullOrEmpty())
                .WithMessage("Description length can not longer than 500 characters");
            
            RuleFor(inputDTO => inputDTO.inputOutputResults.Count).GreaterThan(0)
                .WithMessage("Input Output Result list is required");
        }
    }
}