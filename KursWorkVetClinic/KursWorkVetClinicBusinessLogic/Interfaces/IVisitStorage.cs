using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using KursWorkVetClinicBusinessLogic.BindingModels;

namespace KursWorkVetClinicBusinessLogic.Interfaces
{
    public interface IVisitStorage
    {
        List<VisitViewModel> GetFullList();
        List<VisitViewModel> GetFilteredList(VisitBindingModel model);
        VisitViewModel GetElement(VisitBindingModel model);
        void Insert(VisitBindingModel model);
        void Update(VisitBindingModel model);
        void Delete(VisitBindingModel model);
    }
}