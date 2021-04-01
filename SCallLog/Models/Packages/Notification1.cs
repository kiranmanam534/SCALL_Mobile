using PMS.Models.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class Notification1
    {
        public static string NewUserCreate(string fromMail, SCL_Users UserData)
        {
            string emailStatus = "";
            string fromName = "Leads";
            string bodyHtml = NotificationBodyData1.NewUserCreate(UserData).ToString();
            string bodyText = "";

            string heading = "New User Account Credentials";

            emailStatus = Elasticmail.SendEmail(UserData.EmailID, heading, bodyText, bodyHtml, fromMail, fromName, "");
            return emailStatus;
        }
        public static string NewUserUpdate(string fromMail, SCL_Users UserData)
        {
            string emailStatus = "";
            string fromName = "Leads";
            string bodyHtml = NotificationBodyData1.NewUserUpdate(UserData).ToString();
            string bodyText = "";

            string heading = "User Account Details Updated";

            emailStatus = Elasticmail.SendEmail(UserData.EmailID, heading, bodyText, bodyHtml, fromMail, fromName, "");
            return emailStatus;
        }
    }
}