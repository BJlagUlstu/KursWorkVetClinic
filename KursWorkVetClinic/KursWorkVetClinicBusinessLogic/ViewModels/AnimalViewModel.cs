using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.ViewModels
{
    public class AnimalViewModel
    {
        public int? Id { get; set; }
        public string AnimalName { get; set; }
        public List<string> Services { get; set; } = new List<string>();
        public override string ToString()
        {
            return AnimalName;
        }
    }
}