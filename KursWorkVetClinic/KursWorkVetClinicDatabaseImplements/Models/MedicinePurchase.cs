
namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class MedicinePurchase
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public int PurchaseId { get; set; }
        public int Count { get; set; }
        public virtual Medicine Medicine { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}