using System.Windows;
using Unity;
using System;

namespace KursWorkVetClinicView
{
    public partial class Statistics : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public Statistics()
        {
            InitializeComponent();
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedStaticsComboBox.SelectedItem == null)
            {
                MessageBox.Show("Укажите нужную статистику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (SelectedStaticsComboBox.Text == "Животные")
                {
                    var form = Container.Resolve<DiagramByAnimals>();
                    Close();
                    form.ShowDialog();
                }
                else
                {
                    var form = Container.Resolve<DiagramsByVisitsAndPurchases>();
                    Close();
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}