using SCallLog.Models;
using SCallLog.Models.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCallLog.Controllers
{
    public class UserController : Controller
    {
        GlobalClass gc = new GlobalClass();
        tbl_user asUser = new tbl_user();

        // GET: User
        public ActionResult Index()
        {
            if (Session["am_userid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult AllocatedComplaints()
        {
            if (Session["am_userid"] != null)
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
            if (Session["am_userid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult AsignedComplaintsMap()
        {
            if (Session["am_userid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }

        public ActionResult WorkOrders()
        {
            if (Session["am_userid"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Login");
            }
        }
        public ActionResult getComplaintsCount()
        {
            Dictionary<string, object> dic = asUser.getComplaintsCount();

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAssignedComplaints(int pageIndex, int pageSize, string sorting, string search)
        {
            //Commentedby prasad on 19-02-2021 due to error
            //Dictionary<string, object> dic = asUser.getAssignedComplaints(pageIndex, pageSize, sorting, search);
            Dictionary<string, object> dic = asUser.getMapAssignedComplaints();
            return Json(dic, JsonRequestBehavior.AllowGet);

            //return null;

            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult getWorkOrders(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = asUser.getWorkOrders(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult getMapAssignedComplaints()
        {
            Dictionary<string, object> dic = asUser.getMapAssignedComplaints();
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult getJobcardLocationDetails(int complaintId)
        {
            Dictionary<string, object> dic = asUser.getJobcardLocationDetails(complaintId);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult addWorkOrder(List<WorkOrder> workOrders)
        {
            Dictionary<string, object> dic = asUser.addWorkOrder(workOrders);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult getUserJobCards(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = asUser.getUserJobCards(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult getJobCardsByCompany(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = asUser.getCompanyJobCards(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult getJobCardHistory(string refNumber)
        {
            Dictionary<string, object> dic = asUser.getJobCardHistory(refNumber);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult updateComplaintStatus(int ID, string Status,string PStatus, string SDate, string EDate, string SHours, string Comments)
        {
            Dictionary<string, object> dic = asUser.updateComplaintStatus(ID, Status, PStatus, SDate, EDate, SHours, Comments);
            return Json(dic, JsonRequestBehavior.AllowGet);

        }
        public ActionResult updateJobcardStatus(JobcardUpdatedData jobData)
        {
            Dictionary<string, object> dic = asUser.updateJobcardStatus(jobData);
            return Json(dic, JsonRequestBehavior.AllowGet);

        }
    }
}