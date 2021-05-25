using System.Windows;
using Unity;

namespace KursWorkVetClinicView
{
    public partial class FirstWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public FirstWindow()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<Login>();
            Close();
            form.ShowDialog();
        }
        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<Register>();
            Close();
            form.ShowDialog();
        }
    }
}