using ECommerce.BackendApis.Controllers;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.UserInfos;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Tests.ControllerTests
{
    public class UserControllersTest
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
        private readonly Random rand = new();

        public UserControllersTest()
        {
        }

        #region Authenticate

        [Fact]
        public async Task Authenticate_WithModelStateError_ReturnBadRequest()
        {
            // Arrage
            var controller = new UsersController(mockUnitOfWork.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Authenticate(It.IsAny<LoginRequest>());

            // Assert
            var respone = Assert.IsType<BadRequestObjectResult>(result);
            mockUnitOfWork.Verify(x => x.User.Authencate(It.IsAny<LoginRequest>()), Times.Never());
        }

        [Fact]
        public async Task Authenticate_WithUnExistUser_ReturnBadRequest()
        {
            // Arrage
            mockUnitOfWork.Setup(x => x.User.Authencate(It.IsAny<LoginRequest>()))
                    .ReturnsAsync(It.IsAny<string>());
            var controller = new UsersController(mockUnitOfWork.Object);

            // Act
            var result = await controller.Authenticate(It.IsAny<LoginRequest>());

            // Assert
            var respone = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User name or Password is incorrect", respone.Value);
            mockUnitOfWork.Verify(x => x.User.Authencate(It.IsAny<LoginRequest>()), Times.Once());
        }

        [Fact]
        public async Task Authenticate_WithLoginRequest_ReturnJsonToken()
        {
            // Arrage
            var sampleRequest = new LoginRequest()
            {
                UserName = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
                RememberMe = rand.Next(2) == 0,
            };
            var sampleToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" +
                ".eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0aGFuZ25oMTM5NEBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJUaGFuZyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlN5c3RlbS5SdW50aW1lLkNvbXBpbGVyU2VydmljZXMuQXN5bmNUYXNrTWV0aG9kQnVpbGRlcmAxK0FzeW5jU3RhdGVNYWNoaW5lQm94YDFbU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuSUxpc3RgMVtTeXN0ZW0uU3RyaW5nXSxNaWNyb3NvZnQuQXNwTmV0Q29yZS5JZGVudGl0eS5Vc2VyTWFuYWdlcmAxKzxHZXRSb2xlc0FzeW5jPmRfXzExMVtFQ29tbWVyY2UuTW9kZWxzLkVudGl0aWVzLlVzZXJdXSIsImV4cCI6MTY0NTg3MDYzNywiaXNzIjoidGVzdC5jb20iLCJhdWQiOiJ0ZXN0LmNvbSJ9" +
                "._KeZR2Ey3jF7Fps8Dz_CXNsyctOPDzStxlv9h75c-u0";

            mockUnitOfWork.Setup(x => x.User.Authencate(sampleRequest))
                    .ReturnsAsync(sampleToken);

            var controller = new UsersController(mockUnitOfWork.Object);

            // Act
            var actionResult = await controller.Authenticate(sampleRequest);

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            mockUnitOfWork.Verify(x => x.User.Authencate(sampleRequest), Times.Once);
            Assert.Equal((new { token = sampleToken }).ToString(), result.Value.ToString());
        }

        #endregion Authenticate

        #region Register

        [Fact]
        public async Task Register_WithModelStateError_ReturnBadRequest()
        {
            // Arrage
            var controller = new UsersController(mockUnitOfWork.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Register(It.IsAny<RegisterRequest>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            mockUnitOfWork.Verify(x => x.User.CreateUser(It.IsAny<RegisterRequest>()), Times.Never());
        }

        [Fact]
        public async Task Register_WithRegisterError_ReturnBadRequest()
        {
            // Arrage
            mockUnitOfWork.Setup(x => x.User.CreateUser(It.IsAny<RegisterRequest>()))
                    .ReturnsAsync(It.IsAny<string>());
            var controller = new UsersController(mockUnitOfWork.Object);

            // Act
            var result = await controller.Register(It.IsAny<RegisterRequest>());

            // Assert
            var respone = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("register is unsuccess", respone.Value);
            mockUnitOfWork.Verify(x => x.User.CreateUser(It.IsAny<RegisterRequest>()), Times.Once());
        }

        [Fact]
        public async Task Register_WithErrorGetVM_ReturnBadRequest()
        {
            // Arrage
            var sampleUserId = Guid.NewGuid().ToString();

            var sampleRequest = new RegisterRequest()
            {
                FirstName = Guid.NewGuid().ToString(),
                DateOfBirth = DateTime.Now,
                Email = Guid.NewGuid().ToString() + "@gmail.com",
                LastName = Guid.NewGuid().ToString() + "@",
                Password = Guid.NewGuid().ToString(),
                PhoneNumber = rand.Next(100000000, 999999999).ToString(),
                UserName = Guid.NewGuid().ToString(),
            };

            mockUnitOfWork.Setup(x => x.User.CreateUser(sampleRequest))
                    .ReturnsAsync(sampleUserId);
            mockUnitOfWork.Setup(x => x.User.GetById(sampleUserId))
                    .ReturnsAsync(It.IsAny<UserInfoViewModel>());

            var controller = new UsersController(mockUnitOfWork.Object);

            // Act
            var result = await controller.Register(sampleRequest);

            // Assert
            var respone = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot get user information view modal", respone.Value);
            mockUnitOfWork.Verify(x => x.User.CreateUser(sampleRequest), Times.Once());
            mockUnitOfWork.Verify(x => x.User.GetById(sampleUserId), Times.Once());
        }

        [Fact]
        public async Task Register_WithLoginRequest_ReturnOkResult()
        {
            // Arrage
            var sampleUserId = Guid.NewGuid().ToString();

            var sampleRequest = new RegisterRequest()
            {
                FirstName = Guid.NewGuid().ToString(),
                DateOfBirth = DateTime.Now,
                Email = Guid.NewGuid().ToString() + "@gmail.com",
                LastName = Guid.NewGuid().ToString() + "@",
                Password = Guid.NewGuid().ToString(),
                PhoneNumber = rand.Next(100000000, 999999999).ToString(),
                UserName = Guid.NewGuid().ToString(),
            };

            var sampleResult = new UserInfoViewModel()
            {
                avatarUrl = SystemConstants.ImageSettings.DefaultAvatart,
                DateOfBirth = sampleRequest.DateOfBirth.ToString(),
                Email = sampleRequest.Email,
                FirstName = sampleRequest.FirstName,
                Id = sampleUserId.ToString(),
                LastName = sampleRequest.LastName,
                Name = sampleRequest.FirstName + " " + sampleRequest.LastName,
                PhoneNumber = sampleRequest.PhoneNumber,
                Role = "customer",
                UserName = sampleRequest.UserName,
            };

            mockUnitOfWork.Setup(x => x.User.CreateUser(sampleRequest))
                    .ReturnsAsync(sampleUserId.ToString());
            mockUnitOfWork.Setup(x => x.User.GetById(sampleUserId))
                    .ReturnsAsync(sampleResult);

            var controller = new UsersController(mockUnitOfWork.Object);

            // Act
            var actionResult = await controller.Register(sampleRequest) as CreatedAtActionResult;

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            var returnValue = Assert.IsType<UserInfoViewModel>(createdAtActionResult.Value);
            mockUnitOfWork.Verify(x => x.User.CreateUser(sampleRequest), Times.Once);
            mockUnitOfWork.Verify(x => x.User.GetById(sampleUserId), Times.Once);
            Assert.Equal(sampleResult.Role, returnValue.Role);
            Assert.Equal(sampleResult.UserName, returnValue.UserName);
        }

        #endregion Register
    }
}