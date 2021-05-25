using System.Windows;
using Unity;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using KursWorkVetClinicBusinessLogic.BindingModels;
using System;

namespace KursWorkVetClinicView
{
    public partial class PersonalData : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly UserLogic user_logic;
        public PersonalData(UserLogic _user_logic)
        {
            InitializeComponent();
            user_logic = _user_logic;
            LoadData();
        }
        private void LoadData()
        {
            LoginTextBox.Text = App.User.Login;
            UsernameTextBox.Text = App.User.Fullname;
            PasswordBox.Password = App.User.Password;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                MessageBox.Show("Введите имя пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                user_logic.CreateOrUpdate(new UserBindingModel
                {
                    Id = App.User.UserId,
                    Login = LoginTextBox.Text,
                    Fullname = UsernameTextBox.Text,
                    Password = PasswordBox.Password,
                });
                App.User = user_logic.Read(new UserBindingModel { Login = LoginTextBox.Text })?[0];
                MessageBox.Show("Личные данные успешно изменены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}