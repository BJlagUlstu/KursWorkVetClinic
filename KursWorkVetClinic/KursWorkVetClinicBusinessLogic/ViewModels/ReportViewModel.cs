using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.ViewModels
{
    public class ReportViewModel
    {
        public string AnimalName { get; set; }
        public List<string> Services { get; set; }
        internal IEnumerable<string> ToArray()
        {
            return Services.ToArray();
        }
    }
}