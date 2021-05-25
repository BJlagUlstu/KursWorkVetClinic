using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        [Required]
        public string FIO { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Login { get; set; }
        [ForeignKey("DoctorId")]
        public virtual List<Service> Services { get; set; }
    }
}