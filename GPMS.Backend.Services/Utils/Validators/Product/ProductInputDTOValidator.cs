using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class ProductInputDTOValidator : AbstractValidator<ProductInputDTO>
    {
        public ProductInputDTOValidator()
        {
            RuleFor(inputDTO => inputDTO.Code).NotNull().NotEmpty()
                .WithMessage("Code is required");
            RuleFor(inputDTO => inputDTO.Code).MaximumLength(20)
            .When(inputDTO => !inputDTO.Code.IsNullOrEmpty()).WithMessage("Code can not longer than 20 characters");

            RuleFor(inputDTO => inputDTO.Name).NotNull().NotEmpty()
                .WithMessage("Name is required");
            RuleFor(inputDTO => inputDTO.Name).MaximumLength(100)
                .When(inputDTO => !inputDTO.Name.IsNullOrEmpty())
                .WithMessage("Name can not longer than 100 characters");
            RuleFor(inputDTO => inputDTO.Name).Matches(@"^[a-zA-Z0-9À-ỹ\s]+$")
                .When(inputDTO => !inputDTO.Name.IsNullOrEmpty())
                .WithMessage("Name can not contains special character");

            RuleFor(inputDTO => inputDTO.Sizes).NotNull().NotEmpty()
                .WithMessage("Size is required");
            RuleFor(inputDTO => inputDTO.Sizes).MaximumLength(100)
                .When(inputDTO => !inputDTO.Sizes.IsNullOrEmpty())
                .WithMessage("Size can not longer than 100 characters");

            RuleFor(inputDTO => inputDTO.Colors).NotNull().NotEmpty()
                .WithMessage("Color is required");
            RuleFor(inputDTO => inputDTO.Colors).MaximumLength(100)
                .When(inputDTO => !inputDTO.Colors.IsNullOrEmpty())
                .WithMessage("Color can not longer than 100 characters");

            RuleFor(inputDTO => inputDTO.Description).MaximumLength(500)
                .When(inputDTO => !inputDTO.Description.IsNullOrEmpty())
                .WithMessage("Description can not longer than 500 characters");

            RuleFor(inputDTO => inputDTO.CategoryId).NotNull().NotEmpty()
                .WithMessage("CategoryId is required");

            RuleFor(inputDTO => inputDTO.SemiFinishedProducts.Count).GreaterThan(0).WithMessage("Semi finished product list is required");

            RuleFor(inputDTO => inputDTO.Specifications.Count).GreaterThan(0).WithMessage("Specification list is required");

            RuleFor(inputDTO => inputDTO.Processes.Count).GreaterThan(0).WithMessage("Process list is required");
        }
    }
}