using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System.Windows;
using System;
using System.Collections.Generic;
using Unity;

namespace KursWorkVetClinicView
{
    public partial class MedicineBinding : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly PurchaseLogic purchase_logic;
        private readonly MedicineLogic medicine_logic;
        private readonly Dictionary<string, (int, int)> MedicinesPurchases = new Dictionary<string, (int, int)>();
        public MedicineBinding(PurchaseLogic _purchase_logic, MedicineLogic _medicine_logic)
        {
            InitializeComponent();
            purchase_logic = _purchase_logic;
            medicine_logic = _medicine_logic;
            FillData();
        }
        private void FillData()
        {
            SelectedPurchaseComboBox.ItemsSource = purchase_logic.Read(new PurchaseBindingModel
            {
                UserId = (int)App.User.UserId
            });
            SelectedMedicineComboBox.ItemsSource = medicine_logic.Read(null);
        }
        private void SaveData()
        {
            try
            {
                MedicinesPurchases.Add(((MedicineViewModel)SelectedMedicineComboBox.SelectedItem).MedicineName, 
                    (Convert.ToInt32(CountTextBox.Text), ((MedicineViewModel)SelectedMedicineComboBox.SelectedItem).Cost));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPurchaseComboBox.SelectedItem == null)
            {
                MessageBox.Show("Укажите ID покупки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedMedicineComboBox.SelectedItem == null)
            {
                MessageBox.Show("Укажите лекарство", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CountTextBox.Text == null)
            {
                MessageBox.Show("Укажите количество лекарства", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                SaveData();
                int sum = Convert.ToInt32(SumTextBox.Text);
                int count = Convert.ToInt32(CountTextBox.Text);
                purchase_logic.MedicineBinding(new AddingMedicineBindingModel
                {
                    PurchaseId = ((PurchaseViewModel)SelectedPurchaseComboBox.SelectedItem).Id,
                    MedicineName = SelectedMedicineComboBox.SelectedItem.ToString(),
                    Sum = sum,
                    Count = count,
                    Cost = sum / count
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void Count_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (SelectedPurchaseComboBox.SelectedItem != null && SelectedMedicineComboBox.SelectedItem != null)
                {
                    SumTextBox.Text = (Convert.ToInt32(CountTextBox.Text) * ((MedicineViewModel)SelectedMedicineComboBox.SelectedItem).Cost).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SelectedPurchaseComboBox_SelectionChangeCommitted(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var form = Container.Resolve<Purchase>();
            form.Id = ((PurchaseViewModel)SelectedPurchaseComboBox.SelectedItem).Id;
            form.ReportWindow();
            form.ShowDialog();
        }
    }
}