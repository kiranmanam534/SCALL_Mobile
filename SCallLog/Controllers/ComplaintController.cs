using Newtonsoft.Json;
using SCallLog.Models;
using SCallLog.Models.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SCallLog.Controllers
{
    public class ComplaintController : Controller
    {
        GlobalClass gc = new GlobalClass();
        tbl_complaints ascomp = new tbl_complaints();
        // GET: Complaint
        public ActionResult Compaints()
        {
            if (Session["am_userid_17*"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult AddComplaint()
        {
            
            if (Session["am_userid_17*"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult ViewJobCards()
        {

            if (Session["am_userid_17*"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult CreateJobCard()
        {

            if (Session["am_userid_17*"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }
        public ActionResult getUsers()
        {
            Dictionary<string, object> dic = ascomp.getUsers();

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUserStatus()
        {
            Dictionary<string, object> dic = ascomp.getUserStatus();

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getComplaintHistory(string refNumber)
        {
            Dictionary<string, object> dic = ascomp.getComplaintHistory(refNumber);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getAllComplaints(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = ascomp.getAllComplaints(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult getReceivedComplaints(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = ascomp.getReceivedComplaints(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult getComplaintsCount(int userID)
        {
            Dictionary<string, object> dic = ascomp.getComplaintsCount(userID);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignedComplaintUser(int ID, int UserID, string Comments)
        {
            Dictionary<string, object> dic = ascomp.AssignedComplaintUser(ID, UserID, Comments);
            return Json(dic, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public ActionResult SaveComplaint()
        {

            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Session["cid_SCL_account"].ToString());

            var FormData = Request.Form["FormData"];
            var ComplaintMasterResponse = JsonConvert.DeserializeObject<SCL_Mobile_Complaints>(FormData);
            var files = Request.Files;
            var Browser = Request.Browser.Browser.ToUpper();
            string file_Name = "";
            string fileName = "";

            Dictionary<string, object> dct = new Dictionary<string, object>();
            using (var db = new scallEntities())
            {
                try
                { 
                    string siteURL = WebConfigurationManager.AppSettings["imagePathURL"];
                    string refNumber = ComplaintMasterResponse.Category_Id;
                    var complants = db.SCL_Mobile_Complaints.OrderByDescending(x => x.ID).FirstOrDefault();
                    if (complants != null)
                    {
                        refNumber = refNumber + (complants.ID + 1);
                    }
                    else
                    {
                        refNumber = refNumber + "000";
                    }
                    var date = DateTime.Now;
                    var Depts = db.SCL_Department.Where(x => x.DEPARTMENT_ID == ComplaintMasterResponse.Dept_Id).FirstOrDefault();
                    var cats = db.SCL_Category.Where(x => x.CATEGORY_ID == ComplaintMasterResponse.Category_Id).FirstOrDefault();
                    var subCats = db.SCL_Sub_Category.Where(x => x.SUB_CATEGORY_ID == ComplaintMasterResponse.SubCategory_Id).FirstOrDefault();
                    ComplaintMasterResponse.Dept_Desc = Depts!=null?Depts.DEPARTMENT_DESC:"";
                    ComplaintMasterResponse.Category_Desc = cats!=null? cats.CATEGORY_DESC:"";
                    ComplaintMasterResponse.SubCategory_Desc = subCats!=null ? subCats.SUB_CATEGORY_DESC:"";
                    ComplaintMasterResponse.Complaint_ReferenceNo = refNumber;
                    ComplaintMasterResponse.Complaint_Date = date.ToString();
                    ComplaintMasterResponse.CreatedDate = DateTime.Now;
                    ComplaintMasterResponse.ComplaintStatus = "Received";
                    ComplaintMasterResponse.CompanyID = LoginUserCompanyID;

                    db.SCL_Mobile_Complaints.Add(ComplaintMasterResponse);
                    int n = db.SaveChanges();


                    if (n > 0)
                    {
                        dct.Add("success", true);
                        if (files.Count > 0)
                        {  //  Get all files from Request object 
                            for (int i = 0; i < files.Count; i++)
                            {
                                HttpPostedFileBase file = files[i];

                                if (Browser == "IE" || Browser == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                    fileName = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fileName = file.FileName;
                                }

                                fileName = file.FileName;
                                file_Name = string.Format("{0}-{1}", DateTime.Now.ToString("ddMMMyyyyHHmmss"), fileName.Replace("-", ""));
                                fileName = Path.Combine(HttpContext.Server.MapPath("~/Attachments/"), file_Name);
                                file.SaveAs(fileName);

                                SCL_ComplaintImages img = new SCL_ComplaintImages();
                                //img.AttachmentName = string.Format("{0}/{1}", siteURL, fileName);
                                img.AttachmentName = file_Name;
                                img.Complaint_ID = ComplaintMasterResponse.ID;
                                img.Complaint_ReferenceNo = ComplaintMasterResponse.Complaint_ReferenceNo;
                                db.SCL_ComplaintImages.Add(img);
                                db.SaveChanges(); 
                            }
                        }                        
                    }
                    else
                    {
                        dct.Add("success", false);
                    }
                }
                catch (Exception ex)
                {
                    dct.Add("isError", true);
                    dct.Add("exception", ex.Message);
                }

            }

            return Json(dct, JsonRequestBehavior.AllowGet);

        }
    }
}