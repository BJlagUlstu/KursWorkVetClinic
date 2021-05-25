using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System.Windows;
using Unity;
using System;

namespace KursWorkVetClinicView
{
    public partial class Visits : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly VisitLogic visit_logic;
        public Visits(VisitLogic _visit_logic)
        {
            InitializeComponent();
            visit_logic = _visit_logic;
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = visit_logic.Read(null);
                if (list != null)
                {
                    dataGridView.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<Visit>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<Visit>();
                form.Id = ((VisitViewModel)dataGridView.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItems.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = Convert.ToInt32(((VisitViewModel)dataGridView.SelectedItem).Id);
                    try
                    {
                        visit_logic.Delete(new VisitBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}