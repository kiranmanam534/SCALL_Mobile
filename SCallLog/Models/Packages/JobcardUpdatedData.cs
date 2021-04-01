using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class JobcardUpdatedData
    {      
        public int ID { get; set; }
        public string compStatus { get; set; }
        public string PStatus { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ServiceHours { get; set; }
        public string Comments { get; set; }
        public string VehicalNo { get; set; }
        public string JobDistance { get; set; }
    }
}