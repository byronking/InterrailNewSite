using InterrailPPRS.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterrailPPRS.Admin
{
    public partial class Facility2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetAllFacilities();
        }

        private void GetAllFacilities()
        {
            var repository = new InterrailFacilityRepository();
            var facilitiesList = repository.GetAllFacilities();

            lblRecordCount.Text = facilitiesList.Count + " records returned";

            grdFacilities.DataSource = facilitiesList;
            grdFacilities.DataBind();
        }

        protected void btnSearchFacilities_Click(object sender, EventArgs e)
        {
            var repository = new InterrailFacilityRepository();
            var facilitiesList = repository.SearchFacilities(txtSearch.Text);

            if (facilitiesList.Count == 0 || facilitiesList.Count > 1)
            {
                lblRecordCount.Text = facilitiesList.Count + " records returned";
            }
            else
            {
                lblRecordCount.Text = facilitiesList.Count + " record returned";
            }

            grdFacilities.DataSource = facilitiesList;
            grdFacilities.DataBind();
        }

        protected void grdFacilities_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdFacilities.PageIndex = e.NewPageIndex;

            if (txtSearch.Text != string.Empty)
            {
                var repository = new InterrailFacilityRepository();
                var facilitiesList = repository.GetAllFacilities();
                grdFacilities.DataSource = facilitiesList;
            }

            grdFacilities.DataBind();
        }

        protected void grdFacilities_Sorting(object sender, GridViewSortEventArgs e)
        {
            var repository = new InterrailFacilityRepository();
            var facilitiesList = repository.GetAllFacilities();
            var sortDirection = e.SortDirection;
            var sortColumn = e.SortExpression;
            var sortedFacilitiesList = new List<InterrailFacility>();

            if (ViewState["SortDirection"] != null)
            {
                sortDirection = (SortDirection)ViewState["SortDirection"];
            }

            switch (sortColumn)
            {
                case "FacilityName":
                    {
                        if (sortDirection == SortDirection.Ascending)
                        {
                            sortedFacilitiesList = facilitiesList.OrderBy(column => column.FacilityName).ToList();
                            ViewState["SortDirection"] = SortDirection.Descending;
                        }
                        else
                        {
                            sortedFacilitiesList = facilitiesList.OrderByDescending(column => column.FacilityName).ToList();
                            ViewState["SortDirection"] = SortDirection.Ascending;
                        }
                    }
                    break;

                case "AlphaCode":
                    {
                        if (e.SortDirection == SortDirection.Ascending)
                        {
                            sortedFacilitiesList = facilitiesList.OrderBy(column => column.AlphaCode).ToList();
                        }
                        else
                        {
                            sortedFacilitiesList = facilitiesList.OrderByDescending(column => column.AlphaCode).ToList();
                        }
                    }
                    break;

                case "FacilityNumber":
                    {
                        if (e.SortDirection == SortDirection.Ascending)
                        {
                            sortedFacilitiesList = facilitiesList.OrderBy(column => column.FacilityNumber).ToList();
                        }
                        else
                        {
                            sortedFacilitiesList = facilitiesList.OrderByDescending(column => column.FacilityNumber).ToList();
                        }
                    }
                    break;
            }

            grdFacilities.DataSource = sortedFacilitiesList;
            grdFacilities.DataBind();
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
    }
}