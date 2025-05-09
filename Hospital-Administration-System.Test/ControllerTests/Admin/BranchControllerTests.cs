using Hospital_Administration_System.Controllers.Admin_Controllers;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Hospital_Administration_System.ViewModels.Branch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hospital_Administration_System.Test.ControllerTests.Admin
{
    [TestFixture]
    public class BranchControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private BranchController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new BranchController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Index_ReturnsViewResultWithBranches()
        {
            // Arrange
            var branches = new List<Branch>
            {
                new Branch { BranchID = 1, Name = "Branch 1", Location = "Assiut", ContactNumber = "010" },
                new Branch { BranchID = 2, Name = "Branch 2", Location = "Aswan", ContactNumber = "011" }
            };
            _mockUnitOfWork.Setup(x => x.BranchService.GetAllBranchesAsync())
                .ReturnsAsync(branches);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.AssignableFrom<List<Branch>>());
            Assert.That(viewResult.Model, Is.EqualTo(branches));
            Assert.That(((IEnumerable<Branch>)viewResult.Model).Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task Index_WithNoBranches_ReturnsEmptyList()
        {
            // Arrange
            var emptyBranches = new List<Branch>();
            _mockUnitOfWork.Setup(x => x.BranchService.GetAllBranchesAsync())
                .ReturnsAsync(emptyBranches);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.AssignableFrom<List<Branch>>());
            Assert.That(((IEnumerable<Branch>)viewResult.Model).Count(), Is.EqualTo(0));
        }

        [Test]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.Null);
        }

        [Test]
        public async Task Create_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var model = new BranchCreateVM 
            { 
                Name = "New Branch",
                Location = "123 Main St",
                ContactNumber = "123-456-7890"
            };
            _mockUnitOfWork.Setup(x => x.BranchService.AddAsync(model))
                .ReturnsAsync(new BranchResponseVM { Succeeded = true });

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
            var model = new BranchCreateVM();
            _controller.ModelState.AddModelError("Name", "Name is required");
            _controller.ModelState.AddModelError("Address", "Address is required");

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
            var model = new BranchCreateVM 
            { 
                Name = "New Branch",
                Location = "123 Main St",
                ContactNumber = "123-456-7890"
            };
            _mockUnitOfWork.Setup(x => x.BranchService.AddAsync(model))
                .ReturnsAsync(new BranchResponseVM { Succeeded = false, Error = "Failed to add branch" });

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
            var branchId = 1;
            var branch = new Branch 
            { 
                BranchID = branchId,
                Name = "Test Branch",
                Location = "123 Test St",
                ContactNumber = "123-456-7890"
            };
            _mockUnitOfWork.Setup(x => x.BranchService.GetByIdAsync(branchId))
                .ReturnsAsync(branch);

            // Act
            var result = await _controller.Edit(branchId);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            var model = (BranchEditVM)viewResult.Model;
            Assert.That(model.BranchID, Is.EqualTo(branch.BranchID));
            Assert.That(model.Name, Is.EqualTo(branch.Name));
            Assert.That(model.Location, Is.EqualTo(branch.Location));
            Assert.That(model.ContactNumber, Is.EqualTo(branch.ContactNumber));
        }

        [Test]
        public async Task Edit_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var branchId = 999;
            _mockUnitOfWork.Setup(x => x.BranchService.GetByIdAsync(branchId))
                .ReturnsAsync((Branch)null);

            // Act
            var result = await _controller.Edit(branchId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Edit_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var model = new BranchEditVM 
            { 
                BranchID = 1,
                Name = "Updated Branch",
                Location = "456 New St",
                ContactNumber = "987-654-3210"
            };
            _mockUnitOfWork.Setup(x => x.BranchService.UpdateAsync(model))
                .ReturnsAsync(new BranchResponseVM { Succeeded = true });

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
            var model = new BranchEditVM();
            _controller.ModelState.AddModelError("Name", "Name is required");
            _controller.ModelState.AddModelError("Address", "Address is required");

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
            var model = new BranchEditVM 
            { 
                BranchID = 1,
                Name = "Updated Branch",
                Location = "456 New St",
                ContactNumber = "987-654-3210"
            };
            _mockUnitOfWork.Setup(x => x.BranchService.UpdateAsync(model))
                .ReturnsAsync(new BranchResponseVM { Succeeded = false, Error = "Failed to update branch" });

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
            var branchId = 1;
            _mockUnitOfWork.Setup(x => x.BranchService.DeleteAsync(branchId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(branchId);

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
            var branchId = 999;
            _mockUnitOfWork.Setup(x => x.BranchService.DeleteAsync(branchId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(branchId);

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