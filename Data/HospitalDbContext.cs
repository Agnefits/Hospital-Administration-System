using Microsoft.EntityFrameworkCore;
using Hospital_Administration_System.Models;

namespace Hospital_Administration_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<Laboratory> Laboratories { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Analysis> Analyses { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory) // Ensures the correct path
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection"); // Use the correct key name
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string is missing.");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            //User unique values
            modelBuilder.Entity<User>().HasIndex(u => new { u.Username, u.Email }).IsUnique();

            //User one-to-one relationships
            modelBuilder.Entity<Admin>().HasOne(a => a.User).WithOne().HasForeignKey<Admin>(a => a.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Doctor>().HasOne(d => d.User).WithOne().HasForeignKey<Doctor>(d => d.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Pharmacist>().HasOne(p => p.User).WithOne().HasForeignKey<Pharmacist>(p => p.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Nurse>().HasOne(n => n.User).WithOne().HasForeignKey<Nurse>(n => n.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Patient>().HasOne(p => p.User).WithOne().HasForeignKey<Patient>(p => p.UserID).OnDelete(DeleteBehavior.Restrict);

            //Department relationships
            modelBuilder.Entity<Department>().HasOne(d => d.Branch).WithMany().HasForeignKey(d => d.BranchID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Department>().HasOne(d => d.HeadDoctor).WithMany().HasForeignKey(d => d.HeadDoctorID).OnDelete(DeleteBehavior.SetNull);

            //Doctor and nurse with department relationships
            modelBuilder.Entity<Doctor>().HasOne(d => d.Department).WithMany().HasForeignKey(d => d.DepartmentID).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Nurse>().HasOne(n => n.Department).WithMany().HasForeignKey(n => n.DepartmentID).OnDelete(DeleteBehavior.SetNull);

            //Pharmacy relationships
            modelBuilder.Entity<Pharmacy>().HasOne(p => p.Branch).WithMany().HasForeignKey(p => p.BranchID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Pharmacist>().HasOne(p => p.Pharmacy).WithMany().HasForeignKey(p => p.PharmacyID).OnDelete(DeleteBehavior.Restrict);

            //Laboratory relationships
            modelBuilder.Entity<Laboratory>().HasOne(l => l.Branch).WithMany().HasForeignKey(l => l.BranchID).OnDelete(DeleteBehavior.Cascade);

            //Reservation relationships
            modelBuilder.Entity<Reservation>().HasOne(r => r.Patient).WithMany().HasForeignKey(r => r.PatientID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reservation>().HasOne(r => r.Doctor).WithMany().HasForeignKey(r => r.DoctorID).OnDelete(DeleteBehavior.Restrict);

            //Analysis relationships
            modelBuilder.Entity<Analysis>().HasOne(a => a.Patient).WithMany().HasForeignKey(a => a.PatientID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Analysis>().HasOne(a => a.Laboratory).WithMany().HasForeignKey(a => a.LabID).OnDelete(DeleteBehavior.Restrict);

            //Patient items relationships
            modelBuilder.Entity<Receipt>().HasOne(r => r.Patient).WithMany().HasForeignKey(r => r.PatientID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MedicalRecord>().HasOne(m => m.Patient).WithMany().HasForeignKey(m => m.PatientID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MedicalRecord>().HasOne(m => m.Doctor).WithMany().HasForeignKey(m => m.DoctorID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Prescription>().HasOne(p => p.Patient).WithMany().HasForeignKey(p => p.PatientID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Prescription>().HasOne(p => p.Doctor).WithMany().HasForeignKey(p => p.DoctorID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Billing>().HasOne(b => b.Patient).WithMany().HasForeignKey(b => b.PatientID).OnDelete(DeleteBehavior.Cascade);

            //Log relationships
            modelBuilder.Entity<Log>().HasOne(l => l.User).WithMany().HasForeignKey(l => l.UserID).OnDelete(DeleteBehavior.Restrict);
            */
            //Adding Default admin user
            modelBuilder.Entity<User>().HasData(new User
            {
                UserID = 1,
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"), // Hashing password before adding
                Email = "admin@example.com",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                AdditionalData = null,
                Deleted = false
            });

            modelBuilder.Entity<Admin>().HasData(new Admin
            {
                AdminID = 1,
                UserID = 1,
                FullName = "Admin",
                ContactNumber = "0000000000",
                AdditionalData = null,
                Deleted = false
            });

        }
    }

}
