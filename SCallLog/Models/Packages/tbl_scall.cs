using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class tbl_scall
    {
        GlobalClass gc = new GlobalClass();
        string recstatus = "Received";
        string assstatus = "Assigned";
        string compstatus = "Completed";
        string holdstatus = "Hold";
        string inprogressstatus = "InProgress";
        // DashBoard

        public Dictionary<string, object> getComplaintsCount()
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();            
            try
            {                
                var rersult = (from d in gc.db.SCL_Department
                               orderby d.DEPARTMENT_DESC
                               select new
                               {
                                   d.DEPARTMENT_DESC,
                                   d.DEPARTMENT_ID,
                                   assCount =(from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   recCount =(from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.ComplaintStatus == recstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount =(from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count()

                               }).ToList();


                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null && d.CompanyID == LoginUserCompanyID
                                select new
                               {
                                   d.Dept_Desc,
                                   d.Category_Desc,
                                   d.SubCategory_Desc,
                                   d.Lattitude,
                                   d.Longitude,
                                   d.Address,
                                   d.ComplaintStatus,
                                   d.Complaint_Date,
                                   d.automatic_complaint,
                                  
                               }).ToList();



                dic.Add("deptCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);
                

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getDashboardData(string date)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                var result = (gc.db.sp_getDashboardData1(LoginUserCompanyID)).ToList();

                var ReceivedCount = gc.db.sp_getComplaintCountsByMonthWise(date, "Received", LoginUserCompanyID).ToList();
                var AssignedCount = gc.db.sp_getComplaintCountsByMonthWise(date, "Assigned", LoginUserCompanyID).ToList();
                var InProgressCount = gc.db.sp_getComplaintCountsByMonthWise(date, "InProgress", LoginUserCompanyID).ToList();
                var HoldCount = gc.db.sp_getComplaintCountsByMonthWise(date, "Hold", LoginUserCompanyID).ToList();
                var CompletedCount = gc.db.sp_getComplaintCountsByMonthWise(date, "Completed", LoginUserCompanyID).ToList();
                var CityParksCount = gc.db.sp_getComplaintCountsByDepartmentWise("City Parks", LoginUserCompanyID).ToList();
                var ElectricityCount = gc.db.sp_getComplaintCountsByDepartmentWise("Electricity", LoginUserCompanyID).ToList();
                var MetroPoliceCount = gc.db.sp_getComplaintCountsByDepartmentWise("Metro Police", LoginUserCompanyID).ToList();
                var RoadsandStromWaterCount = gc.db.sp_getComplaintCountsByDepartmentWise("Roads and Strom Water", LoginUserCompanyID).ToList();
                var SolidWasteCount = gc.db.sp_getComplaintCountsByDepartmentWise("Solid Waste", LoginUserCompanyID).ToList();
                var WaterandSanitationCount = gc.db.sp_getComplaintCountsByDepartmentWise("Water and Sanitation", LoginUserCompanyID).ToList();
                var TransportCount = gc.db.sp_getComplaintCountsByDepartmentWise("Transport", LoginUserCompanyID).ToList();
                var AdministrationCount = gc.db.sp_getComplaintCountsByDepartmentWise("Administration", LoginUserCompanyID).ToList();
                var MyBillsCount = gc.db.sp_getComplaintCountsByDepartmentWise("My Bills", LoginUserCompanyID).ToList();
                var PropertyRatesCount = gc.db.sp_getComplaintCountsByDepartmentWise("Property Rates", LoginUserCompanyID).ToList();
                var AutometicCount = gc.db.sp_getComplaintCountsByDepartmentWise("Automatic", LoginUserCompanyID).ToList();
                            
                dic.Add("DashboardCount", result);
                dic.Add("ReceivedCount", ReceivedCount);
                dic.Add("AssignedCount", AssignedCount);
                dic.Add("InProgressCount", InProgressCount);
                dic.Add("HoldCount", HoldCount);
                dic.Add("CompletedCount", CompletedCount);
                dic.Add("CityParksCount", CityParksCount);
                dic.Add("ElectricityCount", ElectricityCount);
                dic.Add("MetroPoliceCount", MetroPoliceCount);
                dic.Add("RoadsandStromWaterCount", RoadsandStromWaterCount);
                dic.Add("SolidWasteCount", SolidWasteCount);
                dic.Add("WaterandSanitationCount", WaterandSanitationCount);
                dic.Add("TransportCount", TransportCount);
                dic.Add("AdministrationCount", AdministrationCount);
                dic.Add("MyBillsCount", MyBillsCount);
                dic.Add("PropertyRatesCount", PropertyRatesCount);
                dic.Add("AutometicCount", AutometicCount);
                dic.Add("success", true);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getDashboardAreaCahrtData(string year)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {                
                var ReceivedCount = gc.db.sp_getComplaintCountsByMonthWise(year, "Received", LoginUserCompanyID).ToList();
                var AssignedCount = gc.db.sp_getComplaintCountsByMonthWise(year, "Assigned", LoginUserCompanyID).ToList();
                var InProgressCount = gc.db.sp_getComplaintCountsByMonthWise(year, "InProgress", LoginUserCompanyID).ToList();
                var HoldCount = gc.db.sp_getComplaintCountsByMonthWise(year, "Hold", LoginUserCompanyID).ToList();
                var CompletedCount = gc.db.sp_getComplaintCountsByMonthWise(year, "Completed", LoginUserCompanyID).ToList();
                
                dic.Add("ReceivedCount", ReceivedCount);
                dic.Add("AssignedCount", AssignedCount);
                dic.Add("InProgressCount", InProgressCount);
                dic.Add("HoldCount", HoldCount);
                dic.Add("CompletedCount", CompletedCount);
                
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }


        public Dictionary<string, object> getComplaintsCategorywiseCount(string Department,string DeptID)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {


                var rersult = (from d in gc.db.SCL_Category
                               where d.DEPARTMENT_ID == DeptID
                               orderby d.CATEGORY_DESC
                               select new
                               {
                                   d.CATEGORY_DESC,
                                   d.CATEGORY_ID,
                                   assCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   recCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.ComplaintStatus == recstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.CompanyID == LoginUserCompanyID select c.Category_Desc).Count()

                               }).ToList();

                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null &&
                                d.Dept_Desc == Department && d.CompanyID == LoginUserCompanyID
                                select new
                                {
                                    d.Dept_Desc,
                                    d.Category_Desc,
                                    d.SubCategory_Desc,
                                    d.Lattitude,
                                    d.Longitude,
                                    d.Address,
                                    d.ComplaintStatus,
                                    d.Complaint_Date,
                                    d.automatic_complaint,

                                }).ToList();


                dic.Add("catCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getComplaintsSubCategorywiseCount(string Category, string CatID)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {


                var rersult = (from d in gc.db.SCL_Sub_Category
                               where d.CATEGORY_ID == CatID
                               orderby d.SUB_CATEGORY_DESC
                               select new
                               {
                                   d.SUB_CATEGORY_DESC,
                                   d.SUB_CATEGORY_ID,
                                   assCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   recCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.ComplaintStatus == recstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.CompanyID == LoginUserCompanyID select c.SubCategory_Desc).Count()

                               }).ToList();


                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null &&
                                d.Category_Id == CatID && d.CompanyID == LoginUserCompanyID
                                select new
                                {
                                    d.Dept_Desc,
                                    d.Category_Desc,
                                    d.SubCategory_Desc,
                                    d.Lattitude,
                                    d.Longitude,
                                    d.Address,
                                    d.ComplaintStatus,
                                    d.Complaint_Date,
                                    d.automatic_complaint,

                                }).ToList();

                dic.Add("scatCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        //Location DashBoard

        public Dictionary<string, object> getLocationComplaintsCount(string Location)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {


                var rersult = (from d in gc.db.SCL_Department
                               orderby d.DEPARTMENT_DESC
                               select new
                               {
                                   d.DEPARTMENT_DESC,
                                   d.DEPARTMENT_ID,
                                   assCount = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.Address.ToLower().Contains(Location.ToLower()) && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   recCount = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.Address.ToLower().Contains(Location.ToLower()) && c.ComplaintStatus == recstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.Address.ToLower().Contains(Location.ToLower()) && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.Address.ToLower().Contains(Location.ToLower()) && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count()

                               }).ToList();


                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null && d.Address.ToLower().Contains(Location.ToLower()) && d.CompanyID == LoginUserCompanyID
                                select new
                                {
                                    d.Dept_Desc,
                                    d.Category_Desc,
                                    d.SubCategory_Desc,
                                    d.Lattitude,
                                    d.Longitude,
                                    d.Address,
                                    d.ComplaintStatus,
                                    d.Complaint_Date,
                                    d.automatic_complaint,

                                }).ToList();



                dic.Add("deptCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getLocationComplaintsCategorywiseCount(string Department, string DeptID, string Location)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {


                var rersult = (from d in gc.db.SCL_Category
                               where d.DEPARTMENT_ID == DeptID
                               orderby d.CATEGORY_DESC
                               select new
                               {
                                   d.CATEGORY_DESC,
                                   d.CATEGORY_ID,
                                   assCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.Address.ToLower().Contains(Location.ToLower()) && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   recCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.Address.ToLower().Contains(Location.ToLower()) && c.ComplaintStatus == recstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.Address.ToLower().Contains(Location.ToLower()) && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.Address.ToLower().Contains(Location.ToLower()) && c.CompanyID == LoginUserCompanyID select c.Category_Desc).Count()

                               }).ToList();

                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null &&
                                d.Dept_Desc == Department &&
                                d.Address.ToLower().Contains(Location.ToLower())
                                && d.CompanyID == LoginUserCompanyID
                                select new
                                {
                                    d.Dept_Desc,
                                    d.Category_Desc,
                                    d.SubCategory_Desc,
                                    d.Lattitude,
                                    d.Longitude,
                                    d.Address,
                                    d.ComplaintStatus,
                                    d.Complaint_Date,
                                    d.automatic_complaint,

                                }).ToList();


                dic.Add("catCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getLocationComplaintsSubCategorywiseCount(string Category, string CatID, string Location)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {


                var rersult = (from d in gc.db.SCL_Sub_Category
                               where d.CATEGORY_ID == CatID
                               orderby d.SUB_CATEGORY_DESC
                               select new
                               {
                                   d.SUB_CATEGORY_DESC,
                                   d.SUB_CATEGORY_ID,
                                   assCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.Address.ToLower().Contains(Location.ToLower()) && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   recCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.Address.ToLower().Contains(Location.ToLower()) && c.ComplaintStatus == recstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.Address.ToLower().Contains(Location.ToLower()) && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.Address.ToLower().Contains(Location.ToLower()) && c.CompanyID == LoginUserCompanyID select c.SubCategory_Desc).Count()

                               }).ToList();

                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null &&
                                d.Category_Id == CatID &&
                                d.Address.ToLower().Contains(Location.ToLower())
                                && d.CompanyID == LoginUserCompanyID
                                select new
                                {
                                    d.Dept_Desc,
                                    d.Category_Desc,
                                    d.SubCategory_Desc,
                                    d.Lattitude,
                                    d.Longitude,
                                    d.Address,
                                    d.ComplaintStatus,
                                    d.Complaint_Date,
                                    d.automatic_complaint,

                                }).ToList();

                dic.Add("scatCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        //Users DasshBoard

        public Dictionary<string, object> getUserComplaintsCount()
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {


                var rersult = (from d in gc.db.SCL_Users
                               where d.CompanyID == LoginUserCompanyID && d.userType != "Admin"
                               orderby d.FirstName
                               select new
                               {
                                   d.ID,
                                   d.FirstName,
                                   d.LastName,
                                   d.EmailID,
                                   assCount = (from c in gc.db.SCL_Mobile_Complaints where c.AllocateduserID == d.ID && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   holdCount = (from c in gc.db.SCL_Mobile_Complaints where c.AllocateduserID == d.ID && c.ComplaintStatus == holdstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   inProgressCount = (from c in gc.db.SCL_Mobile_Complaints where c.AllocateduserID == d.ID && c.ComplaintStatus == inprogressstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount = (from c in gc.db.SCL_Mobile_Complaints where c.AllocateduserID == d.ID && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.AllocateduserID == d.ID && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count()

                               }).ToList();


                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null && d.AllocateduserID != null
                                && d.CompanyID == LoginUserCompanyID
                                select new
                                {
                                    d.Dept_Desc,
                                    d.Category_Desc,
                                    d.SubCategory_Desc,
                                    d.Lattitude,
                                    d.Longitude,
                                    d.Address,
                                    d.ComplaintStatus,
                                    d.Complaint_Date,
                                    d.automatic_complaint,

                                }).ToList();



                dic.Add("UsersCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getComplaintUsersCount(int userID)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {


                var rersult = (from d in gc.db.SCL_Department
                               orderby d.DEPARTMENT_DESC
                               select new
                               {
                                   d.DEPARTMENT_DESC,
                                   d.DEPARTMENT_ID,
                                   assCount = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.AllocateduserID == userID && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   holdCount = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.AllocateduserID == userID && c.ComplaintStatus == holdstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   inProgressCount = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.AllocateduserID == userID && c.ComplaintStatus == inprogressstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.AllocateduserID == userID && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.Dept_Desc == d.DEPARTMENT_DESC && c.AllocateduserID == userID && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count()

                               }).ToList();


                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null && d.AllocateduserID == userID
                                && d.CompanyID == LoginUserCompanyID
                                select new
                                {
                                    d.Dept_Desc,
                                    d.Category_Desc,
                                    d.SubCategory_Desc,
                                    d.Lattitude,
                                    d.Longitude,
                                    d.Address,
                                    d.ComplaintStatus,
                                    d.Complaint_Date,
                                    d.automatic_complaint,

                                }).ToList();



                dic.Add("deptCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getComplaintsCategorywiseUserCount(string Department, string DeptID, int userID)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {


                var rersult = (from d in gc.db.SCL_Category
                               where d.DEPARTMENT_ID == DeptID
                               orderby d.CATEGORY_DESC
                               select new
                               {
                                   d.CATEGORY_DESC,
                                   d.CATEGORY_ID,
                                   assCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.AllocateduserID == userID && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   holdCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.AllocateduserID == userID && c.ComplaintStatus == holdstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   inProgressCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.AllocateduserID == userID && c.ComplaintStatus == inprogressstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.AllocateduserID == userID && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.Category_Id == d.CATEGORY_ID && c.Dept_Desc == Department && c.AllocateduserID == userID && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count()
                                   
                               }).ToList();

                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null &&
                                d.Dept_Desc == Department &&
                                d.AllocateduserID == userID
                                && d.CompanyID == LoginUserCompanyID
                                select new
                                {
                                    d.Dept_Desc,
                                    d.Category_Desc,
                                    d.SubCategory_Desc,
                                    d.Lattitude,
                                    d.Longitude,
                                    d.Address,
                                    d.ComplaintStatus,
                                    d.Complaint_Date,
                                    d.automatic_complaint,

                                }).ToList();


                dic.Add("catCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getComplaintsSubCategorywiseUsersCount(string Category, string CatID, int userID)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {


                var rersult = (from d in gc.db.SCL_Sub_Category
                               where d.CATEGORY_ID == CatID
                               orderby d.SUB_CATEGORY_DESC
                               select new
                               {
                                   d.SUB_CATEGORY_DESC,
                                   d.SUB_CATEGORY_ID,
                                   assCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.AllocateduserID == userID && c.ComplaintStatus == assstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   holdCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.AllocateduserID == userID && c.ComplaintStatus == holdstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   inProgressCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.AllocateduserID == userID && c.ComplaintStatus == inprogressstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   compCount = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.AllocateduserID == userID && c.ComplaintStatus == compstatus && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count(),
                                   count = (from c in gc.db.SCL_Mobile_Complaints where c.SubCategory_Id == d.SUB_CATEGORY_ID && c.Category_Id == CatID && c.AllocateduserID == userID && c.CompanyID == LoginUserCompanyID select c.Dept_Desc).Count()
                                   
                               }).ToList();


                var rersult1 = (from d in gc.db.SCL_Mobile_Complaints
                                where d.Lattitude != null &&
                                d.Category_Id == CatID &&
                                d.AllocateduserID == userID
                                && d.CompanyID == LoginUserCompanyID
                                select new
                                {
                                    d.Dept_Desc,
                                    d.Category_Desc,
                                    d.SubCategory_Desc,
                                    d.Lattitude,
                                    d.Longitude,
                                    d.Address,
                                    d.ComplaintStatus,
                                    d.Complaint_Date,
                                    d.automatic_complaint,

                                }).ToList();

                dic.Add("scatCount", rersult);
                dic.Add("CompLocations", rersult1);
                dic.Add("success", true);


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }


        //Dashboard Tables

        public Dictionary<string, object> getAllDepartmentComplaints(int pageIndex, int pageSize, string sorting, string search, string SelectedDepartment, string SelectedCategory, string Department, string SColumn)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {
                
                bool isDate = IsDateTime(search);
                DateTime dt;
                if (isDate)
                {
                    dt = Convert.ToDateTime(search);
                }
                Pagenation<SCL_Mobile_Complaints> complaintList = new Pagenation<SCL_Mobile_Complaints>();
                List<SCL_Mobile_Complaints> listCmp = new List<SCL_Mobile_Complaints>();
                Func<SCL_Mobile_Complaints, object> orderBy = complaintList.orderByType1(sorting);
                if (SColumn == "Department")
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.Dept_Desc == Department
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else if (SColumn == "Category")
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.Category_Id == Department 
                                && cd.Dept_Desc == SelectedDepartment
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else if (SColumn == "SubCategory")
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.SubCategory_Id == Department 
                                && cd.Category_Id == SelectedCategory 
                                && cd.Dept_Desc == SelectedDepartment
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.Dept_Desc == Department
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listCmp = listCmp.Where(x => DateTime.Compare(x.CreatedDate.Value.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listCmp = listCmp.Where(x =>
                          (x.Complaint_ReferenceNo.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Category_Desc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.SubCategory_Desc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Dept_Desc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.ComplaintStatus.ToUpper() ?? "").Contains(search.ToUpper()) 
                           ).ToList();
                        }

                    }

                }
                complaintList.totalCount = listCmp.Count().ToString();
                listCmp = listCmp.OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
                complaintList.complaints = listCmp;

                if (listCmp.Count() > 0)
                {
                    dic.Add("success", complaintList);
                }
                else
                {
                    dic.Add("success", "No results found!");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getAllLocationDepartmentComplaints(int pageIndex, int pageSize, string sorting, string search, string SearchLocation, string SelectedDepartment, string SelectedCategory, string Department, string SColumn)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {

                bool isDate = IsDateTime(search);
                DateTime dt;
                if (isDate)
                {
                    dt = Convert.ToDateTime(search);
                }
                Pagenation<SCL_Mobile_Complaints> complaintList = new Pagenation<SCL_Mobile_Complaints>();
                List<SCL_Mobile_Complaints> listCmp = new List<SCL_Mobile_Complaints>();
                Func<SCL_Mobile_Complaints, object> orderBy = complaintList.orderByType1(sorting);
                if (SColumn == "Department")
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.Dept_Desc == Department
                                && cd.CompanyID == CompanyID
                                && cd.Address.ToLower().Contains(SearchLocation.ToLower())
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else if (SColumn == "Category")
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.Category_Id == Department 
                                && cd.Dept_Desc == SelectedDepartment 
                                && cd.Address.ToLower().Contains(SearchLocation.ToLower())
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else if (SColumn == "SubCategory")
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.SubCategory_Id == Department 
                                && cd.Category_Id == SelectedCategory 
                                && cd.Dept_Desc == SelectedDepartment
                                && cd.CompanyID == CompanyID 
                                && cd.Address.ToLower().Contains(SearchLocation.ToLower())
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.Dept_Desc == Department
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listCmp = listCmp.Where(x => DateTime.Compare(x.CreatedDate.Value.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listCmp = listCmp.Where(x =>
                          (x.Complaint_ReferenceNo.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Category_Desc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.SubCategory_Desc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Dept_Desc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.ComplaintStatus.ToUpper() ?? "").Contains(search.ToUpper())
                           ).ToList();
                        }

                    }

                }
                complaintList.totalCount = listCmp.Count().ToString();
                listCmp = listCmp.OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
                complaintList.complaints = listCmp;

                if (listCmp.Count() > 0)
                {
                    dic.Add("success", complaintList);
                }
                else
                {
                    dic.Add("success", "No results found!");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }

        public Dictionary<string, object> getAllUserDepartmentComplaints(int pageIndex, int pageSize, string sorting, string search, int selectedUserID, string SelectedDepartment, string SelectedCategory, string Department, string SColumn)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {

                bool isDate = IsDateTime(search);
                DateTime dt;
                if (isDate)
                {
                    dt = Convert.ToDateTime(search);
                }
                Pagenation<SCL_Mobile_Complaints> complaintList = new Pagenation<SCL_Mobile_Complaints>();
                List<SCL_Mobile_Complaints> listCmp = new List<SCL_Mobile_Complaints>();
                Func<SCL_Mobile_Complaints, object> orderBy = complaintList.orderByType1(sorting);
                
                if (SColumn == "User")
                {
                    var usID = Convert.ToInt32(Department);
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.AllocateduserID == usID
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else if (SColumn == "Department")
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.Dept_Desc == Department
                                && cd.AllocateduserID == selectedUserID
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else if (SColumn == "Category")
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.Category_Id == Department
                                && cd.Dept_Desc == SelectedDepartment
                                && cd.AllocateduserID == selectedUserID
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else if (SColumn == "SubCategory")
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.SubCategory_Id == Department
                                && cd.Category_Id == SelectedCategory
                                && cd.Dept_Desc == SelectedDepartment
                                && cd.AllocateduserID == selectedUserID
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }
                else
                {
                    listCmp = ((from cd in gc.db.SCL_Mobile_Complaints
                                where cd.AllocateduserID == selectedUserID
                                && cd.CompanyID == CompanyID
                                select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_Mobile_Complaints
                            {
                                ID = d.cd.ID,
                                Complaint_ReferenceNo = d.cd.Complaint_ReferenceNo,
                                ComplaintStatus = d.cd.ComplaintStatus,
                                Category_Desc = d.cd.Category_Desc,
                                SubCategory_Desc = d.cd.SubCategory_Desc,
                                Dept_Desc = d.cd.Dept_Desc,
                                Complaint_Date = d.cd.Complaint_Date,
                                Address = d.cd.Address,
                                automatic_complaint = d.cd.automatic_complaint
                            }).AsQueryable().ToList();

                }



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listCmp = listCmp.Where(x => DateTime.Compare(x.CreatedDate.Value.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listCmp = listCmp.Where(x =>
                          (x.Complaint_ReferenceNo.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Category_Desc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.SubCategory_Desc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Dept_Desc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.ComplaintStatus.ToUpper() ?? "").Contains(search.ToUpper())
                          ).ToList();
                        }

                    }

                }
                complaintList.totalCount = listCmp.Count().ToString();
                listCmp = listCmp.OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
                complaintList.complaints = listCmp;

                if (listCmp.Count() > 0)
                {
                    dic.Add("success", complaintList);
                }
                else
                {
                    dic.Add("success", "No results found!");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                dic.Add("error", raise.Message);
            }
            return dic;
        }


        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }



    }
}