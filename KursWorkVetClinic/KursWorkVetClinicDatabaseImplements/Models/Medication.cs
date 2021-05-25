using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class Medication
    {
        public int Id { get; set; }
        [Required]
        public string MedicationName { get; set; }
        [ForeignKey("MedicationId")]
        public virtual List<MedicationMedicine> MedicationsMedicines { get; set; }
        [ForeignKey("MedicationId")]
        public virtual List<MedicationService> MedicationsServices { get; set; }
    }
}