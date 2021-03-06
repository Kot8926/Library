﻿using Library.Domain.Entities;
using System.Linq;


namespace Library.Domain.Abstract
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }

        void SaveBook(Book book);

        Book DeleteBook(int idBook);
    }
}
