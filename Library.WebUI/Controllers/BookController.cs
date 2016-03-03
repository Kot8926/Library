using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Domain.Abstract;
using Library.Domain.Entities;

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

        public ViewResult List(int page =1)
        {
            return View(reposit.Books
                .OrderBy(b => b.BookId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize));
        }

    }
}
