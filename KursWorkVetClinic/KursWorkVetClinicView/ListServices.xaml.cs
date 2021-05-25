using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using System.Windows;
using System;
using System.Collections.Generic;

namespace KursWorkVetClinicView
{
    public partial class ListServices : Window
    {
        private readonly ReportLogic report_logic;
        private readonly AnimalLogic animal_logic;
        private List<string> Services;
        public ListServices(ReportLogic _report_logic, AnimalLogic _animal_logic)
        {
            InitializeComponent();
            report_logic = _report_logic;
            animal_logic = _animal_logic;
        }
        private void LoadData()
        {
            try
            {
                if (Services != null)
                {
                    CanSelectedAnimalsListBox.Items.Clear();
                    foreach (var service in animal_logic.Read(null))
                    {
                        CanSelectedAnimalsListBox.Items.Add(service.AnimalName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WordButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.SaveFileDialog { Filter = "docx|*.docx" })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    List<string> AnimalsName = new List<string>();
                    foreach (var animalName in CanSelectedAnimalsListBox.SelectedItems)
                    {
                        AnimalsName.Add(animalName.ToString());
                    }
                    report_logic.SaveServicesToWordFile(new ReportBindingModel { FileName = dialog.FileName, AnimalsName = AnimalsName });
                    MessageBox.Show("Файл со списком услуг успешно создан", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void ExcelButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    List<string> AnimalsName = new List<string>();
                    foreach (var animalName in CanSelectedAnimalsListBox.SelectedItems)
                    {
                        AnimalsName.Add(animalName.ToString());
                    }
                    report_logic.SaveAnimalsToExcelFile(new ReportBindingModel { FileName = dialog.FileName, AnimalsName = AnimalsName });
                    MessageBox.Show("Файл со списком услуг успешно создан", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void CancelButton__Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ListServices_Load(object sender, RoutedEventArgs e)
        {
            Services = new List<string>();
            LoadData();
        }
    }
}