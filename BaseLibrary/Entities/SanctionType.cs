using BaseLibrary.Entities.Base;

namespace BaseLibrary.Entities
{
    public class SanctionType : BaseEntity
    {
        public List<Sanction>? Sanctions { get; set; }
    }
}
