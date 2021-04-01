using SCallLog.Models;
using SCallLog.Models.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCallLog.Controllers
{
    public class SCallController : Controller
    {
        GlobalClass gc = new GlobalClass();
        tbl_scall asCall = new tbl_scall();
        // GET: SCall
        public ActionResult Index()
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

        public ActionResult MainIndex()
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

        public ActionResult LocationComplaints()
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

        public ActionResult UserComplaints()
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

        //DashBoad

        public ActionResult getComplaintsCount()
        {
            Dictionary<string, object> dic = asCall.getComplaintsCount();

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDashboardData(string year)
        {
            Dictionary<string, object> dic = asCall.getDashboardData(year);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDashboardAreaCahrtData(string year)
        {
            Dictionary<string, object> dic = asCall.getDashboardAreaCahrtData(year);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getComplaintsCategorywiseCount(string Department, string DeptID)
        {
            Dictionary<string, object> dic = asCall.getComplaintsCategorywiseCount(Department, DeptID);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getComplaintsSubCategorywiseCount(string Category, string CatID)
        {
            Dictionary<string, object> dic = asCall.getComplaintsSubCategorywiseCount(Category, CatID);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //Location DashBoard

        public ActionResult getLocationComplaintsCount(string Location)
        {
            Dictionary<string, object> dic = asCall.getLocationComplaintsCount(Location);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getLocationComplaintsCategorywiseCount(string Department, string DeptID, string Location)
        {
            Dictionary<string, object> dic = asCall.getLocationComplaintsCategorywiseCount(Department, DeptID, Location);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getLocationComplaintsSubCategorywiseCount(string Category, string CatID, string Location)
        {
            Dictionary<string, object> dic = asCall.getLocationComplaintsSubCategorywiseCount(Category, CatID, Location);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //Users DashBoard
        public ActionResult getUserComplaintsCount()
        {
            Dictionary<string, object> dic = asCall.getUserComplaintsCount();

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getComplaintUsersCount(int userID)
        {
            Dictionary<string, object> dic = asCall.getComplaintUsersCount(userID);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getComplaintsCategorywiseUserCount(string Department, string DeptID, int userID)
        {
            Dictionary<string, object> dic = asCall.getComplaintsCategorywiseUserCount(Department, DeptID, userID);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getComplaintsSubCategorywiseUsersCount(string Category, string CatID, int userID)
        {
            Dictionary<string, object> dic = asCall.getComplaintsSubCategorywiseUsersCount(Category, CatID, userID);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }


        //DashBoard tables

        public ActionResult getAllDepartmentComplaints(int pageIndex, int pageSize, string sorting, string search,string SelectedDepartment,string SelectedCategory, string Department, string SColumn)
        {
            Dictionary<string, object> dic = asCall.getAllDepartmentComplaints(pageIndex, pageSize, sorting, search, SelectedDepartment, SelectedCategory, Department, SColumn);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult getAllLocationDepartmentComplaints(int pageIndex, int pageSize, string sorting, string search, string SearchLocation, string SelectedDepartment, string SelectedCategory, string Department, string SColumn)
        {
            Dictionary<string, object> dic = asCall.getAllLocationDepartmentComplaints(pageIndex, pageSize, sorting, search, SearchLocation, SelectedDepartment, SelectedCategory, Department, SColumn);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult getAllUserDepartmentComplaints(int pageIndex, int pageSize, string sorting, string search, int selectedUserID, string SelectedDepartment, string SelectedCategory, string Department, string SColumn)
        {
            Dictionary<string, object> dic = asCall.getAllUserDepartmentComplaints(pageIndex, pageSize, sorting, search, selectedUserID, SelectedDepartment, SelectedCategory, Department, SColumn);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }


    }
}