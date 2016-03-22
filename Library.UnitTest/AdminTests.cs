using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Domain.Abstract;
using Library.Domain.Entities;
using Library.WebUI.Controllers;


namespace Library.UnitTest
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void Index_Contains_All_Books()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
            new Book {BookId = 1, Name = "book1"},
            new Book {BookId = 2, Name = "book2"},
            new Book {BookId = 3, Name = "book3"},
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //Act
            Book[] result = ((IEnumerable<Book>)target.Index().Model).ToArray();

            //Assert
            Assert.AreEqual(result[0].Name, "book1");
            Assert.AreEqual(result.Length, 3);
        }
    }
}
