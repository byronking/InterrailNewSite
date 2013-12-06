using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

using Interrial.PPRS.Dal.TypedListClasses;
using Interrial.PPRS.Dal.EntityClasses;
using Interrial.PPRS.Dal.FactoryClasses;
using Interrial.PPRS.Dal.CollectionClasses;
using Interrial.PPRS.Dal.HelperClasses;
using Interrial.PPRS.Dal;

using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;

using InterrailPPRS.App_Code;

namespace InterrailPPRS.Admin
{
    public partial class User : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            GrantAccess("Super, Admin");

            GetAllUsers();
        }

        /// <summary>
        /// This returns all of the Interrail users
        /// </summary>
        private void GetAllUsers()
        {
            var userRepo = new InterrailUserRepository();
            var userList = userRepo.GetAllInterrailUsers();

            lblRecordCount.Text = userList.Count + " records returned";

            grdAllUsers.DataSource = userList;
            grdAllUsers.DataBind();
        }

        protected void grdAllUsers_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            grdAllUsers.PageIndex = e.NewPageIndex;

            if (txtSearch.Text != string.Empty)
            {
                var userRepo = new InterrailUserRepository();
                var userList = userRepo.SearchInterrailUsers(txtSearch.Text);
                grdAllUsers.DataSource = userList;                
            }            

            grdAllUsers.DataBind();
        }

        #region Sorting code
        protected void grdAllUsers_Sorting(Object sender, GridViewSortEventArgs e)
        {
            var userRepo = new InterrailUserRepository();
            var userList = userRepo.GetAllInterrailUsers();
            var sortDirection = e.SortDirection;
            var sortColumn = e.SortExpression;
            var sortedUserList = new List<InterrailUser>();            

            if (ViewState["SortDirection"] != null)
            {
                sortDirection = (SortDirection)ViewState["SortDirection"];
            }

            switch (sortColumn)
            {
                case "UserId":
                    {
                        if (sortDirection == SortDirection.Ascending)
                        {
                            sortedUserList = userList.OrderBy(column => column.UserId).ToList();
                            ViewState["SortDirection"] = SortDirection.Descending;
                        }
                        else
                        {
                            sortedUserList = userList.OrderByDescending(column => column.UserId).ToList();
                            ViewState["SortDirection"] = SortDirection.Ascending;
                        }
                    }
                    break;

                case "UserLongName":
                    {
                        if (e.SortDirection == SortDirection.Ascending)
                        {
                            sortedUserList = userList.OrderBy(column => column.UserLongName).ToList();
                        }
                        else
                        {
                            sortedUserList = userList.OrderByDescending(column => column.UserLongName).ToList();
                        }
                    }
                    break;

                case "UserName":
                    {
                        if (e.SortDirection == SortDirection.Ascending)
                        {
                            sortedUserList = userList.OrderBy(column => column.UserName).ToList();
                        }
                        else
                        {
                            sortedUserList = userList.OrderByDescending(column => column.UserName).ToList();
                        }
                    }
                    break;
            }

            grdAllUsers.DataSource = sortedUserList;
            grdAllUsers.DataBind();
        }

        private void SortGridView(string sortExpression, SortDirection sortDirection)
        {
            var userRepo = new InterrailUserRepository();
            var userList = userRepo.GetAllInterrailUsers();
            var sortedUserList = new List<InterrailUser>();

            if (sortDirection == SortDirection.Ascending)
            {
                sortedUserList = userList.OrderBy(column => column.UserName).ToList();
            }
            else
            {
                sortedUserList = userList.OrderByDescending(column => column.UserName).ToList();
            }
        }
        #endregion

        /// <summary>
        /// This responds to the click of the search button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var userRepo = new InterrailUserRepository();
            var userList = userRepo.SearchInterrailUsers(txtSearch.Text);

            if (userList.Count == 0 || userList.Count > 1)
            {
                lblRecordCount.Text = userList.Count + " records returned";
            }
            else
            {
                lblRecordCount.Text = userList.Count + " record returned";
            }

            grdAllUsers.DataSource = userList;
            grdAllUsers.DataBind();
        }
    }
}