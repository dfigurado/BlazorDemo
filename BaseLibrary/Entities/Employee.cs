using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities
{
    public class Employee : BaseEntity
    {
        [Required]
        public string? CivilId { get; set; } = string.Empty;
        [Required]
        public string? FileNumber { get; set; } = string.Empty;
        [Required]
        public string? FullName { get; set;} = string.Empty;
        [Required]
        public string? JobTitle { get; set; } = string.Empty;
        [Required]
        public string? Address { get; set; } = string.Empty;
        [Required, DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string? Other { get; set; }

        // Relationships : Many to One

        public Branch? Branch { get; set; }
        public int BranchId { get; set; }

        public Area? Area { get; set; }
        public int AreaId { get; set; }
    }
}
