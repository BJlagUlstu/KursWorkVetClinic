using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class Animal
    {
        public int? Id { get; set; }
        [Required]
        public string AnimalName { get; set; }
        [ForeignKey("AnimalId")]
        public virtual List<AnimalPurchase> AnimalsPurchases { get; set; }
    }
}