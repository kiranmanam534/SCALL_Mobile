using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCallLog.Models
{
    public class GlobalClass
    {
        public scallEntities db = new scallEntities();
        public scallEntities dbLazy = new scallEntities();
    }
}