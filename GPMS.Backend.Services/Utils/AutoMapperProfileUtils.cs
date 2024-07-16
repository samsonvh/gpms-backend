using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;

namespace GPMS.Backend.Services.Utils
{
    public class AutoMapperProfileUtils : Profile
    {
        public AutoMapperProfileUtils()
        {
            //account
            CreateMap<Account, AccountListingDTO>();
            CreateMap<AccountInputDTO, Account>();
            CreateMap<Account, AccountDTO>();
            CreateMap<Account, CreateUpdateResponseDTO<Account>>();

            //staff
            CreateMap<StaffInputDTO, Staff>()
                .ForMember(staff => staff.Code, opt => opt.MapFrom(dto => dto.Code))
                .ForMember(staff => staff.FullName, opt => opt.MapFrom(dto => dto.FullName))
                .ForMember(staff => staff.Position, opt => opt.MapFrom(dto => dto.Position))
                .ForMember(staff => staff.DepartmentId, opt => opt.MapFrom(dto => dto.DepartmentId))
                .ForMember(staff => staff.Account, opt => opt.Ignore());
        }
    }
}