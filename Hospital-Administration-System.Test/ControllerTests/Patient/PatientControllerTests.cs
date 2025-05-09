using Hospital_Administration_System.Controllers.Patient_Controllers;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Hospital_Administration_System.ViewModels.Reservation;
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

namespace Hospital_Administration_System.Test.ControllerTests.Patient
{
    [TestFixture]
    public class PatientControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private PatientController _controller;
        private ClaimsPrincipal _user;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new PatientController(_mockUnitOfWork.Object);

            // Setup test user with Patient role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                new Claim(ClaimTypes.Role, "Patient")
            };
            _user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            _controller.ControllerContext = new ControllerContext
            {
                
                HttpContext = new DefaultHttpContext { User = _user }
            };
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void PatientDashboard_ReturnsViewResult()
        {
            // Act
            var result = _controller.PatientDashboard();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task Reservations_WithValidUser_ReturnsViewWithReservations()
        {
            // Arrange
            var expectedReservations = new List<Reservation>();
            var user = new User { Patient = new Models.Patient { PatientID = 1 } };
            
            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.PatientService.GetPatientReservations(It.IsAny<int>()))
                .ReturnsAsync(expectedReservations);

            // Act
            var result = await _controller.Reservations();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedReservations));
        }

        [Test]
        public async Task Reservations_WithInvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.Reservations();

            // Assert
            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public async Task AddReservation_Get_ReturnsViewWithDoctors()
        {
            // Arrange
            var expectedDoctors = new List<Models.Doctor>();
            _mockUnitOfWork.Setup(x => x.DoctorService.GetDoctorsAsync())
                .ReturnsAsync(expectedDoctors);

            // Act
            var result = await _controller.AddReservation();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(_controller.ViewData["Doctors"], Is.EqualTo(expectedDoctors));
        }

        [Test]
        public async Task AddReservation_Post_WithValidModel_RedirectsToReservations()
        {
            // Arrange
            var model = new ReservationCreateVM();
            var user = new User { Patient = new Models.Patient { PatientID = 1 } };
            var response = new ReservationResponseVM { Succeeded = true };

            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.ReservationService.AddAsync(It.IsAny<ReservationCreateVM>()))
                .ReturnsAsync(response);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.AddReservation(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Reservations"));
        }

        [Test]
        public async Task AddReservation_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            var model = new ReservationCreateVM();
            _controller.ModelState.AddModelError("Error", "Model error");


            var doctors = new List<Models.Doctor>
            {
                new Models.Doctor { DoctorID = 1, FullName = "Cardiology", UserID = "1", DepartmentID = 1}
            };

            _mockUnitOfWork.Setup(x => x.DoctorService.GetDoctorsAsync())
                .ReturnsAsync(doctors);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.AddReservation(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task Prescriptions_WithValidUser_ReturnsJsonResult()
        {
            // Arrange
            var expectedPrescriptions = new List<Prescription>();
            var user = new User { Patient = new Models.Patient { PatientID = 1 } };

            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.PatientService.GetPatientPrescriptions(It.IsAny<int>()))
                .ReturnsAsync(expectedPrescriptions);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.Prescriptions();

            // Assert
            Assert.That(result, Is.TypeOf<JsonResult>());
            var jsonResult = (JsonResult)result;
            Assert.That(jsonResult.Value, Is.EqualTo(expectedPrescriptions));
        }

        [Test]
        public async Task Medicals_WithValidUser_ReturnsViewResult()
        {
            // Arrange
            var expectedMedicals = new List<MedicalRecord>();
            var user = new User { Patient = new Models.Patient { PatientID = 1 } };

            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.PatientService.GetPatientMedicals(It.IsAny<int>()))
                .ReturnsAsync(expectedMedicals);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.Medicals();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedMedicals));
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }
    }
} 