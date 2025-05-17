using Hospital_Administration_System.Controllers.Nurse_Controllers;
using Hospital_Administration_System.Data;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Hospital_Administration_System.Services;
using Hospital_Administration_System.ViewModels.Patient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Security.Claims;

namespace Hospital_Administration_System.Tests.Controllers.Nurse_Controllers;

[TestFixture]
public class NurseControllerTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private NurseController _controller;
    private Mock<IReservationRepository> _mockReservationService;
    private Mock<INurseRepository> _mockNurseService;
    private Mock<IPatientConditionMonitoring> _mockPatientConditionMonitoringService;
    private Mock<IPatientRepository> _mockPatientService;
    private Mock<IUserRepository> _mockUserService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockReservationService = new Mock<IReservationRepository>();
        _mockNurseService = new Mock<INurseRepository>();
        _mockPatientConditionMonitoringService = new Mock<IPatientConditionMonitoring>();
        _mockPatientService = new Mock<IPatientRepository>();
        _mockUserService = new Mock<IUserRepository>();

        _mockUnitOfWork.Setup(uow => uow.ReservationService).Returns(_mockReservationService.Object);
        _mockUnitOfWork.Setup(uow => uow.NurseService).Returns(_mockNurseService.Object);
        _mockUnitOfWork.Setup(uow => uow.PatientConditionMonitoringService).Returns(_mockPatientConditionMonitoringService.Object);
        _mockUnitOfWork.Setup(uow => uow.PatientService).Returns(_mockPatientService.Object);
        _mockUnitOfWork.Setup(uow => uow.UserService).Returns(_mockUserService.Object);

        _controller = new NurseController(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task Appointments_WhenCalled_ReturnsViewWithAppointments()
    {
        // Arrange
        var appointments = new List<Reservation>();
        _mockReservationService.Setup(service => service.GetAllReservationsAsync())
            .ReturnsAsync(appointments);

        // Act
        var result = await _controller.AppointmentsAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult.Model, Is.InstanceOf<IEnumerable<Reservation>>());
        Assert.That(viewResult.Model, Is.EqualTo(appointments));
    }

    [Test]
    public async Task Index_WhenUserIsNurse_ReturnsViewWithPatientMonitoring()
    {
        // Arrange
        var userId = "test-user-id";
        var nurse = new Nurse { NurseID = 1, FullName = "Test Nurse" };
        var patientMonitoring = new List<PatientConditionMonitoring>();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, "Nurse")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        _mockNurseService.Setup(service => service.GetByUserIdAsync(userId))
            .ReturnsAsync(nurse);
        _mockPatientConditionMonitoringService.Setup(service => service.GetAllPatientConditionMonitoringAsync(nurse.NurseID))
            .ReturnsAsync(patientMonitoring);

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult.Model, Is.InstanceOf<IEnumerable<PatientConditionMonitoring>>());
        Assert.That(viewResult.Model, Is.EqualTo(patientMonitoring));
    }

    [Test]
    public async Task Index_WhenUserIsNotNurse_ReturnsUnauthorized()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "Doctor")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
    }

    [Test]
    public async Task AddPatient_WhenUserIsNurse_ReturnsViewWithViewModel()
    {
        // Arrange
        var userId = "test-user-id";
        var user = new User { Nurse = new Nurse { NurseID = 1, FullName = "Test Nurse" } };
        var patients = new List<Patient>();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, "Nurse")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        _mockUserService.Setup(service => service.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _mockNurseService.Setup(service => service.GetByIdAsync(user.Nurse.NurseID))
            .ReturnsAsync(user.Nurse);
        _mockPatientService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(patients);

        // Act
        var result = await _controller.AddPatient();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        var model = viewResult.Model as PatientConditionMonitoringVM;
        Assert.That(model.NurseId, Is.EqualTo(user.Nurse.NurseID));
        Assert.That(model.NurseName, Is.EqualTo(user.Nurse.FullName));
    }

    [Test]
    public async Task AddPatient_WhenUserIsNotNurse_RedirectsToHomeIndex()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "Doctor")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        // Act
        var result = await _controller.AddPatient();

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
    }

    [Test]
    public async Task AddPatient_WithValidModel_RedirectsToIndex()
    {
        // Arrange
        var viewModel = new PatientConditionMonitoringVM
        {
            PatientId = 1,
            NurseId = 1,
            Temperature = 37.0f,
            HeartRate = 80,
            SystolicBP = 120,
            DiastolicBP = 80,
            RespiratoryRate = 16,
            OxygenSaturation = 98
        };

        // Act
        var result = await _controller.AddPatient(viewModel);

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        _mockPatientConditionMonitoringService.Verify(service =>
            service.AddPatientConditionMonitoring(viewModel), Times.Once);
    }

    [Test]
    public async Task AddPatient_WithInvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var viewModel = new PatientConditionMonitoringVM();
        var patients = new List<Patient>();

        _controller.ModelState.AddModelError("Temperature", "Required");
        _mockPatientService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(patients);

        // Act
        var result = await _controller.AddPatient(viewModel);

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult.Model, Is.EqualTo(viewModel));
    }

    [Test]
    public async Task UpdatePatientData_WhenPatientExists_ReturnsViewWithViewModel()
    {
        // Arrange
        var patientId = 1;
        var patientMonitoring = new PatientConditionMonitoring
        {
            PatientId = patientId,
            Patient = new Patient { FullName = "Test Patient", Gender = "Male" },
            Age = 30,
            Temperature = 37.0f,
            HeartRate = 80,
            SystolicBP = 120,
            DiastolicBP = 80,
            RespiratoryRate = 16,
            OxygenSaturation = 98,
            NurseId = 1,
            Nurse = new Nurse { FullName = "Test Nurse" }
        };
        var patients = new List<Patient>();

        _mockPatientConditionMonitoringService.Setup(service => service.GetByIdAsync(patientId))
            .ReturnsAsync(patientMonitoring);
        _mockPatientService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(patients);

        // Act
        var result = await _controller.UpdatePatientData(patientId);

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        var model = viewResult.Model as PatientConditionMonitoringVM;
        Assert.That(model.PatientId, Is.EqualTo(patientMonitoring.PatientId));
        Assert.That(model.FullName, Is.EqualTo(patientMonitoring.Patient.FullName));
        Assert.That(model.Temperature, Is.EqualTo(patientMonitoring.Temperature));
    }

    [Test]
    public async Task UpdatePatientData_WhenPatientDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var patientId = 1;
        _mockPatientConditionMonitoringService.Setup(service => service.GetByIdAsync(patientId))
            .ReturnsAsync((PatientConditionMonitoring)null);

        // Act
        var result = await _controller.UpdatePatientData(patientId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task UpdateAsync_WithValidModel_RedirectsToIndex()
    {
        // Arrange
        var viewModel = new PatientConditionMonitoringVM
        {
            PatientId = 1,
            NurseId = 1,
            Temperature = 37.0f,
            HeartRate = 80,
            SystolicBP = 120,
            DiastolicBP = 80,
            RespiratoryRate = 16,
            OxygenSaturation = 98
        };

        // Act
        var result = await _controller.UpdateAsync(viewModel);

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        _mockPatientConditionMonitoringService.Verify(service =>
            service.UpdatePatientConditionMonitoring(viewModel), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_WithInvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var viewModel = new PatientConditionMonitoringVM();
        var patients = new List<Patient>();

        _controller.ModelState.AddModelError("Temperature", "Required");
        _mockPatientService.Setup(service => service.GetAllAsync())
            .ReturnsAsync(patients);

        // Act
        var result = await _controller.UpdateAsync(viewModel);

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        var viewResult = result as ViewResult;
        Assert.That(viewResult.ViewName, Is.EqualTo("UpdatePatientData"));
        Assert.That(viewResult.Model, Is.EqualTo(viewModel));
    }


    [TearDown]
    public void TearDown()
    {
        _controller?.Dispose();
    }
}