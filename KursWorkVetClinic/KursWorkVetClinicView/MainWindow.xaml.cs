using System.Windows;
using Unity;

namespace KursWorkVetClinicView
{
    public partial class MainWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AnimalsItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<Animals>();
            form.ShowDialog();
        }
        private void PurchasesItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<Purchases>();
            form.ShowDialog();
        }
        private void VisitsItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<Visits>();
            form.ShowDialog();
        }
        private void MedicinesBindingItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<MedicineBinding>();
            form.ShowDialog();
        }
        private void GetListServicesItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<ListServices>();
            form.ShowDialog();
        }
        private void ReportItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<Report>();
            form.ShowDialog();
        }
        private void PersonalData_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<PersonalData>();
            form.ShowDialog();
        }
        private void StatisticsItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<Statistics>();
            form.ShowDialog();
        }
    }
}