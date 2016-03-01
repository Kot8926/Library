using Library.Domain.Entities;
using System.Data.Entity;

namespace Library.Domain.Concrete
{
    //Класс контекста
    public class EFDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
    }
}
