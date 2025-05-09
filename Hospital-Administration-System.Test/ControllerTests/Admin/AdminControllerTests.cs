using Hospital_Administration_System.Controllers.Admin_Controllers;
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
using System.Threading.Tasks;

namespace Hospital_Administration_System.Test.ControllerTests.Admin
{
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private AdminController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new AdminController(_mockUnitOfWork.Object);
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
        public async Task Logs_WithValidParameters_ReturnsViewResult()
        {
            // Arrange
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(7);
            var search = "test";
            var expectedLogs = new List<Log>();
            _mockUnitOfWork.Setup(x => x.LogService.GetFilteredLogsAsync(startDate, endDate, search))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _controller.Logs(startDate, endDate, search);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedLogs));
            Assert.That(_controller.ViewData["StartDate"], Is.EqualTo(startDate.ToString("yyyy-MM-dd")));
            Assert.That(_controller.ViewData["EndDate"], Is.EqualTo(endDate.ToString("yyyy-MM-dd")));
            Assert.That(_controller.ViewData["Search"], Is.EqualTo(search));
        }

        [Test]
        public async Task Logs_WithNullParameters_ReturnsViewResult()
        {
            // Arrange
            var expectedLogs = new List<Log>();
            _mockUnitOfWork.Setup(x => x.LogService.GetFilteredLogsAsync(null, null, null))
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _controller.Logs(null, null, null);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedLogs));
        }

        [Test]
        public async Task UpcomingAppointments_ReturnsViewResultWithAppointments()
        {
            // Arrange
            var expectedAppointments = new List<Reservation>();
            _mockUnitOfWork.Setup(x => x.ReservationService.GetAllReservationsAsync())
                .ReturnsAsync(expectedAppointments);

            // Act
            var result = await _controller.UpcomingAppointments();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedAppointments));
        }

        [Test]
        public async Task UpdateReservationStatus_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var model = new ReservationEditStatusVM
            {
                ReservationID = 1,
                ReservationStatus = ReservationStatus.Confirmed
            };

            _mockUnitOfWork.Setup(x => x.DoctorService.UpdateReservationStatusAsync(model))
                .ReturnsAsync(new DoctorResponseVM { Succeeded = true });

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.UpdateReservationStatus(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("UpcomingAppointments"));
        }

        [Test]
        public async Task UpdateReservationStatus_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var model = new ReservationEditStatusVM();
            _controller.ModelState.AddModelError("Status", "Status is required");

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.UpdateReservationStatus(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public async Task UpdateReservationStatus_WithFailedUpdate_ReturnsBadRequest()
        {
            // Arrange
            var model = new ReservationEditStatusVM
            {
                ReservationID = 1,
                ReservationStatus = ReservationStatus.Confirmed
            };

            _mockUnitOfWork.Setup(x => x.DoctorService.UpdateReservationStatusAsync(model))
                .ReturnsAsync(new DoctorResponseVM { Succeeded = false, Error = "Update failed" });


            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.UpdateReservationStatus(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var badRequestResult = (RedirectToActionResult)result;
            Assert.That(_controller.TempData["Error"], Is.EqualTo("Update failed"));
        }

        [Test]
        public async Task Billings_ReturnsViewResultWithBillings()
        {
            // Arrange
            var expectedBillings = new List<Billing>();
            _mockUnitOfWork.Setup(x => x.BillingService.GetAllBillingsAsync())
                .ReturnsAsync(expectedBillings);

            // Act
            var result = await _controller.Billings();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(expectedBillings));
        }

        [Test]
        public async Task Billings_WithNoBillings_ReturnsEmptyList()
        {
            // Arrange
            var emptyBillings = new List<Billing>();
            _mockUnitOfWork.Setup(x => x.BillingService.GetAllBillingsAsync())
                .ReturnsAsync(emptyBillings);

            // Act
            var result = await _controller.Billings();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(emptyBillings));
            Assert.That(((List<Billing>)viewResult.Model).Count, Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
} 