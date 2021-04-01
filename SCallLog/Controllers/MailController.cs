using ActiveUp.Net.Mail;
using SCallLog.Models;
using SCallLog.Models.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCallLog.Controllers
{
    public class MailController : Controller
    {
        GlobalClass gc = new GlobalClass();
        // GET: Mail
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HtmlMarker()
        {
            return View();
        }
        public ActionResult ReadImap()
        {
            var mailRepository = new MailRepository(
                                    "mail.pyconline.co.za",
                                    993,
                                    true,
                                    "test@pyconline.co.za",
                                    "P@ssw0rd12*"
                                );
            var emailList = mailRepository.GetAllMails("inbox");
            return Json(emailList, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        //public void ReadImap()
        //{
        //    var mailRepository = new MailRepository(
        //                            "imap.gmail.com",
        //                            993,
        //                            true,
        //                            "kannavenkateswarlu29@gmail.com",
        //                            "8099353698"
        //                        );

        //    var emailList = mailRepository.GetAllMails("inbox");

        //    foreach (Message email in emailList)
        //    {
        //        Console.WriteLine("<p>{0}: {1}</p><p>{2}</p>", email.From, email.Subject, email.BodyHtml.Text);
        //        if (email.Attachments.Count > 0)
        //        {
        //            foreach (MimePart attachment in email.Attachments)
        //            {
        //                Console.WriteLine("<p>Attachment: {0} {1}</p>", attachment.ContentName, attachment.ContentType.MimeType);
        //            }
        //        }
        //    }
        //}
    }
}