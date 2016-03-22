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
        //Index выводит все книги на страницу-----------------------------------------------------------------------------------------
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

        //Редактируем нужную книгу-----------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void Can_edit_book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> { 
            new Book {BookId = 1, Name="book1"},
            new Book {BookId = 2, Name="book2"},
            new Book {BookId = 3, Name="book3"},
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //Act
            Book result1 = target.Edit(1).ViewData.Model as Book;
            Book result2 = target.Edit(2).ViewData.Model as Book;
            Book result3 = target.Edit(3).ViewData.Model as Book;

            //Assert
            Assert.AreEqual(result1.Name, "book1");
            Assert.AreEqual(result2.Name, "book2");
            Assert.AreEqual(result3.Name, "book3");
        }

        //Возвращает null если книга неверная------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void Cant_edit_nonexisted_book()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> { 
            new Book {BookId = 1, Name="book1"},
            new Book {BookId = 2, Name="book2"},
            new Book {BookId = 3, Name="book3"},
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //Act
            Book result = target.Edit(4).ViewData.Model as Book;

            //Assert
            Assert.IsNull(result);
        } 
    }
}
