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
    public partial class Company : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            GrantAccess("Super, Admin, User");

            GetAllCompanies();
        }

        /// <summary>
        /// This returns all of the Interrail users
        /// </summary>
        private void GetAllCompanies()
        {
            var companyRepo = new InterrailCompanyRepository();
            var companiesList = companyRepo.GetAllInterrailCompanies();

            lblRecordCount.Text = companiesList.Count + " records returned";

            grdAllCompanies.DataSource = companiesList;
            grdAllCompanies.DataBind();
        }

        /// <summary>
        /// This responds to the page changing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAllCompanies_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            grdAllCompanies.PageIndex = e.NewPageIndex;

            if (txtSearch.Text != string.Empty)
            {
                var companyRepo = new InterrailCompanyRepository();
                var userList = companyRepo.SearchInterrailCompanies(txtSearch.Text);
                grdAllCompanies.DataSource = userList;
            }

            grdAllCompanies.DataBind();
        }

        /// <summary>
        /// This responds to the click of the search button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var userRepo = new InterrailCompanyRepository();
            var userList = userRepo.SearchInterrailCompanies(txtSearch.Text);

            if (userList.Count == 0 || userList.Count > 1)
            {
                lblRecordCount.Text = userList.Count + " records returned";
            }
            else
            {
                lblRecordCount.Text = userList.Count + " record returned";
            }

            grdAllCompanies.DataSource = userList;
            grdAllCompanies.DataBind();
        }
    }
}