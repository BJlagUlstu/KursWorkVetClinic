using System;
using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;

namespace KursWorkVetClinicBusinessLogic.BusinessLogic
{
    public class PurchaseLogic
    {
        private readonly IPurchaseStorage _purchaseStorage;
        public PurchaseLogic(IPurchaseStorage purchaseStorage)
        {
            _purchaseStorage = purchaseStorage;
        }
        public List<PurchaseViewModel> Read(PurchaseBindingModel model)
        {
            if (model == null)
            {
                return _purchaseStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<PurchaseViewModel> { _purchaseStorage.GetElement(model) };
            }
            return _purchaseStorage.GetFilteredList(model);
        }
        public void CreateOrUpdate(PurchaseBindingModel model)
        {
            var element = _purchaseStorage.GetElement(new PurchaseBindingModel
            {
                Id = model.Id
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть покупка с таким ID");
            }
            if (model.Id.HasValue)
            {
                _purchaseStorage.Update(model);
            }
            else
            {
                _purchaseStorage.Insert(model);
            }
        }
        public void Delete(PurchaseBindingModel model)
        {
            var element = _purchaseStorage.GetElement(new PurchaseBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Покупка не найдена");
            }
            _purchaseStorage.Delete(model);
        }
        public void MedicineBinding(AddingMedicineBindingModel model)
        {
            PurchaseViewModel purchase = _purchaseStorage.GetElement(new PurchaseBindingModel
            {
                Id = model.PurchaseId
            });
            if (purchase.MedicinesPurchases.Count == 0)
            {
                purchase.MedicinesPurchases = new Dictionary<string, (int, int)>
                {
                    { model.MedicineName, (model.Count, model.Cost) }
                };
            }
            else
            {
                if(!purchase.MedicinesPurchases.ContainsKey(model.MedicineName))
                {
                    purchase.MedicinesPurchases.Add(model.MedicineName, (model.Count, model.Cost));
                }
                else
                {
                    purchase.MedicinesPurchases[model.MedicineName] = (purchase.MedicinesPurchases[model.MedicineName].Item1 + model.Count, model.Cost);
                }
            }
            decimal sum = purchase.Sum + model.Sum;
            _purchaseStorage.Update(new PurchaseBindingModel
            {
                Id = purchase.Id,
                UserId = purchase.UserId,
                Sum = sum,
                DatePayment = purchase.DatePayment,
                MedicinesPurchases = purchase.MedicinesPurchases,
                AnimalsPurchases = purchase.AnimalsPurchases
            });
        }
    }
}