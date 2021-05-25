using System;
using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;

namespace KursWorkVetClinicBusinessLogic.BusinessLogic
{
    public class VisitLogic
    {
        private readonly IVisitStorage _visitStorage;
        public VisitLogic(IVisitStorage visitStorage)
        {
            _visitStorage = visitStorage;
        }
        public List<VisitViewModel> Read(VisitBindingModel model)
        {
            if (model == null)
            {
                return _visitStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<VisitViewModel> { _visitStorage.GetElement(model) };
            }
            return _visitStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(VisitBindingModel model)
        {
            var element = _visitStorage.GetElement(new VisitBindingModel
            {
                Id = model.Id
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть посещение с таким ID");
            }
            if (model.Id.HasValue)
            {
                _visitStorage.Update(model);
            }
            else
            {
                _visitStorage.Insert(model);
            }
        }
        public void Delete(VisitBindingModel model)
        {
            var element = _visitStorage.GetElement(new VisitBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Посещение не найдено");
            }
            _visitStorage.Delete(model);
        }
    }
}