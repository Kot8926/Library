﻿using System;
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

    }
}