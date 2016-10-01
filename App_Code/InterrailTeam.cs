using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailTeam
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int FacilityId { get; set; }
        public string TeamMembers { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool Active { get; set; }        
    }
}