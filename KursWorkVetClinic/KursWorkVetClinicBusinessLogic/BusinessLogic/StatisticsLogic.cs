using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.BusinessLogic
{
    public class StatisticsLogic
    {
        private readonly IVisitStorage _visitStorage;
        private readonly IPurchaseStorage _purchaseStorage;
        private readonly IAnimalStorage _animalStorage;
        public StatisticsLogic(IVisitStorage visitStorage, IPurchaseStorage purchaseStorage, IAnimalStorage animalStorage)
        {
            _visitStorage = visitStorage;
            _purchaseStorage = purchaseStorage;
            _animalStorage = animalStorage;
        }
        public List<StatisticsByAnimalsViewModel> GetAnimals(StatisticsBindingModel model)
        {
            var visits = _visitStorage.GetFilteredList(new VisitBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });
            var animals = _animalStorage.GetFullList();
            var list = new List<StatisticsByAnimalsViewModel>();
            foreach (var animal in animals)
            {
                var numberOfVisits = new Dictionary<string, int>();
                var counter = 0;
                foreach (var visit in visits)
                {
                    if (!visit.AnimalsVisits.Contains(animal.AnimalName))
                    {
                        continue;
                    }
                    counter++;
                }
                numberOfVisits.Add(animal.AnimalName, counter);
                var record = new StatisticsByAnimalsViewModel
                {
                    AnimalName = animal.AnimalName,
                    NumberOfVisits = numberOfVisits[animal.AnimalName]
                };
                list.Add(record);
            }
            return list;
        }
        public List<StatisticsByVisitsAndPurchasesViewModel> GetVisits(StatisticsBindingModel model)
        {
            var visits = _visitStorage.GetFilteredList(new VisitBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });
            var list = new List<StatisticsByVisitsAndPurchasesViewModel>();
            var numberOfVisits = new Dictionary<DateTime, int>();
            foreach (var visit in visits)
            {
                if (numberOfVisits.Count == 0 || !numberOfVisits.ContainsKey(visit.DateVisit))
                {
                    numberOfVisits.Add(visit.DateVisit, 1);
                } 
                else if (numberOfVisits.ContainsKey(visit.DateVisit))
                {
                    numberOfVisits[visit.DateVisit] += 1;
                }
            }
            foreach (var item in numberOfVisits)
            {
                var record = new StatisticsByVisitsAndPurchasesViewModel
                {
                    Data = item.Key.ToShortDateString(),
                    AmountPerDay = item.Value
                };
                list.Add(record);
            }
            return list;
        }
        public List<StatisticsByVisitsAndPurchasesViewModel> GetPurchases(StatisticsBindingModel model)
        {
            var purchases = _purchaseStorage.GetFilteredList(new PurchaseBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });
            var list = new List<StatisticsByVisitsAndPurchasesViewModel>();
            var proceeds = new Dictionary<string, int>();
            foreach (var purchase in purchases)
            {
                if (proceeds.Count == 0 || !proceeds.ContainsKey(purchase.DatePayment.ToShortDateString()))
                {
                    proceeds.Add(purchase.DatePayment.ToShortDateString(), (int)purchase.Sum);
                }
                else if (proceeds.ContainsKey(purchase.DatePayment.ToShortDateString()))
                {
                    proceeds[purchase.DatePayment.ToShortDateString()] += (int)purchase.Sum;
                }
            }
            foreach (var item in proceeds)
            {
                var record = new StatisticsByVisitsAndPurchasesViewModel
                {
                    Data = item.Key,
                    AmountPerDay = item.Value
                };
                list.Add(record);
            }
            return list;
        }
    }
}