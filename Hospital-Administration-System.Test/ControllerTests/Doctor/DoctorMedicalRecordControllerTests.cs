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
    public class DoctorMedicalRecordControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private DoctorMedicalRecordController _controller;
        private ClaimsPrincipal _user;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new DoctorMedicalRecordController(_mockUnitOfWork.Object);

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
        public async Task Create_Get_ReturnsViewWithPatients()
        {
            // Arrange
            var expectedPatients = new List<Models.Patient>();
            _mockUnitOfWork.Setup(x => x.PatientService.GetAllAsync())
                .ReturnsAsync(expectedPatients);

            // Act
            var result = await _controller.Create();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(_controller.ViewData["Patients"], Is.EqualTo(expectedPatients));
        }

        [Test]
        public async Task Create_Post_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new MedicalRecordCreateVM();
            var user = new User { Doctor = new Models.Doctor { DoctorID = 1 } };
            var response = new DoctorResponseVM { Succeeded = true };

            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.MedicalRecordService.AddAsync(It.IsAny<MedicalRecordCreateVM>(), It.IsAny<int>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Create_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            var model = new MedicalRecordCreateVM();
            _controller.ModelState.AddModelError("Error", "Model error");

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task Create_Post_WithInvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            var model = new MedicalRecordCreateVM();
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        public async Task Edit_Get_WithValidId_ReturnsViewWithMedicalRecord()
        {
            // Arrange
            var expectedMedicalRecord = new MedicalRecord { RecordID = 1 };
            var patients = new List<Models.Patient>();

            _mockUnitOfWork.Setup(x => x.MedicalRecordService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedMedicalRecord);
            _mockUnitOfWork.Setup(x => x.PatientService.GetAllAsync())
                .ReturnsAsync(patients);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedMedicalRecord));
            Assert.That(_controller.ViewData["Patients"], Is.EqualTo(patients));
        }

        [Test]
        public async Task Edit_Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.MedicalRecordService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((MedicalRecord)null);
            _mockUnitOfWork.Setup(x => x.PatientService.GetAllAsync())
                .ReturnsAsync(new List<Models.Patient>());

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Edit_Post_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new MedicalRecordEditVM { RecordID = 1 };
            var response = new DoctorResponseVM { Succeeded = true };

            _mockUnitOfWork.Setup(x => x.MedicalRecordService.UpdateAsync(It.IsAny<MedicalRecordEditVM>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Edit(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Edit_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            var model = new MedicalRecordEditVM();
            _controller.ModelState.AddModelError("Error", "Model error");

            // Act
            var result = await _controller.Edit(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task Delete_WithValidId_RedirectsToIndex()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.MedicalRecordService.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Delete_WithInvalidId_RedirectsToIndexWithError()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.MedicalRecordService.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(_controller.ModelState.ErrorCount, Is.GreaterThan(0));
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }
    }
} 