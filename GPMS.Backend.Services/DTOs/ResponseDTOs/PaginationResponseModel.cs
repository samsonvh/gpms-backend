using GPMS.Backend.Services.Filters;

namespace GPMS.Backend.Services.DTOs.ResponseDTOs
{
    public class PaginationResponseModel : PaginationRequestModel
    {
        public int TotalRows { get; set; }
    }
}