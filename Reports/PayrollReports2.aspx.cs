using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InterrailPPRS.Reports
{
    public partial class PayrollReports2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GrantAccess("Super, Admin, User");


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