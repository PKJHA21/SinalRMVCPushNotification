using Microsoft.AspNet.SignalR;
using NotificationStatusUpdate.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotificationStatusUpdate.Hubs
{
    public class NotificationHub : Hub
    {
               //Logged Use Call
        public void GetNotification()
        {
            try
            {
                //Get TotalNotification
                var totalNotif = LoadNotifData();
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                context.Clients.All.broadcaastNotif(totalNotif); 
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //Specific User Call
        public void SendNotification()
        {
            try
            {
                //Get TotalNotification
                var totalNotif = LoadNotifData();
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                context.Clients.All.broadcaastNotif(totalNotif);
              
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private NotificationCountResult LoadNotifData()
        {
            var notificationCountResults = new NotificationCountResult();
            var notifications = new List<Notification>();
            int total = 0;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["noticConn"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT [Id],[TranType],[NotificationMessage],[IdRead]
              FROM [dbo].[Notifications]", connection))
                {
                    // Make sure the command object does not already have
                    // a notification object associated with it.
                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            notifications.Add(new Notification
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                                TranType = reader.IsDBNull(reader.GetOrdinal("TranType")) ? "" : reader.GetString(reader.GetOrdinal("TranType")),
                                NotificationMessage = reader.IsDBNull(reader.GetOrdinal("NotificationMessage")) ? "" : reader.GetString(reader.GetOrdinal("NotificationMessage")),
                                IdRead = reader.IsDBNull(reader.GetOrdinal("IdRead")) ? false : reader.GetBoolean(reader.GetOrdinal("IdRead"))
                              });
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                        }
                    }

                }
            }

            notificationCountResults.notifications = notifications.ToList();
            notificationCountResults.Count = notifications.Count();
            return notificationCountResults;
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SendNotification();
            }
        }

        public override Task OnConnected()
        {
            SendNotification();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            SendNotification();
            return base.OnDisconnected(stopCalled);
        }
    }
}