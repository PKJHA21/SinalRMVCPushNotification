using NotificationStatusUpdate.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NotificationStatusUpdate
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string Constr = ConfigurationManager.ConnectionStrings["noticConn"].ConnectionString;
        protected void Application_Start()
        {
           
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
          
            SqlDependency.Start(Constr);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            NotificationHub objNotifHub = new NotificationHub();
            objNotifHub.SendNotification();
        }

        protected void Application_End()
        {
            //STOP SQL DEPENDENCY
            SqlDependency.Stop(Constr);
        }
    }
}
