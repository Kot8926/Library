using System;
using Moq;
using System.Web.Mvc;
using Library.WebUI.Infrastructure.Abstract;
using Library.WebUI.Models;
using Library.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.UnitTest
{
    [TestClass]
    public class AdminSecuritytest
    {
        //Верные данные аутентификации------------------------------------------------------------------------------------
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //Arrange
            //Создаем имитацию поставщика аутентификации
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("username", "password")).Returns(true);

            //Модель представления
            LoginViewModel model = new LoginViewModel { UserName = "username", Password = "password" };

            AccountController target = new AccountController(mock.Object);

            //Act
            ActionResult result = target.Login(model, "Url");

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("Url", ((RedirectResult)result).Url );
        }

        //Неверные данные аутентификации------------------------------------------------------------------------------------
        [TestMethod]
        public void Cannot_Login_With_Valid_Credentials()
        {
            //Arrange
            //Создаем имитацию поставщика аутентификации
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("Badname", "Badpas")).Returns(false);

            //Модель представления
            LoginViewModel model = new LoginViewModel { UserName = "Badname", Password = "Badpas" };

            AccountController target = new AccountController(mock.Object);

            //Act
            ActionResult result = target.Login(model, "Url");

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
