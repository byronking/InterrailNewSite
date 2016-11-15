using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InterrailPPRS.App_Code;

namespace InterrailPPRS.Admin
{
    public partial class EmployeeEdit : System.Web.UI.Page
    {
        public string EmployeeNumber { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public int FacilityId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmployeeId"] != null)
            {
                var saveEmployeeId = Convert.ToInt32(Session["EmployeeId"]);
                var employeeId = Convert.ToInt32(Request.QueryString["id"]);
                if (saveEmployeeId == employeeId)
                {
                    lblRecordSaved.Visible = true;
                    //txtEmployeeNumber.Enabled = false;
                }
            }

            if (Session["FacilityId"] != null)
            {
                FacilityId = Convert.ToInt32(Session["FacilityId"]);
            }

            if (!Page.IsPostBack)
            {
                LoadEmploymentSourcesByFacility(FacilityId);
                LoadSavedUser();
            }            
        }

        /// <summary>
        /// This loads the page for a new or existing user.
        /// </summary>
        private void LoadSavedUser()
        {
            if (Request.QueryString["Id"] != null)
            {
                if (Convert.ToInt32(Request.QueryString["Id"]) == 0)
                {
                    lblTitleLabel.Text = "Create New Employee";
                    GenerateNewEmployeeNumber();
                    txtTemporaryNumber.Text =  GenerateNewTempNumber().ToString();
                }
                else
                {
                    // Get the saved data.
                    var repository = new InterrailEmployeeRepository();
                    var employee = repository.GetEmployeeById(Convert.ToInt32(Request["Id"]));

                    txtFirstName.Text = employee.FirstName;
                    txtMiddleInitial.Text = employee.MiddleInitial;
                    txtLastName.Text = employee.LastName;
                    txtSSN.Text = employee.SSN;

                    if (employee.BirthDate != null)
                    {
                        txtDOB.Text = Convert.ToDateTime(employee.BirthDate).ToShortDateString();
                    }

                    txtEmployeePhone.Text = employee.EmployeePhone;
                    txtAddress1.Text = employee.Address1;
                    txtAddress2.Text = employee.Address2;
                    txtCity.Text = employee.City;
                    txtState.Text = employee.State;
                    txtZipCode.Text = employee.ZipCode;
                    txtEmergencyContact.Text = employee.EmergencyContact;
                    txtEmergencyContactPhone.Text = employee.EmergencyContactPhone;

                    if (employee.TempEmployee == false)
                    {
                        radioPermanent.Checked = true;
                    }

                    radioTemporary.Checked = employee.TempEmployee;

                    if (employee.EmployeeNumber == string.Empty)
                    {
                        GenerateNewEmployeeNumber();
                    }
                    else
                    {
                        txtEmployeeNumber.Text = employee.EmployeeNumber;
                    }

                    //txtEmployeeNumber.Enabled = false;

                    if (employee.TempNumber == string.Empty)
                    {
                        txtTemporaryNumber.Text = GenerateNewTempNumber().ToString();
                    }
                    else
                    {
                        txtTemporaryNumber.Text = employee.TempNumber;
                    }

                    if (employee.HireDate != null)
                    {
                        txtHireDate.Text = Convert.ToDateTime(employee.HireDate).ToShortDateString();
                    }

                    if (employee.InactiveDate != null)
                    {
                        txtInactiveDate.Text = Convert.ToDateTime(employee.InactiveDate).ToShortDateString();
                    }

                    if (employee.TempStartDate != null)
                    {
                        txtStartDate.Text = Convert.ToDateTime(employee.TempStartDate).ToShortDateString();
                    }

                    ddlEmploymentSource.SelectedValue = employee.EmploymentSourceId.ToString();

                    if (employee.TerminationDate != null)
                    {
                        txtTerminationDate.Text = Convert.ToDateTime(employee.TerminationDate).ToShortDateString();
                    }

                    chkBoxSalaried.Checked = employee.Salaried;
                    chkBoxActive.Checked = employee.Active;

                    if (employee.LastModifiedOn != null)
                    {
                        lblLastModifiedDate.Text = Convert.ToDateTime(employee.LastModifiedOn).ToString();
                    }

                    lblModifiedByUser.Text = employee.LastModifiedBy;
                }
            }
            else
            {
                Response.Redirect("Employee.aspx");
            }
        }

