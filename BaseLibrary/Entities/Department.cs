namespace BaseLibrary.Entities
{
    public class Department : BaseEntity
    {
        public int GeneralDepartmentId { get; set; }
        public GeneralDepartment? GeneralDepartment { get; set; }
        public List<Branch>? Branches { get; set; }

    }
}
