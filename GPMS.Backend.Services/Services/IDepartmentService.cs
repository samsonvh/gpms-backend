﻿using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IDepartmentService
    {
        Task<DefaultPageResponseListingDTO<DepartmentListingDTO>> GetAllDepartments(DepartmentFilterModel departmentFilterModel);
        Task<DepartmentDTO> Details(Guid id);
    }
}
