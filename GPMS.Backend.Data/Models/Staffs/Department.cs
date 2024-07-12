namespace GPMS.Backend.Data.Models.Staffs
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Staff> Staffs { get; set; } = new List<Staff>();
    }
}
