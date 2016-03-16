using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Domain.Abstract;
using Library.Domain.Entities;
using Library.WebUI.Models;

namespace Library.WebUI.Controllers
{
    public class BookController : Controller
    {
        public int PageSize = 4;
        private IBookRepository reposit;
        public BookController(IBookRepository bookRep)
        {
            this.reposit = bookRep;   
        }
        
        public ViewResult List(string genre, int page = 1)
        {
            //Формируем данные для передачи в представление
            BookListViewModel vModel = new BookListViewModel
            {
                Books = reposit.Books
                    .Where(b => genre == null || b.Genre == genre)
                    .OrderBy(b => b.BookId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    TotalItem = genre == null ?
                        reposit.Books.Count() :
                        reposit.Books.Where(b => b.Genre == genre).Count(),
                    ItemsPerPage = PageSize,
                },
                CurrentGenre = genre
            };
            
            return View(vModel);
        }

    }
}
