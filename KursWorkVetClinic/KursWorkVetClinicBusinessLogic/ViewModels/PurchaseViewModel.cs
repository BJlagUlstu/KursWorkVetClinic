﻿using System;
using System.Collections.Generic;

namespace KursWorkVetClinicBusinessLogic.ViewModels
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Sum { get; set; }
        public string Username { get; set; }
        public DateTime DatePayment { get; set; }
        public Dictionary<string, (int, int)> MedicinesPurchases { get; set; }
        public List<string> AnimalsPurchases { get; set; }
        public override string ToString()
        {
            return Id.ToString();
        }
    }
}