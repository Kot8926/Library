using System.Linq;
using Library.Domain.Entities;
using Library.Domain.Abstract;

namespace Library.Domain.Concrete
{
    //Реализация доступа к хранилищу
    public class EFBookRepository : IBookRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Book> Books
        {
            get { return context.Books; }
        }

        //Сохранение изменений и добавление книги
        public void SaveBook(Book book)
        {
            //Если id = 0 добавляем
            if (book.BookId == 0)
            {
                context.Books.Add(book);
            }
            else
            {
                //Иначе обновляем существующий
                Book dbEntry = context.Books.Find(book.BookId);
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.Author = book.Author;
                    dbEntry.Genre = book.Genre;
                    dbEntry.Year = book.Year;
                    dbEntry.PriceLoss = book.PriceLoss;
                }             
            }

            context.SaveChanges();
        }

        //Удаление книги
        public Book DeleteBook(int idBook)
        {
            Book dbEntry = context.Books.Find(idBook);
            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
