//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCallLog.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SCL_UsersAudit
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string EmailID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string Status { get; set; }
        public string ProfilePicture { get; set; }
        public string PIN { get; set; }
        public Nullable<int> AuditCreatedBy { get; set; }
        public Nullable<System.DateTime> AuditCreatedDateTime { get; set; }
    }
}
