using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;

namespace GPMS.Backend.Services.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDTO> LoginWithCredential(LoginInputDTO loginInputDTO);
    }
}