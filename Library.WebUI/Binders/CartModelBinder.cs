using System;
using System.Web.Mvc;
using Library.Domain.Entities;

namespace Library.WebUI.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //Получить Cart из сессии
            Cart cart = (Cart)controllerContext.HttpContext.Session[sessionKey];

            //Если сессия пустая
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }

            return cart;
        }
    }
}