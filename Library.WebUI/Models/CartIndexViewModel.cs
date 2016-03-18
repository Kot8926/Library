using Library.Domain.Entities;

namespace Library.WebUI.Models
{
    //Модель представления для передачи в Index
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}