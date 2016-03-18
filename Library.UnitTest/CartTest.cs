using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;
using System.Web.Mvc;
using Library.WebUI.Controllers;
using Library.Domain.Entities;
using Library.Domain.Abstract;
using Library.WebUI.Models;

namespace Library.UnitTest
{
    [TestClass]
    public class CartTest
    {
        //Добавление нового элемента в корзину метод домена------------------------------------------------------------------------------
        [TestMethod]
        public void Add_New_Book_Test()
        {
            //Arrage
            Book book1 = new Book { BookId = 1, Name = "book1" };
            Book book2 = new Book { BookId = 2, Name = "book2" };

            Cart target = new Cart();

            //Act
            target.AddBook(book1, 1);
            target.AddBook(book2, 1);
            CartLine[] result = target.Lines.ToArray();


            //Assert
            Assert.AreEqual(result[0].Book.Name, book1.Name);
            Assert.AreEqual(result[1].Book.Name, book2.Name);
            Assert.IsTrue(result.Length == 2);
        }

        //Добавление существующего элемента в корзину метод домена----------------------------------------------------------------------
        [TestMethod]
        public void Add_Quantity_For_Existing_Book()
        {
            //Arrage
            Book book1 = new Book { BookId = 1, Name = "book1" };
            Book book2 = new Book { BookId = 2, Name = "book2" };

            Cart target = new Cart();

            //Act
            target.AddBook(book1, 1);
            target.AddBook(book2, 1);
            target.AddBook(book1, 10);

            //Assert
            CartLine[] result = target.Lines.ToArray();
            Assert.IsTrue(result.Length == 2);
            Assert.AreEqual(result[0].Quantity, 11);
        }

        //Удаление элемента из корзины метод домена--------------------------------------------------------------------------------------
        [TestMethod]
        public void Delete_Book()
        {
            //Arrage
            Book book1 = new Book { BookId = 1, Name = "book1" };
            Book book2 = new Book { BookId = 2, Name = "book2" };
            Book book3 = new Book { BookId = 3, Name = "book3" };

            Cart target = new Cart();

            target.AddBook(book1, 1);
            target.AddBook(book2, 1);
            target.AddBook(book3, 10);
            target.AddBook(book2, 3);

            //Act
            target.DeleteBook(book2);

            //Assert
            Assert.AreEqual(target.Lines.Where(b => b.Book.BookId == 2).Count(), 0);
            Assert.IsTrue(target.Lines.Count() == 2);
        }

        //Рассчет общей суммы при утрате метод домена-------------------------------------------------------------------------------------
        [TestMethod]
        public void Calculate_Price_Lost()
        {
            //Arrage
            Book book1 = new Book { BookId = 1, PriceLoss = 10 };
            Book book2 = new Book { BookId = 2, PriceLoss = 2 };
            Book book3 = new Book { BookId = 3,  PriceLoss = 5 };

            Cart target = new Cart();

            //Act
            target.AddBook(book1, 3);
            target.AddBook(book2, 2);
            target.AddBook(book3, 1);
            decimal result = target.CostForLost();

            //Assert
            Assert.AreEqual(result, 39M);
        }

        //Очистить контейнер метод домена--------------------------------------------------------------------------------------------------
        [TestMethod]
        public void Clear_All_Cart()
        {
            //Arrage
            Book book1 = new Book { BookId = 1, Name = "book1" };
            Book book2 = new Book { BookId = 2, Name = "book2" };
            Book book3 = new Book { BookId = 3, Name = "book3" };

            Cart target = new Cart();

            target.AddBook(book1, 1);
            target.AddBook(book2, 1);
            target.AddBook(book3, 10);
            target.AddBook(book2, 3);

            //Act
            target.ClearAll();

            //Assert
            Assert.IsTrue(target.Lines.Count() == 0);
        }

        //Добавление элемента в корзину в контроллере---------------------------------------------------------------
        [TestMethod]
        public void Add_To_Cart()
        {
            //Arrange
            //Имитируем хранилище
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book> 
            { 
                new Book { BookId = 1, Name = "book1" },
            }.AsQueryable());

            //Создаем корзину
            Cart cart = new Cart();
            //Контроллер
            CartController controller = new CartController(mock.Object);

            //Act
            controller.AddToCart(cart, 1, null);

            //Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Book.Name, "book1");
        }

        //Проверка перенаправления в представление. После добавления элемента в корзину. И возврата Url
        [TestMethod]
        public void Goes_To_Index()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book> 
            { 
                new Book { BookId = 1, Name = "book1" },
                new Book { BookId = 2, Name = "book2" },
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object);

            controller.AddToCart(cart, 1, null);
            controller.AddToCart(cart, 2, null);

            //Act
            CartIndexViewModel result = (CartIndexViewModel)controller.Index(cart, "Url").Model;
            RedirectToRouteResult routeRes = controller.AddToCart(cart, 1, "Url");
            
            
            //Assert
            CartLine[] list = result.Cart.Lines.ToArray();

            Assert.AreEqual(result.Cart.Lines.Count(), 2);
            Assert.AreEqual(list[0].Book.Name, "book1");
            Assert.AreEqual(result.ReturnUrl, "Url");
            Assert.AreEqual(routeRes.RouteValues["action"], "Index");
            Assert.AreEqual(routeRes.RouteValues["returnUrl"], "Url");
            
        }
    }
}
