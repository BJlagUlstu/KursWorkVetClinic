using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicDatabaseImplements.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using KursWorkVetClinicDatabaseImplements;
using KursWorkVetClinicBusinessLogic.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KursWorkVetClinicDatabaseImplements.Implements
{
    public class ServiceStorage : IServiceStorage
    {
        public void Delete(ServiceBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                Service element = context.Services.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Services.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Услуга не найдена");
                }
            }
        }
        public ServiceViewModel GetElement(ServiceBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                var service = context.Services.FirstOrDefault(rec => rec.ServiceName == model.ServiceName || rec.Id == model.Id);
                return service != null ?
                new ServiceViewModel
                {
                    Id = service.Id,
                    ServiceName = service.ServiceName
                } :
               null;
            }
        }
        public List<ServiceViewModel> GetFilteredList(ServiceBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                return context.Services
               .Where(rec => rec.ServiceName.Contains(model.ServiceName))
               .ToList()
               .Select(rec => new ServiceViewModel
               {
                   Id = rec.Id,
                   ServiceName = rec.ServiceName
               }).ToList();
            }
        }
        public List<ServiceViewModel> GetFullList()
        {
            using (var context = new VetClinicDataBase())
            {
                return context.Services
               .ToList()
               .Select(rec => new ServiceViewModel
               {
                   Id = rec.Id,
                   ServiceName = rec.ServiceName
               }).ToList();
            }
        }
        public void Insert(ServiceBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Services.Add(CreateModel(model, new Service()));
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Update(ServiceBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Services.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Услуга не найдена");
                        }
                        CreateModel(model, element);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        private Service CreateModel(ServiceBindingModel model, Service service)
        {
            service.ServiceName = model.ServiceName;
            return service;
        }
    }
}