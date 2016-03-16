using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Library.Domain.Entities;

namespace Library.UnitTest
{
    [TestClass]
    public class CartTest
    {
        //Добавление нового элемента в корзину------------------------------------------------------------------------------
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

        //Добавление существующего элемента в корзину----------------------------------------------------------------------
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

        //Удаление элемента из корзины--------------------------------------------------------------------------------------
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

        //Рассчет общей суммы при утрате-------------------------------------------------------------------------------------
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

        //Очистить контейнер--------------------------------------------------------------------------------------------------
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
    }
}
