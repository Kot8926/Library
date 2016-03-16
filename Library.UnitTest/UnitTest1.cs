using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Library.Domain.Entities;
using Library.Domain.Abstract;
using Library.WebUI.Controllers;
using Library.WebUI.Models;
using Library.WebUI.HtmlHelpers;
using System.Web.Mvc;
using System.Linq;
using System;
using Moq;

namespace Library.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        //Создание страниц через контроллер-------------------------------------------------------------------------------------
        public void TestPageForList()
        {
            //Arrage
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book> {
                new Book {BookId =1, Name = "b1"},
                new Book {BookId =2, Name = "b2"},
                new Book {BookId =3, Name = "b3"},
                new Book {BookId =4, Name = "b4"},
                new Book {BookId =5, Name = "b5"},
                new Book {BookId =6, Name = "b6"},
            }.AsQueryable());

            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            //IEnumerable<Book> result = (IEnumerable<Book>)bookContr.List(2).Model;
            BookListViewModel result = (BookListViewModel)controller.List(null , 2).Model;

            //Assert
            Book[] books = result.Books.ToArray();
            Assert.IsTrue(books.Count() == 3);
            Assert.AreEqual("b5", books[1].Name);
            Assert.AreEqual("b6", books[2].Name);
        }

        [TestMethod]
        //Ссылки на страницы через вспомогательный-----------------------------------------------------------------------------------------
        //метод HTML PageLinks
        public void TestPageLinks()
        {
            //Arrage
            //Определим HtmlHelper - нужно сделать это 
            // для того чтобы применить метод расширения
            HtmlHelper myHelper = null;

            //Воссоздадим класс PaginInfo. Модель представления
            PagingInfo pagInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItem = 28,
                ItemsPerPage = 10
            };

            //Через делегат формируем вид ссылки
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Act
            MvcHtmlString result = myHelper.PageLinks(pagInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>"
            + @"<a class=""selected"" href=""Page2"">2</a>"
            + @"<a href=""Page3"">3</a>");
        }

        [TestMethod]
        //Что передает метод в представление-----------------------------------------------------------------------------------
        public void TestList()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book> {
                new Book {BookId =1, Name = "b1"},
                new Book {BookId =2, Name = "b2"},
                new Book {BookId =3, Name = "b3"},
                new Book {BookId =4, Name = "b4"},
                new Book {BookId =5, Name = "b5"},
                new Book {BookId =6, Name = "b6"},
            }.AsQueryable());

            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            BookListViewModel result = (BookListViewModel)controller.List(null, 2).Model;

            //Assert
            PagingInfo pInfo = result.PagingInfo;
            Assert.AreEqual(pInfo.CurrentPage, 2);
            Assert.AreEqual(pInfo.ItemsPerPage, 3);
            Assert.AreEqual(pInfo.TotalItem, 6);
            Assert.AreEqual(pInfo.TotalPages, 2);           
        }

        //Фильтрация по жанрам--------------------------------------------------------------------------------------------------
        [TestMethod]
        public void TestFilterGenreBook()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book>{
                new Book {BookId =1, Name ="b1", Genre ="g1"},
                new Book {BookId =2, Name ="b2", Genre ="g1"},
                new Book {BookId =3, Name ="b3", Genre ="g2"},
                new Book {BookId =4, Name ="b4", Genre ="g1"},
                new Book {BookId =5, Name ="b5", Genre ="g2"},
                new Book {BookId =6, Name ="b6", Genre ="g1"},
                new Book {BookId =7, Name ="b7", Genre ="g2"},
            }.AsQueryable());

            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            BookListViewModel result = (BookListViewModel)controller.List("g2", 1).Model;

            //Assert
            Book[] arrBook = result.Books.ToArray();
            Assert.AreEqual(arrBook.Length, 3);
            Assert.IsTrue(arrBook[0].Name == "b3");
            Assert.IsTrue(arrBook[2].Name == "b7" && arrBook[0].Genre == "g2");
        }

        //Тест формирования списка жанров в контроллере Nav--------------------------------------------------------------
        [TestMethod]
        public void CreateListGenre()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book>
            {
                new Book{BookId = 1, Name = "b1", Genre = "genre1"},
                new Book{BookId = 2, Name = "b2", Genre = "genre1"},
                new Book{BookId = 3, Name = "b3", Genre = "genre3"},
                new Book{BookId = 4, Name = "b4", Genre = "genre2"},
                new Book{BookId = 5, Name = "b5", Genre = "genre1"},
            }.AsQueryable());

            NavController controller = new NavController(mock.Object);

            //Act
            String[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.IsTrue(result[0] == "genre1" &&
                                result[1] == "genre2" &&
                                result[2] == "genre3");
        }   

        //Подсветка выбранного жанра в списке
        [TestMethod]
        public void SelectedGenre()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book> {
                new Book {BookId =1, Name = "book1", Genre ="genre1"},
                new Book {BookId =2, Name = "book2", Genre ="genre2"},
                new Book {BookId =3, Name = "book3", Genre ="genre1"},
            }.AsQueryable());

            NavController controller = new NavController(mock.Object);

            string genreToSelected = "genre2";
            //Act
            string result = controller.Menu(genreToSelected).ViewBag.CurrentGenre;

            //Assert
            Assert.IsTrue(result == genreToSelected);

        }

        //Количество страниц в выбранном жанре
        [TestMethod]
        public void CountPagesGenre()
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Genre = "g1"},
                new Book {BookId = 2, Genre = "g2"},
                new Book {BookId = 3, Genre = "g1"},
                new Book {BookId = 4, Genre = "g3"},
                new Book {BookId = 5, Genre = "g1"},
                new Book {BookId = 6, Genre = "g3"},
                new Book {BookId = 7, Genre = "g1"},
            }.AsQueryable());

            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            int res = ((BookListViewModel)controller.List(null).Model).PagingInfo.TotalPages;
            int res1 = ((BookListViewModel)controller.List("g1").Model).PagingInfo.TotalPages;
            int res2 = ((BookListViewModel)controller.List("g2").Model).PagingInfo.TotalPages;
            int res3 = ((BookListViewModel)controller.List("g3").Model).PagingInfo.TotalPages;

            //Assert
            Assert.IsTrue(res == 3);
            Assert.IsTrue(res1 == 2);
            Assert.IsTrue(res2 == 1);
            Assert.IsTrue(res3 == 1);
        }
    }
}
