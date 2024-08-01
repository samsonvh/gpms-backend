using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.ResponseDTOs
{
    public class DefaultPageResponseListingDTO<L> 
    {
        public List<L>? Data { get; set; }
        public PaginationResponseModel Pagination { get; set; }
    }
}