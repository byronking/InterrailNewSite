using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using Interrial.PPRS.Dal.TypedListClasses;
using Interrial.PPRS.Dal.EntityClasses;
using Interrial.PPRS.Dal.FactoryClasses;
using Interrial.PPRS.Dal.CollectionClasses;
using Interrial.PPRS.Dal.HelperClasses;
using Interrial.PPRS.Dal;

using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;

namespace InterrailPPRS
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

     
        }

        private void Authenticate()
        {
            //IPredicateExpression filterOr;
            IPredicateExpression filter;
            ISortExpression sorter;

            string LoginAction = Page.Request.ServerVariables["URL"];
            string valUsername = "";


            string fldUserAuthorization = "UserType";
            string redirectLoginSuccess = "";
            string redirectLoginFailed = "";
            string currentapppath = System.Configuration.ConfigurationManager.AppSettings["ROOT_PATH"];

            DbUtils.ActualConnectionString = System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"];

            currentapppath = currentapppath + System.Configuration.ConfigurationManager.AppSettings["root"];
            if (Request.QueryString.Count != 0) { LoginAction = LoginAction + "?" + Request.QueryString; }

            valUsername = (string)(Request.Form["edtUserName"]);
            

            if (valUsername != "")
            {

                fldUserAuthorization = "UserType";
                redirectLoginSuccess = currentapppath + "default.aspx";
                redirectLoginFailed = currentapppath + "login.aspx";

                filter = new PredicateExpression();
                filter.Add(UserProfileFields.UserId == valUsername.Replace("'", "''"));
                filter.AddWithAnd(UserProfileFields.Password == ((string)Page.Request.Form["edtPassword"]).Replace("'", "''"));
                sorter = new SortExpression();
                sorter.Add(IrgcompanyFields.CompanyName | SortOperator.Ascending);

                UserProfileTypedList up = new UserProfileTypedList();
                up.Fill(0, null, false, filter);

                if (up.Rows.Count > 0)
                {

                    // username and password match - this is a valid user
                    System.Web.Security.FormsAuthentication.SetAuthCookie(valUsername, false);

                    UserProfileRow upr = (UserProfileRow)up.Rows[0];

                    Session["Username"] = valUsername;
                    Session["dbPath"] = ConfigurationManager.AppSettings["MM_Main_STRING"];
                    Session["dbMode"] = "Production";

                    

                    if (fldUserAuthorization != "")
                    {
                        Session["UserAuthorization"] = upr.UserType.Trim();
                        Session["UserType"] = upr.UserType.Trim();
                        Session["UserID"] = upr.Id;
                    }
                    else
                    {
                        Session["UserAuthorization"] = "";
                        Session["UserType"] = "";
                        Session["UserID"] = "";
                    }

                    if (Request.QueryString["accessdenied"] != null && true)
                    {
                        redirectLoginSuccess = Request.QueryString["accessdenied"];
                    }

                    if (Session["UserType"].ToString() == "User")
                    {

                        // Get Company Name
                        if (upr.CompanyName != null)
                        {
                            Session["CompanyName"] = upr.CompanyName.Trim();
                        }
                        else
                        {
                            Session["CompanyName"] = "Inter-Rail Transport, Inc.";
                        }

                    }
                    else
                    {
                        Session["CompanyName"] = "Inter-Rail Transport, Inc.";
                    }


                    // Get User's First Active Facility (Set as Default)
                    if (Session["UserType"].ToString() == "User" || Session["UserType"].ToString() == "Production")
                    {

                        filter = new PredicateExpression();
                        filter.Add(UserProfileFields.Id == Session["UserID"]);
                        filter.AddWithAnd(FacilityFields.Active == 1);
                        sorter = new SortExpression();
                        sorter.Add(FacilityFields.FacilityNumber | SortOperator.Ascending);

                        FacilityTypedList fac = new FacilityTypedList();
                        fac.Fill(0, null, false, filter);

                        if (fac.Rows.Count > 0)
                        {

                            FacilityRow fr = (FacilityRow)fac.Rows[0];
                            Session["FacilityID"] = fr.FacilityId;
                            Session["FacilityName"] = fr.Name;
                            Session["Facilities"] = fac.Rows.Count;

                        }
                        else
                        {

                            string RedirectLoginFailed = currentapppath + "login.aspx?why=nofacility";
                            Page.Response.Redirect("http://" + Request.ServerVariables["HTTP_HOST"] + RedirectLoginFailed);

                        }

                    }
                    else
                    {

                        filter = new PredicateExpression();
                        filter.Add(FacilityFields.Active == 1);
                        sorter = new SortExpression();
                        sorter.Add(FacilityFields.FacilityNumber | SortOperator.Ascending);

                        FacilityTypedList fac = new FacilityTypedList();
                        fac.Fill(0, null, false, filter);

                        if (fac.Rows.Count > 0)
                        {
                            FacilityRow fr = (FacilityRow)fac.Rows[0];
                            Session["FacilityID"] = fr.Id;
                            Session["FacilityName"] = fr.Name;
                            Session["Facilities"] = fac.Rows.Count;
                        }
                        else
                        {
                            Session["FacilityID"] = 0;
                            Session["FacilityName"] = "N/A";
                            Session["Facilities"] = 0;
                        }
                    }

                    Page.Response.Redirect("http://" + Request.ServerVariables["HTTP_HOST"] + redirectLoginSuccess);

                }

                Page.Response.Redirect("http://" + Request.ServerVariables["HTTP_HOST"] + redirectLoginFailed);
            }
        }

        protected void Logon_Click(object sender, EventArgs e)
        {
            Authenticate();
        }
    }
}