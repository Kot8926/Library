using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Entities;

namespace Library.Domain.Entities
{
    //Информация о одной книге в корзине
    public class CartLine
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }


    public class Cart
    {
        //Контейнер для книг в корзине
        private List<CartLine> bookList  = new List<CartLine>();

        //Добавление книги
        public void AddBook(Book book, int quantity)
        {
            //Проверяем есть ли такой элемент в контейнере
            CartLine item = bookList.Where(b => b.Book.BookId == book.BookId).FirstOrDefault();

            if (item == null)
            {
                bookList.Add(new CartLine { Book = book, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        //Удаление книги
        public void DeleteBook(Book book)
        {
            bookList.RemoveAll(b => b.Book.BookId == book.BookId);
        }

        //Очистить контейнер
        public void ClearAll()
        {
            bookList.Clear();
        }

        //Стоимость возмещения при утрате всех книг
        public decimal CostForLost()
        {
            return bookList.Sum(s => s.Book.PriceLoss * s.Quantity);
        }

        //Доступ к содержимому
        public IEnumerable<CartLine> Lines
        {
            get { return bookList;  }
        }
    }
}
