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
using System.IO;


namespace InterrailPPRS.Rebilling
{
	/// <summary>
	/// Summary description for ManageAttachments.
	/// </summary>
	public partial class ManageAttachments : System.Web.UI.Page
	{
	
		private string RebillID;
		private string ConnectionString;
		private DataSet ds;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if (!IsPostBack)
			{
				lbCompanyName.Text =   Page.Request.Form["CompanyName"];
				ConnectionString =  Page.Request.Form["connectionstring"];
				RebillID = Page.Request.Form["RebillID"];
				hlBack.NavigateUrl = "RebillEdit.aspx?ID=" + RebillID;
				ViewState["RebillID"] = RebillID;
				ViewState["ConnectionString"] = ConnectionString;
				BindData();
			}
			else
			{
				ConnectionString = (string) ViewState["ConnectionString"];
				RebillID = (string) ViewState["RebillID"];
			}
		}
		private void BindData()
		{
			string strSQL = "SELECT  ID, Title, Path from RebillAttachments where RebillDetailID = " + RebillID;

			SqlConnection myConnection = new SqlConnection(ConnectionString);
			myConnection.Open();
      
			SqlDataAdapter schemaDA = new SqlDataAdapter(strSQL, myConnection);

			ds = new DataSet();
			schemaDA.Fill(ds, "Data");

			DataGrid1.DataSource = ds.Tables["Data"];
			DataGrid1.DataBind();   
            
			if  (ds.Tables["Data"].Rows.Count > 0)
			{
				lbNoRecords.Visible = false;
				DataGrid1.Visible = true;
			}
			else
			{
				lbNoRecords.Visible = true;
				DataGrid1.Visible = false;
			}

		}



		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
			this.DataGrid1.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_CancelCommand);
			this.DataGrid1.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_EditCommand);
			this.DataGrid1.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_UpdateCommand);

		}
		#endregion
		private void tbID_TextChanged(object sender, System.EventArgs e)
		{
                
		}

		private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{

				string sql;
				string ID = e.CommandArgument.ToString();

               
				try
				{

					sql = " Select Path from RebillAttachments where  ID =  " + ID;

					SqlConnection myConnection = new SqlConnection(ConnectionString);
      
					SqlCommand myCommand = new SqlCommand(sql,myConnection);
					myConnection.Open();
					SqlDataReader myReader =  myCommand.ExecuteReader();
					if(myReader.Read()) 
					{
						string path = (string) myReader["Path"];
						if (File.Exists(Server.MapPath(path)))
						{
							File.Delete(Server.MapPath(path));
						}
					}
					myConnection.Close();
				}
				catch
				{
				}

                
				//do the delete

				sql = " Delete from RebillAttachments where  ID =  " + ID;

				SqlConnection myConnection2 = new SqlConnection(ConnectionString);
      
				SqlCommand myCommand2 = new SqlCommand(sql,myConnection2);
				myConnection2.Open();
				myCommand2.ExecuteNonQuery();
				myConnection2.Close();

				//Rebind the DataGrid
				DataGrid1.EditItemIndex = -1;
				BindData();
			}

		}

		private void DataGrid1_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			string sql;
			string ID = e.Item.Cells[0].Text;

			sql = " Update RebillAttachments  ";
			sql = sql + "  Set ";
			sql = sql + " Title = '" + ((TextBox)(e.Item.FindControl("edit_Title"))).Text.Replace("'","''")   + "' ";
			sql =  sql + " where  ID =  " + ID;

			SqlConnection myConnection = new SqlConnection(ConnectionString);
      
			SqlCommand myCommand = new SqlCommand(sql,myConnection);
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			//Rebind the DataGrid
			DataGrid1.EditItemIndex = -1;
			BindData();

		}

		private void DataGrid1_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataGrid1.EditItemIndex = -1;
			BindData();
		}

		private void DataGrid1_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataGrid1.EditItemIndex = e.Item.ItemIndex;
			BindData();
		}

		protected void bnAdd_Click(object sender, System.EventArgs e)
		{
			if (Request.Files.Count > 0)
			{
				if (Request.Files[0].ContentLength < 1)
				{
					lbMessage.Text = "Pick a file";
					return;
				}
				lbMessage.Text = "";



				string sql1;
				string FacilityAlphaCode = "Other";

				sql1 = " SELECT     Facility.AlphaCode FROM RebillDetail INNER JOIN ";
                sql1 = sql1 + " Facility ON RebillDetail.FacilityID = Facility.Id ";
                sql1 = sql1 + " WHERE  RebillDetail.ID = " + RebillID;

				SqlConnection myConnection1 = new SqlConnection(ConnectionString);
      
				SqlCommand myCommand1 = new SqlCommand(sql1,myConnection1);
				myConnection1.Open();
				SqlDataReader myReader1 =  myCommand1.ExecuteReader();
				if(myReader1.Read()) 
				{
					FacilityAlphaCode = (string) myReader1["AlphaCode"];
					FacilityAlphaCode = FacilityAlphaCode.Trim();
				}
				myConnection1.Close();

				string FacPath = Server.MapPath("RebillAttachments" );
				string relpath = "RebillAttachments";

				FacPath = FacPath + @"\" + FacilityAlphaCode;
				relpath = relpath + "/" + FacilityAlphaCode;

				if (!Directory.Exists(FacPath))
				{
					Directory.CreateDirectory(FacPath);
				}

				FacPath = FacPath + @"\" +DateTime.Now.ToString("yyyy-MM");
				relpath = relpath + "/" + DateTime.Now.ToString("yyyy-MM");

				if (!Directory.Exists(FacPath))
				{
					Directory.CreateDirectory(FacPath);
				}



					
				string newfilename = FacPath + @"\" + RebillID + "-" + Path.GetFileName(Request.Files[0].FileName);
                string relpathfile = relpath + "/" + RebillID + "-" + Path.GetFileName(Request.Files[0].FileName);

               
				try
				{
					int i = 1;
					while ((File.Exists(newfilename)) && (i<10000))
					{
						string nextfile =   RebillID + "-" + Path.GetFileNameWithoutExtension(Request.Files[0].FileName)+ i.ToString() +   Path.GetExtension(Request.Files[0].FileName);
						relpathfile = relpath + "/" + nextfile;
						newfilename = Server.MapPath(relpath + "/" + nextfile);
						i = i + 1;
					}

					Request.Files[0].SaveAs(newfilename);

				}
				catch
				{
					lbMessage.Text = "Error saving file";
					return;
				}
                
				string sql = " insert into RebillAttachments (RebillDetailID, Title, Path) Values (" + RebillID + ", '" + tbTitle.Text.Replace("'", "''") + "', '" + relpathfile + "') ";

				SqlConnection myConnection = new SqlConnection(ConnectionString);
      
				SqlCommand myCommand = new SqlCommand(sql,myConnection);
				myConnection.Open();
				myCommand.ExecuteNonQuery();
				myConnection.Close();

				tbTitle.Text = "";

				//Rebind the DataGrid
				DataGrid1.EditItemIndex = -1;
				BindData();

			}

			else
			{
				lbMessage.Text = "Select a file";
			}

		}


	}
}
