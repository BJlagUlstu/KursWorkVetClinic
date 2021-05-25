using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.HelperModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KursWorkVetClinicBusinessLogic.BusinessLogic
{
    public class ReportLogic
    {
        private readonly IVisitStorage _visitStorage;
        private readonly IPurchaseStorage _purchaseStorage;
        private readonly IAnimalStorage _animalStorage;
        private readonly IUserStorage _userStorage;
        public ReportLogic(IVisitStorage visitStorage, IPurchaseStorage purchaseStorage, IAnimalStorage animalStorage, IUserStorage userStorage)
        {
            _visitStorage = visitStorage;
            _purchaseStorage = purchaseStorage;
            _animalStorage = animalStorage;
            _userStorage = userStorage;
        }
        public List<ReportViewModel> GetServices(ReportBindingModel model)
        {
            var visits = _visitStorage.GetFullList();
            var list = new List<ReportViewModel>();
            foreach (var animal in model.AnimalsName)
            {
                var services = new List<string>();
                foreach(var visit in visits)
                {
                    if (!visit.AnimalsVisits.Contains(animal))
                    {
                        continue;
                    }
                    services.AddRange(visit.VisitsServices);
                }
                var readyListServices = new List<string>();
                var g = services.GroupBy(x => x);
                foreach (var grp in g)
                {
                    readyListServices.Add($"{grp.Key} — ({grp.Count()})");
                }
                var record = new ReportViewModel
                {
                    AnimalName = animal,
                    Services = readyListServices
                };
                list.Add(record);
            }
            return list;
        }
        public List<ReportAnimalsVisitsPurchasesViewModel> GetAnimalsPurchasesVisits(ReportBindingModel model)
        {
            var visits = _visitStorage.GetFilteredList(new VisitBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                UserId = model.UserId
            });
            var purchases = _purchaseStorage.GetFilteredList(new PurchaseBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                UserId = model.UserId
            });
            var animals = _animalStorage.GetFullList();
            var list = new List<ReportAnimalsVisitsPurchasesViewModel>();
            foreach (var animal in animals)
            {
                var selectedVisits = new List<string>();
                var selectedPurchases = new List<string>();
                List<int> listPurchaseId = new List<int>();
                List<int> listVisitId = new List<int>();
                foreach (var visit in visits)
                {
                    if (visit.AnimalsVisits.Contains(animal.AnimalName))
                    {
                        selectedVisits.Add($"Дата: {visit.DateVisit.ToShortDateString()}  —  Кол-во услуг: {visit.VisitsServices.Count}");
                        listVisitId.Add(visit.Id);
                    }
                }
                foreach (var purchase in purchases)
                {
                    if (purchase.AnimalsPurchases.Contains(animal.AnimalName))
                    {
                        selectedPurchases.Add($"Дата: {purchase.DatePayment.ToShortDateString()}  —  Кол-во лекарств: {purchase.MedicinesPurchases.Count}  —  Сумма: {purchase.Sum.ToString("G", CultureInfo.InvariantCulture)} руб.");
                        listPurchaseId.Add(purchase.Id);
                    }
                }
                int maxLength = selectedPurchases.Count >= selectedVisits.Count ? selectedPurchases.Count : selectedVisits.Count;
                for (int i = 0; i < maxLength; i++)
                {
                    animal.AnimalName = i > 0 ? "" : animal.AnimalName;
                    var visit = i >= selectedVisits.Count ? "" : selectedVisits[i];
                    var purchase = i >= selectedPurchases.Count ? "" : selectedPurchases[i];
                    var purchaseId = i >= listPurchaseId.Count ? 0 : listPurchaseId[i];
                    var visitId = i >= listVisitId.Count ? 0 : listVisitId[i];
                    if (selectedPurchases.Count != 0 || selectedVisits.Count != 0)
                    {
                        var record = new ReportAnimalsVisitsPurchasesViewModel
                        {
                            AnimalName = animal.AnimalName,
                            Purchases = purchase,
                            Visits = visit,
                            PurchaseId = purchaseId,
                            VisitId = visitId
                        };
                        list.Add(record);
                    }
                }
            }
            return list;
        }
        public void SaveServicesToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordExelInfo
            {
                FileName = model.FileName,
                Title = "Список услуг животных",
                Services = GetServices(model),
            });
        }
        public void SaveAnimalsToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new WordExelInfo
            {
                FileName = model.FileName,
                Title = "Список услуг животных",
                Services = GetServices(model),
            });
        }
        [Obsolete]
        public void SaveAnimalsVisitsPurchasesToPDFFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                DateFrom = (DateTime)model.DateFrom,
                DateTo = (DateTime)model.DateTo,
                Title = "Список животных, их покупок и посещений",
                AnimalsVisitsPurchases = GetAnimalsPurchasesVisits(model),
                Username = _userStorage.GetElement(new UserBindingModel { Id = model.UserId })?.Fullname,
            });
            MailLogic.MailSendAsync(new MailSendInfo
            {
                MailAddress = model.LoginCurrentUserInSystem,
                Subject = $"Новый отчет",
                Text = $"Отчет от {DateTime.Now}"
            });
        }
    }
}