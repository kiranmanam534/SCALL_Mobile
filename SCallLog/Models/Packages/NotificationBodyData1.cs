using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class NotificationBodyData1
    {
        public static StringBuilder NewUserCreate(SCL_Users UserData)
        {
            StringBuilder strBulBodyContent = new StringBuilder();
            strBulBodyContent.Append("<html>");
            strBulBodyContent.Append("<head>");
            strBulBodyContent.Append("</head>");
            strBulBodyContent.Append("<body>");
            strBulBodyContent.Append("<div>");
            strBulBodyContent.Append("<div>Dear User,</div><br /><br/>");
            strBulBodyContent.Append("<div>Leads Credentials</div><br/>");
            strBulBodyContent.Append("<div>Username: " + UserData.EmailID + "</div><br/>");
            strBulBodyContent.Append("<div>Password: " + UserData.UserPass + "</div><br/>");
            //strBulBodyContent.Append("<div>PIN: " + UserData.PIN + "</div><br/>");
            strBulBodyContent.Append("<div>Click the below link to login to your account.</div><br/>");

            string baseUrl = HttpContext.Current.Request.Url.Authority;

            //Encrypter enc = new Encrypter();
            //string url = string.Format("{0}/Master/ActivateUser?e1={1}&e2={2}&e3={3}", baseUrl, enc.Encrypt(UserData.ID.ToString()), enc.Encrypt(UserData.CompanyID.ToString()), enc.Encrypt(UserData.EmailID.ToString()));

            string url = "http://" + baseUrl;

            //strBulBodyContent.Append("<div><a href='" + baseUrl + "'>Go to AssetManagement</a></div><br/>");

            strBulBodyContent.Append("<a href='" + baseUrl + "' style='cursor:pointer'><input type='button' value='Go to Leads' style='height:50px;width:200px;background-color:green;border-radius:3px;color:white' /></a>");

            strBulBodyContent.Append("<br/><br/><br/>Regards<br />");
            strBulBodyContent.Append("Leads Team.");
            strBulBodyContent.Append("</div>");
            strBulBodyContent.Append("</body>");
            strBulBodyContent.Append("</html>");
            return strBulBodyContent;
        }
        public static StringBuilder NewUserUpdate(SCL_Users UserData)
        {
            StringBuilder strBulBodyContent = new StringBuilder();
            strBulBodyContent.Append("<html>");
            strBulBodyContent.Append("<head>");
            strBulBodyContent.Append("</head>");
            strBulBodyContent.Append("<body>");
            strBulBodyContent.Append("<div>");
            strBulBodyContent.Append("<div>Dear User, your account details has been updated.</div><br /><br/>");
            strBulBodyContent.Append("<div>Account details</div><br/>");
            //strBulBodyContent.Append("<div>Username: " + UserData.UserName + "</div><br/>");
            strBulBodyContent.Append("<div>Email ID: " + UserData.EmailID + "</div><br/>");
            strBulBodyContent.Append("<div>Click the below link to login to your account to view your updated details.</div><br/>");

            string baseUrl = HttpContext.Current.Request.Url.Authority;

            //Encrypter enc = new Encrypter();
            //string url = string.Format("{0}/Master/ActivateUser?e1={1}&e2={2}&e3={3}", baseUrl, enc.Encrypt(UserData.ID.ToString()), enc.Encrypt(UserData.CompanyID.ToString()), enc.Encrypt(UserData.EmailID.ToString()));

            string url = "http://" + baseUrl;

            //strBulBodyContent.Append("<div><a href='" + baseUrl + "'>Go to AssetManagement</a></div><br/>");

            strBulBodyContent.Append("<a href='" + baseUrl + "' style='cursor:pointer'><input type='button' value='Go to Leads' style='height:50px;width:200px;background-color:green;border-radius:3px;color:white' /></a>");

            strBulBodyContent.Append("<br/><br/><br/>Regards<br />");
            strBulBodyContent.Append("Leads Team.");
            strBulBodyContent.Append("</div>");
            strBulBodyContent.Append("</body>");
            strBulBodyContent.Append("</html>");
            return strBulBodyContent;
        }
    }
}