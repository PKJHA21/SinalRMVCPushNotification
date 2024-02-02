using Microsoft.Owin;
using Microsoft.Owin.Builder;
using NotificationStatusUpdate.Hubs;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
[assembly: OwinStartup(typeof(NotificationStatusUpdate.Startup))]
namespace NotificationStatusUpdate
{

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}