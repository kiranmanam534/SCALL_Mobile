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
    
    public partial class SCL_Comp_Status
    {
        public int STATUS_ID { get; set; }
        public string STATUS { get; set; }
        public Nullable<int> ISACTIVE { get; set; }
        public Nullable<int> ISDELETED { get; set; }
        public string Flag { get; set; }
        public Nullable<int> CREATEDBY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public Nullable<int> MODIFIEDBY { get; set; }
        public Nullable<System.DateTime> MODIFIEDDATE { get; set; }
    }
}