using System.ComponentModel.DataAnnotations;

namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class MedicationService
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int MedicationId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Medication Medication { get; set; }
        public virtual Service Service { get; set; }
    }
}