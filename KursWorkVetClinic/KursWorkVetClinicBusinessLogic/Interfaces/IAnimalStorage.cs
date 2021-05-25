using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using KursWorkVetClinicBusinessLogic.BindingModels;

namespace KursWorkVetClinicBusinessLogic.Interfaces
{
    public interface IAnimalStorage
    {
        List<AnimalViewModel> GetFullList();
        List<AnimalViewModel> GetFilteredList(AnimalBindingModel model);
        AnimalViewModel GetElement(AnimalBindingModel model);
        void Insert(AnimalBindingModel model);
        void Update(AnimalBindingModel model);
        void Delete(AnimalBindingModel model);
    }
}