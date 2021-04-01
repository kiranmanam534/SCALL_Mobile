using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class tbl_master
    {
        GlobalClass gc = new GlobalClass();

        public Dictionary<string, object> getAllDepartments(int pageIndex, int pageSize, string sorting, string search)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {
                bool isDate = IsDateTime(search);
                DateTime dt;
                if (isDate)
                {
                    dt = Convert.ToDateTime(search);
                }
                Pagenation<SCL> employeeList = new Pagenation<SCL>();
                List<SCL> listEmp = new List<SCL>();
                Func<SCL, object> orderBy = employeeList.orderByType(sorting);
                listEmp = ((from cd in gc.db.SCL_Department
                            //where cd.Status == "Active"
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL
                            {
                                CID = d.cd.ID,
                                DeptName = d.cd.DEPARTMENT_DESC,
                                //DeptDate = d.cd.CreatedDateTime,
                                Status = d.cd.Status,

                            }).AsQueryable().ToList();



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listEmp = listEmp.Where(x => DateTime.Compare(x.DeptDate.Value.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listEmp = listEmp.Where(x =>
                          (x.DeptName.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Status.ToUpper() ?? "").Contains(search.ToUpper())
                           ).ToList();
                        }

                    }

                }
                employeeList.totalCount = listEmp.Count().ToString();
                listEmp = listEmp.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
                employeeList.complaints = listEmp;

                if (listEmp.Count() > 0)
                {
                    dic.Add("success", employeeList);
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
        public Dictionary<string, object> addDepartment(string dept, string status)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {
                string deptartmentId = dept.Length > 2 ? dept.Substring(0,2):"ADMIN";
                var deptInfo = gc.db.SCL_Department.Where(d => d.DEPARTMENT_DESC == dept).ToList();
                if (deptInfo.Count() == 0)
                {
                    SCL_Department dep = new SCL_Department();
                    var isdeptIdExist = gc.db.SCL_Department.Where(d => d.DEPARTMENT_ID == deptartmentId).ToList();
                    if(isdeptIdExist.Count()==0)
                    {
                        dep.DEPARTMENT_ID = deptartmentId;
                    }
                    else
                    {
                        dep.DEPARTMENT_ID = deptartmentId + isdeptIdExist.Count();
                    }                    
                    
                    dep.DEPARTMENT_DESC = dept;
                    dep.Status = status;
                    gc.db.SCL_Department.Add(dep);

                    int n = gc.db.SaveChanges();
                    if (n > 0)
                    {
                        dic.Add("success", "success");
                        //Elasticemail.SendEmail(result.FirstOrDefault().Email, "Lexis Nexis Account Details", body, "", "admin@lexisnexisco.za", "LexisNexis", "");
                    }
                    else
                    {
                        dic.Add("error", "No results found!");
                    }

                }
                else
                {
                    dic.Add("error", "Department name already exist!");
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
        public Dictionary<string, object> updateDepartment(int DeptID, string Depname, string Status)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {
                SCL_Department o = gc.db.SCL_Department.Where(s => s.ID == DeptID).FirstOrDefault();

                o.DEPARTMENT_DESC = Depname;
                o.Status = Status;
                int n = gc.db.SaveChanges();

                if (n > 0)
                {
                    dic.Add("success", "success");
                    //Elasticemail.SendEmail(result.FirstOrDefault().Email, "Lexis Nexis Account Details", body, "", "admin@lexisnexisco.za", "LexisNexis", "");
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

        public Dictionary<string, object> addCategory(string deptId,string category, string status)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
           
            try
            {
                var categoryInfo = gc.db.SCL_Category.Where(d =>d.DEPARTMENT_ID== deptId && d.CATEGORY_DESC == category).ToList();
                if(categoryInfo.Count()==0)
                {
                    var adminCount = gc.db.SCL_Category.Where(d => d.DEPARTMENT_ID == deptId).ToList();
                    SCL_Category sc = new SCL_Category();
                    sc.CATEGORY_DESC = category;
                    sc.Status = status;
                    sc.DEPARTMENT_ID = deptId;
                    sc.CATEGORY_ID = deptId + adminCount.Count();
                    gc.db.SCL_Category.Add(sc);
                    int n = gc.db.SaveChanges();
                    if (n > 0)
                    {
                        dic.Add("success", "Category added successfully");
                        //Elasticemail.SendEmail(result.FirstOrDefault().Email, "Lexis Nexis Account Details", body, "", "admin@lexisnexisco.za", "LexisNexis", "");
                    }
                    else
                    {
                        dic.Add("error", "Category adding failed!");
                    }
                }
                else
                {
                    dic.Add("error", "Category name already exist!");
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
        public Dictionary<string, object> getAllCategories(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();            
            try
            {
                bool isDate = IsDateTime(search);
                DateTime dt;
                if (isDate)
                {
                    dt = Convert.ToDateTime(search);
                }
                Pagenation<SCL> employeeList = new Pagenation<SCL>();
                List<SCL> listEmp = new List<SCL>();
                Func<SCL, object> orderBy = employeeList.orderByType(sorting);
                listEmp = ((from cd in gc.db.SCL_Category
                            //where cd.CompanyID == LoginUserCompanyID
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL
                            {
                                CID = d.cd.ID,
                                DeptName = d.cd.CATEGORY_ID,
                                DeptDesc= d.cd.CATEGORY_DESC,
                                //DeptDate = d.cd.CreatedDateTime,
                                Status = d.cd.Status,

                            }).AsQueryable().ToList();



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listEmp = listEmp.Where(x => DateTime.Compare(x.DeptDate.Value.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listEmp = listEmp.Where(x =>
                          (x.DeptName.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.DeptDesc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Status.ToUpper() ?? "").Contains(search.ToUpper())
                           ).ToList();
                        }

                    }

                }
                employeeList.totalCount = listEmp.Count().ToString();
                listEmp = listEmp.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
                employeeList.complaints = listEmp;

                if (listEmp.Count() > 0)
                {
                    dic.Add("success", employeeList);
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
        public Dictionary<string, object> getAllSubCategories(int pageIndex, int pageSize, string sorting, string search)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                bool isDate = IsDateTime(search);
                DateTime dt;
                if (isDate)
                {
                    dt = Convert.ToDateTime(search);
                }
                Pagenation<SCL> employeeList = new Pagenation<SCL>();
                List<SCL> listEmp = new List<SCL>();
                Func<SCL, object> orderBy = employeeList.orderByType(sorting);
                listEmp = ((from cd in gc.db.SCL_Sub_Category
                                //where cd.CompanyID == LoginUserCompanyID
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL
                            {
                                CID = d.cd.ID,
                                DeptName = d.cd.SUB_CATEGORY_ID,
                                DeptDesc = d.cd.SUB_CATEGORY_DESC,
                                //DeptDate = d.cd.CreatedDateTime,
                                Status = d.cd.Status,

                            }).AsQueryable().ToList();



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listEmp = listEmp.Where(x => DateTime.Compare(x.DeptDate.Value.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listEmp = listEmp.Where(x =>
                          (x.DeptName.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.DeptDesc.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Status.ToUpper() ?? "").Contains(search.ToUpper())
                           ).ToList();
                        }

                    }

                }
                employeeList.totalCount = listEmp.Count().ToString();
                listEmp = listEmp.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
                employeeList.complaints = listEmp;

                if (listEmp.Count() > 0)
                {
                    dic.Add("success", employeeList);
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
        public Dictionary<string, object> updateCategory(int catID, string catname, string status)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();            
            try
            {
                SCL_Category category = gc.db.SCL_Category.Where(s => s.ID == catID).FirstOrDefault();
                if(category!=null)
                {
                    category.CATEGORY_DESC = catname;
                    category.Status = status;
                    int n = gc.db.SaveChanges();

                    if (n > 0)
                    {
                        dic.Add("success", "success");
                        //Elasticemail.SendEmail(result.FirstOrDefault().Email, "Lexis Nexis Account Details", body, "", "admin@lexisnexisco.za", "LexisNexis", "");
                    }

                    else
                    {
                        dic.Add("error", "No results found!");
                    }
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
        public Dictionary<string, object> addSubCategory(string catId,string subcategory, string status)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();            
            try
            {
                var categoryInfo = gc.db.SCL_Sub_Category.Where(d =>d.CATEGORY_ID== catId && d.SUB_CATEGORY_DESC == subcategory).ToList();
                if (categoryInfo.Count() == 0)
                {
                    var adminCount = gc.db.SCL_Sub_Category.Where(d => d.CATEGORY_ID == catId).ToList();
                    SCL_Sub_Category sc = new SCL_Sub_Category();
                    sc.SUB_CATEGORY_DESC = subcategory;
                    sc.Status = status;
                    sc.CATEGORY_ID = catId;
                    sc.SUB_CATEGORY_ID = catId+ adminCount.Count();
                    gc.db.SCL_Sub_Category.Add(sc);
                    int n = gc.db.SaveChanges();
                    if (n > 0)
                    {
                        dic.Add("success", "Sub Category added successfully");
                        //Elasticemail.SendEmail(result.FirstOrDefault().Email, "Lexis Nexis Account Details", body, "", "admin@lexisnexisco.za", "LexisNexis", "");
                    }
                    else
                    {
                        dic.Add("error", "Sub Category adding failed!");
                    }
                }
                else
                {
                    dic.Add("error", "Sub Category name already exist!");
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
        public Dictionary<string, object> updateSubCategory(int subcatID, string subcatname, string status)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                SCL_Sub_Category category = gc.db.SCL_Sub_Category.Where(s => s.ID == subcatID).FirstOrDefault();
                if (category != null)
                {
                    category.SUB_CATEGORY_DESC = subcatname;
                    category.Status = status;
                    int n = gc.db.SaveChanges();
                    if (n > 0)
                    {
                        dic.Add("success", "success");
                        //Elasticemail.SendEmail(result.FirstOrDefault().Email, "Lexis Nexis Account Details", body, "", "admin@lexisnexisco.za", "LexisNexis", "");
                    }
                    else
                    {
                        dic.Add("error", "No results found!");
                    }
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
        public Dictionary<string, object> getAllRoles(int pageIndex, int pageSize, string sorting, string search)
        {
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                bool isDate = IsDateTime(search);
                DateTime dt;
                if (isDate)
                {
                    dt = Convert.ToDateTime(search);
                }
                Pagenation<SCL> employeeList = new Pagenation<SCL>();
                List<SCL> listEmp = new List<SCL>();
                Func<SCL, object> orderBy = employeeList.orderByType(sorting);
                listEmp = ((from cd in gc.db.SCL_UserRoles
                            where cd.CompanyID == LoginUserCompanyID
                            select new { cd })) //.OrderByDescending(s => s.cd.Id))
                              .ToList()
                            .Select(d => new SCL
                            {
                                CID = d.cd.ID,
                                DeptName = d.cd.RoleName,
                                DeptDate = d.cd.CreatedDateTime,
                                Status = d.cd.Status,

                            }).AsQueryable().ToList();



                if (search != "undefined")
                {
                    // searching
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        if (isDate)
                        {
                            dt = Convert.ToDateTime(search);
                            listEmp = listEmp.Where(x => DateTime.Compare(x.DeptDate.Value.Date, dt.Date) <= 0).ToList();
                        }
                        else
                        {
                            listEmp = listEmp.Where(x =>
                          (x.DeptName.ToUpper() ?? "").Contains(search.ToUpper()) ||
                          (x.Status.ToUpper() ?? "").Contains(search.ToUpper())
                           ).ToList();
                        }

                    }

                }
                employeeList.totalCount = listEmp.Count().ToString();
                listEmp = listEmp.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();
                employeeList.complaints = listEmp;

                if (listEmp.Count() > 0)
                {
                    dic.Add("success", employeeList);
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
        public Dictionary<string, object> addRole(string RoleName, string Status)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {
                SCL_UserRoles role = new SCL_UserRoles();
                role.RoleName = RoleName;
                role.CreatedBy = LoginUserCompanyID;
                role.CompanyID = LoginUserCompanyID;
                role.CreatedDateTime = DateTime.Now;
                role.Status = Status;
                gc.db.SCL_UserRoles.Add(role);

                int n = gc.db.SaveChanges();
                if (n > 0)
                {
                    dic.Add("success", "success");
                    //Elasticemail.SendEmail(result.FirstOrDefault().Email, "Lexis Nexis Account Details", body, "", "admin@lexisnexisco.za", "LexisNexis", "");
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
        public Dictionary<string, object> updateRole(int RoleID, string Role, string Status)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            try
            {
                SCL_UserRoles o = gc.db.SCL_UserRoles.Where(s => s.ID == RoleID).FirstOrDefault();

                o.RoleName = Role;
                o.Status = Status;
                o.ModifiedBy = LoginUserCompanyID;
                o.ModifiedDateTime = DateTime.Now;
                int n = gc.db.SaveChanges();

                if (n > 0)
                {
                    dic.Add("success", "success");
                    //Elasticemail.SendEmail(result.FirstOrDefault().Email, "Lexis Nexis Account Details", body, "", "admin@lexisnexisco.za", "LexisNexis", "");
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
        public Dictionary<string, object> getDepartmentsAndRoles()
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {

                int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

                //var departments = (from c in gc.db.SCL_UserDepartments where c.CompanyID == CompanyID && c.Status == "Active" select c).ToList();
                var departments = (from c in gc.db.SCL_Department where c.Status == "Active" select c).ToList();
                var roles = (from c in gc.db.SCL_UserRoles where c.RoleName != "Admin" && c.CompanyID == CompanyID && c.Status == "Active" select c).ToList();

                dic.Add("success", true);
                dic.Add("Departments", departments);
                dic.Add("Roles", roles);

            }
            catch (Exception ex)
            {
                dic.Add("error", ex.Message);
            }

            return dic;

        }
        public Dictionary<string, object> getUsers(int pageIndex, int pageSize, string sorting, string search)
        {
            //int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_COMPANY_ID"].ToString());
            //int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_USER_ID"].ToString());
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());


            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                PagenationUsers employeeList = new PagenationUsers();
                List<Users> listEmp = new List<Users>();
                Func<Users, object> orderBy = employeeList.orderByType(sorting);

                //int CompanyID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_COMPANY_ID"].ToString());
                bool CharCheck = string.IsNullOrWhiteSpace(search);

                listEmp = new List<Users>();

                listEmp = (from USER in gc.db.SCL_Users
                           join DEP in gc.db.SCL_Department on USER.DepartmentID equals DEP.ID
                           join ROLE in gc.db.SCL_UserRoles on USER.RoleID equals ROLE.ID
                           where
                          (USER.CompanyID == LoginUserCompanyID
                          &&
                          (
                          (search != "undefined" && !CharCheck) ?
                          (USER.FirstName.Contains(search) ||
                          USER.LastName.Contains(search) ||
                          USER.UserName.Contains(search) ||
                          USER.EmailID.Contains(search) ||
                          DEP.DEPARTMENT_DESC.Contains(search) ||
                          ROLE.RoleName.Contains(search)) : true
                          ))
                           select new Users
                           {
                               ID = USER.ID,
                               UserName = USER.UserName,
                               RoleName = ROLE.RoleName,
                               FirstName = USER.FirstName,
                               LastName = USER.LastName,
                               DepartmentName = DEP.DEPARTMENT_DESC,
                               EmailID = USER.EmailID
                           }).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable().ToList();

                employeeList.totalCount = (from USER in gc.db.SCL_Users
                                           join DEP in gc.db.SCL_UserDepartments on USER.DepartmentID equals DEP.DepartmentID
                                           join ROLE in gc.db.SCL_UserRoles on USER.RoleID equals ROLE.ID
                                           where
                                            (USER.CompanyID == LoginUserCompanyID
                                            &&
                                            (
                                            (search != "undefined" && !CharCheck) ?
                                            (USER.FirstName.Contains(search) ||
                                            USER.LastName.Contains(search) ||
                                            USER.UserName.Contains(search) ||
                                            USER.EmailID.Contains(search) ||
                                            DEP.DepartmentName.Contains(search) ||
                                            ROLE.RoleName.Contains(search)) : true
                                            ))
                                           select new Users
                                           {
                                               ID = USER.ID,
                                               UserName = USER.UserName,
                                               RoleName = ROLE.RoleName,
                                               FirstName = USER.FirstName,
                                               LastName = USER.LastName,
                                               DepartmentName = DEP.DepartmentName,
                                               EmailID = USER.EmailID
                                           }).Count().ToString();


                employeeList.Users = listEmp;

                if (listEmp.Count() > 0)
                {
                    dic.Add("success", employeeList);
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
        public Dictionary<string, object> CreateNewUser(SCL_Users UserFormData, string password)
        {
            //int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_COMPANY_ID"].ToString());
            //int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_USER_ID"].ToString());

            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());
            int LoginUserID = 1;
            DateTime dt = DateTime.Now;

            Dictionary<string, object> dct = new Dictionary<string, object>();

            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    var UserList = gc.db.SCL_Login.Where(x => x.username == UserFormData.EmailID).FirstOrDefault();

                    if (UserList != null) { dct.Add("error", "Email ID already exists for " + UserList.type + " Type in Database."); return dct; }

                    //count = gc.db.SCL_Users.Where(x => x.UserName == UserFormData.UserName).Count();

                    //if (count > 0) { dct.Add("error", "Username already exists."); return dct; }

                    UserFormData.CreatedDateTime = dt;
                    UserFormData.CompanyID = LoginUserCompanyID;
                    UserFormData.CreatedBy = LoginUserCompanyID;
                    UserFormData.UserName = UserFormData.EmailID;
                    UserFormData.Status = "Activated";
                    UserFormData.UserPass = password;
                    UserFormData.userType = "User";
                    UserFormData.PIN = CreateRandomPassword(6);

                    gc.db.SCL_Users.Add(UserFormData);

                    int saveCount = gc.db.SaveChanges();



                    //getting role wise screen maps to add in userwise
                    List<SCL_UserRoleScreenMapping> RoleScreenList = gc.db.SCL_UserRoleScreenMapping
                        .Where(u => u.RoleID == UserFormData.RoleID && u.CompanyID == UserFormData.CompanyID).ToList();

                    foreach (SCL_UserRoleScreenMapping g in RoleScreenList)
                    {
                        SCL_UserwiseScreenMapping USM = new SCL_UserwiseScreenMapping();
                        USM.CompanyID = LoginUserCompanyID;
                        USM.CreatedBy = LoginUserID;
                        USM.CreatedDateTime = dt;
                        USM.ScreenID = g.ScreenID;
                        USM.UserID = UserFormData.ID;

                        gc.db.SCL_UserwiseScreenMapping.Add(USM);
                    }

                    gc.db.SaveChanges();



                    if (saveCount > 0)
                    {
                        SCL_Login login = new SCL_Login();
                        login.CID = UserFormData.ID;
                        login.username = UserFormData.UserName;
                        login.password = UserFormData.UserPass;
                        login.status = "Active";
                        login.cdate = DateTime.Now;
                        login.type = "USER";
                        gc.db.SCL_Login.Add(login);
                        int m = gc.db.SaveChanges();
                        if (m > 0)
                        {
                            Notification1.NewUserCreate("noreply@assetmanagement.co.za", UserFormData);

                            dct.Add("success", "User created successfully.");
                            tran.Commit();
                        }


                    }
                    else
                    {
                        throw new Exception("User creation failed.");
                        tran.Rollback();
                    }

                }
                catch (Exception ex)
                {
                    dct.Add("error", "User creation failed.");
                    tran.Rollback();
                }

            }
            return dct;
        }

        public Dictionary<string, object> CreateNewUserWithAttachment(HttpFileCollectionBase files, string Password, SCL_Users UserFormData, string Browser)
        {
            //int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_COMPANY_ID"].ToString());
            //int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_USER_ID"].ToString());
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());
            //int LoginUserID = 1;
            DateTime dt = DateTime.Now;

            Dictionary<string, object> dct = new Dictionary<string, object>();

            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    var UserList = gc.db.SCL_Login.Where(x => x.username == UserFormData.EmailID).FirstOrDefault();

                    if (UserList != null) { dct.Add("error", "Email ID already exists for " + UserList.type + " Type in Database."); return dct; }

                    //count = gc.db.SCL_Users.Where(x => x.UserName == UserFormData.UserName).Count();

                    //if (count > 0) { dct.Add("error", "Username already exists."); return dct; }

                    //Uploading attachments to application folder
                    string file_name = "";
                    string fname = "";

                    List<string> fileNameList = new List<string>();

                    if (files.Count > 0)
                    {  //  Get all files from Request object 
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];

                            if (Browser == "IE" || Browser == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }

                            fname = file.FileName;
                            file_name = string.Format("{0}-{1}", DateTime.Now.ToString("ddMMMyyyyHHmmss"), fname.Replace("-", ""));
                            fname = Path.Combine(HttpContext.Current.Server.MapPath("~/Attachments/"), file_name);
                            file.SaveAs(fname);

                            fileNameList.Add(file_name);
                        }

                    }

                    UserFormData.ProfilePicture = fileNameList[0];
                    UserFormData.CreatedDateTime = dt;
                    UserFormData.CompanyID = LoginUserCompanyID;
                    UserFormData.CreatedBy = LoginUserCompanyID;
                    UserFormData.Status = "Activated";
                    UserFormData.UserName = UserFormData.EmailID;
                    UserFormData.UserPass = Password;
                    UserFormData.PIN = CreateRandomPassword(6);

                    gc.db.SCL_Users.Add(UserFormData);

                    int saveCount = gc.db.SaveChanges();

                    //getting role wise screen maps to add in userwise
                    List<SCL_UserRoleScreenMapping> RoleScreenList = gc.db.SCL_UserRoleScreenMapping
                        .Where(u => u.RoleID == UserFormData.RoleID && u.CompanyID == UserFormData.CompanyID).ToList();

                    foreach (SCL_UserRoleScreenMapping g in RoleScreenList)
                    {
                        SCL_UserwiseScreenMapping USM = new SCL_UserwiseScreenMapping();
                        USM.CompanyID = LoginUserCompanyID;
                        USM.CreatedBy = LoginUserCompanyID;
                        USM.CreatedDateTime = dt;
                        USM.ScreenID = g.ScreenID;
                        USM.UserID = UserFormData.ID;

                        gc.db.SCL_UserwiseScreenMapping.Add(USM);
                    }

                    gc.db.SaveChanges();



                    if (saveCount > 0)
                    {
                        SCL_Login login = new SCL_Login();
                        login.CID = UserFormData.ID;
                        login.username = UserFormData.UserName;
                        login.password = UserFormData.UserPass;
                        login.status = "Active";
                        login.cdate = DateTime.Now;
                        login.type = "USER";
                        gc.db.SCL_Login.Add(login);
                        int m = gc.db.SaveChanges();
                        if (m > 0)
                        {
                            Notification1.NewUserCreate("noreply@assetmanagement.co.za", UserFormData);

                            dct.Add("success", "User created successfully.");
                            tran.Commit();
                        }


                    }
                    else
                    {
                        throw new Exception("User creation failed.");
                    }

                }
                catch (Exception ex /*System.Data.Entity.Validation.DbEntityValidationException dbEx*/)
                {
                    dct.Add("error", "User creation failed.");
                    tran.Rollback();
                }

            }
            return dct;
        }

        public Dictionary<string, object> UpdateUser(SCL_Users UserFormData)
        {
            //int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_COMPANY_ID"].ToString());
            //int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_USER_ID"].ToString());
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            //int LoginUserID = 1;
            DateTime dt = DateTime.Now;

            Dictionary<string, object> dct = new Dictionary<string, object>();

            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    //Update
                    int count = gc.db.SCL_Users.Where(x => x.ID == UserFormData.ID && x.CompanyID == LoginUserCompanyID).Count();

                    if (count < 1) { dct.Add("error", "Problem updating User."); return dct; }

                    //count = gc.db.SCL_Users.Where(x => x.ID != UserFormData.ID && x.UserName == UserFormData.UserName).Count();

                    //if (count > 0) { dct.Add("error", "Username already exists."); return dct; }

                    count = gc.db.SCL_Users.Where(x => x.ID != UserFormData.ID && x.EmailID == UserFormData.EmailID).Count();

                    if (count > 0) { dct.Add("error", "Email ID already exists."); return dct; }

                    SCL_Users row = gc.db.SCL_Users.Where(x => x.ID == UserFormData.ID).FirstOrDefault();

                    SCL_UsersAudit aRow = new SCL_UsersAudit();

                    aRow.AuditCreatedBy = row.ID;
                    aRow.AuditCreatedDateTime = dt;

                    aRow.CompanyID = row.CompanyID;
                    aRow.CreatedBy = row.CreatedBy;
                    aRow.CreatedDateTime = row.CreatedDateTime;
                    aRow.DepartmentID = row.DepartmentID;
                    aRow.EmailID = row.EmailID;
                    aRow.FirstName = row.FirstName;
                    aRow.UserID = row.ID;
                    aRow.LastName = row.LastName;
                    aRow.ModifiedBy = row.ModifiedBy;
                    aRow.ModifiedDateTime = row.ModifiedDateTime;
                    aRow.PIN = row.PIN;
                    aRow.ProfilePicture = row.ProfilePicture;
                    aRow.PIN = row.PIN;
                    aRow.RoleID = row.RoleID;
                    aRow.Status = row.Status;
                    aRow.UserName = row.UserName;
                    aRow.UserPass = row.UserPass;

                    gc.db.SCL_UsersAudit.Add(aRow);

                    int added = gc.db.SaveChanges();

                    if (added < 1) { throw new Exception("Problem updating User."); }

                    row.ModifiedBy = LoginUserCompanyID;
                    row.ModifiedDateTime = dt;
                    row.FirstName = UserFormData.FirstName;
                    row.LastName = UserFormData.LastName;
                    row.DepartmentID = UserFormData.DepartmentID;
                    row.RoleID = UserFormData.RoleID;
                    //row.UserName = UserFormData.UserName;
                    row.EmailID = UserFormData.EmailID;



                    int updated = gc.db.SaveChanges();
                    tran.Commit();

                    if (updated > 0)
                    {
                        Notification1.NewUserUpdate("noreply@cfocus.co.za", UserFormData);

                        dct.Add("success", "User updated successfully.");

                    }
                    else
                    {
                        throw new Exception("User updation failed.");
                    }


                }
                catch (Exception ex)
                {
                    dct.Add("error", ex.Message);
                    tran.Rollback();
                }

            }
            return dct;
        }

        public bool UpdateUser(string EmailID, int ID, int CompanyID)
        {

            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    SCL_Users USR = gc.db.SCL_Users.Where(x => x.EmailID == EmailID && x.ID == ID && x.CompanyID == CompanyID).FirstOrDefault();
                    USR.Status = "Activated";

                    int saveCount = gc.db.SaveChanges();

                    tran.Commit();

                    if (saveCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {

                    tran.Rollback();
                    return false;
                }
            }
        }

        public Dictionary<string, object> UpdateUserWithAttachment(HttpFileCollectionBase files, SCL_Users UserFormData, string Browser)
        {
            //int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_COMPANY_ID"].ToString());
            //int LoginUserID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_USER_ID"].ToString());
            int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

            //int LoginUserID = 1;
            DateTime dt = DateTime.Now;

            Dictionary<string, object> dct = new Dictionary<string, object>();

            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    int count = gc.db.SCL_Users.Where(x => x.ID == UserFormData.ID && x.CompanyID == LoginUserCompanyID).Count();

                    if (count < 1) { dct.Add("error", "Problem updating User."); return dct; }

                    //count = gc.db.SCL_Users.Where(x => x.ID != UserFormData.ID && x.UserName == UserFormData.UserName).Count();

                    //if (count > 0) { dct.Add("error", "Username already exists."); return dct; }

                    count = gc.db.SCL_Users.Where(x => x.ID != UserFormData.ID && x.EmailID == UserFormData.EmailID).Count();

                    if (count > 0) { dct.Add("error", "Email ID already exists."); return dct; }


                    //Uploading attachments to application folder
                    string file_name = "";
                    string fname = "";

                    List<string> fileNameList = new List<string>();

                    if (files.Count > 0)
                    {  //  Get all files from Request object 
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];

                            if (Browser == "IE" || Browser == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }

                            fname = file.FileName;
                            file_name = string.Format("{0}-{1}", DateTime.Now.ToString("ddMMMyyyyHHmmss"), fname.Replace("-", ""));
                            fname = Path.Combine(HttpContext.Current.Server.MapPath("~/Attachments/"), file_name);
                            file.SaveAs(fname);

                            fileNameList.Add(file_name);
                        }

                    }


                    SCL_Users row = gc.db.SCL_Users.Where(x => x.ID == UserFormData.ID).FirstOrDefault();

                    SCL_UsersAudit aRow = new SCL_UsersAudit();

                    aRow.AuditCreatedBy = row.ID;
                    aRow.AuditCreatedDateTime = dt;

                    aRow.CompanyID = row.CompanyID;
                    aRow.CreatedBy = row.CreatedBy;
                    aRow.CreatedDateTime = row.CreatedDateTime;
                    aRow.DepartmentID = row.DepartmentID;
                    aRow.EmailID = row.EmailID;
                    aRow.FirstName = row.FirstName;
                    aRow.UserID = row.ID;
                    aRow.LastName = row.LastName;
                    aRow.ModifiedBy = row.ModifiedBy;
                    aRow.ModifiedDateTime = row.ModifiedDateTime;
                    aRow.PIN = row.PIN;
                    aRow.ProfilePicture = row.ProfilePicture;
                    aRow.PIN = row.PIN;
                    aRow.RoleID = row.RoleID;
                    aRow.Status = row.Status;
                    aRow.UserName = row.UserName;
                    aRow.UserPass = row.UserPass;

                    gc.db.SCL_UsersAudit.Add(aRow);

                    int added = gc.db.SaveChanges();

                    if (added < 1) { throw new Exception("Problem updating User."); }

                    row.ModifiedBy = LoginUserCompanyID;
                    row.ModifiedDateTime = dt;
                    row.FirstName = UserFormData.FirstName;
                    row.LastName = UserFormData.LastName;
                    row.DepartmentID = UserFormData.DepartmentID;
                    row.RoleID = UserFormData.RoleID;
                    row.UserName = UserFormData.UserName;
                    row.EmailID = UserFormData.EmailID;
                    row.ProfilePicture = fileNameList[0];

                    int updated = gc.db.SaveChanges();
                    tran.Commit();

                    if (updated > 0)
                    {
                        Notification1.NewUserUpdate("noreply@cfocus.co.za", UserFormData);

                        dct.Add("success", "User updated successfully.");

                    }
                    else
                    {
                        throw new Exception("User updation failed.");
                    }


                }
                catch (Exception ex /*System.Data.Entity.Validation.DbEntityValidationException dbEx*/)
                {
                    dct.Add("error", "User creation failed.");
                    tran.Rollback();
                }

            }
            return dct;
        }
        public string CreateRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public Dictionary<string, object> getUserDetails(int UserID)
        {
            Dictionary<string, object> dct = new Dictionary<string, object>();
            try
            {
                ///int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["AMUSERDATA_COMPANY_ID"].ToString());
                int LoginUserCompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());
                var UserDetails = (from USER in gc.db.SCL_Users
                                   join DEP in gc.db.SCL_UserDepartments on USER.DepartmentID equals DEP.DepartmentID
                                   join ROLE in gc.db.SCL_UserRoles on USER.RoleID equals ROLE.ID
                                   where
                                    USER.CompanyID == LoginUserCompanyID
                                    &&
                                    USER.ID == UserID
                                   select new Users
                                   {
                                       ID = USER.ID,
                                       UserName = USER.UserName,
                                       RoleName = ROLE.RoleName,
                                       FirstName = USER.FirstName,
                                       LastName = USER.LastName,
                                       DepartmentID = USER.DepartmentID,
                                       RoleID = USER.RoleID,
                                       ProfilePicture = USER.ProfilePicture,
                                       DepartmentName = DEP.DepartmentName,
                                       EmailID = USER.EmailID
                                   }).ToList();

                dct.Add("UserDetails", UserDetails);


                dct.Add("success", true);

            }
            catch (Exception ex)
            {
                dct.Add("error", ex.Message);
            }
            return dct;
        }
        public Dictionary<string, object> getKPIUsers()
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                var tt = gc.db.SCL_Users.ToList();


                if (tt.Count() > 0)
                {
                    dic.Add("success", tt);
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

        public Dictionary<string, object> getComplaintDepartments()
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {

                int CompanyID = Convert.ToInt32(HttpContext.Current.Session["cid_SCL_account"].ToString());

                var departments = (from c in gc.db.SCL_Department select c).ToList();
                
                dic.Add("success", true);
                dic.Add("ComplaintDepartments", departments);
               

            }
            catch (Exception ex)
            {
                dic.Add("error", ex.Message);
            }

            return dic;

        }

        public Dictionary<string, object> getComplaintCategoriesbyDeptID(string DeptID)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {

                
                var categories = gc.db.SCL_Category.Where(x=> x.DEPARTMENT_ID == DeptID).ToList();

                dic.Add("success", true);
                dic.Add("ComplaintCategories", categories);


            }
            catch (Exception ex)
            {
                dic.Add("error", ex.Message);
            }

            return dic;

        }

        public Dictionary<string, object> getAllComplaintCategories()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                var categories = gc.db.SCL_Category.ToList();
                dic.Add("success", true);
                dic.Add("ComplaintCategories", categories);
            }
            catch (Exception ex)
            {
                dic.Add("error", ex.Message);
            }

            return dic;

        }

        public Dictionary<string, object> getComplaintSubCategoriesbyCatID(string CatID)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {

                
                var subCategories = gc.db.SCL_Sub_Category.Where(x => x.CATEGORY_ID == CatID).ToList();

                dic.Add("success", true);
                dic.Add("ComplaintSubCategories", subCategories);


            }
            catch (Exception ex)
            {
                dic.Add("error", ex.Message);
            }

            return dic;

        }



    }
}