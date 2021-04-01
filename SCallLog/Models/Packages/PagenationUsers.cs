using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models.Packages
{
    public class PagenationUsers
    {
        public List<Users> Users { get; set; }
        public string totalCount { get; set; }


        public Func<Users, object> orderByType(string orderProperty)
        {
            //orderProperty = orderProperty?.ToLowerInvariant();
            switch (orderProperty)
            {
                case "UserName":
                    return x => x.UserName;
                case "UserRole":
                    return x => x.UserRole;
                case "FirstName":
                    return x => x.FirstName;
                case "LastName":
                    return x => x.LastName;
                case "DepartmentName":
                    return x => x.DepartmentName;
                case "RoleName":
                    return x => x.RoleName;
                case "EmailID":
                    return x => x.EmailID;
                default:
                    return x => x.UserName;

            }

        }
    }
}