namespace BaseLibrary.Entities
{
    public class Area : BaseEntity
    {
        public List<Employee>? Employees { get; set; }
        public City? City { get; set; }
        public int CityId { get; set; } = 0;
    }
}
