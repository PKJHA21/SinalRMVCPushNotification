using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationStatusUpdate.Models
{
    public class NotificationCountResult
    {
        public int Count { get; set; }
       public List<Notification> notifications { get; set; }
    }
}