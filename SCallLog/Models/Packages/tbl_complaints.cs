using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class tbl_complaints
    {
        GlobalClass gc = new GlobalClass();

        public Dictionary<string, object> getUsers()
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //int R = 1;
            //int S = 2;
            //int Q = 3;
            try
            {
                var Users = gc.db.SCL_Users.Where(x => x.CompanyID == LoginUserCompanyID && x.userType != "Admin").AsQueryable().ToList();
                dic.Add("success", true);
                dic.Add("Users", Users);
                
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

        public Dictionary<string, object> getUserStatus()
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //int R = 1;
            //int S = 2;
            //int Q = 3;
            try
            {
                var status = gc.db.SCL_Comp_Status.Where(x => x.ISACTIVE == 1 && x.Flag != "no").AsQueryable().ToList();
                dic.Add("success", true);
                dic.Add("status", status);

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


        public Dictionary<string, object> getComplaintHistory(string refNumber)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //int R = 1;
            //int S = 2;
            //int Q = 3;
            try
            {
                var complaintsHistory = gc.db.sp_getComplaintHistoryByRefNumber(refNumber).ToList();
                dic.Add("success", true);
                dic.Add("ComplaintHistory", complaintsHistory);

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


        public Dictionary<string, object> getAllComplaints(int pageIndex, int pageSize, string sorting, string search)
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
                Func<SCL_Mobile_Complaints, object> orderBy = complaintList.orderByType1("ID");
                listCmp = ((from cd in gc.db.SCL_Mobile_Complaints.Where(x=> x.CompanyID == CompanyID)
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .AsEnumerable()
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
                                automatic_complaint = d.cd.automatic_complaint,
                                Comments = d.cd.Comments,
                                JC_REF = d.cd.JC_REF,
                                img_data = gc.db.SCL_ComplaintImages.Where(x => x.Complaint_ReferenceNo == d.cd.Complaint_ReferenceNo).FirstOrDefault()!=null? gc.db.SCL_ComplaintImages.Where(x => x.Complaint_ReferenceNo == d.cd.Complaint_ReferenceNo).FirstOrDefault().AttachmentName : ""
                            }).AsQueryable().ToList();
                

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

        public Dictionary<string, object> getReceivedComplaints(int pageIndex, int pageSize, string sorting, string search)
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
                Func<SCL_Mobile_Complaints, object> orderBy = complaintList.orderByType1("ID");
                listCmp = ((from cd in gc.db.SCL_Mobile_Complaints.Where(x => x.CompanyID == CompanyID && (x.ComplaintStatus=="Received" || x.ComplaintStatus == "Reallocate" || x.ComplaintStatus == "Redirect"))
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .AsEnumerable()
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
                                automatic_complaint = d.cd.automatic_complaint,
                                Comments = d.cd.Comments,
                                JC_REF = d.cd.JC_REF,
                                img_data = gc.db.SCL_ComplaintImages.Where(x => x.Complaint_ReferenceNo == d.cd.Complaint_ReferenceNo).FirstOrDefault() != null ? gc.db.SCL_ComplaintImages.Where(x => x.Complaint_ReferenceNo == d.cd.Complaint_ReferenceNo).FirstOrDefault().AttachmentName : ""
                            }).AsQueryable().ToList();


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
        public Dictionary<string, object> getComplaintsCount(int userID)
        {
            int LoginUserID = userID;
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {

                var TotalComplaints = gc.db.SCL_JobCards.Where(x => x.USER_ID == LoginUserID && x.COMPANY_ID == LoginUserCompanyID && x.COMP_STATUS != "Completed").Count();
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

        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }

        public Dictionary<string, object> AssignedComplaintUser(int ID, int UserID, string Comments)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());
            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    var Status = "Assigned";
                    var auto_comp="";
                    SCL_Mobile_Complaints comp = gc.db.SCL_Mobile_Complaints.Where(x => x.ID == ID && x.CompanyID == CompanyID).FirstOrDefault();
                    SCL_Mobile_Complaints_Audit _Audit = new SCL_Mobile_Complaints_Audit();
                    
                    if(comp.automatic_complaint == null)
                    {
                        auto_comp = "";
                    }
                    else
                    {
                        auto_comp = comp.automatic_complaint;
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
                    gc.db.SCL_Mobile_Complaints_Audit.Add(_Audit);
                    int xx = gc.db.SaveChanges();
                    if (xx == 1)
                    {
                        comp.ComplaintStatus = Status;
                        comp.ModifiedBy = CompanyID + "";
                        comp.ModifiedDate = DateTime.Now;
                        comp.AllocateduserID = UserID;
                        comp.Comments = Comments;
                        comp.JC_REF = "JC_" + comp.Category_Id + "" + ID;
                        int n = gc.db.SaveChanges();

                        if (n > 0)
                        {
                            SCL_ComplaintsAllocated comp_Allocated = new SCL_ComplaintsAllocated();
                            comp_Allocated.CompID = ID;
                            comp_Allocated.CompanyID = CompanyID;
                            comp_Allocated.UserID = UserID;
                            comp_Allocated.Status = "Assigned";
                            comp_Allocated.CreatedBy = CompanyID;
                            comp_Allocated.CreatedDateTime = DateTime.Now;
                            comp_Allocated.Comments = Comments;
                            gc.db.SCL_ComplaintsAllocated.Add(comp_Allocated);
                            int c = gc.db.SaveChanges();
                            if (c > 0)
                            {
                                if (_Audit.ComplaintStatus.Equals("Reallocate") || _Audit.ComplaintStatus.Equals("Redirect"))
                                {
                                    if(gc.db.SCL_JobCards.Any(x => x.COMPLAINT_ID == ID)){
                                        SCL_JobCards jobs = gc.db.SCL_JobCards.Where(x => x.COMPLAINT_ID == ID).FirstOrDefault();

                                        jobs.USER_ID = UserID;
                                        jobs.COMP_STATUS = comp.ComplaintStatus;
                                        jobs.COMP_COMMENTS = Comments;
                                        jobs.MODIFIED_BY = CompanyID;
                                        jobs.MODIFIED_DATE = DateTime.Now;
                                        gc.db.SaveChanges();
                                        tran.Commit();
                                        dic.Add("JCREF", jobs.JC_REFERENCE);
                                        dic.Add("success", "success");
                                    }
                                    else
                                    {
                                        SCL_JobCards _JOB = new SCL_JobCards();
                                        _JOB.JC_REFERENCE = "JC_" + comp.Category_Id + "" + ID;
                                        _JOB.JC_DATE = DateTime.Now;
                                        _JOB.COMP_REFERENCE = comp.Complaint_ReferenceNo;
                                        _JOB.COMP_DATE = comp.CreatedDate;
                                        _JOB.COMPANY_ID = CompanyID;
                                        _JOB.COMPLAINT_ID = ID;
                                        _JOB.USER_ID = UserID;
                                        _JOB.DEPARTMENT = comp.Dept_Desc;
                                        _JOB.CATEGORY = comp.Category_Desc;
                                        _JOB.SUB_CATEGORY = comp.SubCategory_Desc;
                                        _JOB.ADDRESS = comp.Address;
                                        _JOB.COMP_STATUS = comp.ComplaintStatus;
                                        _JOB.COMP_COMMENTS = Comments;
                                        _JOB.CREATED_BY = CompanyID;
                                        _JOB.CREATED_DATE = DateTime.Now;
                                        gc.db.SCL_JobCards.Add(_JOB);
                                        gc.db.SaveChanges();
                                        tran.Commit();
                                        dic.Add("JCREF", _JOB.JC_REFERENCE);
                                        dic.Add("success", "success");
                                    }
                                    
                                }
                                else
                                {
                                    SCL_JobCards _JOB = new SCL_JobCards();
                                    _JOB.JC_REFERENCE = "JC_" + comp.Category_Id + "" + ID;
                                    _JOB.JC_DATE = DateTime.Now;
                                    _JOB.COMP_REFERENCE = comp.Complaint_ReferenceNo;
                                    _JOB.COMP_DATE = comp.CreatedDate;
                                    _JOB.COMPANY_ID = CompanyID;
                                    _JOB.COMPLAINT_ID = ID;
                                    _JOB.USER_ID = UserID;
                                    _JOB.DEPARTMENT = comp.Dept_Desc;
                                    _JOB.CATEGORY = comp.Category_Desc;
                                    _JOB.SUB_CATEGORY = comp.SubCategory_Desc;
                                    _JOB.ADDRESS = comp.Address;
                                    _JOB.COMP_STATUS = comp.ComplaintStatus;
                                    _JOB.COMP_COMMENTS = Comments;
                                    _JOB.CREATED_BY = CompanyID;
                                    _JOB.CREATED_DATE = DateTime.Now;
                                    gc.db.SCL_JobCards.Add(_JOB);
                                    gc.db.SaveChanges();
                                    tran.Commit();
                                    dic.Add("JCREF", _JOB.JC_REFERENCE);
                                    dic.Add("success", "success");
                                }
                                
                            }else
                            {
                                tran.Rollback();
                                dic.Add("error", "Failed!");
                            }

                        }else
                        {
                            tran.Rollback();
                            dic.Add("error", "Failed!");
                        }
                    }else
                    {
                        tran.Rollback();
                        dic.Add("error", "No results found!");
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

    }
}