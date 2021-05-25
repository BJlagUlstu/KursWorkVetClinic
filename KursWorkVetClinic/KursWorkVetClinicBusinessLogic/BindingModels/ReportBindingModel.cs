using System;
using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.BindingModels
{
    public class ReportBindingModel
    {
        public string FileName { get; set; }
        public int UserId { get; set; }
        public string LoginCurrentUserInSystem { get; set; }
        public List<string> AnimalsName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}