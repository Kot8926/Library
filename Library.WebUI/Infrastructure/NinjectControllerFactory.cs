using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Moq;
using System.Linq;
using Library.Domain.Entities;
using Library.Domain.Abstract;
using System.Collections.Generic;
using Library.Domain.Concrete;

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
            ninjectKernel.Bind<IBookRepository>().To<EFBookRepository>();
        }
    }
}