using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace PMS.Models.Packages
{
    public class Elasticmail
    {
        
        //public static string USERNAME = "admin@sixstepsolution.co.za";
        //public static string API_KEY = "c926921d-bdb5-4214-8777-d4251052e66e";
        public static string USERNAME = System.Configuration.ConfigurationManager.AppSettings["Elastic_UserName"].ToString();
        public static string API_KEY = System.Configuration.ConfigurationManager.AppSettings["Elastic_Api_Key"].ToString();
        public static string SendEmail(string to, string subject, string bodyText, string bodyHtml, string from, string fromName, string attachments)
        {
            //WebClient client = new WebClient();
            WebClient client = new WebClient();
            NameValueCollection values = new NameValueCollection();

            values.Add("username", USERNAME);
            values.Add("api_key", API_KEY);
            values.Add("from", from);
            values.Add("from_name", fromName);
            values.Add("subject", subject);
            if (bodyHtml != null)
                values.Add("body_html", bodyHtml);
            if (bodyText != null)
                values.Add("body_text", bodyText);
            values.Add("to", to);
            values.Add("bcc", "jai1970@gmail.com");
            if (attachments != null)
                values.Add("attachments", attachments);
            byte[] response = client.UploadValues("https://api.elasticemail.com/mailer/send", values);
            return Encoding.UTF8.GetString(response);
        }
        public string UploadAttachment(string filepath, string filename)
        {
            FileStream stream = File.OpenRead(filepath);
            WebRequest request = WebRequest.Create("https://api.elasticemail.com/attachments/upload?username=" + USERNAME + "&api_key=" + API_KEY + "&file=" + filename);
            request.Method = "PUT";
            request.ContentLength = stream.Length;
            Stream outstream = request.GetRequestStream();
            stream.CopyTo(outstream, 4096);
            stream.Close();
            WebResponse response = request.GetResponse();
            string result = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            response.Close();
            return result;
        }
    }
}