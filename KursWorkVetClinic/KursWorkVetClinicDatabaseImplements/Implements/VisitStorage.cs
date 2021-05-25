using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicDatabaseImplements.Models;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KursWorkVetClinicDatabaseImplements.Implements
{
    public class VisitStorage : IVisitStorage
    {
        public List<VisitViewModel> GetFullList()
        {
            using (var context = new VetClinicDataBase())
            {
                return context.Visits
                .Include(rec => rec.AnimalsVisits)
                .ThenInclude(rec => rec.Animal)
                .Include(rec => rec.VisitServices)
                .ThenInclude(rec => rec.Service)
                .Include(rec => rec.User)
                .ToList()
                .Select(rec => new VisitViewModel
                {
                    Id = rec.Id,
                    UserId = rec.UserId,
                    Username = rec.User.Fullname,
                    DateVisit = rec.DateVisit,
                    AnimalsVisits = rec.AnimalsVisits.Select(recAN => recAN.Animal?.AnimalName).ToList(),
                    VisitsServices = rec.VisitServices.Select(recSN => recSN.Service?.ServiceName).ToList()
                })
                .ToList();
            }
        }
        public List<VisitViewModel> GetFilteredList(VisitBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                return context.Visits
               .Include(rec => rec.AnimalsVisits)
               .ThenInclude(rec => rec.Animal)
               .Include(rec => rec.VisitServices)
               .ThenInclude(rec => rec.Service)
               .Include(rec => rec.User)
               .Where(rec => rec.Id.Equals(model.Id) || (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.UserId == model.UserId) ||
               (model.DateFrom.HasValue && model.DateTo.HasValue && model.UserId == 0) ||
               (model.DateFrom.HasValue && model.DateTo.HasValue && rec.DateVisit >= model.DateFrom && rec.DateVisit <= model.DateTo && rec.UserId == model.UserId))
               .ToList()
               .Select(rec => new VisitViewModel
               {
                   Id = rec.Id,
                   UserId = rec.UserId,
                   Username = rec.User.Fullname,
                   DateVisit = rec.DateVisit,
                   AnimalsVisits = rec.AnimalsVisits.Select(recAN => recAN.Animal?.AnimalName).ToList(),
                   VisitsServices = rec.VisitServices.Select(recSN => recSN.Service?.ServiceName).ToList()
               })
               .ToList();
            }
        }
        public VisitViewModel GetElement(VisitBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                var visit = context.Visits
                .Include(rec => rec.AnimalsVisits)
                .ThenInclude(rec => rec.Animal)
                .Include(rec => rec.VisitServices)
                .ThenInclude(rec => rec.Service)
                .Include(rec => rec.User)
                .FirstOrDefault(rec => rec.Id == model.Id);
                return visit != null ?
                new VisitViewModel
                {
                    Id = visit.Id,
                    UserId = visit.UserId,
                    Username = visit.User.Fullname,
                    DateVisit = visit.DateVisit,
                    AnimalsVisits = visit.AnimalsVisits.Select(recAN => recAN.Animal?.AnimalName).ToList(),
                    VisitsServices = visit.VisitServices.Select(recSN => recSN.Service?.ServiceName).ToList()
                } :
               null;
            }
        }
        public void Insert(VisitBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Visit visit = new Visit
                        {
                            DateVisit = model.DateVisit,
                            UserId = model.UserId
                        };
                        context.Visits.Add(visit);
                        context.SaveChanges();
                        CreateModel(model, visit, context);
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
        public void Update(VisitBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Visits.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Посещение не найдено");
                        }
                        element.DateVisit = model.DateVisit;
                        element.UserId = model.UserId;
                        CreateModel(model, element, context);
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
        public void Delete(VisitBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                Visit element = context.Visits.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Visits.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Посещение не найдено");
                }
            }
        }
        private Visit CreateModel(VisitBindingModel model, Visit visit, VetClinicDataBase context)
        {
            if (model.Id.HasValue)
            {
                var visitAnimals = context.AnimalsVisits.Where(rec => rec.VisitId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.AnimalsVisits.RemoveRange(visitAnimals.Where(rec => !model.AnimalsVisits.Contains(context.Animals.FirstOrDefault(recAN => recAN.Id == rec.AnimalId).AnimalName)));
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateAnimal in visitAnimals)
                {
                    model.AnimalsVisits.Remove(updateAnimal.Animal.AnimalName);
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (var av in model.AnimalsVisits)
            {
                context.AnimalsVisits.Add(new AnimalVisit
                {
                    AnimalId = (int)context.Animals.FirstOrDefault(rec => rec.AnimalName == av).Id,
                    VisitId = visit.Id
                });
                context.SaveChanges();
            }
            return visit;
        }
    }
}