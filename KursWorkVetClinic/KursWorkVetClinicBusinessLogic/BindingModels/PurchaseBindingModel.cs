using System;
using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.BindingModels
{
    public class PurchaseBindingModel
    {
        public int? Id { get; set; }
        public decimal Sum { get; set; }
        public int UserId { get; set; }
        public DateTime DatePayment { get; set; }
        public Dictionary<string, (int, int)> MedicinesPurchases { get; set; }
        public List<string> AnimalsPurchases { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}