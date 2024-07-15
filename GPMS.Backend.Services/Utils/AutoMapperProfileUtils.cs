using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs.LisingDTOs;

namespace GPMS.Backend.Services.Utils
{
    public class AutoMapperProfileUtils : Profile
    {
        public AutoMapperProfileUtils()
        {
            //account
            CreateMap<Account, AccountListingDTO>();
        }
        
    }
}