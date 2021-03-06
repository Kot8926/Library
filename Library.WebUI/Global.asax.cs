﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Library.WebUI.Infrastructure;
using Library.WebUI.Binders;
using Library.Domain.Entities;

namespace Library.WebUI
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Указать фабрику контроллеров
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            //Пользовательский механизм связывания данных модели
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}