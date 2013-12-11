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
    public partial class CustomerEdit : PageBase
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            GrantAccess("Super, Admin, User");

            if (!Page.IsPostBack)
            {
                LoadCarrierDropdownList();

                if (Request.QueryString["Id"] != null)
                {
                    var customerId = Convert.ToInt32(Request.QueryString["Id"]);

                    if (customerId != 0)
                    {
                        GetFacilityCustomerById(customerId);                   
                    }
                }  
            }                      
        }

        /// <summary>
        /// This gets the facility customer by id.
        /// </summary>
        /// <param name="customerId"></param>
        private void GetFacilityCustomerById(int customerId)
        {
            var facilityCustomerRepo = new InterrailFacilityCustomerRepository();
            var facilityCustomer = facilityCustomerRepo.GetFacilityCustomerById(customerId);

            ddlCarrierSelect.SelectedValue = facilityCustomer.CustomerCode;
            txtCustomerCode.Text = facilityCustomer.CustomerCode;
            txtCustomerName.Text = facilityCustomer.CustomerName;
            chkDefaultCustomer.Checked = facilityCustomer.DefaultCustomer;
            chkActive.Checked = facilityCustomer.Active;
            txtContactName.Text = facilityCustomer.ContactName;
            txtContactAddress1.Text = facilityCustomer.ContactAddress1;
            txtContactAddress2.Text = facilityCustomer.ContactAddress2;
            txtContactAddress3.Text = facilityCustomer.ContactAddress3;
            lblLastModifiedOn.Text = facilityCustomer.LastModifiedOn.ToShortDateString();
            lblLastModifiedBy.Text = facilityCustomer.LastModifiedBy;
        }

        /// <summary>
        /// This populates the carrier dropdownlist.
        /// </summary>
        private void LoadCarrierDropdownList()
        {
            var carrierRepo = new IRGRailCarrierRepository();
            var carrierList = carrierRepo.GetAllInterrailCarriers();

            ddlCarrierSelect.DataSource = carrierList;
            ddlCarrierSelect.DataTextField = "RailCarrierName";
            ddlCarrierSelect.DataValueField = "RailCarrierCode";
            ddlCarrierSelect.DataBind();
        }

        /// <summary>
        /// This responds to the changing of the carrier dropdownlist. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCarrierSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var carrierRepo = new IRGRailCarrierRepository();
            var carrier = carrierRepo.GetInterrailCarrierByCode(ddlCarrierSelect.SelectedValue);

            txtCustomerCode.Text = carrier.RailCarrierCode;
            txtCustomerName.Text = carrier.RailCarrierName;
        }

        /// <summary>
        /// This inserts a new facility customer or updates an existing one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var userId = Session["Username"].ToString();
            var facilityId = (int)Session["FacilityID"];
            var customerId = Convert.ToInt32(Request.QueryString["Id"]);

            var facilityCustomerRepo = new InterrailFacilityCustomerRepository();

            if (customerId == 0)
            {
                customerId = facilityCustomerRepo.CreateNewFacilityCustomer(facilityId, txtCustomerCode.Text, txtCustomerName.Text, txtContactName.Text, 
                    txtContactAddress1.Text, txtContactAddress2.Text, txtContactAddress3.Text, chkDefaultCustomer.Checked, userId, DateTime.Now, chkActive.Checked);
            }
            else
            {
                customerId = facilityCustomerRepo.UpdateFacilityCustomer(customerId, facilityId, txtCustomerCode.Text, txtCustomerName.Text, txtContactName.Text, 
                    txtContactAddress1.Text, txtContactAddress2.Text, txtContactAddress3.Text, chkDefaultCustomer.Checked, userId, DateTime.Now, chkActive.Checked);
            }

            GetFacilityCustomerById(customerId);
        }
    }
}