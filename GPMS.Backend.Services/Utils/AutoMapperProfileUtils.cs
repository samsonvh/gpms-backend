using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
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
            CreateMap<Account, AccountDTO>()
                .ForMember(dto => dto.FullName, opt => opt.MapFrom(account =>  account.Staff.FullName));
            CreateMap<Account, CreateUpdateResponseDTO<Account>>();
            CreateMap<Account, ChangeStatusResponseDTO<Account, AccountStatus>>();

            //staff
            CreateMap<StaffInputDTO, Staff>()
                .ForMember(staff => staff.Code, opt => opt.MapFrom(dto => dto.Code))
                .ForMember(staff => staff.FullName, opt => opt.MapFrom(dto => dto.FullName))
                .ForMember(staff => staff.Position, opt => opt.MapFrom(dto => dto.Position))
                .ForMember(staff => staff.DepartmentId, opt => opt.MapFrom(dto => dto.DepartmentId))
                .ForMember(staff => staff.Account, opt => opt.Ignore());
            CreateMap<Staff, StaffListingDTO>();
            CreateMap<Staff, ChangeStatusResponseDTO<Staff, StaffStatus>>();
            CreateMap<Staff, ChangePositionResponseDTO<Staff, StaffPosition>>();

            //department
            CreateMap<Department, DepartmentListingDTO>();

        }
    }
}