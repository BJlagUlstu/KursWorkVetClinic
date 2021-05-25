using System;
using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.ViewModels
{
    public class VisitViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime DateVisit { get; set; }
        public List<string> VisitsServices { get; set; }
        public List<string> AnimalsVisits { get; set; }
    }
}