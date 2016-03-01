using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

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

        }
    }
}