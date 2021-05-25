
namespace KursWorkVetClinicBusinessLogic.ViewModels
{
    public class MedicineViewModel
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public int Cost { get; set; }
        public override string ToString()
        {
            return MedicineName;
        }
    }
}