        /// <summary>
        /// This loads the employment sources by facility.
        /// </summary>
        private void LoadEmploymentSourcesByFacility(int facilityId)
        {            
            var repository = new InterrailEmployeeRepository();
            var employmentSourceList = repository.GetEmploymentSourcesByFacilityId(facilityId);

            ddlEmploymentSource.DataSource = employmentSourceList;
            ddlEmploymentSource.DataTextField = "SourceName";
            ddlEmploymentSource.DataValueField = "Id";
            ddlEmploymentSource.DataBind();        
        }

        /// <summary>
        /// This generates the employee number for new employees.
        /// </summary>
        public void GenerateNewEmployeeNumber()
        {
            var day = DateTime.Now.DayOfWeek;
            DateTime d = DateTime.Now.Date;
            
            int deltaStart = DayOfWeek.Friday - 7 - d.DayOfWeek;
            int deltaEnd = DayOfWeek.Thursday - d.DayOfWeek;
            DateTime start = d.AddDays(deltaStart);
            DateTime end = d.AddDays(deltaEnd);

            StartDate = start.ToShortDateString();
            EndDate = end.ToShortDateString();
            Year = d.ToString("yy");
            Month = d.ToString("MM");

            var repository = new InterrailEmployeeRepository();
            //var employeeList = repository.GetEmployeesHiredThisWeek(StartDate, EndDate);

            EmployeeNumber = Year + Month + "000";
            //EmployeeNumber = Convert.ToString(Convert.ToInt32(EmployeeNumber) * 10);

            var employeeNumbersList = repository.GetAllEmployeeNumbers();

            // Return the index of the employee number, if it exists.
            var existingEmployeeNumbersList = employeeNumbersList.FindIndex(e => e.Number.Trim() == EmployeeNumber);

            while (existingEmployeeNumbersList > 0)
            {
                // Existing number! Increment!
                EmployeeNumber = Convert.ToString(Convert.ToInt32(EmployeeNumber) + 1);

                // Check against the list again.
                existingEmployeeNumbersList = employeeNumbersList.FindIndex(e => e.Number.Trim() == EmployeeNumber);
            }

            txtEmployeeNumber.Text = EmployeeNumber;
        }

        private int GenerateNewTempNumber()
        {
            var tempNumber = 0;

            var repository = new InterrailEmployeeRepository();
            tempNumber = repository.GetNewTempNumber();

            return tempNumber;
        }

        /// <summary>
        /// This saves the new user or updates an existing one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                var employee = new InterrailEmployee();
                var repository = new InterrailEmployeeRepository();
                var employeeId = Request.QueryString["Id"];

                employee.EmployeeId = Convert.ToInt32(employeeId);
                employee.Active = chkBoxActive.Checked;
                employee.Address1 = txtAddress1.Text;
                employee.Address2 = txtAddress2.Text;
                employee.BirthDate = string.IsNullOrEmpty(txtDOB.Text) ? (DateTime?)null : DateTime.Parse(txtDOB.Text);
                employee.City = txtCity.Text;
                employee.EmergencyContact = txtEmergencyContact.Text;
                employee.EmergencyContactPhone = txtEmergencyContactPhone.Text;
                employee.Salaried = radioPermanent.Checked;
                employee.EmployeeNumber = txtEmployeeNumber.Text;
                employee.EmployeePhone = txtEmployeePhone.Text;
                employee.EmploymentSourceId = Convert.ToInt32(ddlEmploymentSource.SelectedItem.Value);

                if (FacilityId != 0)
                {
                    employee.FacilityId = FacilityId;
                }

