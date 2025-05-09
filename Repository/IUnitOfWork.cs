namespace Hospital_Administration_System.Repository;

public interface IUnitOfWork
{
    public IDoctorRepository DoctorService { get; }
    public INurseRepository NurseService { get; }
    public IMedicalRecordRepository MedicalRecordService { get; }
    public IPatientRepository PatientService { get; }
    public IReservationRepository ReservationService { get; }
    public IUserRepository UserService { get; }
    public IPrescriptionRepository PrescriptionService { get; }
    public IBillingRepository BillingService { get; }
    public IBranchRepository BranchService { get; }
    public IDepartmentRepository DepartmentService { get; }
    public IPharmacistRepository PharmacistService { get; }
    public IPatientConditionMonitoring PatientConditionMonitoringService { get; }
    public ILogRepository LogService { get; }
    int Complete();
    //public IAppointmentRepository AppointmentRepository { get; }
}
