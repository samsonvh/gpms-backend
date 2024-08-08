using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.LisingDTOs;

namespace GPMS.Backend.Services.DTOs.ResponseDTOs
{
    public class PageResponseStepIOForStepResultListingDTO
    {
        public PaginationResponseModel Pagination { get; set; }
        public StepIOForStepResultListingDTOWrapper Data { get; set; }
    }
}