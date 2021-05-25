using Microsoft.EntityFrameworkCore;
using KursWorkVetClinicDatabaseImplements.Models;

namespace KursWorkVetClinicDatabaseImplements
{
    class VetClinicDataBase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-UI0H4F9\SQLEXPRESS;Initial Catalog=KursWorkVetClinicDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Animal> Animals { set; get; }
        public virtual DbSet<AnimalPurchase> AnimalsPurchases { set; get; }
        public virtual DbSet<AnimalVisit> AnimalsVisits { set; get; }
        public virtual DbSet<Doctor> Doctors { set; get; }
        public virtual DbSet<Medication> Medications { set; get; }
        public virtual DbSet<MedicationMedicine> MedicationsMedicines { set; get; }
        public virtual DbSet<MedicationService> MedicationsServices { set; get; }
        public virtual DbSet<Medicine> Medicines { set; get; }
        public virtual DbSet<MedicinePurchase> MedicinesPurchases { set; get; }
        public virtual DbSet<Purchase> Purchases { set; get; }
        public virtual DbSet<Service> Services { set; get; }
        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<Visit> Visits { set; get; }
        public virtual DbSet<VisitService> VisitsServices { set; get; }
    }
}