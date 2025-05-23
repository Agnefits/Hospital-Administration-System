﻿
namespace Hospital_Administration_System.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
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
    public DbSet<PatientConditionMonitoring> PatientConditionMonitorings { get; set; } 

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PatientConditionMonitoring>()
       .HasOne(m => m.Patient)
       .WithMany(p => p.PatientConditionMonitorings)
       .HasForeignKey(m => m.PatientId)
       .OnDelete(DeleteBehavior.Restrict); // or NoAction

        modelBuilder.Entity<PatientConditionMonitoring>()
            .HasOne(m => m.Nurse)
            .WithMany(n => n.PatientConditionMonitorings)
            .HasForeignKey(m => m.NurseId)
            .OnDelete(DeleteBehavior.Restrict); // or NoAction

        modelBuilder.Entity<Reservation>().Property(x => x.Status).HasConversion<string>(); // map reservation status to string
        // Configure relationships that are known to cause cascade issues:
        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Patient)
            .WithMany(p => p.MedicalRecords)  // Ensure your Patient model has a MedicalRecords collection
            .HasForeignKey(m => m.PatientID)
            .OnDelete(DeleteBehavior.Restrict); // Avoids multiple cascade paths

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Doctor)
            .WithMany(d => d.MedicalRecords)  // Ensure your Doctor model has a MedicalRecords collection
            .HasForeignKey(m => m.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        // Prescription relationships (avoiding cascade delete issues)
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany(pat => pat.Prescriptions)
            .HasForeignKey(p => p.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Doctor)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        // Reservation relationships updated to avoid multiple cascade paths
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Patient)
            .WithMany(p => p.Reservations)
            .HasForeignKey(r => r.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Doctor)
            .WithMany(d => d.Reservations)
            .HasForeignKey(r => r.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure decimal precision to avoid silent truncation
        modelBuilder.Entity<Billing>()
            .Property(b => b.TotalAmount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Receipt>()
            .Property(r => r.Amount)
            .HasColumnType("decimal(18,2)");

        // Configure decimal precision to avoid silent truncation
        modelBuilder.Entity<Billing>()
            .Property(b => b.TotalAmount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Receipt>()
            .Property(r => r.Amount)
            .HasColumnType("decimal(18,2)");
        
        /*
        //User unique values
        modelBuilder.Entity<User>().HasIndex(u => new { u.UserName, u.Email }).IsUnique();

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
    }
}
