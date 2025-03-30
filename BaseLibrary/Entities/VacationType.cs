using BaseLibrary.Entities.Base;

namespace BaseLibrary.Entities
{
    public class VacationType:BaseEntity
    {
        public List<Vacation>? Vacations { get; set; }
    }
}
