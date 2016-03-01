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
    }
}
