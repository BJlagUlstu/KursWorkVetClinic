using System.ComponentModel.DataAnnotations;

namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class AnimalVisit
    {
        public int Id { get; set; }
        [Required]
        public int AnimalId { get; set; }
        [Required]
        public int VisitId { get; set; }
        public virtual Animal Animal { get; set; }
        public virtual Visit Visit { get; set; }
    }
}