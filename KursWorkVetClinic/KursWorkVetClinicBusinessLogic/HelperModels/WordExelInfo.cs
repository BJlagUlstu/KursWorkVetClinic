using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.ViewModels;

namespace KursWorkVetClinicBusinessLogic.HelperModels
{
    public class WordExelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportViewModel> Services { get; set; }
        public List<string> NeededServices { get; set; }
    }
}