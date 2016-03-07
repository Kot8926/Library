using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Domain.Abstract;

namespace Library.WebUI.Controllers
{
    public class NavController : Controller
    {
        IBookRepository reposit;
        public NavController(IBookRepository bookRep)
        {
            reposit = bookRep;
        }

        public PartialViewResult Menu(string genre = null)
        {
            ViewBag.CurrentGenre = genre;
            IEnumerable<string> genres = reposit.Books
                .Select(g => g.Genre)
                .Distinct()
                .OrderBy(g => g);
            return PartialView(genres);
        }
    }
}
