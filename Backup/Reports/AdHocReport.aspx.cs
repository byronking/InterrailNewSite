using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace InterrailPPRS.Reports
{
	/// <summary>
	/// Summary description for WebForm2.
	/// </summary>
	public partial class WebForm2 : System.Web.UI.Page
	{

		const int defaultTimeout = 300;
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
            if (!IsPostBack)
            {
                ViewState["SortItem"] = "";
                ViewState["SortDir"] = "ASC";
                BindData();
            }
		}
     
        private DataSet ds;
        private string connstring
        {
            get
            {
                return ConfigurationSettings.AppSettings["MM_Main_STRING"];
            }
        }

        private void CreateColumns()
        {
            string strSQL = Request.QueryString["SQL"];
            
			
            SqlConnection myConnection = null;
			try
			{
				myConnection = new SqlConnection(connstring);
				myConnection.Open();
            
				strSQL = strSQL.Replace("SELECT", "SELECT TOP 1");

				SqlDataAdapter schemaDA = new SqlDataAdapter(strSQL, myConnection);
				schemaDA.SelectCommand.CommandTimeout = defaultTimeout;

				ds = new DataSet();
				schemaDA.Fill(ds, "Data");

				dgMain.Columns.Clear();

				foreach(DataColumn d in   ds.Tables["Data"].Columns)
				{
					BoundColumn dc = new BoundColumn();
					dc.DataField = d.ColumnName;
					dc.HeaderText = d.ColumnName;
					dc.SortExpression = d.ColumnName;
               
					if (d.DataType == System.Type.GetType("System.DateTime"))
					{
						dc.DataFormatString = "{0:MM/dd/yy}"  ;
					}
					dgMain.Columns.Add(dc);
				}
			}
			finally
			{
				if (myConnection != null)
					myConnection.Close();
			}
        }

        private void BindData()
        {
            //1. Create a connection

            string strSQL = Request.QueryString["SQL"];
            if ((string)ViewState["SortItem"] != "")
            {
                strSQL +=  "ORDER BY " + ViewState["SortItem"] + " " + ViewState["SortDir"];
            }
            else
            {
                strSQL = strSQL +  Request.QueryString["ORDER"];
            }
            
            SqlConnection myConnection = null;
			try
			{
				myConnection = new SqlConnection(connstring);
				myConnection.Open();
      
				SqlDataAdapter schemaDA = new SqlDataAdapter(strSQL, myConnection);
				schemaDA.SelectCommand.CommandTimeout = defaultTimeout;

				ds = new DataSet();
				schemaDA.Fill(ds, "Data");

				dgMain.DataSource = ds.Tables["Data"];
				dgMain.DataBind();   
			}
			finally
			{
				if (myConnection != null)
					myConnection.Close();
			}

        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
            CreateColumns();
			InitializeComponent();

			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
            this.dgMain.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgMain_PageIndexChanged);
            this.dgMain.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.dgMain_SortCommand);
            this.dgMain.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgMain_ItemDataBound);

        }
		#endregion

        private void dgMain_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            dgMain.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        private void dgMain_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            string ob = e.SortExpression;
            ViewState["SortItem"] = "["+ob+"]";
            if ((string)ViewState["SortDir"] != "ASC")
            {
                ViewState["SortDir"] = "ASC";
            }
            else
            {
                ViewState["SortDir"] = "DESC";
            }
            BindData();
        }


        protected void btnShowAll_Click(object sender, System.EventArgs e)
        {
            dgMain.AllowPaging = ! dgMain.AllowPaging;
            if (!dgMain.AllowPaging)
            {
                btnShowAll.Text = "Show Pages";
            }
            else
            {
                btnShowAll.Text = "Show All";
            }
            BindData();
        }

        private void dgMain_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
        
        }
	}
}
