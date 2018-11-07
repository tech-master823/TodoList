using System.Security.Claims;
using System.Threading.Tasks;
using Amoraitis.Todo.UnitTests.Resources;
using Amoraitis.TodoList.Controllers;
using Amoraitis.TodoList.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Amoraitis.Todo.UnitTests.Controllers
{
    public class HomeControllerTest
    {
        private Mock<FakeUserManager> _userManagerMock;
        private HomeController _homeController;

        public HomeControllerTest()
        {
            _userManagerMock =  new Mock<FakeUserManager>();
            _homeController = new HomeController(_userManagerMock.Object);

            _homeController.ControllerContext = new ControllerContext();
            _homeController.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WhenSucceeded()
        {
            var result = await _homeController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_ReturnsRedirectToActionResult_WhenFindAUser()
        {
            _userManagerMock
                .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new ApplicationUser());

            var result = await _homeController.Index();
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Home", redirectToActionResult.ActionName);
            Assert.Equal("Todos", redirectToActionResult.ControllerName);
        }

        [Fact]
        public void About_ReturnsViewResult_WhenSucceeded()
        {
            var result = _homeController.About();
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("Your application description page.", viewResult.ViewData["Message"]);
        }

        [Fact]
        public void Contact_ReturnsViewResult_WhenSucceeded()
        {
            var result = _homeController.Contact();
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("Your contact page.", viewResult.ViewData["Message"]);
        }

        [Fact]
        public void Error_ReturnsViewResult_WhenSucceeded()
        {
            var result = _homeController.Error();
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsAssignableFrom<ErrorViewModel>(viewResult.Model);
        }
    }
}
