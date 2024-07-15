using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class LoginInputDTOValidator : AbstractValidator<LoginInputDTO>
    {
        public LoginInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Email).NotNull().WithMessage("Email is required");
            RuleFor(inputDTO => inputDTO.Email).MaximumLength(100).WithMessage("Email can not longer than 100 character");
            RuleFor(inputDTO => inputDTO.Email).Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                                                .WithMessage("Email must in right format");
            RuleFor(inputDTO => inputDTO.Password).NotNull().WithMessage("Password is required");
            RuleFor(inputDTO => inputDTO.Password).MaximumLength(500).WithMessage("Password is not longer than 500 characters");
        }
    }
}