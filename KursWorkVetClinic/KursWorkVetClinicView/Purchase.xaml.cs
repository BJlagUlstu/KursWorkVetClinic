using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KursWorkVetClinicView
{
    public partial class Purchase : Window
    {
        public int Id { get { return (int)id; }  set { id = value; } }
        private readonly PurchaseLogic purchase_logic;
        private readonly AnimalLogic animal_logic;
        private int? id;
        private string Username;
        private int UserId;
        private decimal Sum;
        private List<string> AnimalsPurchases;
        private bool reportWindow = false;
        private Dictionary<string, (int, int)> MedicinesPurchases;
        public Purchase(PurchaseLogic _purchase_logic, AnimalLogic _animal_logic)
        {
            InitializeComponent();
            purchase_logic = _purchase_logic;
            animal_logic = _animal_logic;
        }
        private void LoadData()
        {
            try
            {
                SelectedAnimalsListBox.Items.Clear();
                foreach (var animal in AnimalsPurchases)
                {
                    SelectedAnimalsListBox.Items.Add(animal);
                }

                CanSelectedAnimalsListBox.Items.Clear();
                foreach (var animal in animal_logic.Read(null))
                {
                    if (AnimalsPurchases.Where(rec => rec.ToString() == animal.AnimalName).ToList().Count == 0)
                    {
                        CanSelectedAnimalsListBox.Items.Add(animal);
                    }
                }

                SelectedMedicinesListBox.Items.Clear();
                foreach (var medicine in MedicinesPurchases)
                {
                    SelectedMedicinesListBox.Items.Add(medicine.Key + "   —   " + medicine.Value.Item1);
                }

                UserLabel.Content = Username;
                SumLabel.Content = Sum;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void ReportWindow()
        {
            SaveButton.IsEnabled = false;
            RefundButton.IsEnabled = false;
            AddButton.IsEnabled = false;
            reportWindow = true;
        }
        private void RefundButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAnimalsListBox.SelectedItems.Count == 1)
            {
                AnimalsPurchases.Remove(AnimalsPurchases.Where(rec => rec.ToString() == (string)SelectedAnimalsListBox.SelectedItem).ToList()[0]);
                LoadData();
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanSelectedAnimalsListBox.SelectedItems.Count == 1)
            {
                AnimalsPurchases.Add(((AnimalViewModel)CanSelectedAnimalsListBox.SelectedItem).AnimalName);
                LoadData();
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalsPurchases == null || AnimalsPurchases.Count == 0)
            {
                MessageBox.Show("Укажите животное", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                purchase_logic.CreateOrUpdate(new PurchaseBindingModel
                {
                    Id = id,
                    Sum = (decimal)SumLabel.Content,
                    DatePayment = DateTime.Now,
                    MedicinesPurchases = MedicinesPurchases,
                    AnimalsPurchases = AnimalsPurchases,
                    UserId = (int)App.User.UserId
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CheckCurrentUser()
        {
            if (Username == App.User.Fullname && UserId == App.User.UserId || UserLabel.Content == null)
            {
                SaveButton.IsEnabled = true;
                RefundButton.IsEnabled = true;
                AddButton.IsEnabled = true;
            }
            else
            {
                SaveButton.IsEnabled = false;
                RefundButton.IsEnabled = false;
                AddButton.IsEnabled = false;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Purchase_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    PurchaseViewModel view = purchase_logic.Read(new PurchaseBindingModel { Id = id.Value })?[0];
                    AnimalsPurchases = view.AnimalsPurchases;
                    MedicinesPurchases = view.MedicinesPurchases;
                    Sum = view.Sum;
                    UserId = view.UserId;
                    Username = view.Username;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                AnimalsPurchases = new List<string>();
                MedicinesPurchases = new Dictionary<string, (int, int)>();
            }
            LoadData();
            if (!reportWindow)
            {
                CheckCurrentUser();
            }
        }
    }
}