using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using System.Windows;
using Unity;
using System;

namespace KursWorkVetClinicView
{
    public partial class Animal : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly AnimalLogic animal_logic;
        public int Id { set { id = value; } }
        private int? id;
        public Animal(AnimalLogic _animal_logic)
        {
            InitializeComponent();
            animal_logic = _animal_logic;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Заполните имя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                animal_logic.CreateOrUpdate(new AnimalBindingModel
                {
                    Id = id,
                    AnimalName = NameTextBox.Text
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
        private void Animal_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = animal_logic.Read(new AnimalBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        NameTextBox.Text = view.AnimalName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}