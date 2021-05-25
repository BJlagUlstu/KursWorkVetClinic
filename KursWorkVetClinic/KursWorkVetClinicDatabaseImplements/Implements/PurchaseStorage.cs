using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicBusinessLogic.ViewModels;
using KursWorkVetClinicDatabaseImplements.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KursWorkVetClinicDatabaseImplements.Implements
{
    public class PurchaseStorage : IPurchaseStorage
    {
        public List<PurchaseViewModel> GetFullList()
        {
            using (var context = new VetClinicDataBase())
            {
                return context.Purchases
                .Include(rec => rec.AnimalsPurchases)
                .ThenInclude(rec => rec.Animal)
                .Include(rec => rec.MedicinesPurchases)
                .ThenInclude(rec => rec.Medicine)
                .Include(rec => rec.User)
                .ToList()
                .Select(rec => new PurchaseViewModel
                {
                    Id = rec.Id,
                    UserId = rec.UserId,
                    Username = rec.User.Fullname,
                    Sum = rec.Sum,
                    DatePayment = rec.DatePayment,
                    MedicinesPurchases = rec.MedicinesPurchases.ToDictionary(recTC => context.Medicines.FirstOrDefault(recMN => recMN.Id == recTC.MedicineId).MedicineName, 
                    recTC => (recTC.Count, context.Medicines.FirstOrDefault(recMP => recMP.Id == recTC.MedicineId).Cost)),
                    AnimalsPurchases = rec.AnimalsPurchases.Select(recAP => recAP.Animal.AnimalName).ToList()
                })
                .ToList();
            }
        }
        public List<PurchaseViewModel> GetFilteredList(PurchaseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                return context.Purchases
               .Include(rec => rec.AnimalsPurchases)
               .ThenInclude(rec => rec.Animal)
               .Include(rec => rec.MedicinesPurchases)
               .ThenInclude(rec => rec.Medicine)
               .Include(rec => rec.User)
               .Where(rec => rec.Id.Equals(model.Id) || (!model.DateFrom.HasValue && !model.DateTo.HasValue && rec.UserId == model.UserId) ||
               (model.DateFrom.HasValue && model.DateTo.HasValue && model.UserId == 0) ||
               (model.DateFrom.HasValue && model.DateTo.HasValue && rec.DatePayment >= model.DateFrom && rec.DatePayment <= model.DateTo && rec.UserId == model.UserId))
               .ToList()
               .Select(rec => new PurchaseViewModel
               {
                   Id = rec.Id,
                   UserId = rec.UserId,
                   Username = rec.User.Fullname,
                   Sum = rec.Sum,
                   DatePayment = rec.DatePayment,
                   MedicinesPurchases = rec.MedicinesPurchases.ToDictionary(recTC => context.Medicines.FirstOrDefault(recMN => recMN.Id == recTC.MedicineId).MedicineName,
                    recTC => (recTC.Count, context.Medicines.FirstOrDefault(recMP => recMP.Id == recTC.MedicineId).Cost)),
                   AnimalsPurchases = rec.AnimalsPurchases.Select(recAP => recAP.Animal.AnimalName).ToList()
               })
               .ToList();
            }
        }
        public PurchaseViewModel GetElement(PurchaseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                var purchase = context.Purchases
                .Include(rec => rec.AnimalsPurchases)
                .ThenInclude(rec => rec.Animal)
                .Include(rec => rec.MedicinesPurchases)
                .ThenInclude(rec => rec.Medicine)
                .Include(rec => rec.User)
                .FirstOrDefault(rec => rec.Id == model.Id);
                return purchase != null ?
                new PurchaseViewModel
                {
                    Id = purchase.Id,
                    UserId = purchase.UserId,
                    Username = purchase.User.Fullname,
                    Sum = purchase.Sum,
                    DatePayment = purchase.DatePayment,
                    MedicinesPurchases = purchase.MedicinesPurchases.ToDictionary(recTC => context.Medicines.FirstOrDefault(recMN => recMN.Id == recTC.MedicineId).MedicineName,
                    recTC => (recTC.Count, context.Medicines.FirstOrDefault(recMP => recMP.Id == recTC.MedicineId).Cost)),
                    AnimalsPurchases = purchase.AnimalsPurchases.Select(recAP => recAP.Animal.AnimalName).ToList()
                } :
               null;
            }
        }
        public void Insert(PurchaseBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Purchase purchase = new Purchase
                        {
                            Sum = model.Sum,
                            DatePayment = model.DatePayment,
                            UserId = model.UserId
                        };
                        context.Purchases.Add(purchase);
                        context.SaveChanges();
                        CreateModel(model, purchase, context);
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
        public void Update(PurchaseBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Purchases.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Покупка не найдена");
                        }
                        element.Sum = model.Sum;
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
        public void Delete(PurchaseBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                Purchase element = context.Purchases.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Purchases.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Покупка не найдена");
                }
            }
        }
        private Purchase CreateModel(PurchaseBindingModel model, Purchase purchase, VetClinicDataBase context)
        {
            if (model.Id.HasValue)
            {
                // Лекарства в покупке
                var medicinesPurchases = context.MedicinesPurchases.Where(rec => rec.PurchaseId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.MedicinesPurchases.RemoveRange(medicinesPurchases.Where(rec => !model.MedicinesPurchases.ContainsKey(context.Medicines.FirstOrDefault(recMN => recMN.Id == rec.MedicineId).MedicineName)).ToList());
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateMedicine in medicinesPurchases)
                {
                    updateMedicine.Count = model.MedicinesPurchases[updateMedicine.Medicine.MedicineName].Item1;
                    model.MedicinesPurchases.Remove(updateMedicine.Medicine.MedicineName);
                }
                context.SaveChanges();

                // Животные в покупке
                var animalsPurchases = context.AnimalsPurchases.Where(rec => rec.PurchaseId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.AnimalsPurchases.RemoveRange(animalsPurchases.Where(rec => !model.AnimalsPurchases.Contains(context.Animals.FirstOrDefault(recAN => recAN.Id == rec.AnimalId).AnimalName)).ToList());
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateAnimal in animalsPurchases)
                {
                    model.AnimalsPurchases.Remove(updateAnimal.Animal.AnimalName);
                }
                context.SaveChanges();
            }

            // добавили новые записи лекарств
            foreach (var mp in model.MedicinesPurchases)
            {
                context.MedicinesPurchases.Add(new MedicinePurchase
                {
                    MedicineId = context.Medicines.FirstOrDefault(rec => rec.MedicineName == mp.Key).Id,
                    PurchaseId = purchase.Id,
                    Count = mp.Value.Item1
                });
                context.SaveChanges();
            }

            // добавили новые записи животных
            foreach (var ap in model.AnimalsPurchases)
            {
                context.AnimalsPurchases.Add(new AnimalPurchase
                {
                    AnimalId = (int)context.Animals.FirstOrDefault(rec => rec.AnimalName == ap).Id,
                    PurchaseId = purchase.Id
                });
                context.SaveChanges();
            }
            return purchase;
        }
    }
}