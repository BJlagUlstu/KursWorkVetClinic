using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using KursWorkVetClinicBusinessLogic.BindingModels;

namespace KursWorkVetClinicBusinessLogic.Interfaces
{
    public interface IServiceStorage
    {
        List<ServiceViewModel> GetFullList();
        List<ServiceViewModel> GetFilteredList(ServiceBindingModel model);
        ServiceViewModel GetElement(ServiceBindingModel model);
        void Insert(ServiceBindingModel model);
        void Update(ServiceBindingModel model);
        void Delete(ServiceBindingModel model);
    }
}