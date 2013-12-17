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
    public partial class Customer : PageBase
    {
        public int FacilityId { get; set; }
        protected override void Page_Load(object sender, EventArgs e)
        {
            GrantAccess("Super, Admin, User");

            GetAllFacilityCustomers();
        }

        /// <summary>
        /// This returns all of the Interrail users
        /// </summary>
        private void GetAllFacilityCustomers()
        {
            if (Session["FacilityId"] != null)
            {
                FacilityId = Convert.ToInt32(Session["FacilityId"]);
            }

            var facilityCustomerRepo = new InterrailFacilityCustomerRepository();
            var companiesList = facilityCustomerRepo.GetFacilityCustomersByFacilityId(FacilityId);

            lblRecordCount.Text = companiesList.Count + " records returned";

            grdAllFacilityCustomer.DataSource = companiesList;
            grdAllFacilityCustomer.DataBind();
        }

        /// <summary>
        /// This responds to the page changing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAllFacilityCustomer_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            grdAllFacilityCustomer.PageIndex = e.NewPageIndex;

            if (txtSearch.Text != string.Empty)
            {
                var facilityCustomerRepo = new InterrailFacilityCustomerRepository();
                var facilityCustomerList = facilityCustomerRepo.SearchInterrailFacilityCustomers(txtSearch.Text, FacilityId);
                grdAllFacilityCustomer.DataSource = facilityCustomerList;
            }

            grdAllFacilityCustomer.DataBind();
        }

        /// <summary>
        /// This responds to the click of the search button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (Session["FacilityId"] != null)
            {
                FacilityId = Convert.ToInt32(Session["FacilityId"]);
            }

            var facilityCustomerRepo = new InterrailFacilityCustomerRepository();
            var facilityCustomerList = facilityCustomerRepo.SearchInterrailFacilityCustomers(txtSearch.Text, FacilityId);

            if (facilityCustomerList.Count == 0 || facilityCustomerList.Count > 1)
            {
                lblRecordCount.Text = facilityCustomerList.Count + " records returned";
            }
            else
            {
                lblRecordCount.Text = facilityCustomerList.Count + " record returned";
            }

            grdAllFacilityCustomer.DataSource = facilityCustomerList;
            grdAllFacilityCustomer.DataBind();
        }
    }
}