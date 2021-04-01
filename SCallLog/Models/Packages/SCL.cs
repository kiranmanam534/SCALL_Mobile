using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class SCL
    {
        public int CID { get; set; }
        public int Count { get; set; }
        
        public string DeptName { get; set; }
        public string DeptDesc { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DeptDate { get; set; }

        
    }
}