using System;

namespace Library.WebUI.Models
{
    //Модель представления. Содержит инфо о страницах.
    public class PagingInfo
    {
        //Инфо о страницах
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItem / ItemsPerPage); }
        }
    }
}