using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models
{
    public class MobileAppResponse<T>
    {
        public IQueryable<T> all { get; set; }
        public T single { get; set; }

        public int loggedId { get; set; }
        public int? companyId { get; set; }
        public string loggedType { get; set; }
        public int statusCode { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public int noOfUsers { get; set; }
        public int noOfComplints { get; set; }
        public IEnumerable<T> complaintCounts { get; set; }

    }



    public class ComplaintCounts
    {
        public int count { get; set; }
        public string name { get; set; }
    }



    public class MasterData
    {
        public string id { get; set; }
        public string name { get; set; }
    }



    public class mobileComplaintInputs
    {

        public int loggedId { get; set; }
        public int? companyId { get; set; }
        public string Category_Id { get; set; }
        public string Category_Desc { get; set; }
        public string SubCategory_Id { get; set; }
        public string SubCategory_Desc { get; set; }
        public string Dept_Id { get; set; }
        public string Dept_Desc { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string img_data { get; set; }
        public string comments { get; set; }
    }
}