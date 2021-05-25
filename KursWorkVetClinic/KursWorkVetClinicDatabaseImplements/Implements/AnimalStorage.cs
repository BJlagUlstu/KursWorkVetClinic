using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicDatabaseImplements.Models;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KursWorkVetClinicDatabaseImplements.Implements
{
    public class AnimalStorage : IAnimalStorage
    {
        public List<AnimalViewModel> GetFullList()
        {
            using (var context = new VetClinicDataBase())
            {
                return context.Animals
                .Select(rec => new AnimalViewModel
                {
                    Id = rec.Id,
                    AnimalName = rec.AnimalName
                })
                .ToList();
            }
        }
        public List<AnimalViewModel> GetFilteredList(AnimalBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                return context.Animals
               .Where(rec => rec.AnimalName.Contains(model.AnimalName))
               .Select(rec => new AnimalViewModel
               {
                   Id = rec.Id,
                   AnimalName = rec.AnimalName
               })
               .ToList();
            }
        }
        public AnimalViewModel GetElement(AnimalBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                var animal = context.Animals.FirstOrDefault(rec => rec.AnimalName == model.AnimalName || rec.Id == model.Id);
                return animal != null ?
                new AnimalViewModel
                {
                    Id = animal.Id,
                    AnimalName = animal.AnimalName
                } :
               null;
            }
        }
        public void Insert(AnimalBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Animals.Add(CreateModel(model, new Animal()));
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
        public void Update(AnimalBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Animals.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Животное не найдено");
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
        public void Delete(AnimalBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                Animal element = context.Animals.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Animals.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Животное не найдено");
                }
            }
        }
        private Animal CreateModel(AnimalBindingModel model, Animal animal)
        {
            animal.AnimalName = model.AnimalName;
            return animal;
        }
    }
}