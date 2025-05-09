using Hospital_Administration_System.Controllers.Patient_Controllers;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Hospital_Administration_System.ViewModels.Reservation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Administration_System.Test.ControllerTests.Patient
{
    [TestFixture]
    public class ReservationControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private ReservationController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new ReservationController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Details_WithValidId_ReturnsViewWithReservation()
        {
            // Arrange
            var expectedReservation = new Reservation { ReservationID = 1 };
            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedReservation);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedReservation));
        }

        [Test]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Reservation)null);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_Get_ReturnsViewWithDoctors()
        {
            // Arrange
            var expectedDoctors = new List<Doctor>();
            _mockUnitOfWork.Setup(x => x.DoctorService.GetDoctorsAsync())
                .ReturnsAsync(expectedDoctors);

            // Act
            var result = await _controller.Create();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            var model = (ViewModels.ReservationViewModel)viewResult.Model;
            Assert.That(model.Doctors, Is.Not.Null);
        }

        [Test]
        public async Task Edit_Post_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new ViewModels.ReservationViewModel { ReservationID = 1 };
            var existingReservation = new Reservation { ReservationID = 1 };
            
            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(existingReservation);
            _mockUnitOfWork.Setup(x => x.DoctorService.GetDoctorsAsync())
                .ReturnsAsync(new List<Doctor>());

            // Act
            var result = await _controller.Edit(1, model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public async Task Edit_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            var model = new ViewModels.ReservationViewModel { ReservationID = 1 };
            _controller.ModelState.AddModelError("Error", "Model error");


            var doctors = new List<Doctor>
            {
                new Doctor { DepartmentID = 1, FullName = "Name", UserID = "1"}
            };

            _mockUnitOfWork.Setup(x => x.DoctorService.GetDoctorsAsync())
                .ReturnsAsync(doctors);

            // Act
            var result = await _controller.Edit(1, model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public async Task Edit_Post_WithNonExistentReservation_ReturnsNotFound()
        {
            // Arrange
            var model = new ViewModels.ReservationViewModel { ReservationID = 1 };
            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Reservation)null);

            // Act
            var result = await _controller.Edit(1, model);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_WithValidId_ReturnsViewWithReservation()
        {
            // Arrange
            var expectedReservation = new Reservation { ReservationID = 1 };
            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedReservation);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedReservation));
        }

        [Test]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Reservation)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteConfirmed_WithValidId_RedirectsToIndex()
        {
            // Arrange
            var reservation = new Reservation { ReservationID = 1 };
            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(reservation);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task DeleteConfirmed_WithInvalidId_RedirectsToIndex()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.ReservationService.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Reservation)null);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }
    }
} 