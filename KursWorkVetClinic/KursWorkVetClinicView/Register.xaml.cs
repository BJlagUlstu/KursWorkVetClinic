using System.Windows;
using Unity;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using KursWorkVetClinicBusinessLogic.BindingModels;
using System;

namespace KursWorkVetClinicView
{
    public partial class Register : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly UserLogic user_logic;
        public Register(UserLogic _user_logic)
        {
            InitializeComponent();
            user_logic = _user_logic;
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    Id = null,
                    Login = LoginTextBox.Text,
                    Fullname = UsernameTextBox.Text,
                    Password = PasswordBox.Password,
                });
                MessageBox.Show("Регистрация прошла успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                var form = Container.Resolve<Login>();
                Close();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<FirstWindow>();
            Close();
            form.ShowDialog();
        }
    }
}