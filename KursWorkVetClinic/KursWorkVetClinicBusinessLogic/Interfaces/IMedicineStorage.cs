using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.Interfaces
{
    public interface IMedicineStorage
    {
        List<MedicineViewModel> GetFullList();
        List<MedicineViewModel> GetFilteredList(MedicineBindingModel model);
        MedicineViewModel GetElement(MedicineBindingModel model);
        void Insert(MedicineBindingModel model);
        void Update(MedicineBindingModel model);
        void Delete(MedicineBindingModel model);
    }
}