using GPMS.Backend.Data.Enums.Statuses.Staffs;

namespace GPMS.Backend.Data.Models.Staffs
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public AccountStatus Status { get; set; }
        public Staff? Staff { get; set; }
    }
}
