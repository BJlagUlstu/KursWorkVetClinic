using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System;

namespace KursWorkVetClinicBusinessLogic.HelperModels
{
    class PdfInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<ReportAnimalsVisitsPurchasesViewModel> AnimalsVisitsPurchases { get; set; }
    }
}