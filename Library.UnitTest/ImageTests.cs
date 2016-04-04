using System;
using Moq;
using System.Linq;
using System.Web.Mvc;
using Library.Domain.Abstract;
using Library.Domain.Entities;
using Library.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.UnitTest
{
    //Получаем изображение по правильному ид---------------------------------------------------------------------------
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data() 
      { 
        // Arrange
        Book book = new Book 
        { 
          BookId = 2, 
          Name = "Test", 
          ImageData = new byte[] { }, 
          ImageMimeType = "image/png" 
        }; 
        
        Mock<IBookRepository> mock = new Mock<IBookRepository>(); 
        mock.Setup(m => m.Books).Returns(new Book[] { 
          new Book {BookId = 1, Name = "B1"}, 
          book, 
          new Book {BookId = 3, Name = "B3"} 
        }.AsQueryable()); 
        
        BookController target = new BookController(mock.Object); 

        //Act
        ActionResult result = target.GetImage(2); 

        // Assert 
        Assert.IsNotNull(result); 
        Assert.IsInstanceOfType(result, typeof(FileResult)); 
        Assert.AreEqual(book.ImageMimeType, ((FileResult)result).ContentType); 
      }

        //Не получаем изображение по правильному ид---------------------------------------------------------------------------
        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[] { 
          new Book {BookId = 1, Name = "B1"}, 
          new Book {BookId = 2, Name = "B2"} 
        }.AsQueryable());

            BookController target = new BookController(mock.Object);

            // Act
            ActionResult result = target.GetImage(100);
            // Assert 
            Assert.IsNull(result);
        } 
    }
}
