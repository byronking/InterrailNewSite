using InterrailPPRS.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterrailPPRS.Rebilling
{
    public partial class ApproveRebillData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GrantAccess("Super, Admin, User");

            if (!Page.IsPostBack)
            {
                int facilityId;
                var rebillDataList = new List<RebillData>();

                if (Session["FacilityID"] != null && Session["FacilityID"] != "")
                {
                    facilityId = System.Convert.ToInt32(Session["FacilityID"]);

                    var repository = new RebillRepository();
                    rebillDataList = repository.GetRebillDataForApproval(facilityId);
                }

                if (Session["FacilityName"] != null)
                {
                    lblFacilityName.Text = Session["FacilityName"].ToString();
                }

                grdRebillDataForApproval.DataSource = rebillDataList;
                grdRebillDataForApproval.DataBind();
            }

            if (Session["SaveSuccessful"] != null)
            {
                if (Convert.ToBoolean(Session["SaveSuccessful"]) == true)
                {
                    lblSaveMessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    if (Session["ApproveRebillDataErrorMessage"] != null)
                    {
                        var errorMessage = Session["ApproveRebillDataErrorMessage"].ToString();
                        lblSaveMessage.ForeColor = System.Drawing.Color.Red;
                        lblSaveMessage.Text = "There was a problem approving the records! " + errorMessage;
                    }                    
                }

                lblSaveMessage.Visible = true;
            }

            // Clean-up
            Session["SaveSuccessful"] = null;
        }

        protected void grdRebillDataForApproval_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdRebillDataForApproval.PageIndex = e.NewPageIndex;

            //if (txtSearch.Text != string.Empty)
            //{
            //    var repository = new InterrailFacilityRepository();
            //    var facilitiesList = repository.GetAllFacilities();
            //    grdFacilities.DataSource = facilitiesList;
            //}

            grdRebillDataForApproval.DataBind();
        }

        protected void grdRebillDataForApproval_Sorting(object sender, GridViewSortEventArgs e)
        {
            int facilityId;
            var rebillDataList = new List<RebillData>();

            if (Session["FacilityID"] != null && Session["FacilityID"] != "")
            {
                facilityId = System.Convert.ToInt32(Session["FacilityID"]);

                var repository = new RebillRepository();
                rebillDataList = repository.GetRebillDataForApproval(facilityId);
            }

            var sortDirection = e.SortDirection;
            var sortColumn = e.SortExpression;
            var sortedRebillDataList = new List<RebillData>();

            if (ViewState["SortDirection"] != null)
            {
                sortDirection = (SortDirection)ViewState["SortDirection"];
            }

            switch (sortColumn)
            {
                case "WorkDate":
                    {
                        if (sortDirection == SortDirection.Ascending)
                        {
                            sortedRebillDataList = rebillDataList.OrderBy(column => column.WorkDate).ToList();
                            ViewState["SortDirection"] = SortDirection.Descending;
                        }
                        else
                        {
                            sortedRebillDataList = rebillDataList.OrderByDescending(column => column.WorkDate).ToList();
                            ViewState["SortDirection"] = SortDirection.Ascending;
                        }
                    }
                    break;

                case "CustomerName":
                    {
                        if (e.SortDirection == SortDirection.Ascending)
                        {
                            sortedRebillDataList = rebillDataList.OrderBy(column => column.CustomerName).ToList();
                        }
                        else
                        {
                            sortedRebillDataList = rebillDataList.OrderByDescending(column => column.CustomerName).ToList();
                        }
                    }
                    break;
            }

            grdRebillDataForApproval.DataSource = sortedRebillDataList;
            grdRebillDataForApproval.DataBind();
        }

        private void SortGridView(string sortExpression, SortDirection sortDirection)
        {
            var repository = new InterrailFacilityRepository();
            var facilitiesList = repository.GetAllFacilities();
            var sortedFacilitiesList = new List<InterrailFacility>();

            if (sortDirection == SortDirection.Ascending)
            {
                sortedFacilitiesList = facilitiesList.OrderBy(column => column.FacilityName).ToList();
            }
            else
            {
                sortedFacilitiesList = facilitiesList.OrderByDescending(column => column.FacilityName).ToList();
            }
        }

        protected void btnApproveChecked_Click(object sender, EventArgs e)
        {
            var rebillingIdList = new List<int>();

            foreach (GridViewRow row in grdRebillDataForApproval.Rows)
            {
                CheckBox chkSelectWorkDate = (row.Cells[0].FindControl("chkSelectWorkDate") as CheckBox);
                Label lblSelectWorkDate = (row.Cells[0].FindControl("lblSelectWorkDate") as Label);
                if (chkSelectWorkDate != null)
                {
                    if (chkSelectWorkDate.Checked)
                    {
                        rebillingIdList.Add(Convert.ToInt32(lblSelectWorkDate.Text));
                    }
                }
            }

            var userType = string.Empty;
            var rebillStatus = string.Empty;
            var userName = string.Empty;

            if (Session["UserName"] != null)
            {
                userName = Session["UserName"].ToString();
            }

            if (Session["UserType"] != null)
            {
                userType = Session["UserType"].ToString();

                if (userType == "USER")
                {
                    rebillStatus = "FACILITY";
                }
                else
                {
                    rebillStatus = "CORPORATE";
                }
            }

            var repository = new RebillRepository();
            var saveResult = repository.ApproveRebillData(rebillingIdList, rebillStatus, userName);

            if (saveResult.SaveSuccessful)
            {                
                Session["SaveSuccessful"] = true;
            }
            else
            {
                Session["SaveSuccessful"] = false;
                Session["ApproveRebillDataErrorMessage"] = saveResult.SaveMessage;
            }

            Response.Redirect("ApproveRebillData.aspx");
        }

        protected void btnApproveDateRange_Click(object sender, EventArgs e)
        {
            var startDate = Convert.ToDateTime(txtStartDate.Text);
            var endDate = Convert.ToDateTime(txtEndDate.Text);
            var userType = string.Empty;
            var rebillStatus = string.Empty;
            var userName = string.Empty;
            var facilityId = 0;

            if (Session["UserName"] != null)
            {
                userName = Session["UserName"].ToString();
            }

            if (Session["UserType"] != null)
            {
                userType = Session["UserType"].ToString();

                if (userType == "USER")
                {
                    rebillStatus = "FACILITY";
                }
                else
                {
                    rebillStatus = "CORPORATE";
                }
            }

            if (Session["FacilityId"] != null)
            {
                facilityId = Convert.ToInt32(Session["FacilityId"]);
            }

            var repository = new RebillRepository();
            var saveResult = repository.ApproveRebillData(startDate, endDate, facilityId, rebillStatus, userName);

            if (saveResult.SaveSuccessful)
            {
                Session["SaveSuccessful"] = true;
            }
            else
            {
                Session["SaveSuccessful"] = false;
                Session["ApproveRebillDataErrorMessage"] = saveResult.SaveMessage;
            }

            Response.Redirect("ApproveRebillData.aspx");
        } 

        public void GrantAccess(string AccessType)
        {
            string currentapppath = "";
            string authorizedUsers = "";
            string authFailedURL = "";
            bool grantAccess = false;
            string WhyNoAccess = "";
            string qsChar = "";
            string referrer = "";

            if (Page.Request.ServerVariables["APPL_MD_PATH"].ToString().Length > 0)
            {
                currentapppath = Page.Request.ServerVariables["APPL_MD_PATH"].ToString().Substring(Request.ServerVariables["APPL_MD_PATH"].ToString().LastIndexOf("/"));
            }

            if (currentapppath.Length == 0 || currentapppath == "/Root")
            {
                currentapppath = "/";
            }
            else
            {
                currentapppath = currentapppath + "/";
            }

            authorizedUsers = "Super";
            authFailedURL = currentapppath + "Login.aspx";
            grantAccess = false;

            if (Session["UserType"] != null && Session["UserType"].ToString() != "")
            {
                if (AccessType.IndexOf((string)Session["UserType"]) >= 1)
                {
                    grantAccess = true;
                }
                else
                {
                    grantAccess = false;
                    authFailedURL = currentapppath + "AccessDenied.aspx";
                }
            }

            if (!grantAccess)
            {
                qsChar = "?";
                if (authFailedURL.IndexOf("?") >= 1)
                {
                    qsChar = "&";
                    referrer = Request.ServerVariables["URL"];
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        referrer = referrer + "?" + Request.QueryString.ToString();
                        if (Session["UserType"].ToString() != "")
                        {
                            WhyNoAccess = "why=noaccess&";
                        }
                        else
                        {
                            WhyNoAccess = "";
                        }
                        authFailedURL = authFailedURL + qsChar + WhyNoAccess + "accessdenied=" + Server.UrlEncode(referrer);
                        Response.Redirect(authFailedURL);
                    }

                }
            }

        }       
    }
}