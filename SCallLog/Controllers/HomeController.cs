using Newtonsoft.Json;
using SCallLog.Models;
using SCallLog.Models.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCallLog.Controllers
{

    public class HomeController : Controller
    {
        tbl_welcome ad = new tbl_welcome();
        GlobalClass gc = new GlobalClass();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult addCompany()
        {
            var UserFormData = Request.Form["UserFormData"];
            var FreeTrailTemp = Request.Form["FreeTrail"];


            var UserData = JsonConvert.DeserializeObject<SCL_CompanyRegistration>(UserFormData);
            var FreeTrail = JsonConvert.DeserializeObject<string>(FreeTrailTemp);



            Dictionary<string, object> dic = ad.addCompany(Request.Files, FreeTrail, UserData, Request.Browser.Browser.ToUpper());
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult addCustomer(string FirstName, string LastName, string Email, string Password, string Telephone)
        {
            Dictionary<string, object> dic = ad.addCustomer(FirstName, LastName, Email, Password, Telephone);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult getPassword(string Company_Email)
        {
            Dictionary<string, object> dic = ad.getPassword(Company_Email);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult getUserPassword(string User_Email)
        {
            Dictionary<string, object> dic = ad.getUserPassword(User_Email);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }


        [HttpGet]
        public ActionResult SCLLogin(string Username, string Password)
        {
            Dictionary<string, object> dct = new Dictionary<string, object>();
            if (Username != "" && Password != "")
            {
                var result = gc.db.SCL_Login.Where(s => s.username == Username && s.password == Password).AsQueryable();
                if (result.Count() > 0)
                {
                    if (result.FirstOrDefault().type.Equals("ADMIN"))
                    {
                        if (result.FirstOrDefault().SubscriptionEndDate < DateTime.Now)
                        {
                            dct.Add("error", "Your Subscription was Expired. Contact to Admin for Subscription");
                        }
                        else
                        {
                            var result3 = gc.db.SCL_UserRoles.Where(x => x.CompanyID == result.FirstOrDefault().CID && x.RoleName.Equals("Admin")).FirstOrDefault();
                            if (result3 != null)
                            {
                                var result2 = gc.db.SCL_Users.Where(x => x.CompanyID == result.FirstOrDefault().CID && x.RoleID == result3.ID).FirstOrDefault();
                                if (result2 != null)
                                {
                                    if (result2.ProfilePicture != null)
                                    {
                                        Session["SCL_user_profile"] = result2.ProfilePicture;
                                    }
                                    else
                                    {
                                        Session["SCL_user_profile"] = "picture.jpg";
                                    }

                                }
                                else
                                {
                                    Session["SCL_user_profile"] = "picture.jpg";
                                }
                            }
                            else
                            {
                                Session["SCL_user_profile"] = "picture.jpg";
                            }

                            var result1 = gc.db.SCL_CompanyRegistration.Where(s => s.CID == result.FirstOrDefault().CID).AsQueryable();
                            Session["am_userid_17*"] = Username;
                            Session["email_SCL_account"] = result1.FirstOrDefault().company_email;
                            Session["cid_SCL_account"] = result1.FirstOrDefault().CID;
                            Session["name_SCL_account"] = result1.FirstOrDefault().company_name;
                            Session["username_SCL_account"] = result1.FirstOrDefault().first_name + " " + result1.FirstOrDefault().last_name;

                            Session["mobile_SCL_account"] = result1.FirstOrDefault().company_mobile;
                            Session["remaining_days_SCL_account"] = (result1.FirstOrDefault().SubscriptionEndDate - DateTime.Now);
                            if (result1.FirstOrDefault().ProfilePicture != null)
                            {
                                Session["SCL_company_picture"] = result1.FirstOrDefault().ProfilePicture;
                            }
                            else
                            {
                                Session["SCL_company_picture"] = "picture.jpg";
                            }

                            dct.Add("Type", result.FirstOrDefault().type);
                            dct.Add("Days", (result1.FirstOrDefault().SubscriptionEndDate - DateTime.Now));
                            dct.Add("success", result.Count());
                        }

                    }
                    else if (result.FirstOrDefault().type.Equals("USER"))
                    {
                        var result1 = gc.db.SCL_Users.Where(s => s.ID == result.FirstOrDefault().CID).FirstOrDefault();
                        var result2 = gc.db.SCL_CompanyRegistration.Where(s => s.CID == result1.CompanyID).FirstOrDefault();
                        if (result2.ProfilePicture != null)
                        {
                            Session["SCL_company_picture"] = result2.ProfilePicture;
                        }
                        else
                        {
                            Session["SCL_company_picture"] = "picture.jpg";
                        }
                        Session["name_SCL_account"] = result2.company_name;

                        Session["am_userid"] = result1.ID;
                        Session["email_SCL_account"] = Username;
                        Session["cid_SCL_account"] = result1.CompanyID;
                        if (result1.ProfilePicture != null)
                        {
                            Session["SCL_user_profile"] = result1.ProfilePicture;
                        }
                        else
                        {
                            Session["SCL_user_profile"] = "picture.jpg";
                        }
                        Session["username_SCL_account"] = result1.FirstName + " " + result1.LastName;
                        dct.Add("success", result.Count());
                        dct.Add("Type", result.FirstOrDefault().type);

                    }
                    else
                    {
                        dct.Add("error", "Invalid username and password");
                    }

                }
                else
                {
                    dct.Add("error", "Invalid username and password");
                }

            }
            return Json(dct, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoginUser(string Username, string Password)
        {
            Dictionary<string, object> dct = new Dictionary<string, object>();
            if (Username != "" && Password != "")
            {
                var result = gc.db.SCL_Users.Where(s => s.EmailID == Username && s.UserPass == Password).AsQueryable();
                if (result.Count() > 0)
                {
                    Session["am_userid"] = result.FirstOrDefault().ID;
                    Session["email_SCL_account"] = Username;
                    Session["cid_SCL_account"] = result.FirstOrDefault().CompanyID;
                    Session["SCL_user_profile"] = result.FirstOrDefault().ProfilePicture;
                    Session["name_SCL_account"] = result.FirstOrDefault().FirstName + " " + result.FirstOrDefault().LastName;
                    dct.Add("success", result.Count());
                }
                else
                {
                    dct.Add("error", "Invalid username and password");
                }

            }
            return Json(dct, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCompanyList()
        {
            Dictionary<string, object> dic = ad.getCompanyList();
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult getCustomerList()
        {
            Dictionary<string, object> dic = ad.getCustomerList();
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
    }
}