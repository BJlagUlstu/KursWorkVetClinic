using System;
using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.BindingModels
{
    public class VisitBindingModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateVisit { get; set; }
        public List<string> AnimalsVisits { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}