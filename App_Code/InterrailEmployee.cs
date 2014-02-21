using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string TempNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? InactiveDate { get; set; }
        public DateTime? TempStartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string SSN { get; set; }
        public DateTime? BirthDate { get; set; }
        public string EmployeePhone { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhone { get; set; }
        public bool TempEmployee { get; set; }
        public bool Salaried { get; set; }
        public int? EmploymentSourceId { get; set; }
        public int FacilityId { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool Active { get; set; }
    }
}