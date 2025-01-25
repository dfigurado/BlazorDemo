namespace BaseLibrary.Entities
{
    public class City : BaseEntity
    {
        public Country? Country { get; set; }
        public int CountryId { get; set; } = 0;

        public List<Area>? Area { get; set; }
    }
}
