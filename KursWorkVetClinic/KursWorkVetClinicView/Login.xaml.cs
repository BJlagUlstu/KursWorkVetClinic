using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.BusinessLogic;
using System.Windows;
using Unity;
using System;

namespace KursWorkVetClinicView
{
    public partial class Login : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly UserLogic user_logic;
        public Login(UserLogic _user_logic)
        {
            InitializeComponent();
            user_logic = _user_logic;
        }
        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                user_logic.Login(new UserBindingModel
                {
                    Id = null,
                    Login = LoginTextBox.Text,
                    Password = PasswordBox.Password,
                });
                App.User = user_logic.Read(new UserBindingModel { Login = LoginTextBox.Text })?[0];
                var form = Container.Resolve<MainWindow>();
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