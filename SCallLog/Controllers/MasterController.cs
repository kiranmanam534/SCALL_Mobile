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
    public class MasterController : Controller
    {
        GlobalClass gc = new GlobalClass();
        tbl_master asData = new tbl_master();
        // GET: Master
        public ActionResult Logout()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("../Home/Login");
        }

        public ActionResult AddNewDepartment()
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
        public ActionResult AddCategories()
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
        public ActionResult AddSubCategories()
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
        public ActionResult AddNewRole()
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
        public ActionResult CreateUser()
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
        public ActionResult getDepartmentsAndRoles()
        {

            Dictionary<string, object> dic = asData.getDepartmentsAndRoles();
            //var result = Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getComplaintDepartments()
        {

            Dictionary<string, object> dic = asData.getComplaintDepartments();
            //var result = Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getComplaintCategoriesbyDeptID(string DeptID)
        {

            Dictionary<string, object> dic = asData.getComplaintCategoriesbyDeptID(DeptID);
            //var result = Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;

            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAllComplaintCategories()
        {

            Dictionary<string, object> dic = asData.getAllComplaintCategories();
            //var result = Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getComplaintSubCategoriesbyCatID(string CatID)
        {

            Dictionary<string, object> dic = asData.getComplaintSubCategoriesbyCatID(CatID);
            //var result = Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAllDepartments(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = asData.getAllDepartments(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult getAllCategories(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = asData.getAllCategories(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult getAllSubCategories(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = asData.getAllSubCategories(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult getUsers(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = asData.getUsers(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        //public ActionResult getUsers(int pageIndex, int pageSize, string sorting, string search)
        //{
        //    //if (Session["AMUSERDATA_USER_NAME"] == null)
        //    //{
        //    //    Dictionary<string, object> dic1 = new Dictionary<string, object>();
        //    //    dic1.Add("expire", true);
        //    //    return Json(dic1, JsonRequestBehavior.AllowGet);
        //    //}

        //    Dictionary<string, object> dic = asData.getUsers(pageIndex, pageSize, sorting, search);
        //    var result = Json(dic, JsonRequestBehavior.AllowGet);
        //    result.MaxJsonLength = Int32.MaxValue;

        //    return result;
        //}
        public ActionResult addDepartment(string dept, string status)
        {
            Dictionary<string, object> dic = asData.addDepartment(dept, status);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult updateDepartment(int DeptID, string Depname, string Status)
        {
            Dictionary<string, object> dic = asData.updateDepartment(DeptID, Depname, Status);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult updateCategory(int catID, string catname, string status)
        {
            Dictionary<string, object> dic = asData.updateCategory(catID, catname, status);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult updateSubCategory(int subCatID, string subCatName, string status)
        {
            Dictionary<string, object> dic = asData.updateSubCategory(subCatID, subCatName, status);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult getAllRoles(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = asData.getAllRoles(pageIndex, pageSize, sorting, search);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult addRole(string RoleName, string Status)
        {
            Dictionary<string, object> dic = asData.addRole(RoleName, Status);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult updateRole(int RoleID, string Role, string Status)
        {
            Dictionary<string, object> dic = asData.updateRole(RoleID, Role, Status);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        [HttpPost]
        public ActionResult CreateNewUser(SCL_Users UserFormData, string password, string SaveType)
        {

            //if (Session["AMUSERDATA_USER_NAME"] == null)
            //{
            //    Dictionary<string, object> dic1 = new Dictionary<string, object>();
            //    dic1.Add("expire", true);
            //    return Json(dic1, JsonRequestBehavior.AllowGet);
            //}

            Dictionary<string, object> dic = SaveType == "Add" ? asData.CreateNewUser(UserFormData, password) : asData.UpdateUser(UserFormData);
            var result = Json(dic, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = Int32.MaxValue;

            return result;
        }

        [HttpPost]
        public ActionResult CreateNewUserWithAttachment()
        {

            var UserFormData = Request.Form["UserFormData"];
            var SaveTypeTemp = Request.Form["SaveType"];
            var passwordTemp = Request.Form["password"];


            var UserData = JsonConvert.DeserializeObject<SCL_Users>(UserFormData);
            var SaveType = JsonConvert.DeserializeObject<string>(SaveTypeTemp);
            var Password = JsonConvert.DeserializeObject<string>(passwordTemp);

            Dictionary<string, object> dic = SaveType == "Add" ?
                asData.CreateNewUserWithAttachment(Request.Files, Password, UserData, Request.Browser.Browser.ToUpper()) :
                asData.UpdateUserWithAttachment(Request.Files, UserData, Request.Browser.Browser.ToUpper());
            return Json(dic, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getUserDetails(int UserID)
        {
            //if (Session["AMUSERDATA_USER_NAME"] == null)
            //{
            //    Dictionary<string, object> dic1 = new Dictionary<string, object>();
            //    dic1.Add("expire", true);
            //    return Json(dic1, JsonRequestBehavior.AllowGet);
            //}
            Dictionary<string, object> dic = asData.getUserDetails(UserID);
            var result = Json(dic, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = Int32.MaxValue;

            return result;
        }
        public ActionResult getKPIUsers()
        {
            Dictionary<string, object> dic = asData.getKPIUsers();
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }

        public ActionResult addCategory(string deptId,string cat, string status)
        {
            Dictionary<string, object> dic = asData.addCategory(deptId,cat, status);
            return Json(dic, JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = Int32.MaxValue;
            //return result;
        }
        public ActionResult addSubCategory(string catId,string subcat, string status)
        {
            Dictionary<string, object> dic = asData.addSubCategory(catId, subcat, status);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
    }
}