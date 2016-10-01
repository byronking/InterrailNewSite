using InterrailPPRS.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterrailPPRS.Rebilling
{
    public partial class RebillDetail2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var facilityId = 0;

            if (Session["FacilityID"] != null)
            {
                facilityId = Convert.ToInt32(Session["FacilityID"]);
            }

            var repository = new RebillRepository();
            var rebillDetailList = repository.GetRebillDetailByFacilityId(facilityId);

            grdRebillData.DataSource = rebillDetailList;
            grdRebillData.DataBind();
        }

        protected void grdRebillData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdRebillData.PageIndex = e.NewPageIndex;
            grdRebillData.DataBind();
        }
    }
}