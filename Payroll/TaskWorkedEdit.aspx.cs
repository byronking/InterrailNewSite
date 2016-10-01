using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InterrailPPRS.App_Code;

namespace InterrailPPRS.Payroll
{
    public partial class TaskWorkedEdit : System.Web.UI.Page
    {
        public int TaskWorkedId { get; set; }
        public int TaskId { get; set; }
        public string WorkDate { get; set; }
        public int FacilityId { get; set; }
        public int RebillDetailId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateEmployeesDropdown();
            }

            if (Request.QueryString["WorkDate"] != null)
            {
                WorkDate = Request.QueryString["WorkDate"].ToString();
                lblDate.Text = Request.QueryString["WorkDate"].ToString();
            }

            if (Request.QueryString["Shift"] != null)
            {
                lblShift.Text = Request.QueryString["Shift"].ToString();
            }

            if (Session["FacilityId"] != null)
            {
                FacilityId = Convert.ToInt32(Session["FacilityId"]);
            }

            if (Request.QueryString["RebillDetailId"] != null)
            {
                RebillDetailId = Convert.ToInt32(Request.QueryString["RebillDetailId"]);

                // Get the UPM
                PayrollRepository repository = new PayrollRepository();
                var upm = repository.GetUpmForCurrentTask(RebillDetailId, FacilityId);
                txtUpm.Text = upm.ToString();
            }

            if (Request["Type"] != null && Request["Type"].ToString() == "Rebill" && RebillDetailId != 0)
            {
                // Get the rebill detail
                GetRebillDetail(RebillDetailId);
            }
            else
            {
                // Get the tasks list
                GetTasksList(FacilityId);
            }

            if (Request.QueryString["Id"] != null)
            {
                if (Convert.ToInt32(Request.QueryString["Id"]) == 0)
                {
                    lblRecordStatus.Text = "OPEN";
                }
                else
                {
                    TaskWorkedId = Convert.ToInt32(Request.QueryString["Id"]);

                    if (!Page.IsPostBack)
                    {
                        // Get the existing task worked
                        //TaskWorkedId = Convert.ToInt32(Request.QueryString["Id"]);
                        PayrollRepository repository = new PayrollRepository();
                        var taskWorked = repository.GetTaskWorked(TaskWorkedId);

                        lblDate.Text = Convert.ToDateTime(taskWorked.WorkDate).ToShortDateString();
                        lblShift.Text = taskWorked.ShiftId.ToString();
                        ddlEmployees.SelectedValue = taskWorked.EmployeeId.ToString();
                        ddlTasks.SelectedValue = taskWorked.TaskId.ToString();
                        txtHours.Text = taskWorked.HoursWorked.ToString();
                        txtUpm.Text = taskWorked.Upm.ToString();
                        ddlOutOfTown.SelectedValue = taskWorked.OutOfTownType;
                        txtNotes.Text = taskWorked.Notes;
                        lblLastModifiedBy.Text = taskWorked.LastModifiedBy;
                        lblLastModifiedOn.Text = taskWorked.LastModifiedOn;

                        btnDelete.Visible = true;
                        PageBase pageBase = new PageBase();
                        lblRecordStatus.Text = pageBase.getPayRollStatus(WorkDate, FacilityId);
                    }
                }               
            }            
        }

        private void GetRebillDetail(int rebillDetailId)
        {
            RebillRepository repository = new RebillRepository();
            var rebillDetailList = repository.GetRebillDetail(rebillDetailId);
            lblTask.Text = rebillDetailList[0].Description;
            TaskId = rebillDetailList[0].TaskId;
            //OtherTaskId = rebillDetailList[0].O
            lblTaskDesc.Visible = true;
            lblTask.Visible = true;
        }

        private void GetTasksList(int facilityId)
        {
            PayrollRepository repository = new PayrollRepository();
            var taskList = repository.GetFacilityTasks(facilityId);

            foreach (var task in taskList)
            {
                var listItem = new ListItem();
                //listItem.Value = task.TaskId + ", " + task.OtherTaskId;
                listItem.Value = task.TaskId.ToString();
                listItem.Text = task.TaskDescription;
                ddlTasks.Items.Add(listItem);
            }            

            lblTaskDropdown.Visible = true;
            ddlTasks.Visible = true;
        }

        private void PopulateEmployeesDropdown()
        {
            InterrailEmployeeRepository repository = new InterrailEmployeeRepository();
            var employeesForTasksList = repository.GetEmployeesForTasks(FacilityId);

            foreach(var employee in employeesForTasksList)
            {
                var listItem = new ListItem();
                if (employee.EmployeeNumber != null)
                {
                    listItem.Text = employee.Lastname + ", " + employee.FirstName + "(" + employee.EmployeeNumber + ")";
                    listItem.Value = employee.EmployeeId.ToString();
                }
                else
                {
                    listItem.Text = employee.Lastname + ", " + employee.FirstName + "(" + employee.TempNumber + ")";
                    listItem.Value = employee.EmployeeId.ToString();
                }

                ddlEmployees.Items.Add(listItem);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            PayrollRepository repository = new PayrollRepository();

            var taskId = ddlTasks.SelectedValue;
            if (taskId == string.Empty)
            {
                taskId = TaskId.ToString();
            }

            var taskWorked = new InterrailTaskWorked()
            {
                TaskWorkedId = TaskWorkedId,
                TaskId = Convert.ToInt32(taskId),
                OtherTaskId = 0,
                FacilityId = FacilityId,
                EmployeeId = Convert.ToInt32(ddlEmployees.SelectedValue),
                RebillDetailId = RebillDetailId,
                WorkDate = lblDate.Text,
                ShiftId = Convert.ToInt32(lblShift.Text),
                Upm = Convert.ToDouble(txtUpm.Text),
                HoursWorked = Convert.ToDouble(txtHours.Text),
                PayrollStatus = lblRecordStatus.Text,
                OutOfTownType = ddlOutOfTown.SelectedValue,
                LastModifiedBy = Session["Username"].ToString(),
                LastModifiedOn = DateTime.Now.ToString(),
                Notes = txtNotes.Text
            };

            repository.SaveTaskWorked(taskWorked);
            PageBase pageBase = new PageBase();
            pageBase.UpdateUPM(taskWorked.WorkDate, taskWorked.TaskId, taskWorked.ShiftId.ToString(), FacilityId, taskWorked.RebillDetailId, taskWorked.LastModifiedBy, taskWorked.LastModifiedOn);

            //while (pageBaseReader.Read())
            //{
            //    txtUpm.Text = pageBaseReader["UPM"].ToString();
            //}

            Response.Redirect("payroll.aspx?workdate=" + Request["WorkDate"] + "&shift=" + Request["Shift"]); 
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null)
            {
                var taskWorkedId = Request.QueryString["Id"].ToString();
                PayrollRepository repository = new PayrollRepository();
                repository.DeleteTaskWorked(Convert.ToInt32(taskWorkedId));
            }

            Response.Redirect("payroll.aspx?workdate=" + Request["WorkDate"] + "&shift=" + Request["Shift"]); 
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("payroll.aspx?workdate=" + Request["WorkDate"] + "&shift=" + Request["Shift"]); 
        }
    }
}