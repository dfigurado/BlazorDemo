namespace BaseLibrary.Entities
{
    public class Branch : BaseEntity
    {
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
