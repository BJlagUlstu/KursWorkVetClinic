using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using System.Windows;
using System;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System.Windows.Controls;
using Unity;

namespace KursWorkVetClinicView
{
    public partial class Report : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly ReportLogic report_logic;
        private readonly UserLogic user_logic;
        private readonly string filename = "E:\\Report.pdf";
        public Report(ReportLogic _report_logic, UserLogic _user_logic)
        {
            InitializeComponent();
            report_logic = _report_logic;
            user_logic = _user_logic;
            FillData();
        }
        private void FillData()
        {
            SelectedUserComboBox.ItemsSource = user_logic.Read(null);
        }
        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.ItemsSource = null;
            if (dataFromDataPicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату начала", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dataToDataPicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dataFromDataPicker.SelectedDate >= dataToDataPicker.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedUserComboBox.SelectedItem == null)
            {
                MessageBox.Show("Укажите пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var report = report_logic.GetAnimalsPurchasesVisits(new ReportBindingModel
                {
                    DateFrom = dataFromDataPicker.SelectedDate,
                    DateTo = dataToDataPicker.SelectedDate,
                    UserId = (int)((UserViewModel)SelectedUserComboBox.SelectedItem).UserId
                });
                if (report != null)
                {
                    dataGridView.Items.Clear();
                    dataGridView.ItemsSource = report;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        [Obsolete]
        private void ButtonSendByMail_Click(object sender, RoutedEventArgs e)
        {
            if (dataFromDataPicker.SelectedDate >= dataToDataPicker.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedUserComboBox.SelectedItem == null)
            {
                MessageBox.Show("Укажите пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                report_logic.SaveAnimalsVisitsPurchasesToPDFFile(new ReportBindingModel
                {
                    FileName = filename,
                    DateFrom = dataFromDataPicker.SelectedDate,
                    DateTo = dataToDataPicker.SelectedDate,
                    UserId = (int)((UserViewModel)SelectedUserComboBox.SelectedItem).UserId,
                    LoginCurrentUserInSystem = App.User.Login
                });
                MessageBox.Show("Отчет успешно отправлен на почту", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DataGridView_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dataGridView.CurrentCell != null && dataGridView.CurrentCell.Column != null && dataGridView.SelectedItem != null)
            {
                var header = dataGridView.CurrentCell.Column.Header;
                if (header.ToString() == "Имя животного")
                {
                    dataGridView.UnselectAllCells();
                    return;
                }
                else if (header.ToString() == "Покупки")
                {
                    var form = Container.Resolve<Purchase>();
                    form.Id = ((ReportAnimalsVisitsPurchasesViewModel)dataGridView.SelectedItem).PurchaseId;
                    if (form.Id == 0) { dataGridView.UnselectAllCells(); return; }
                    form.ReportWindow();
                    form.ShowDialog();
                }
                else
                {
                    var form = Container.Resolve<Visit>();
                    form.Id = ((ReportAnimalsVisitsPurchasesViewModel)dataGridView.SelectedItem).VisitId;
                    if (form.Id == 0) { dataGridView.UnselectAllCells(); return; }
                    form.ReportWindow();
                    form.ShowDialog();
                }
                dataGridView.UnselectAllCells();
            }
        }
    }
}