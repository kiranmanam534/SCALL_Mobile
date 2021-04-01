using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class WorkOrder
    {
        public int COMPLAINT_ID { get; set; }
        public string JC_REFERENCE { get; set; }
        
        public bool isComplaintChecked { get; set; }
    }
}