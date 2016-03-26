using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Domain.Abstract;
using Library.Domain.Entities;

namespace Library.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IBookRepository repository;
        public AdminController(IBookRepository reposit)
        {
            repository = reposit;
        }

        //Список всех книг
        public ViewResult Index()
        {
            return View(repository.Books);
        }

        //Редактирование 
        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books.FirstOrDefault(b => bookId == b.BookId);
            return View(book);

        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            DateTime date = DateTime.Now;

            if (string.IsNullOrEmpty(book.Name))
            {
                ModelState.AddModelError("Name", "Пожалуйста ведите имя");
            }
            if (string.IsNullOrEmpty(book.Author))
            {
                ModelState.AddModelError("Author", "Пожалуйста ведите автора");
            }
            if (string.IsNullOrEmpty(book.Genre))
            {
                ModelState.AddModelError("Genre", "Пожалуйста Укажите жанр");
            }
            if (book.Year <= 1200 || book.Year >= date.Year)
            {
                ModelState.AddModelError("Year", "Пожалуйста ведите год");
            }
            if ((double)book.PriceLoss < 0.01 || (double)book.PriceLoss > double.MaxValue)
            {
                ModelState.AddModelError("PriceLoss", "Пожалуйста ведите стоимость");
            }

            if (ModelState.IsValid)
            {
                repository.SaveBook(book);
                TempData["message"] = string.Format("{0} был сохранен", book.Name);
                return RedirectToAction("Index");
            }
            else
            {
                //Если что то не так с данными
                return View(book);
            }           
        }

        //Создание новой книги
        public ViewResult Create()
        {
            return View("Edit", new Book());
        }

        //Удаление книги
        [HttpPost]
        public ActionResult Delete(int bookId)
        {
            Book deleteBook = repository.DeleteBook(bookId);
            if(deleteBook != null)
            {
                TempData["message"] = string.Format("Книга {0} была успешно удалена", deleteBook.Name);
            }
         
            return RedirectToAction("Index");
        }

    }
}
