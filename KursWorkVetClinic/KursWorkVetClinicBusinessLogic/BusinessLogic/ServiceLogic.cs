using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.BusinessLogic
{
    public class ServiceLogic
    {
        private readonly IServiceStorage _serviceStorage;
        public ServiceLogic(IServiceStorage serviceStorage)
        {
            _serviceStorage = serviceStorage;
        }
        public List<ServiceViewModel> Read(ServiceBindingModel model)
        {
            if (model == null)
            {
                return _serviceStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ServiceViewModel> { _serviceStorage.GetElement(model) };
            }
            return _serviceStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(ServiceBindingModel model)
        {
            var element = _serviceStorage.GetElement(new ServiceBindingModel
            {
                ServiceName = model.ServiceName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть лекарство с таким названием");
            }
            if (model.Id.HasValue)
            {
                _serviceStorage.Update(model);
            }
            else
            {
                _serviceStorage.Insert(model);
            }
        }
        public void Delete(ServiceBindingModel model)
        {
            var element = _serviceStorage.GetElement(new ServiceBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Лекарство не найдено");
            }
            _serviceStorage.Delete(model);
        }
    }
}