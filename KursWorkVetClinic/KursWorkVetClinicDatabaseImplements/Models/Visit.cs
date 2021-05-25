using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public DateTime DateVisit { get; set; }
        public int UserId { get; set; }
        [ForeignKey("VisitId")]
        public virtual List<AnimalVisit> AnimalsVisits { get; set; }
        [ForeignKey("VisitId")]
        public virtual List<VisitService> VisitServices { get; set; }
        public virtual User User { get; set; }
    }
}