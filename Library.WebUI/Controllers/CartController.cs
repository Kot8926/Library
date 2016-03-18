﻿using System;
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

        //Помещаем обьект в корзину. Перенаправляем в метод Index.
        public RedirectToRouteResult AddToCart(Cart cart, int bookId, string returnUrl)
        {
            Book book = reposit.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                cart.AddBook(book, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        //Удаляем обьект из корзины. Перенаправляем к Index
        public RedirectToRouteResult DeleteToCart(Cart cart, int bookId, string returnUrl)
        {
            Book book = reposit.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                cart.DeleteBook(book);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel 
            { 
                Cart = cart,
                ReturnUrl = returnUrl,
            });
        }
    }
}
