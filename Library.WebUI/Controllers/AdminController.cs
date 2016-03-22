using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Domain.Abstract;

namespace Library.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IBookRepository repository;
        public AdminController(IBookRepository reposit)
        {
            repository = reposit;
        }

        public ViewResult Index()
        {
            return View(repository.Books);
        }

    }
}
