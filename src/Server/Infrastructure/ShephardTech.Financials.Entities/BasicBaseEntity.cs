using System.ComponentModel.DataAnnotations;

namespace ShephardTech.Financials.Entities
{
    public partial class BasicBaseEntity
    {

        [Required]
        public bool Deleted { get; set; } = false;

        [StringLength(50)]
        public string? CreatedBy { get; set; }
        [StringLength(50)]
        public string? UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
    }
}
