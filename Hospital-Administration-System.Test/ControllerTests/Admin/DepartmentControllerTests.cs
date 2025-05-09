using Hospital_Administration_System.Controllers.Admin_Controllers;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Hospital_Administration_System.ViewModels.Department;
using Hospital_Administration_System.ViewModels.Doctor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NuGet.Protocol;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital_Administration_System.Test.ControllerTests.Admin
{
    [TestFixture]
    public class DepartmentControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private DepartmentController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new DepartmentController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Index_ReturnsViewResultWithDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department
                {
                    DepartmentID = 1,
                    Name = "Cardiology",
                    AdditionalData = "Heart care department",
                    BranchID = 1
                },
                new Department
                {
                    DepartmentID = 2,
                    Name = "Neurology",
                    AdditionalData = "Brain and nervous system care",
                    BranchID = 1
                }
            };
            _mockUnitOfWork.Setup(x => x.DepartmentService.GetAllDepartmentsAsync())
                .ReturnsAsync(departments);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.AssignableFrom<List<Department>>());
            Assert.That(viewResult.Model, Is.EqualTo(departments));
            Assert.That(((IEnumerable<Department>)viewResult.Model).Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task Index_WithNoDepartments_ReturnsEmptyList()
        {
            // Arrange
            var emptyDepartments = new List<Department>();
            _mockUnitOfWork.Setup(x => x.DepartmentService.GetAllDepartmentsAsync())
                .ReturnsAsync(emptyDepartments);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.AssignableFrom<List<Department>>());
            Assert.That(((IEnumerable<Department>)viewResult.Model).Count(), Is.EqualTo(0));
        }

        [Test]
        public void Create_ReturnsViewResultWithViewBagData()
        {
            // Arrange
            var branches = new List<Branch>
            {
                new Branch { BranchID = 1, Name = "Main Branch", Location = "456 New St", ContactNumber = "987-654-3210" }
            };
            var doctors = new List<User>
            {
                new User {
                    Id= "1",
                    Email = "test@email.com",
                    UserName = "test@email.com",
                    Doctor = new Models.Doctor { UserID = "1", DoctorID = 1, FullName = "Dr. Smith" }
                }
            };

            _mockUnitOfWork.Setup(x => x.BranchService.GetAllActiveBranches())
                .Returns(branches);
            _mockUnitOfWork.Setup(x => x.UserService.GetActiveDoctors())
                .Returns(doctors);

            // Act
            var result = _controller.Create();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(_controller.ViewBag.Branches, Is.Not.Null);
            Assert.That(_controller.ViewBag.Doctors, Is.Not.Null);
            Assert.That(((SelectList)_controller.ViewBag.Branches).Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Create_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var model = new DepartmentCreateVM
            {
                Name = "New Department",
                AdditionalData = "Department description",
                BranchID = 1,
                HeadDoctorID = "1"
            };
            _mockUnitOfWork.Setup(x => x.DepartmentService.AddAsync(model))
                .ReturnsAsync(new DepartmentResponseVM { Succeeded = true });

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(_controller.ModelState.IsValid, Is.True);
        }

        [Test]
        public async Task Create_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new DepartmentCreateVM();
            _controller.ModelState.AddModelError("Name", "Name is required");
            _controller.ModelState.AddModelError("BranchID", "Branch is required");

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(model));
            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(_controller.ModelState.ErrorCount, Is.EqualTo(2));
        }

        [Test]
        public async Task Create_WithFailedAdd_ReturnsViewResult()
        {
            // Arrange
            var model = new DepartmentCreateVM
            {
                Name = "New Department",
                AdditionalData = "Department description",
                BranchID = 1,
                HeadDoctorID = "1"
            };
            _mockUnitOfWork.Setup(x => x.DepartmentService.AddAsync(model))
                .ReturnsAsync(new DepartmentResponseVM { Succeeded = false, Error = "Failed to add department" });

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(model));
            Assert.That(_controller.ModelState.ErrorCount, Is.GreaterThan(0));
        }

        [Test]
        public async Task Edit_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var departmentId = 1;
            var department = new Department
            {
                DepartmentID = departmentId,
                Name = "Test Department",
                AdditionalData = "Test Description",
                BranchID = 1,
                HeadDoctorID = "1"
            };
            var branches = new List<Branch>
            {
                new Branch { BranchID = 1, Name = "Main Branch" }
            };
            var doctors = new List<User>
            {
                new User {
                    Id= "1",
                    Email = "test@email.com",
                    UserName = "test@email.com",
                    Doctor = new Models.Doctor { UserID = "1", DoctorID = 1, FullName = "Dr. Smith" }
                }
            };

            _mockUnitOfWork.Setup(x => x.DepartmentService.GetByIdAsync(departmentId))
                .ReturnsAsync(department);

            _mockUnitOfWork.Setup(x => x.BranchService.GetAllActiveBranches())
                .Returns(branches);
            _mockUnitOfWork.Setup(x => x.UserService.GetActiveDoctors())
                .Returns(doctors);

            // Act
            var result = await _controller.Edit(departmentId);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            var model = (DepartmentEditVM)viewResult.Model;
            Assert.That(model.DepartmentID, Is.EqualTo(department.DepartmentID));
            Assert.That(model.Name, Is.EqualTo(department.Name));
            Assert.That(model.AdditionalData, Is.EqualTo(department.AdditionalData));
            Assert.That(model.BranchID, Is.EqualTo(department.BranchID));
            Assert.That(model.HeadDoctorID, Is.EqualTo(department.HeadDoctorID));
            Assert.That(_controller.ViewBag.Branches, Is.Not.Null);
            Assert.That(_controller.ViewBag.Doctors, Is.Not.Null);
        }

        [Test]
        public async Task Edit_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var departmentId = 999;
            _mockUnitOfWork.Setup(x => x.DepartmentService.GetByIdAsync(departmentId))
                .ReturnsAsync((Department)null);

            // Act
            var result = await _controller.Edit(departmentId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Edit_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var model = new DepartmentEditVM
            {
                DepartmentID = 1,
                Name = "Updated Department",
                AdditionalData = "Updated Description",
                BranchID = 1,
                HeadDoctorID = "1"
            };
            _mockUnitOfWork.Setup(x => x.DepartmentService.UpdateAsync(model))
                .ReturnsAsync(new DepartmentResponseVM { Succeeded = true });

            // Act
            var result = await _controller.Edit(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(_controller.ModelState.IsValid, Is.True);
        }

        [Test]
        public async Task Edit_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new DepartmentEditVM();
            _controller.ModelState.AddModelError("Name", "Name is required");
            _controller.ModelState.AddModelError("BranchID", "Branch is required");

            // Act
            var result = await _controller.Edit(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(model));
            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(_controller.ModelState.ErrorCount, Is.EqualTo(2));
        }

        [Test]
        public async Task Edit_WithFailedUpdate_ReturnsViewResult()
        {
            // Arrange
            var model = new DepartmentEditVM
            {
                DepartmentID = 1,
                Name = "Updated Department",
                AdditionalData = "Updated Description",
                BranchID = 1,
                HeadDoctorID = "1"
            };
            _mockUnitOfWork.Setup(x => x.DepartmentService.UpdateAsync(model))
                .ReturnsAsync(new DepartmentResponseVM { Succeeded = false, Error = "Failed to update department" });

            // Act
            var result = await _controller.Edit(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(model));
            Assert.That(_controller.ModelState.ErrorCount, Is.GreaterThan(0));
        }

        [Test]
        public async Task Delete_WithValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var departmentId = 1;
            _mockUnitOfWork.Setup(x => x.DepartmentService.DeleteAsync(departmentId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(departmentId);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(_controller.ModelState.IsValid, Is.True);
        }

        [Test]
        public async Task Delete_WithInvalidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var departmentId = 999;
            _mockUnitOfWork.Setup(x => x.DepartmentService.DeleteAsync(departmentId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(departmentId);

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