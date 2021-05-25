using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System.Windows;
using Unity;
using System;

namespace KursWorkVetClinicView
{
    public partial class Animals : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly AnimalLogic animallogic;
        public Animals(AnimalLogic _animallogic)
        {
            InitializeComponent();
            animallogic = _animallogic;
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = animallogic.Read(null);
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
            var form = Container.Resolve<Animal>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItems.Count == 1)
            {
                var form = Container.Resolve<Animal>();
                form.Id = (int)((AnimalViewModel)dataGridView.SelectedItem).Id;
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
                    int id = Convert.ToInt32(((AnimalViewModel)dataGridView.SelectedItem).Id);
                    try
                    {
                        animallogic.Delete(new AnimalBindingModel { Id = id });
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