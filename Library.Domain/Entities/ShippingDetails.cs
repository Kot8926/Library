using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Пожалуйста введите имя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Пожалуйста введите фамилию")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Введите страну")]
        public string Contry { get; set; }
        [Required(ErrorMessage = "Введите город")]
        public string City { get; set; }
        [Required(ErrorMessage = "Введите адрес")]
        public string Adress { get; set; }

        public string Index { get; set; }
        public bool GiftWrap { get; set; }


    }
}
