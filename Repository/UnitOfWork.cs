namespace Hospital_Administration_System.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UnitOfWork(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;

        DoctorService = new DoctorService(_context);
        NurseService = new NurseService(_context);
        MedicalRecordService = new MedicalRecordService(_context);
        PatientService = new PatientService(_context);
        ReservationService = new ReservationService(_context);
        UserService = new UserService(_context, _userManager, _roleManager);
        PrescriptionService = new PrescriptionService(_context);
        BillingService = new BillingService(_context);
        BranchService = new BranchService(_context);
        DepartmentService = new DepartmentService(_context);
        PharmacistService = new PharmacistService(_context);
        PatientConditionMonitoringService = new PatientConditionMonitoringService(_context);
        LogService = new LogService(_context);

    }
    public IDoctorRepository DoctorService { get; private set; }

    public INurseRepository NurseService { get; private set; }

    public IMedicalRecordRepository MedicalRecordService { get; private set; }

    public IPatientRepository PatientService { get; private set; }

    public IReservationRepository ReservationService { get; private set; }

    public IUserRepository UserService { get; private set; }
    public IPrescriptionRepository PrescriptionService { get; private set; }
    public IBillingRepository BillingService { get; private set; }
    public IBranchRepository BranchService { get; private set; }
    public IDepartmentRepository DepartmentService { get; private set; }
    public IPharmacistRepository PharmacistService { get; private set; }
    public IPatientConditionMonitoring PatientConditionMonitoringService { get; private set; }

    public ILogRepository LogService { get; private set; }

    public int Complete()
    {
        return _context.SaveChanges();
    }
}
