using Hospital_Administration_System.Controllers.Doctor_Controllers;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Hospital_Administration_System.ViewModels.Doctor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hospital_Administration_System.Test.ControllerTests.Doctor
{
    [TestFixture]
    public class DoctorControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private DoctorController _controller;
        private ClaimsPrincipal _user;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new DoctorController(_mockUnitOfWork.Object);

            // Setup test user with Doctor role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                new Claim(ClaimTypes.Role, "Doctor")
            };
            _user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = _user }
            };
            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task Index_WithValidUser_ReturnsViewWithReservations()
        {
            // Arrange
            var expectedReservations = new List<Reservation>();
            var user = new User { Doctor = new Models.Doctor { DoctorID = 1 } };

            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.DoctorService.GetDoctorReservationsAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedReservations);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedReservations));
        }

        [Test]
        public async Task Index_WithInvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public async Task Patients_WithValidUser_ReturnsViewWithPatients()
        {
            // Arrange
            var expectedPatients = new List<Models.Patient>();
            var user = new User { Doctor = new Models.Doctor { DoctorID = 1, DepartmentID = 1 } };

            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.DoctorService.GetDepartmentPatientsAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedPatients);

            // Act
            var result = await _controller.Patients();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedPatients));
        }

        [Test]
        public async Task Prescriptions_WithValidUser_ReturnsViewWithPrescriptions()
        {
            // Arrange
            var expectedPrescriptions = new List<Prescription>();
            var user = new User { Doctor = new Models.Doctor { DoctorID = 1, DepartmentID = 1 } };

            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.DoctorService.GetDepartmentPrescriptionsAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedPrescriptions);

            // Act
            var result = await _controller.Prescriptions();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedPrescriptions));
        }

        [Test]
        public async Task MedicalRecords_WithValidUser_ReturnsViewWithRecords()
        {
            // Arrange
            var expectedRecords = new List<MedicalRecord>();
            var user = new User { Doctor = new Models.Doctor { DoctorID = 1, DepartmentID = 1 } };
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(7);
            var patientName = "Test Patient";

            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.DoctorService.GetDepartmentMedicalsAsync(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()))
                .ReturnsAsync(expectedRecords);

            // Act
            var result = await _controller.MedicalRecords(startDate, endDate, patientName);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedRecords));
            Assert.That(_controller.ViewData["StartDate"], Is.EqualTo(startDate.ToString("yyyy-MM-dd")));
            Assert.That(_controller.ViewData["EndDate"], Is.EqualTo(endDate.ToString("yyyy-MM-dd")));
            Assert.That(_controller.ViewData["PatientName"], Is.EqualTo(patientName));
        }

        [Test]
        public async Task UpdateReservationStatus_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new ReservationEditStatusVM { ReservationID = 1, ReservationStatus = ReservationStatus.Confirmed };
            var response = new DoctorResponseVM { Succeeded = true };

            _mockUnitOfWork.Setup(x => x.DoctorService.UpdateReservationStatusAsync(It.IsAny<ReservationEditStatusVM>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateReservationStatus(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task UpdateReservationStatus_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var model = new ReservationEditStatusVM();
            _controller.ModelState.AddModelError("Error", "Model error");

            // Act
            var result = await _controller.UpdateReservationStatus(model);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task RedirectReservation_Get_WithValidId_ReturnsViewWithDoctors()
        {
            // Arrange
            var reservation = new Reservation { ReservationID = 1 };
            var doctors = new List<Models.Doctor>();

            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(reservation);
            _mockUnitOfWork.Setup(x => x.DoctorService.GetDoctorsAsync())
                .ReturnsAsync(doctors);

            // Act
            var result = await _controller.RedirectReservation(1);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(reservation));
            Assert.That(_controller.ViewData["Doctors"], Is.EqualTo(doctors));
        }

        [Test]
        public async Task RedirectReservation_Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Reservation)null);

            // Act
            var result = await _controller.RedirectReservation(1);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task RedirectReservation_Post_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new ReservationRedirectionVM { OldReservationID = 1, DoctorID = 2 };
            var response = new DoctorResponseVM { Succeeded = true };

            _mockUnitOfWork.Setup(x => x.DoctorService.RedirectReservationAsync(It.IsAny<ReservationRedirectionVM>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.RedirectReservation(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task RedirectReservation_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            var model = new ReservationRedirectionVM();
            _controller.ModelState.AddModelError("Error", "Model error");

            var doctors = new List<Models.Doctor>();
            _mockUnitOfWork.Setup(x => x.DoctorService.GetDoctorsAsync())
                .ReturnsAsync(doctors);

            // Act
            var result = await _controller.RedirectReservation(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }
    }
} 