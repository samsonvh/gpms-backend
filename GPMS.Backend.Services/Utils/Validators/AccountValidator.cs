using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Data.Models.Staffs;

namespace GPMS.Backend.Services.Utils.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            
        }
    }
}