
namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class AnimalPurchase
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public int PurchaseId { get; set; }
        public virtual Animal Animal { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}