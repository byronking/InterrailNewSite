using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailUser
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string UserLongName { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}