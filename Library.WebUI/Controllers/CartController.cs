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
    public class CartController : Controller
    {
        private IBookRepository reposit;
        public CartController(IBookRepository bookRep)
        {
            reposit = bookRep;
        }

        //Создаем сессию. Для сохранения и извлечения обьектов
        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }

            return cart;
        }

        //Помещаем обьект в корзину. Перенаправляем к представлению Index.
        public RedirectToRouteResult AddToCart(int bookId, string returnUrl)
        {
            Book book = reposit.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                GetCart().AddBook(book, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        //Удаляем обьект из корзины. Перенаправляем к Index
        public RedirectToRouteResult DeleteToCart(int bookId, string returnUrl)
        {
            Book book = reposit.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                GetCart().DeleteBook(book);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel 
            { 
                Cart = GetCart(),
                ReturnUrl = returnUrl,
            });
        }
    }
}
