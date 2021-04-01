using PMS.Models.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class tbl_welcome
    {
        GlobalClass gc = new GlobalClass();
        public Dictionary<string, object> addCompany(HttpFileCollectionBase files, string FreeTrail, SCL_CompanyRegistration UserFormData, string Browser)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            var subDate = DateTime.Now;
            var SubEndDate = DateTime.Now;
            if (FreeTrail.Equals("YES"))
            {

                SubEndDate = subDate.AddDays(30);
            }
            else
            {
                SubEndDate = subDate.AddDays(10);
            }

            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {

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

                    //string password_ln = CreateRandomPassword(8);
                    string body = "Welcome to Leads Portal.\n\nYour Account Details are \n\n username: " + UserFormData.company_email + "\n\n Password: " + UserFormData.company_password + "\n\n\n Thank You\n\n";

                    UserFormData.company_status = "Active";
                    UserFormData.cdate = subDate;
                    UserFormData.Flag = FreeTrail;
                    UserFormData.SubscriptionEndDate = SubEndDate;
                    UserFormData.company_uniqueID = "Not Required";
                    UserFormData.ProfilePicture = fileNameList[0];
                    gc.db.SCL_CompanyRegistration.Add(UserFormData);

                    int n = gc.db.SaveChanges();
                    if (n > 0)
                    {

                        SCL_UserRoles role = new SCL_UserRoles();
                        role.RoleName = "Admin";
                        role.CreatedBy = UserFormData.CID;
                        role.CompanyID = UserFormData.CID;
                        role.CreatedDateTime = subDate;
                        role.Status = "Active";
                        gc.db.SCL_UserRoles.Add(role);
                        gc.db.SaveChanges();

                        SCL_UserDepartments dep = new SCL_UserDepartments();
                        dep.DepartmentName = "Admin";
                        dep.CreatedBy = UserFormData.CID;
                        dep.CompanyID = UserFormData.CID;
                        dep.CreatedDateTime = subDate;
                        dep.Status = "Active";
                        gc.db.SCL_UserDepartments.Add(dep);

                        gc.db.SaveChanges();

                        SCL_Users users = new SCL_Users();
                        users.UserName = UserFormData.company_email;
                        users.UserPass = UserFormData.company_password;
                        users.RoleID = role.ID;
                        users.DepartmentID = dep.DepartmentID;
                        users.FirstName = UserFormData.first_name;
                        users.LastName = UserFormData.last_name;
                        users.EmailID = UserFormData.company_email;
                        users.CompanyID = UserFormData.CID;
                        users.Status = "Active";
                        users.CreatedBy = UserFormData.CID;
                        users.CreatedDateTime = subDate;
                        users.userType = "Admin";
                        gc.db.SCL_Users.Add(users);
                        int x = gc.db.SaveChanges();

                        List<SCL_UserRoleScreenMapping> RoleScreenList = gc.db.SCL_UserRoleScreenMapping
                        .Where(u => u.RoleID == users.RoleID && u.CompanyID == users.CompanyID).ToList();

                        foreach (SCL_UserRoleScreenMapping g in RoleScreenList)
                        {
                            SCL_UserwiseScreenMapping USM = new SCL_UserwiseScreenMapping();
                            USM.CompanyID = users.CompanyID;
                            USM.CreatedBy = users.CompanyID;
                            USM.CreatedDateTime = subDate;
                            USM.ScreenID = g.ScreenID;
                            USM.UserID = users.ID;

                            gc.db.SCL_UserwiseScreenMapping.Add(USM);
                        }

                        gc.db.SaveChanges();

                        SCL_Login login = new SCL_Login();
                        login.CID = UserFormData.CID;
                        login.username = UserFormData.company_email;
                        login.password = UserFormData.company_password;
                        login.status = "Active";
                        login.cdate = subDate;
                        login.type = "ADMIN";
                        login.SubscriptionEndDate = SubEndDate;
                        login.Flag = FreeTrail;
                        gc.db.SCL_Login.Add(login);
                        int m = gc.db.SaveChanges();
                        if (m > 0 && x > 0)
                        {

                            Elasticmail.SendEmail(UserFormData.company_email, "Leads Account Details", body, "", "admin@sixstepsolution.co.za", "Leads", "");

                            tran.Commit();
                            dic.Add("success", "success");
                        }
                        else
                        {
                            tran.Rollback();
                            dic.Add("Error", "Error");
                        }


                    }

                    else
                    {
                        dic.Add("error", "No results found!");
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    tran.Rollback();
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
            }
            return dic;
        }

        public Dictionary<string, object> addCustomer(string FirstName, string LastName, string Email, string Password, string Telephone)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            using (var tran = gc.db.Database.BeginTransaction())
            {
                try
                {
                    //string body = "Welcome to SCL Portal.\n\nYour Account Details are \n\n username: " + email + "\n\n Password: " + password_ln + "\n\n\n Thank You\n\n";
                    SCL_Customer cr = new SCL_Customer();
                    cr.FirstName = FirstName;
                    cr.LastName = LastName;
                    cr.EmailAddress = Email;
                    cr.Username = Email;
                    cr.Telephone = Telephone;
                    cr.Password = Password;
                    cr.Status = "Active";
                    cr.IsActive = true;
                    cr.CustomerID = 1;
                    cr.CreatedDateTime = DateTime.Now;
                    gc.db.SCL_Customer.Add(cr);

                    int n = gc.db.SaveChanges();
                    if (n > 0)
                    {
                        SCL_Login login = new SCL_Login();
                        login.CID = cr.CustomerID;
                        login.username = cr.EmailAddress;
                        login.password = cr.Password;
                        login.status = "Active";
                        login.cdate = DateTime.Now;
                        login.type = "CUSTOMER";
                        gc.db.SCL_Login.Add(login);
                        int m = gc.db.SaveChanges();

                        if (m > 0)
                        {
                            tran.Commit();
                            dic.Add("success", "success");
                        }
                        else
                        {
                            tran.Rollback();
                            dic.Add("Error", "Error");
                        }


                    }

                    else
                    {
                        dic.Add("error", "No results found!");
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    tran.Rollback();
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
            }
            return dic;
        }


        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public Dictionary<string, object> getCompanyList()
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                var tt = gc.db.SCL_Login.ToList();


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

        public Dictionary<string, object> getCustomerList()
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                var tt = gc.db.SCL_Customer.ToList();


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

        public Dictionary<string, object> getPassword(string Company_Email)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                var tt = gc.db.SCL_Login.Where(s => s.username == Company_Email).ToList();


                if (tt.Count() > 0)
                {
                    string body = "Welcome to CRM Portal.\n\nYour Account Details are \n\n username: " + Company_Email + "\n\n Password: " + tt.FirstOrDefault().password + "\n\n\n Thank You\n\n";
                    Elasticmail.SendEmail(Company_Email, "CRM " + tt.FirstOrDefault().type + " Forgot Password Details", body, "", "admin@CRM.co.za", "CRM", "");
                    dic.Add("success", "Password Send to Your Registered Mail ID.");
                }
                else
                {
                    dic.Add("success", "Your Registerd Email ID is Not Available");
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

        public Dictionary<string, object> getUserPassword(string User_Email)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                var tt = gc.db.SCL_Users.Where(s => s.EmailID == User_Email).ToList();


                if (tt.Count() > 0)
                {
                    string body = "Welcome to SCL Portal.\n\nYour Account Details are \n\n username: " + User_Email + "\n\n Password: " + tt.FirstOrDefault().UserPass + "\n\n\n Thank You\n\n";
                    Elasticmail.SendEmail(User_Email, "leads User Forgot Password Details", body, "", "admin@Leads.co.za", "Leads", "");
                    dic.Add("success", "Password Send to Your Registered Mail ID.");
                }
                else
                {
                    dic.Add("success", "Your Registerd Email ID is Not Available");
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
    }
}