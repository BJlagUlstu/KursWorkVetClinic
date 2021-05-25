using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using KursWorkVetClinicBusinessLogic.BindingModels;

namespace KursWorkVetClinicBusinessLogic.Interfaces
{
    public interface IPurchaseStorage
    {
        List<PurchaseViewModel> GetFullList();
        List<PurchaseViewModel> GetFilteredList(PurchaseBindingModel model);
        PurchaseViewModel GetElement(PurchaseBindingModel model);
        void Insert(PurchaseBindingModel model);
        void Update(PurchaseBindingModel model);
        void Delete(PurchaseBindingModel model);
    }
}