using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class Pagenation<T>
    {
        public List<T> complaints { get; set; }
        public string totalCount { get; set; }
        public Func<SCL, object> orderByType(string orderProperty)
        {
            //orderProperty = orderProperty?.ToLowerInvariant();
            switch (orderProperty)
            {
                case "CID":
                    return x => x.CID;
                
                case "Status":
                    return x => x.Status;
                case "DeptName":
                    return x => x.DeptName;
                case "DeptDate":
                    return x => x.DeptDate;
                //case "RoleName":
                //    return x => x.RoleName;
                //case "UserName":
                //    return x => x.UserName;
                //case "Createddate":
                //    return x => x.CreatedDate;
                
                default:
                    return x => x.DeptName;

            }
        }

        public Func<SCL_Mobile_Complaints, object> orderByType1(string orderProperty)
        {
            //orderProperty = orderProperty?.ToLowerInvariant();
            switch (orderProperty)
            {
                case "Complaint_ReferenceNo":
                    return x => x.Complaint_ReferenceNo;

                case "ComplaintStatus":
                    return x => x.ComplaintStatus;
                case "Category_Desc":
                    return x => x.Category_Desc;
                case "SubCategory_Desc":
                    return x => x.SubCategory_Desc;
                case "Dept_Desc":
                    return x => x.Dept_Desc;
                case "ID":
                    return x => x.ID;
                case "Complaint_Date":
                    return x => x.Complaint_Date;
                case "Address":
                    return x => x.Address;
                case "automatic_complaint":
                    return x => x.automatic_complaint;
                default:
                    return x => x.Complaint_ReferenceNo;

            }
        }

        public Func<SCL_JobCards, object> orderByType2(string orderProperty)
        {
            //orderProperty = orderProperty?.ToLowerInvariant();
            switch (orderProperty)
            {
                case "JC_REFERENCE":
                    return x => x.JC_REFERENCE;
                case "JC_DATE":
                    return x => x.JC_DATE;
                case "CATEGORY":
                    return x => x.CATEGORY;
                case "SUB_CATEGORY":
                    return x => x.SUB_CATEGORY;
                case "DEPARTMENT":
                    return x => x.DEPARTMENT;
                case "ID":
                    return x => x.COMPLAINT_ID;
                case "COMP_STATUS":
                    return x => x.COMP_STATUS;
                case "ADDRESS":
                    return x => x.ADDRESS;
                case "COMP_COMMENTS":
                    return x => x.COMP_COMMENTS;
                default:
                    return x => x.JC_DATE;

            }
        }
    }
}