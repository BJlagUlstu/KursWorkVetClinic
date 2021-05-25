using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KursWorkVetClinicView
{
    public partial class Visit : Window
    {
        public int Id { get { return (int)id; } set { id = value; } }
        private readonly VisitLogic visit_logic;
        private readonly AnimalLogic animal_logic;
        private int? id;
        private string Username;
        private int UserId;
        private bool reportWindow = false;
        private List<string> AnimalsVisits;
        private List<string> VisitsServices;
        public Visit(VisitLogic _visit_logic, AnimalLogic _animal_logic)
        {
            InitializeComponent();
            visit_logic = _visit_logic;
            animal_logic = _animal_logic;
        }
        private void LoadData()
        {
            try
            {
                SelectedAnimalsListBox.Items.Clear();
                foreach (var currentAnimals in AnimalsVisits)
                {
                    SelectedAnimalsListBox.Items.Add(currentAnimals);
                }

                CanSelectedAnimalsListBox.Items.Clear();
                foreach (var animal in animal_logic.Read(null))
                {
                    if (AnimalsVisits.Where(rec => rec == animal.AnimalName).ToList().Count == 0)
                    {
                        CanSelectedAnimalsListBox.Items.Add(animal);
                    }
                }

                SelectedServicesListBox.Items.Clear();
                foreach (var service in VisitsServices)
                {
                    SelectedServicesListBox.Items.Add(service);
                }

                UserLabel.Content = Username;
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
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanSelectedAnimalsListBox.SelectedItems.Count == 1)
            {
                AnimalsVisits.Add(((AnimalViewModel)CanSelectedAnimalsListBox.SelectedItem).AnimalName);
                LoadData();
            }
        }
        private void RefundButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAnimalsListBox.SelectedItems.Count == 1)
            {
                AnimalsVisits.Remove(AnimalsVisits.Where(rec => rec == (string)SelectedAnimalsListBox.SelectedItem).ToList()[0]);
                LoadData();
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!DateVisitDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Укажите дату", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (AnimalsVisits == null || AnimalsVisits.Count == 0)
            {
                MessageBox.Show("Укажите животных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                visit_logic.CreateOrUpdate(new VisitBindingModel
                {
                    Id = id,
                    DateVisit = DateVisitDatePicker.SelectedDate.Value,
                    AnimalsVisits = AnimalsVisits,
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
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Visit_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try 
                {
                    VisitViewModel view = visit_logic.Read(new VisitBindingModel { Id = id.Value })?[0];
                    DateVisitDatePicker.SelectedDate = view.DateVisit;
                    AnimalsVisits = view.AnimalsVisits;
                    VisitsServices = view.VisitsServices;
                    Username = view.Username;
                    UserId = view.UserId;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                AnimalsVisits = new List<string>();
                VisitsServices = new List<string>();
            }
            LoadData();
            if (!reportWindow)
            {
                CheckCurrentUser();
            }
        }
    }
}