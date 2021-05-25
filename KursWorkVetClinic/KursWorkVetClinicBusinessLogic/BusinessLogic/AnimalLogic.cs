using System;
using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;

namespace KursWorkVetClinicBusinessLogic.BusinessLogic
{
    public class AnimalLogic
    {
        private readonly IAnimalStorage _animalStorage;
        public AnimalLogic(IAnimalStorage animalStorage)
        {
            _animalStorage = animalStorage;
        }
        public List<AnimalViewModel> Read(AnimalBindingModel model)
        {
            if (model == null)
            {
                return _animalStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<AnimalViewModel> { _animalStorage.GetElement(model) };
            }
            return _animalStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(AnimalBindingModel model)
        {
            var element = _animalStorage.GetElement(new AnimalBindingModel
            {
                Id = model.Id
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть животное с таким названием");
            }
            if (model.Id.HasValue)
            {
                _animalStorage.Update(model);
            }
            else
            {
                _animalStorage.Insert(model);
            }
        }
        public void Delete(AnimalBindingModel model)
        {
            var element = _animalStorage.GetElement(new AnimalBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Животное не найдено");
            }
            _animalStorage.Delete(model);
        }
    }
}