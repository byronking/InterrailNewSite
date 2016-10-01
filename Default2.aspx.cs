using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterrailPPRS
{
    public partial class Default2 : System.Web.UI.Page
    {
        public bool isDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }

        public int PayPeriodEnd { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //var startDate = "5/2/2014";
            //getPayPeriods3(0, 2, startDate);

            var todayName = DateTime.Now.DayOfWeek;
            var lastNearestFridayDate = GetLastNearestFriday(todayName);
            var nextNearestThursday = lastNearestFridayDate.AddDays(6);

            var payPeriodStart = lastNearestFridayDate;
            var payPeriodEnd = nextNearestThursday;

            getPayPeriods2(0, 12, payPeriodStart, payPeriodEnd);
        }

        public void getPayPeriods2(int nStart, int nPeriods, DateTime payPeriodStart, DateTime payPeriodEnd)
        {
            for (int i = nStart; i < nPeriods; i++)
            {
                var selectStartDate = payPeriodStart.AddDays(-7 * i);
                var selectEndDate = payPeriodEnd.AddDays(-7 * i);
                //payPeriodStart = payPeriodStart.AddDays(-7 * i);
                //payPeriodEnd = payPeriodEnd.AddDays(-7 * i);

                var period = new ListItem();
                period.Text = selectStartDate.ToShortDateString() + " - " + selectEndDate.ToShortDateString();
                period.Value = selectStartDate.ToShortDateString() + "," + selectEndDate.ToShortDateString();

                ddlPayPeriods.Items.Add(period);
                //strHTML = strHTML + @"<option value=" + payPeriodStart + "," + payPeriodEnd + ">" + payPeriodStart + " - " + payPeriodEnd + "</option>";
            }
        }

        private DateTime GetLastNearestFriday(DayOfWeek day)
        {
            // What the fridayDate is set to initially is irrelevant.
            DateTime fridayDate = DateTime.Now;

            switch (day)
            {
                case DayOfWeek.Sunday:
                    {
                        fridayDate = DateTime.Now.AddDays(-2);
                        break;
                    }
                case DayOfWeek.Monday:
                    {
                        fridayDate = DateTime.Now.AddDays(-3);
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        fridayDate = DateTime.Now.AddDays(-4);
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        fridayDate = DateTime.Now.AddDays(-5);
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        fridayDate = DateTime.Now.AddDays(-6);
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        fridayDate = DateTime.Now.AddDays(-7);
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        fridayDate = DateTime.Now.AddDays(-8);
                        break;
                    }
            }

            return fridayDate;
        }
    }
}