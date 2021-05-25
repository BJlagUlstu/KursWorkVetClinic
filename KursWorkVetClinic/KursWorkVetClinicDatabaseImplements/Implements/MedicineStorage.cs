using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicDatabaseImplements.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using KursWorkVetClinicBusinessLogic.ViewModels;

namespace KursWorkVetClinicDatabaseImplements.Implements
{
    public class MedicineStorage : IMedicineStorage
    {
        public void Delete(MedicineBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                Medicine element = context.Medicines.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Medicines.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Лекарство не найдено");
                }
            }
        }
        public MedicineViewModel GetElement(MedicineBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                var service = context.Medicines.FirstOrDefault(rec => rec.MedicineName == model.MedicineName || rec.Id == model.Id);
                return service != null ?
                new MedicineViewModel
                {
                    Id = service.Id,
                    MedicineName = service.MedicineName,
                    Cost = service.Cost
                } :
               null;
            }
        }
        public List<MedicineViewModel> GetFilteredList(MedicineBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                return context.Medicines.Where(rec => rec.MedicineName.Contains(model.MedicineName))
               .ToList()
               .Select(rec => new MedicineViewModel
               {
                   Id = rec.Id,
                   MedicineName = rec.MedicineName,
                   Cost = rec.Cost
               }).ToList();
            }
        }
        public List<MedicineViewModel> GetFullList()
        {
            using (var context = new VetClinicDataBase())
            {
                return context.Medicines
               .ToList()
               .Select(rec => new MedicineViewModel
               {
                   Id = rec.Id,
                   MedicineName = rec.MedicineName,
                   Cost = rec.Cost
               }).ToList();
            }
        }
        public void Insert(MedicineBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Medicines.Add(CreateModel(model, new Medicine()));
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
        public void Update(MedicineBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Medicines.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
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
        private Medicine CreateModel(MedicineBindingModel model, Medicine medicine)
        {
            medicine.MedicineName = model.MedicineName;
            return medicine;
        }
    }
}