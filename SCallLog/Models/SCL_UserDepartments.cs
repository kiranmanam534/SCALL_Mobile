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
    
    public partial class SCL_UserDepartments
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
