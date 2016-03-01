using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Moq;
using System.Linq;
using Library.Domain.Entities;
using Library.Domain.Abstract;
using System.Collections.Generic;

namespace Library.WebUI.Infrastructure
{
    //Создаем пользовательскую фабрику обьектов.
    //Наследуясь от стандартной.
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private StandardKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            //Создаем контейнер
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            //Получение контроллера из контейнера используя
            //его тип
            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }

        //Установить зависимости
        private void AddBindings()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {Name = "Шерлок холмс", PriceLoss = 130},
                new Book {Name = "Гарри поттер", PriceLoss = 120},
                new Book {Name = "Война", PriceLoss = 300}
            }.AsQueryable());

            ninjectKernel.Bind<IBookRepository>().ToConstant(mock.Object);
        }
    }
}