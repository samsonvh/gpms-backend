using FluentValidation;
using GPMS.Backend.Services.DTOs.InputDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class AccountInputDTOValidator : AbstractValidator<AccountInputDTO>
    {
        public AccountInputDTOValidator()
        {

            RuleFor(dto => dto.Code)
                .NotNull().WithMessage("Code must is null")
                .NotEmpty().WithMessage("Code must not empty.")
                .MaximumLength(20).WithMessage("Code must not exceed 20 characters.");

            RuleFor(dto => dto.StaffInputDTO.Code)
                .NotNull().WithMessage("Staff code is null")
                .NotEmpty().WithMessage("Staff code is required.")
                .MaximumLength(20).WithMessage("Staff code must not exceed 20 characters.");


            RuleFor(inputDTO => inputDTO.Email).NotNull().WithMessage("Email is required");
            RuleFor(inputDTO => inputDTO.Email).Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                                                .WithMessage("Email must in right format");

            RuleFor(dto => dto.Password)
                .NotNull().WithMessage("Pass word is null")
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(dto => dto.StaffInputDTO.FullName)
                .NotNull().WithMessage("Full name is null")
                .NotEmpty().WithMessage("Full name is required");

            RuleFor(dto => dto.StaffInputDTO.Position)
                .NotNull().WithMessage("Position is reuiqred");
        }
    }
}
