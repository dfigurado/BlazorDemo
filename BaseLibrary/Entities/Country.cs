using BaseLibrary.Entities.Base;

namespace BaseLibrary.Entities
{
    public class Country : BaseEntity
    {
        public List<City>? Cities { get; set; }
    }
}
