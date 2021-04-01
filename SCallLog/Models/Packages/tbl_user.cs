using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class tbl_user
    {
        GlobalClass gc = new GlobalClass();

        public Dictionary<string, object> getComplaintsCount()
        {
            int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["am_userid"].ToString());
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
           
            try
            {
                
                var TotalComplaints = gc.db.SCL_JobCards.Where(x=> x.COMPANY_ID == LoginUserCompanyID).Count();
                var AllocatedComplaints = gc.db.SCL_JobCards.Where(x => x.USER_ID == LoginUserID && x.COMP_STATUS == "Assigned" && x.COMPANY_ID == LoginUserCompanyID).Count();
                var CompletedComplaints = gc.db.SCL_JobCards.Where(x => x.USER_ID == LoginUserID && x.COMP_STATUS == "Completed" && x.COMPANY_ID == LoginUserCompanyID).Count();
                var HoldComplaints = gc.db.SCL_JobCards.Where(x => x.USER_ID == LoginUserID && x.COMP_STATUS == "Hold" && x.COMPANY_ID == LoginUserCompanyID).Count();
                var ProgressComplaints = gc.db.SCL_JobCards.Where(x => x.USER_ID == LoginUserID && x.COMP_STATUS == "InProgress" && x.COMPANY_ID == LoginUserCompanyID).Count();
                
                dic.Add("success", true);
                dic.Add("TotalComplaints", TotalComplaints);
                dic.Add("AllocatedComplaints", AllocatedComplaints);
                dic.Add("CompletedComplaints", CompletedComplaints);
                dic.Add("HoldComplaints", HoldComplaints);
                dic.Add("ProgressComplaints", ProgressComplaints);
                

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

        public Dictionary<string, object> getWorkOrders(int pageIndex, int pageSize, string sorting, string search)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["am_userid"].ToString());
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {
                bool isDate = IsDateTime(search);
                DateTime dt;
                if (isDate)
                {
                    dt = Convert.ToDateTime(search);
                }
                Pagenation<SCL_JobCards> complaintList = new Pagenation<SCL_JobCards>();
                List<SCL_JobCards> listCmp = new List<SCL_JobCards>();
                Func<SCL_JobCards, object> orderBy = complaintList.orderByType2(sorting);
                listCmp = ((from cd in gc.db.SCL_JobCards
                            where cd.USER_ID == LoginUserID
                            && cd.COMPANY_ID == CompanyID
                            && cd.COMP_STATUS != "Completed"
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_JobCards
                            {
                                COMPLAINT_ID = d.cd.COMPLAINT_ID,
                                JC_REFERENCE = d.cd.JC_REFERENCE,
                                COMP_STATUS = d.cd.COMP_STATUS,
                                CATEGORY = d.cd.CATEGORY,
                                SUB_CATEGORY = d.cd.SUB_CATEGORY,
                                DEPARTMENT = d.cd.DEPARTMENT,
                                JC_DATE = d.cd.JC_DATE,
                                ADDRESS = d.cd.ADDRESS,
                                COMP_COMMENTS = d.cd.COMP_COMMENTS,
                            }).AsQueryable().ToList();



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listCmp = listCmp.Where(x => DateTime.Compare(x.JC_DATE.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listCmp = listCmp.Where(x =>
                          (x.JC_REFERENCE.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.CATEGORY.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.SUB_CATEGORY.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.DEPARTMENT.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.COMP_STATUS.ToUpper() ?? "").Contains(search.ToUpper()) 
                           ).ToList();
                        }

                    }

                }
                complaintList.totalCount = listCmp.Count().ToString();
                listCmp = listCmp.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
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

        public Dictionary<string, object> getMapAssignedComplaints()
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["am_userid"].ToString());
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {
                
                Pagenation<Map_Jobcards> complaintList = new Pagenation<Map_Jobcards>();
                List<Map_Jobcards> listCmp = new List<Map_Jobcards>();
                Func<SCL_JobCards, object> orderBy = complaintList.orderByType2("JC_DATE");
                listCmp = ((from cd in gc.db.SCL_JobCards
                            where cd.USER_ID == LoginUserID
                            && cd.COMPANY_ID == CompanyID
                            && cd.COMP_STATUS != "Completed"
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new Map_Jobcards
                            {
                                COMPLAINT_ID = d.cd.COMPLAINT_ID,
                                JC_REFERENCE = d.cd.JC_REFERENCE,
                                COMP_STATUS = d.cd.COMP_STATUS,
                                CATEGORY = d.cd.CATEGORY,
                                SUB_CATEGORY = d.cd.SUB_CATEGORY,
                                DEPARTMENT = d.cd.DEPARTMENT,
                                JC_DATE = d.cd.JC_DATE,
                                ADDRESS = d.cd.ADDRESS,
                                COMP_COMMENTS = d.cd.COMP_COMMENTS,
                                LAT = gc.db.SCL_Mobile_Complaints.FirstOrDefault(x=>x.ID == d.cd.COMPLAINT_ID).Lattitude,
                                LONG = gc.db.SCL_Mobile_Complaints.FirstOrDefault(x => x.ID == d.cd.COMPLAINT_ID).Longitude,

                            }).AsQueryable().ToList();



                complaintList.totalCount = listCmp.Count().ToString();
                complaintList.complaints = listCmp;

                if (listCmp.Count() > 0)
                {
                    dic.Add("success", complaintList);
                }
                else
                {
                    dic.Add("error", "No results found!");
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

        public Dictionary<string, object> getJobcardLocationDetails(int complaintId)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();            
            try
            {
                var result = (from d in gc.db.SCL_Mobile_Complaints
                                where d.ID == complaintId
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
                                    d.automatic_complaint
                                }).ToList();
                
                if (result.Count() > 0)
                {
                    dic.Add("success", true);
                    dic.Add("CompLocations", result);
                }
                else
                {
                    dic.Add("error", "No results found!");
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


        public Dictionary<string, object> getJobCardHistory(string refNumber)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //int R = 1;
            //int S = 2;
            //int Q = 3;
            try
            {
                var jobCardHistory = gc.db.sp_getJobCardHistoryByRefNumber(refNumber).ToList();
                dic.Add("success", true);
                dic.Add("jobCardHistory", jobCardHistory);

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


        public Dictionary<string, object> getUserJobCards(int pageIndex, int pageSize, string sorting, string search)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["am_userid"].ToString());
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {
                bool isDate = IsDateTime(search);
                DateTime dt;
                if (isDate)
                {
                    dt = Convert.ToDateTime(search);
                }
                Pagenation<SCL_JobCards> complaintList = new Pagenation<SCL_JobCards>();
                List<SCL_JobCards> listCmp = new List<SCL_JobCards>();
                Func<SCL_JobCards, object> orderBy = complaintList.orderByType2(sorting);
                listCmp = ((from cd in gc.db.SCL_JobCards
                            where cd.USER_ID == LoginUserID
                            && cd.COMPANY_ID == CompanyID
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_JobCards
                            {
                                COMPLAINT_ID = d.cd.COMPLAINT_ID,
                                COMP_REFERENCE = d.cd.COMP_REFERENCE,
                                JC_REFERENCE = d.cd.JC_REFERENCE,
                                COMP_STATUS = d.cd.COMP_STATUS,
                                CATEGORY = d.cd.CATEGORY,
                                SUB_CATEGORY = d.cd.SUB_CATEGORY,
                                DEPARTMENT = d.cd.DEPARTMENT,
                                JC_DATE = d.cd.JC_DATE,
                                ADDRESS = d.cd.ADDRESS,
                                COMP_COMMENTS = d.cd.COMP_COMMENTS,
                            }).AsQueryable().ToList();



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listCmp = listCmp.Where(x => DateTime.Compare(x.JC_DATE.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listCmp = listCmp.Where(x =>
                          (x.JC_REFERENCE.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.CATEGORY.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.SUB_CATEGORY.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.DEPARTMENT.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.COMP_STATUS.ToUpper() ?? "").Contains(search.ToUpper())
                           ).ToList();
                        }

                    }

                }
                complaintList.totalCount = listCmp.Count().ToString();
                listCmp = listCmp.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
                complaintList.complaints = listCmp;

                if (listCmp.Count() > 0)
                {
                    dic.Add("success", complaintList);
                }
                else
                {
                    dic.Add("error", "No results found!");
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

        public Dictionary<string, object> getCompanyJobCards(int pageIndex, int pageSize, string sorting, string search)
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
                Pagenation<SCL_JobCards> complaintList = new Pagenation<SCL_JobCards>();
                List<SCL_JobCards> listCmp = new List<SCL_JobCards>();
                Func<SCL_JobCards, object> orderBy = complaintList.orderByType2(sorting);
                listCmp = ((from cd in gc.db.SCL_JobCards
                            where cd.COMPANY_ID == CompanyID
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL_JobCards
                            {
                                COMPLAINT_ID = d.cd.COMPLAINT_ID,
                                COMP_REFERENCE = d.cd.COMP_REFERENCE,
                                JC_REFERENCE = d.cd.JC_REFERENCE,
                                COMP_STATUS = d.cd.COMP_STATUS,
                                CATEGORY = d.cd.CATEGORY,
                                SUB_CATEGORY = d.cd.SUB_CATEGORY,
                                DEPARTMENT = d.cd.DEPARTMENT,
                                JC_DATE = d.cd.JC_DATE,
                                ADDRESS = d.cd.ADDRESS,
                                COMP_COMMENTS = d.cd.COMP_COMMENTS
                            }).AsQueryable().ToList();



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listCmp = listCmp.Where(x => DateTime.Compare(x.JC_DATE.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listCmp = listCmp.Where(x =>
                          (x.JC_REFERENCE.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.CATEGORY.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.SUB_CATEGORY.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.DEPARTMENT.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.COMP_STATUS.ToUpper() ?? "").Contains(search.ToUpper())
                           ).ToList();
                        }

                    }

                }
                complaintList.totalCount = listCmp.Count().ToString();
                listCmp = listCmp.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
                complaintList.complaints = listCmp;

                if (listCmp.Count() > 0)
                {
                    dic.Add("success", complaintList);
                }
                else
                {
                    dic.Add("error", "No results found!");
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

        public Dictionary<string, object> updateComplaintStatus(int ID, string Status, string pStatus, string sdate, string edate, string shours, string Comments)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());
            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    var _comp = "";
                    
                    var auto_comp = "";
                    SCL_Mobile_Complaints comp = gc.db.SCL_Mobile_Complaints.Where(x => x.ID == ID).FirstOrDefault();
                    SCL_Mobile_Complaints_Audit _Audit = new SCL_Mobile_Complaints_Audit();
                    if (comp.automatic_complaint == null)
                    {
                        auto_comp = "";
                    }
                    else
                    {
                        auto_comp = comp.automatic_complaint;
                    }
                    if (comp.Comments == null)
                    {
                        _comp = "";
                    }
                    else
                    {
                        _comp = comp.Comments;
                    }
                    _Audit.Complaint_ID = comp.ID;
                    _Audit.Complaint_ReferenceNo = comp.Complaint_ReferenceNo;
                    _Audit.Complaint_Date = comp.Complaint_Date.ToString();
                    _Audit.Category_Id = comp.Category_Id;
                    _Audit.Category_Desc = comp.Category_Desc;
                    _Audit.SubCategory_Desc = comp.SubCategory_Desc;
                    _Audit.SubCategory_Id = comp.SubCategory_Id;
                    _Audit.Dept_Desc = comp.Dept_Desc;
                    _Audit.CreatedDate = comp.CreatedDate;
                    _Audit.ModifiedBy = CompanyID.ToString();
                    _Audit.ModifiedDate = DateTime.Now;
                    _Audit.ComplaintStatus = comp.ComplaintStatus;
                    _Audit.Lattitude = comp.Lattitude;
                    _Audit.Longitude = comp.Longitude;
                    _Audit.automatic_complaint = auto_comp;
                    _Audit.userID = comp.userID;
                    _Audit.Comments = _comp;
                    gc.db.SCL_Mobile_Complaints_Audit.Add(_Audit);
                    int xx = gc.db.SaveChanges();
                    if (xx == 1)
                    {
                        comp.ComplaintStatus = Status;
                        comp.ModifiedBy = CompanyID + "";
                        comp.ModifiedDate = DateTime.Now;
                        comp.Comments = Comments;
                        int n = gc.db.SaveChanges();

                        if (n > 0)
                        {
                            SCL_ComplaintsAllocated comp_Allocated = new SCL_ComplaintsAllocated();
                            comp_Allocated.CompID = ID;
                            comp_Allocated.CompanyID = CompanyID;
                            comp_Allocated.UserID = comp.AllocateduserID;
                            comp_Allocated.Status = Status;
                            comp_Allocated.CreatedBy = CompanyID;
                            comp_Allocated.CreatedDateTime = DateTime.Now;
                            comp_Allocated.Comments = Comments;
                            gc.db.SCL_ComplaintsAllocated.Add(comp_Allocated);
                            int c= gc.db.SaveChanges();
                            if (c > 0)
                            {
                                SCL_JobCards jobs = gc.db.SCL_JobCards.Where(x => x.COMPLAINT_ID == ID).FirstOrDefault();
                                jobs.COMP_STATUS = Status;
                                jobs.COMP_COMMENTS = Comments;
                                jobs.MODIFIED_BY = CompanyID;
                                jobs.MODIFIED_DATE = DateTime.Now;
                                if (pStatus.Equals("Assigned"))
                                {
                                    jobs.START_DATE = sdate;
                                    jobs.END_DATE = edate;
                                    jobs.SERVICE_HOURS = shours;
                                }
                                
                                int j = gc.db.SaveChanges();
                                if (j > 0)
                                {
                                    tran.Commit();
                                    dic.Add("success", "success");
                                }
                                else
                                {
                                    tran.Rollback();
                                    dic.Add("error", "Update Fail");
                                }
                                
                            }
                            else
                            {
                                tran.Rollback();
                                dic.Add("error", "Update Fail");
                            }
                            

                        }
                        else
                        {
                            tran.Rollback();
                            dic.Add("error", "Update Fail");
                        }
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    tran.Rollback();
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
            }
            return dic;
        }

        public Dictionary<string, object> updateJobcardStatus(JobcardUpdatedData jobData)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());
            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    var _comp = "";
                    var auto_comp = "";
                    SCL_Mobile_Complaints comp = gc.db.SCL_Mobile_Complaints.Where(x => x.ID == jobData.ID).FirstOrDefault();
                    SCL_Mobile_Complaints_Audit _Audit = new SCL_Mobile_Complaints_Audit();
                    if(comp!=null)
                    {
                        auto_comp = comp.automatic_complaint;
                        _comp = comp.Comments;
                    }
                    
                    _Audit.Complaint_ID = comp.ID;
                    _Audit.Complaint_ReferenceNo = comp.Complaint_ReferenceNo;
                    _Audit.Complaint_Date = comp.Complaint_Date.ToString();
                    _Audit.Category_Id = comp.Category_Id;
                    _Audit.Category_Desc = comp.Category_Desc;
                    _Audit.SubCategory_Desc = comp.SubCategory_Desc;
                    _Audit.SubCategory_Id = comp.SubCategory_Id;
                    _Audit.Dept_Desc = comp.Dept_Desc;
                    _Audit.CreatedDate = comp.CreatedDate;
                    _Audit.ModifiedBy = CompanyID.ToString();
                    _Audit.ModifiedDate = DateTime.Now;
                    _Audit.ComplaintStatus = comp.ComplaintStatus;
                    _Audit.Lattitude = comp.Lattitude;
                    _Audit.Longitude = comp.Longitude;
                    _Audit.automatic_complaint = auto_comp;
                    _Audit.userID = comp.userID;
                    _Audit.Comments = _comp;
                    gc.db.SCL_Mobile_Complaints_Audit.Add(_Audit);
                    int xx = gc.db.SaveChanges();
                    if (xx == 1)
                    {
                        comp.ComplaintStatus = jobData.compStatus;
                        comp.ModifiedBy = CompanyID + "";
                        comp.ModifiedDate = DateTime.Now;
                        comp.Comments = jobData.Comments;
                        int n = gc.db.SaveChanges();

                        if (n > 0)
                        {
                            SCL_ComplaintsAllocated comp_Allocated = new SCL_ComplaintsAllocated();
                            comp_Allocated.CompID = jobData.ID;
                            comp_Allocated.CompanyID = CompanyID;
                            comp_Allocated.UserID = comp.AllocateduserID;
                            comp_Allocated.Status = jobData.compStatus;
                            comp_Allocated.CreatedBy = CompanyID;
                            comp_Allocated.CreatedDateTime = DateTime.Now;
                            comp_Allocated.Comments = jobData.Comments;
                            gc.db.SCL_ComplaintsAllocated.Add(comp_Allocated);
                            int c = gc.db.SaveChanges();
                            if (c > 0)
                            {
                                SCL_JobCards jobs = gc.db.SCL_JobCards.Where(x => x.COMPLAINT_ID == jobData.ID).FirstOrDefault();
                                jobs.COMP_STATUS = jobData.compStatus;
                                jobs.COMP_COMMENTS = jobData.Comments;
                                jobs.MODIFIED_BY = CompanyID;
                                jobs.MODIFIED_DATE = DateTime.Now;
                                if (jobData.PStatus.Equals("Assigned") || jobData.PStatus.Equals("Allocated"))
                                {
                                    jobs.START_DATE = jobData.StartDate;
                                    jobs.END_DATE = jobData.EndDate;
                                    jobs.VEHICLE_NO = jobData.VehicalNo;
                                    jobs.JOB_DISTANCE = jobData.JobDistance;
                                    jobs.SERVICE_HOURS = jobData.ServiceHours;
                                }

                                int j = gc.db.SaveChanges();
                                if (j > 0)
                                {
                                    tran.Commit();
                                    dic.Add("success", "success");
                                }
                                else
                                {
                                    tran.Rollback();
                                    dic.Add("error", "Update Failed!");
                                }

                            }
                            else
                            {
                                tran.Rollback();
                                dic.Add("error", "Update Failed!");
                            }


                        }
                        else
                        {
                            tran.Rollback();
                            dic.Add("error", "Update Failed!");
                        }
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    tran.Rollback();
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
            }
            return dic;
        }


        public Dictionary<string, object> addWorkOrder(List<WorkOrder> workOrders)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());
            int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["am_userid"].ToString());
            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    DateTime dt = DateTime.Now;
                int count = gc.db.SCL_WorkOrder_Master.Count();
                string wo_ref = string.Format("WO{0}{1:D5}", dt.ToString("yyyyMMdd"), count+1);
                SCL_WorkOrder_Master _Master = new SCL_WorkOrder_Master();
                _Master.WO_REFERENCE = wo_ref;
                _Master.WO_DATE = dt;
                _Master.USER_ID = LoginUserID;
                _Master.COMPANY_ID = CompanyID;
                _Master.CREATED_BY = LoginUserID;
                _Master.CREATED_DATE = dt;
                gc.db.SCL_WorkOrder_Master.Add(_Master);
                int wm = gc.db.SaveChanges();

                if (wm >0)
                {
                    foreach(WorkOrder item in workOrders)
                    {
                        SCL_WorkOrder_Details _Details = new SCL_WorkOrder_Details();
                        _Details.WO_REFERENCE = wo_ref;
                            _Details.COMPANY_ID = CompanyID;
                            _Details.USER_ID = LoginUserID;
                        _Details.COMPLAINT_ID = item.COMPLAINT_ID;
                        _Details.JC_REFERENCE = item.JC_REFERENCE;
                        _Details.WODID = _Master.WOID;
                        _Details.WO_DATE = dt;
                        _Details.CREATED_DATE = dt;
                        gc.db.SCL_WorkOrder_Details.Add(_Details);
                            if (gc.db.SaveChanges() > 0)
                            {                            

                            var _comp = "";
                            var auto_comp = "";
                            SCL_Mobile_Complaints comp = gc.db.SCL_Mobile_Complaints.Where(x => x.ID == item.COMPLAINT_ID).FirstOrDefault();
                            SCL_Mobile_Complaints_Audit _Audit = new SCL_Mobile_Complaints_Audit();
                            if (comp.automatic_complaint == null)
                            {
                                auto_comp = "";
                            }
                            else
                            {
                                auto_comp = comp.automatic_complaint;
                            }
                            if (comp.Comments == null)
                            {
                                _comp = "";
                            }
                            else
                            {
                                _comp = comp.Comments;
                            }
                            _Audit.Complaint_ID = comp.ID;
                            _Audit.Complaint_ReferenceNo = comp.Complaint_ReferenceNo;
                            _Audit.Complaint_Date = comp.Complaint_Date.ToString();
                            _Audit.Category_Id = comp.Category_Id;
                            _Audit.Category_Desc = comp.Category_Desc;
                            _Audit.SubCategory_Desc = comp.SubCategory_Desc;
                            _Audit.SubCategory_Id = comp.SubCategory_Id;
                            _Audit.Dept_Desc = comp.Dept_Desc;
                            _Audit.CreatedDate = comp.CreatedDate;
                            _Audit.ModifiedBy = CompanyID.ToString();
                            _Audit.ModifiedDate = DateTime.Now;
                            _Audit.ComplaintStatus = comp.ComplaintStatus;
                            _Audit.Lattitude = comp.Lattitude;
                            _Audit.Longitude = comp.Longitude;
                            _Audit.automatic_complaint = auto_comp;
                            _Audit.userID = comp.userID;
                            _Audit.Comments = _comp;
                            gc.db.SCL_Mobile_Complaints_Audit.Add(_Audit);
                            int xx = gc.db.SaveChanges();
                            if (xx == 1)
                            {
                                comp.ComplaintStatus = "Allocated";
                                comp.ModifiedBy = CompanyID + "";
                                comp.ModifiedDate = DateTime.Now;
                                int n = gc.db.SaveChanges();

                                if (n > 0)
                                {
                                    SCL_ComplaintsAllocated comp_Allocated = new SCL_ComplaintsAllocated();
                                    comp_Allocated.CompID = item.COMPLAINT_ID;
                                    comp_Allocated.CompanyID = CompanyID;
                                    comp_Allocated.UserID = comp.AllocateduserID;
                                    comp_Allocated.Status = "Allocated";
                                    comp_Allocated.CreatedBy = CompanyID;
                                    comp_Allocated.CreatedDateTime = DateTime.Now;
                                    gc.db.SCL_ComplaintsAllocated.Add(comp_Allocated);
                                    int c = gc.db.SaveChanges();
                                    if (c > 0)
                                    {
                                        SCL_JobCards jobs = gc.db.SCL_JobCards.Where(x => x.COMPLAINT_ID == item.COMPLAINT_ID).FirstOrDefault();
                                        jobs.COMP_STATUS = "Allocated";
                                        jobs.MODIFIED_BY = CompanyID;
                                        jobs.MODIFIED_DATE = DateTime.Now;
                                        int j = gc.db.SaveChanges();
                                        if (j > 0)
                                        {
                                            
                                        }
                                        else
                                        {
                                            tran.Rollback();
                                            dic.Add("error", "Update Fail");
                                        }

                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        dic.Add("error", "Update Fail");
                                    }


                                }
                                else
                                {
                                    tran.Rollback();
                                    dic.Add("error", "Update Fail");
                                }
                            }
                        }

                        }

                    }


                    tran.Commit();
                    dic.Add("success", "success");

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                tran.Rollback();
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
        }
            return dic;
        }

    }
}