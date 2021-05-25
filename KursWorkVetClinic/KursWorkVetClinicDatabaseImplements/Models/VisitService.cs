using System.ComponentModel.DataAnnotations;

namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class VisitService
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int VisitId { get; set; }
        [Required]
        public virtual Visit Visit { get; set; }
        public virtual Service Service { get; set; }
    }
}