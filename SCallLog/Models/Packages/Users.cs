using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class Users
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public int UserRole { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string EmailID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string ProfilePicture { get; set; }

        public string RoleName { get; set; }
        public string DepartmentName { get; set; }
    }
}