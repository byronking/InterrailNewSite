using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterrailPPRS.Rebilling
{
    public partial class Default2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void calStartDate_SelectionChanged(object sender, EventArgs e)
        {
            var startDate = calStartDate.SelectedDate;
            txtEndDate.Text = startDate.AddDays(6).ToShortDateString();

            lblDateRange.Text = startDate.ToShortDateString() + " to " + txtEndDate.Text;
        }
    }
}