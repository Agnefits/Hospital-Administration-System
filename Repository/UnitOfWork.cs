namespace Hospital_Administration_System.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        DoctorService = new DoctorService(_context);
        NurseService = new NurseService(_context);
        MedicalRecordService = new MedicalRecordService(_context);
        PatientService = new PatientService(_context);
        ReservationService = new ReservationService(_context);
        UserService = new UserService(_context);
        PrescriptionService = new PrescriptionService(_context);
        BillingService = new BillingService(_context);
    }
    public IDoctorRepository DoctorService { get; private set; }

    public INurseRepository NurseService { get; private set; }

    public IMedicalRecordRepository MedicalRecordService { get; private set; }

    public IPatientRepository PatientService { get; private set; }

    public IReservationRepository ReservationService { get; private set; }

    public IUserRepository UserService { get; private set; }
    public IPrescriptionRepository PrescriptionService { get; private set; }
    public IBillingRepository BillingService { get; private set; }
    public int Complete()
    {
        return _context.SaveChanges();
    }
}
