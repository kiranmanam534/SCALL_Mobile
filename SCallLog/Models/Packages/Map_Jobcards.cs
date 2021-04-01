using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class Map_Jobcards
    {
        public int JCID { get; set; }
        public string JC_REFERENCE { get; set; }
        public System.DateTime JC_DATE { get; set; }
        public string COMP_REFERENCE { get; set; }
        public Nullable<System.DateTime> COMP_DATE { get; set; }
        public int COMPANY_ID { get; set; }
        public int COMPLAINT_ID { get; set; }
        public int USER_ID { get; set; }
        public string DEPARTMENT { get; set; }
        public string CATEGORY { get; set; }
        public string SUB_CATEGORY { get; set; }
        public string ADDRESS { get; set; }
        public string COMP_STATUS { get; set; }
        public string COMP_COMMENTS { get; set; }
        public Nullable<int> FLAG { get; set; }
        public Nullable<int> CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public Nullable<int> MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }
        public string SERVICE_HOURS { get; set; }

        public string LAT { get; set; }

        public string LONG { get; set; }

    }
}