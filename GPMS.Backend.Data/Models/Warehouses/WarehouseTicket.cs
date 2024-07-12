using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Requests;

namespace GPMS.Backend.Data.Models.Warehouses
{
    public class WarehouseTicket
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public WarehouseTicketType Type { get; set; }

        public Guid? WarehouseRequestId { get; set; }
        public WarehouseRequest? WarehouseRequest { get; set; }
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse {  get; set; }
        public Guid? ProductSpecificationId { get; set; }
        public ProductSpecification? ProductSpecification { get; set; }

    }
}
