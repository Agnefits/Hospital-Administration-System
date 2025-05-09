using Hospital_Administration_System.Controllers.Admin_Controllers;
using Hospital_Administration_System.Models;
using Hospital_Administration_System.Repository;
using Hospital_Administration_System.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NuGet.Protocol;

namespace Hospital_Administration_System.Test.ControllerTests.Admin
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new UsersController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task Index_ReturnsViewResultWithUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = "user1",
                    UserName = "john.doe@example.com",
                    Email = "john.doe@example.com",
                },
                new User
                {
                    Id = "user2",
                    UserName = "jane.smith@example.com",
                    Email = "jane.smith@example.com",
                }
            };
            _mockUnitOfWork.Setup(x => x.UserService.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.AssignableFrom<List<User>>());
            Assert.That(viewResult.Model, Is.EqualTo(users));
            Assert.That(((IEnumerable<User>)viewResult.Model).Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task Index_WithNoUsers_ReturnsEmptyList()
        {
            // Arrange
            var emptyUsers = new List<User>();
            _mockUnitOfWork.Setup(x => x.UserService.GetAllUsersAsync())
                .ReturnsAsync(emptyUsers);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.AssignableFrom<List<User>>());
            Assert.That(((IEnumerable<User>)viewResult.Model).Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task Details_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var userId = "test-id";
            var user = new User
            {
                Id = userId,
                UserName = "test.user@example.com",
                Email = "test.user@example.com",
            };
            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.Details(userId);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.AssignableFrom<User>());
            Assert.That(viewResult.Model, Is.EqualTo(user));
            var model = (User)viewResult.Model;
            Assert.That(model.UserName, Is.EqualTo(user.UserName));
            Assert.That(model.Email, Is.EqualTo(user.Email));
        }

        [Test]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var userId = "invalid-id";
            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Details(userId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Create_ReturnsViewResultWithViewBagData()
        {
            // Arrange

            var departments = new List<Department>
            {
                new Department { DepartmentID = 1, Name = "Cardiology", BranchID = 1}
            };

            var pharmacies = new List<Pharmacy>
            {
                new Pharmacy { PharmacyID = 1, Name = "Cardiology", BranchID = 1}
            };

            _mockUnitOfWork.Setup(x => x.DepartmentService.GetAllDepartmentsAsync())
                .ReturnsAsync(departments);

            _mockUnitOfWork.Setup(x => x.PharmacistService.GetAllPharmaciesAsync())
                .ReturnsAsync(pharmacies);

            // Act
            var result = _controller.Create();

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(_controller.ViewBag.Departments, Is.Not.Null);
            Assert.That(_controller.ViewBag.Pharmacies, Is.Not.Null);
            Assert.That(((SelectList)_controller.ViewBag.Departments).Count(), Is.EqualTo(1));
            Assert.That(((SelectList)_controller.ViewBag.Pharmacies).Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Create_WithValidModel_ReturnsRedirectToActionResult()
        {
            var model = new UserCreateVM
            {
                FullName = "New User",
                Email = "new.user@example.com",
                ContactNumber = "010",
                Password = "Password123!",
                Role = "Doctor",
                Specialization = "Stomach",
                DepartmentID = 1,
            };

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            _mockUnitOfWork.Setup(x => x.UserService.AddAsync(model))
                .ReturnsAsync(new UserResponseVM { Succeeded = true });

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(_controller.TempData["SuccessMessage"], Is.EqualTo("User created successfully!"));
            Assert.That(_controller.ModelState.IsValid, Is.True);
        }

        [Test]
        public async Task Create_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new UserCreateVM();
            _controller.ModelState.AddModelError("Email", "Email is required");
            _controller.ModelState.AddModelError("Password", "Password is required");

            var departments = new List<Department>
            {
                new Department { DepartmentID = 1, Name = "Cardiology", BranchID = 1}
            };

            var pharmacies = new List<Pharmacy>
            {
                new Pharmacy { PharmacyID = 1, Name = "Cardiology", BranchID = 1}
            };

            _mockUnitOfWork.Setup(x => x.DepartmentService.GetAllDepartmentsAsync())
                .ReturnsAsync(departments);

            _mockUnitOfWork.Setup(x => x.PharmacistService.GetAllPharmaciesAsync())
                .ReturnsAsync(pharmacies);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

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
            var model = new UserCreateVM
            {
                FullName = "new.user",
                Email = "new.user@example.com",
                Password = "Password123!",
                Role = "Doctor",
                DepartmentID = 1,
                PharmacyID = 1
            };
            _mockUnitOfWork.Setup(x => x.UserService.AddAsync(model))
                .ReturnsAsync(new UserResponseVM { Succeeded = false, Error = "Failed to create user" });

            var departments = new List<Department>
            {
                new Department { DepartmentID = 1, Name = "Cardiology", BranchID = 1}
            };

            var pharmacies = new List<Pharmacy>
            {
                new Pharmacy { PharmacyID = 1, Name = "Cardiology", BranchID = 1}
            };

            _mockUnitOfWork.Setup(x => x.DepartmentService.GetAllDepartmentsAsync())
                .ReturnsAsync(departments);

            _mockUnitOfWork.Setup(x => x.PharmacistService.GetAllPharmaciesAsync())
                .ReturnsAsync(pharmacies);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(model));
            Assert.That(_controller.TempData["ErrorMessage"], Is.EqualTo("Failed to create user"));
        }

        [Test]
        public async Task Edit_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var userId = "test-id";
            var user = new User
            {
                Id = userId,
                UserName = "test.user@example.com",
                Email = "test.user@example.com",
            };
            var departments = new List<Department>
            {
                new Department { DepartmentID = 1, Name = "Cardiology" }
            };

            var pharmacies = new List<Pharmacy>
            {
                new Pharmacy { PharmacyID = 1, Name = "Cardiology", BranchID = 1}
            };

            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(userId))
                .ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.PharmacistService.GetAllPharmaciesAsync())
                .ReturnsAsync(pharmacies);
            _mockUnitOfWork.Setup(x => x.DepartmentService.GetAllDepartmentsAsync())
                .ReturnsAsync(departments);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.Edit(userId);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(_controller.ViewBag.Departments, Is.Not.Null);
            Assert.That(_controller.ViewBag.Pharmacies, Is.Not.Null);
            Assert.That(((SelectList)_controller.ViewBag.Departments).Count(), Is.EqualTo(1));
            Assert.That(((SelectList)_controller.ViewBag.Pharmacies).Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Edit_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var userId = "invalid-id";
            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Edit(userId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Edit_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var model = new UserEditVM
            {
                UserId = "test-id",
                FullName = "updated.user",
                Email = "updated.user@example.com",
                ContactNumber = "010",
                Role = "Doctor",
                DepartmentID = 1,
                PharmacyID = 1
            };
            _mockUnitOfWork.Setup(x => x.UserService.UpdateAsync(model))
                .ReturnsAsync(new UserResponseVM { Succeeded = true });

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.Edit(model);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(_controller.TempData["SuccessMessage"], Is.EqualTo("User updated successfully!"));
            Assert.That(_controller.ModelState.IsValid, Is.True);
        }

        [Test]
        public async Task Edit_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var model = new UserEditVM();
            _controller.ModelState.AddModelError("Email", "Email is required");
            _controller.ModelState.AddModelError("Role", "Role is required");

            var departments = new List<Department>
            {
                new Department { DepartmentID = 1, Name = "Cardiology" }
            };

            var pharmacies = new List<Pharmacy>
            {
                new Pharmacy { PharmacyID = 1, Name = "Cardiology", BranchID = 1}
            };

            _mockUnitOfWork.Setup(x => x.PharmacistService.GetAllPharmaciesAsync())
                .ReturnsAsync(pharmacies);
            _mockUnitOfWork.Setup(x => x.DepartmentService.GetAllDepartmentsAsync())
                .ReturnsAsync(departments);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

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
            var model = new UserEditVM
            {
                UserId = "test-id",
                FullName = "updated.user",
                Email = "updated.user@example.com",
                Role = "Doctor",
                DepartmentID = 1,
                PharmacyID = 1
            };
            _mockUnitOfWork.Setup(x => x.UserService.UpdateAsync(model))
                .ReturnsAsync(new UserResponseVM { Succeeded = false, Error = "Failed to update user" });


            var departments = new List<Department>
            {
                new Department { DepartmentID = 1, Name = "Cardiology" }
            };

            var pharmacies = new List<Pharmacy>
            {
                new Pharmacy { PharmacyID = 1, Name = "Cardiology", BranchID = 1}
            };

            _mockUnitOfWork.Setup(x => x.PharmacistService.GetAllPharmaciesAsync())
                .ReturnsAsync(pharmacies);
            _mockUnitOfWork.Setup(x => x.DepartmentService.GetAllDepartmentsAsync())
                .ReturnsAsync(departments);

            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Act
            var result = await _controller.Edit(model);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.EqualTo(model));
            Assert.That(_controller.TempData["ErrorMessage"], Is.EqualTo("Failed to update user"));
        }

        [Test]
        public async Task Delete_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var userId = "test-id";
            var user = new User
            {
                Id = userId,
                UserName = "test.user@example.com",
                Email = "test.user@example.com",
            };
            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.AssignableFrom<User>());
            Assert.That(viewResult.Model, Is.EqualTo(user));
            var model = (User)viewResult.Model;
            Assert.That(model.UserName, Is.EqualTo(user.UserName));
            Assert.That(model.Email, Is.EqualTo(user.Email));
        }

        [Test]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var userId = "invalid-id";
            _mockUnitOfWork.Setup(x => x.UserService.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteConfirmed_WithValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var userId = "test-id";
            _mockUnitOfWork.Setup(x => x.UserService.DeleteAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteConfirmed(userId);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task DeleteConfirmed_WithInvalidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var userId = "invalid-id";
            _mockUnitOfWork.Setup(x => x.UserService.DeleteAsync(userId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteConfirmed(userId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }
    }
}