using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterrailPPRS.Rebilling
{
    public partial class Rebilling2 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyName"] != null)
            {
                lblCompanyName.Text = Session["CompanyName"].ToString();
            }
        }
    }
}