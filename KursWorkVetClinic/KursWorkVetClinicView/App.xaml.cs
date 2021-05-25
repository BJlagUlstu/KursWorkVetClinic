using KursWorkVetClinicBusinessLogic.BusinessLogic;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicDatabaseImplements.Implements;
using Unity;
using Unity.Lifetime;
using System.Windows;
using KursWorkVetClinicBusinessLogic.HelperModels;
using System.Configuration;
using System;
using KursWorkVetClinicBusinessLogic.ViewModels;

namespace KursWorkVetClinicView
{
    public partial class App : Application
    {
        public static UserViewModel User { get; set; }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var container = BuildUnityContainer();
            var form = container.Resolve<FirstWindow>();
            SetMailConfig();
            form.ShowDialog();
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IAnimalStorage, AnimalStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPurchaseStorage, PurchaseStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IVisitStorage, VisitStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IUserStorage, UserStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMedicineStorage, MedicineStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IServiceStorage, ServiceStorage>(new HierarchicalLifetimeManager());

            currentContainer.RegisterType<AnimalLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<PurchaseLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<MedicineLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<VisitLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<UserLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ReportLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ServiceLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<StatisticsLogic>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
        private void SetMailConfig()
        {
            MailLogic.MailConfig(new MailConfig
            {
                SmtpHost = ConfigurationManager.AppSettings["SmtpHost"],
                SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]),
                MailLogin = ConfigurationManager.AppSettings["MailLogin"],
                MailPassword = ConfigurationManager.AppSettings["MailPassword"],
            });
        }
    }
}