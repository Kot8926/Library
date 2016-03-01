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
        private IBookRepository reposit;
        public BookController(IBookRepository bookRep)
        {
            this.reposit = bookRep;   
        }

        public ViewResult List()
        {
            return View(reposit.Books);
        }

    }
}
