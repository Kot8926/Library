using System.Web.Mvc;


namespace Library.Domain.Entities
{
    public class Book
    {
        [HiddenInput( DisplayValue = false )]
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public decimal PriceLoss { get; set; }
    }
}
