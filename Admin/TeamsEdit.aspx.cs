using InterrailPPRS.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterrailPPRS.Admin
{
    public partial class TeamsEdit : System.Web.UI.Page
    {
        public int FacilityId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string UserName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FacilityName"] != null)
            {
                lblFacility.Text = Session["FacilityName"].ToString();
            }

            if (Session["FacilityId"] != null)
            {
                FacilityId = Convert.ToInt32(Session["FacilityId"]);
            }

            if (Request.QueryString["Id"] != null)
            {
                TeamId = Convert.ToInt32(Request.QueryString["Id"]);
            }

            if (Session["Username"] != null)
            {
                UserName = Session["Username"].ToString();
            }

            if (!Page.IsPostBack)
            {
                LoadEmployees();                

                if (TeamId != 0)
                {
                    LoadTeamInfo(TeamId);
                }
            }
        }

        /// <summary>
        /// This populates the Employees listbox.
        /// </summary>
        private void LoadEmployees()
        {
            var employeesList = new List<EmployeesForLists>();

            var repository = new EmployeesRepository();
            employeesList = repository.GetEmployeesForLists(FacilityId);

            employeesList = employeesList.OrderBy(x => x.LastName).ToList();

            foreach (var emp in employeesList)
            {
                var listItem = new ListItem();
                listItem.Text = emp.LastName + ", " + emp.FirstName + " (" + emp.EmployeeNumber + ")";
                listItem.Value = emp.Id.ToString();
                listEmployees.Items.Add(listItem);
            }
        }

        /// <summary>
        /// This populates the team members list box and the team info fields.
        /// </summary>
        /// <param name="teamId"></param>
        private void LoadTeamInfo(int teamId)
        {
            var teamList = new List<InterrailTeam>();
            var teamMembersList = new List<EmployeesForLists>();

            var repository = new TeamsRepository();
            teamList = repository.GetTeamById(teamId);

            var empRepository = new EmployeesRepository();            

            if (teamList[0].TeamMembers != string.Empty)
            {
                var teamMembers = teamList[0].TeamMembers.Split(',');

                foreach (var employeeNumber in teamMembers)
                {
                    var emp = empRepository.GetEmployeesById(Convert.ToInt32(employeeNumber));
                    teamMembersList.Add(emp);
                }

                teamMembersList = teamMembersList.OrderBy(x => x.LastName).ToList();

                foreach (var emp in teamMembersList)
                {
                    var listItem = new ListItem();
                    listItem.Text = emp.LastName + ", " + emp.FirstName + " (" + emp.EmployeeNumber + ")";
                    listItem.Value = emp.Id.ToString();

                    if (!listTeamMembers.Items.Contains(listItem))
                    {
                        listTeamMembers.Items.Add(listItem);
                    }
                }
            }

            TeamName = teamList[0].TeamName.Trim();
            txtTeamName.Text = TeamName;
            chkActive.Checked = teamList[0].Active;
            lblLastModifiedOn.Text = teamList[0].LastModifiedOn.ToString();
            lblLastModifiedBy.Text = teamList[0].LastModifiedBy.Trim();
        }

        /// <summary>
        /// This adds members to a team.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddMembers_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in listEmployees.Items)
            {
                if (item.Selected && !listTeamMembers.Items.Contains(item))
                {
                    listTeamMembers.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// This remove members from a team.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveMembers_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listTeamMembers.Items.Count; i++)
            {
                if (listTeamMembers.Items[i].Selected)
                {
                    listTeamMembers.Items.Remove(listTeamMembers.SelectedItem);
                }
            } 
        }

        /// <summary>
        /// This saves the team info.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string[] listBoxItems = new string[listTeamMembers.Items.Count];

            for (int i = 0; i < listTeamMembers.Items.Count; i++)
            {
                var value = listTeamMembers.Items[i].Value;
                listBoxItems[i] = value;
            }

            var membersList = string.Empty;

            if (listTeamMembers.Items.Count > 0)
            {
                membersList = String.Join(",", listBoxItems);
            }

            var team = new InterrailTeam()
            {
                Id = TeamId,
                TeamName = txtTeamName.Text,
                FacilityId = FacilityId,
                TeamMembers = membersList,
                LastModifiedBy = UserName,
                LastModifiedOn = DateTime.Now,
                Active = chkActive.Checked
            };

            var repository = new TeamsRepository();

            if (team.Id != 0)
            {
                repository.UpdateExistingTeam(team);
                LoadTeamInfo(team.Id);
            }
            else
            {
                var return_value = repository.SaveNewTeam(team);
                LoadTeamInfo(return_value);
            }            
        }

        /// <summary>
        /// This redirects to the Teams page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Teams.aspx?ID=" + TeamId);
        }
    }
}