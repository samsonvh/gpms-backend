﻿using GPMS.Backend.Services.DTOs.LisingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class DepartmentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<StaffListingDTO> Staffs { get; set; }
    }
}