                employee.FirstName = txtFirstName.Text;
                employee.HireDate = string.IsNullOrEmpty(txtHireDate.Text) ? (DateTime?)null : DateTime.Parse(txtHireDate.Text);
                employee.InactiveDate = string.IsNullOrEmpty(txtInactiveDate.Text) ? (DateTime?)null : DateTime.Parse(txtInactiveDate.Text);
                if (Session["UserId"] != null)
                {
                    employee.LastModifiedBy = Session["Username"].ToString();
                }
                else
                {
                    employee.LastModifiedBy = GenericHelper.GetLoggedOnUser();
                }

                employee.LastModifiedOn = DateTime.Now;
                employee.LastName = txtLastName.Text;
                employee.MiddleInitial = txtMiddleInitial.Text;
                employee.Salaried = chkBoxSalaried.Checked;
                employee.SSN = txtSSN.Text;
                employee.State = txtState.Text;

                employee.TempEmployee = radioTemporary.Checked;
                if (radioTemporary.Checked)
                {
                    employee.TempNumber = txtTemporaryNumber.Text;
                }
                else
                {
                    employee.TempNumber = null;
                }
                
                employee.TempStartDate = string.IsNullOrEmpty(txtStartDate.Text) ? (DateTime?)null : DateTime.Parse(txtStartDate.Text);
                employee.TerminationDate = string.IsNullOrEmpty(txtTerminationDate.Text) ? (DateTime?)null : DateTime.Parse(txtTerminationDate.Text);
                employee.ZipCode = txtZipCode.Text;

                // Validate employee number
                if (employee.EmployeeId == 0 && radioPermanent.Checked)
                {
                    var validEmployeeNumber = repository.ValidateEmployeeNumber(Convert.ToInt32(employee.EmployeeNumber));

                    if (validEmployeeNumber)
                    {
                        // Save the employee.
                        employeeId = repository.SaveInterrailEmployee(employee).ToString();
                        Session["EmployeeId"] = employeeId;
                        Response.Redirect("~/Admin/EmployeeEdit.aspx?id=" + employeeId, false);
                    }
                    else
                    {
                        // Stop processing and let the user know the number is not unique.
                        lblValidationMessage.Text = employee.EmployeeNumber + " has already been assigned to an employee. Please increment " +
                        employee.EmployeeNumber + " by 1 and try again.";
                        lblValidationMessage.Visible = true;
                    }
                }
                else
                {
                    // Save the employee.
                    employeeId = repository.SaveInterrailEmployee(employee).ToString();
                    Session["EmployeeId"] = employeeId;
                    Response.Redirect("~/Admin/EmployeeEdit.aspx?id=" + employeeId, false);
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                lblValidationMessage.Text = ex.Message;
                lblValidationMessage.Visible = true;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Employee.aspx");
        }

        public bool CheckSecurity(string list)
        {
            if (list.Contains((string)Session["UserType"]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ChangeFacilityLink()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- Start Change Default Facility - Include -->");
            if ((int)Session["Facilities"] > 1)
            {
                sb.AppendLine("     <tr> ");
                sb.AppendLine("        <td width=\"8%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"13%\"><img src=\"../Images/SmallRedArrow.gif\" width=\"10\" height=\"12\"></td>");
                sb.AppendLine("       <td width=\"79%\"><a href=\"../Common/SetDefaultFacility.aspx\">Change Facility</a></td>");
                sb.AppendLine("     </tr>");
                sb.AppendLine("     <tr> ");
                sb.AppendLine("       <td width=\"8%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"13%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"79%\">" + (string)Session["FacilityName"] + "</td>");
                sb.AppendLine("     </tr>");
            }
            else
            {
                sb.AppendLine("     <tr> ");
                sb.AppendLine("        <td width=\"8%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"13%\">&nbsp;</td>");
                sb.AppendLine("       <td width=\"79%\">" + (string)Session["FacilityName"] + "</td>");
                sb.AppendLine("     </tr>");

            }
            sb.AppendLine("<!-- End Change Default Facility - Include -->");
            return sb.ToString();
        }
    }
}