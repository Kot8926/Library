using System.Collections.Generic;
using Library.Domain.Entities;

namespace Library.WebUI.Models
{
    //Класс модели представления. Собирает вместе данные
    //доменной модели и список страниц. Для передачи в представление.
    public class BookListViewModel
    {
        //Список обьектов
        public IEnumerable<Book> Books { get; set; }
        //Список страниц
        public PagingInfo PagingInfo { get; set; }
    }
}