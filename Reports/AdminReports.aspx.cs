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
	/// Summary description for AdminReports.
	/// </summary>
	public partial class AdminReport : PageBase
	{
	
		public DataSet ds;
		private string pagescript = "";
		private string connstring
		{
			get  //get you one!
			{
				//ConnectionString is set on the web.config in this example
                return ConfigurationSettings.AppSettings["MM_Main_STRING"];
				//but you can just set it here if desired.
				// return "Server=.;uid=sa;pwd=;database=Interrail";
			}
			set   //set
			{
                connstring = ConfigurationSettings.AppSettings["MM_Main_STRING"];
			}

		}

        protected override void Page_Load(object sender, EventArgs e)
		{

            base.Page_Load(sender, e);

			// Put user code to initialize the page here
 
			ds = new DataSet();


			if (!IsPostBack)
			{
				BindTable();
				DBTableDropDown.Items.Insert(0, new ListItem("Select a Table"));	
				DBTableDropDown.SelectedIndex = 0;
				lbFields.Items.Clear();
				// BindColumns();
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


		}
		#endregion

		private void BindTable ()
		{

			SqlConnection myConnection = new SqlConnection(connstring);
			string SQL = "SELECT TABLE_NAME, Substring(TABLE_NAME, 4, 50) as Table_Name_View FROM INFORMATION_SCHEMA.TABLES " +
				"WHERE TABLE_NAME Like 'mv_%' " +
				"ORDER BY TABLE_NAME";
			SqlDataAdapter schemaDA = new SqlDataAdapter(SQL, myConnection);

 
			schemaDA.Fill(ds, "Tables");
	

 
			DBTableDropDown.DataSource=ds.Tables["Tables"];
			DBTableDropDown.DataTextField = "TABLE_NAME_View";
			DBTableDropDown.DataValueField = "TABLE_NAME";
			//DBTableDropDown.SelectedIndex = DBTableDropDown.SelectedIndex;
			DBTableDropDown.DataBind();
			//DBTableDropDown.SelectedIndex = DBTableDropDown.SelectedIndex;
            
			BindColumns();
		}
        
		private void BindColumns()
		{
            
			SqlConnection myConnection2 = new SqlConnection(connstring);
			string SQL2 = "SELECT *, rtrim(Table_Name)+'~'+rtrim(Column_Name)+'~'+rtrim(Data_Type) as value FROM INFORMATION_SCHEMA.COLUMNS " +
				" where TABLE_NAME = '" +  DBTableDropDown.SelectedItem.Value + "' " ;
			//+
			//    "ORDER BY COLUMN_NAME";
			SqlDataAdapter schemaDA2 = new SqlDataAdapter(SQL2, myConnection2);

			schemaDA2.Fill(ds, "Columns");
      

			lbFields.DataSource = ds.Tables["Columns"];
			lbFields.DataTextField = "COLUMN_NAME";
			lbFields.DataValueField = "Value";
			//lbFields.SelectedIndex = lbFields.SelectedIndex;
			lbFields.DataBind();
			//lbFields.SelectedIndex = lbFields.SelectedIndex;
        
        
		}

 	
   
		protected void DBTableDropDown_Changed(object sender, System.EventArgs e)
		{
			BindColumns();  
			lbSelect.Items.Clear();
			lbWhere.Items.Clear();
			//lbOrder.Items.Clear();
			updateWhereOptions();
		}

		protected void ListBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
        
		}

		private void DropDownList1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
        
		}

		private void Button1_Click(object sender, System.EventArgs e)
		{
        
		}

		private void updateWhereOptions()
		{
            
			if (lbFields.SelectedIndex > -1)
			{
				Label1.Text = lbFields.SelectedItem.Value;
				string type =   lbFields.SelectedItem.Value.Split('~')[2];
				Label1.Text = type;
            
				switch(type)
				{
					case "smalldatetime":
					case "datetime":
						lbWhereOp.Items.Clear();
						lbWhereOp.Items.Add(new ListItem(">",">~1"));
						lbWhereOp.Items.Add(new ListItem("<","<~1"));
						lbWhereOp.Items.Add(new ListItem("=","=~1"));
						lbWhereOp.Items.Add(new ListItem("Between","Between~2"));
						break;
					case "int":
						lbWhereOp.Items.Clear();
						lbWhereOp.Items.Add(new ListItem(">",">~1"));
						lbWhereOp.Items.Add(new ListItem("<","<~1"));
						lbWhereOp.Items.Add(new ListItem("=","=~1"));
						lbWhereOp.Items.Add(new ListItem("Between","Between~2"));
						break;
					case "numeric":
						lbWhereOp.Items.Clear();
						lbWhereOp.Items.Add(new ListItem(">",">~1"));
						lbWhereOp.Items.Add(new ListItem("<","<~1"));
						lbWhereOp.Items.Add(new ListItem("=","=~1"));
						lbWhereOp.Items.Add(new ListItem("Between","Between~2"));
						break;
					case "varchar":
						lbWhereOp.Items.Clear();
						lbWhereOp.Items.Add(new ListItem(">",">~1"));
						lbWhereOp.Items.Add(new ListItem("<","<~1"));
						lbWhereOp.Items.Add(new ListItem("=","=~1"));
						lbWhereOp.Items.Add(new ListItem("Like","Like~1"));
						break;
					case "char":
						lbWhereOp.Items.Clear();
						lbWhereOp.Items.Add(new ListItem(">",">~1"));
						lbWhereOp.Items.Add(new ListItem("<","<~1"));
						lbWhereOp.Items.Add(new ListItem("=","=~1"));
						lbWhereOp.Items.Add(new ListItem("Like","Like~1"));
						break;
					default:
						lbWhereOp.Items.Clear();
						break;
				}
			}        
		}


		protected void lbFields_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (lbFields.SelectedIndex > -1)
			{
				updateWhereOptions();
			}
			else
			{ 
				Label1.Text = "";
			}
			lbFields.SelectedIndex = lbFields.SelectedIndex;

		}

		private void Button1_ServerClick(object sender, System.EventArgs e)
		{
        
		}

		protected void bntAddSelect_Click(object sender, System.EventArgs e)
		{
            try
            {
                lbSelect.Items.Add(new ListItem(lbFields.SelectedItem.Text, lbFields.SelectedItem.Value));
            }catch(Exception ex){
            
            }
		}

		protected void bntSelectUp_Click(object sender, System.EventArgs e)
		{
			if (lbSelect.SelectedIndex > 0)
			{
				int indx =    lbSelect.SelectedIndex;
				ListItem theItem = lbSelect.Items[indx];
				lbSelect.Items.Remove(theItem);
				lbSelect.Items.Insert( indx-1, theItem);
			}
       
		}

		protected void bntSelectDown_Click(object sender, System.EventArgs e)
		{
			if ((lbSelect.SelectedIndex > -1)  && (lbSelect.SelectedIndex < lbSelect.Items.Count-1))
			{
				int indx =    lbSelect.SelectedIndex;
				ListItem theItem = lbSelect.Items[indx];
				lbSelect.Items.Remove(theItem);
				lbSelect.Items.Insert( indx+1, theItem);
			}
       
		}

		protected void bntSelectDelete_Click(object sender, System.EventArgs e)
		{
			if (lbSelect.SelectedIndex > -1)
			{
				int indx =    lbSelect.SelectedIndex;
				ListItem theItem = lbSelect.Items[indx];
				lbSelect.Items.Remove(theItem);
			}
        
		}

		protected void lbWhereOp_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (lbWhereOp.SelectedIndex > -1)
			{
				string s = lbWhereOp.SelectedItem.Value;
				switch(s.Split('~')[1])
				{
					case "2":
						tbWhereOne.Visible = true;
						tbWhereTwo.Visible = true;
						break;
					default:
						tbWhereOne.Visible = true;
						tbWhereTwo.Visible = false;
						break;

				}

			}

        
		}

		protected void bntAddWhere_Click(object sender, System.EventArgs e)
		{
			if (lbWhereOp.SelectedIndex > -1)
			{

				if (lbWhere.Items.Count > 0)
				{
					lbWhere.Items.Add(new ListItem("AND", System.Convert.ToString(lbWhere.Items.Count)+"~AND"));
				}
				string op =   lbWhereOp.SelectedItem.Value.Split('~')[0];
				string fname =  lbFields.SelectedItem.Value.Split('~')[1];
				string showwherepart = "";
				string dowherepart = "";
				string sep = "";
				string type =   lbFields.SelectedItem.Value.Split('~')[2];
				switch(type)
				{
					case "int":
					case "numeric":
						sep = "";
						break;
					case "smalldatetime":
					case "datetime":
					case "varchar":
					case "char":
						sep = "'";
						break;
					default:
						sep = "";
						break;
				}
                 
				switch(op)
				{
					case ">":
					case "<":
					case "=":
					case "<>":
						showwherepart = fname + op + sep + tbWhereOne.Text + sep;
						dowherepart = "[" + fname + "]" + op + sep + tbWhereOne.Text + sep;
						lbWhere.Items.Add(new ListItem(showwherepart,  lbWhere.Items.Count.ToString()+"~"+dowherepart));
						break;
					case "Like":
						showwherepart = fname + " Like " + sep + tbWhereOne.Text + "%" + sep;
						dowherepart = "[" + fname + "]" + " Like " + sep + tbWhereOne.Text + "%" + sep;
						lbWhere.Items.Add(new ListItem(showwherepart, lbWhere.Items.Count.ToString()+"~"+dowherepart));
						break;
					case "Between":
						showwherepart = fname + " Between " + sep + tbWhereOne.Text + sep + " AND " + sep + tbWhereTwo.Text + sep;
						dowherepart = "[" + fname + "]"+ " Between " + sep + tbWhereOne.Text + sep + " AND " + sep + tbWhereTwo.Text + sep;
						lbWhere.Items.Add(new ListItem(showwherepart, lbWhere.Items.Count.ToString()+"~"+dowherepart));
						break;
					default:
						break;
				}

			}
		}

		protected void btnWhereDelete_Click(object sender, System.EventArgs e)
		{
			if (lbWhere.SelectedIndex > -1)
			{
				int indx =    lbWhere.SelectedIndex;
				ListItem theItem = lbWhere.Items[indx];
				lbWhere.Items.Remove(theItem);
			}
      
		}

		private void Button3_Click(object sender, System.EventArgs e)
		{
        
		}

		protected void btnWhereAnd_Click(object sender, System.EventArgs e)
		{
			if ((lbWhere.SelectedIndex > -1) && (lbWhere.SelectedItem.Text == "OR"))
			{
				lbWhere.SelectedItem.Text = "AND";
				lbWhere.SelectedItem.Value = lbWhere.Items.Count.ToString()+"~"+"AND";
			}
		}

		protected void btnWhereOr_Click(object sender, System.EventArgs e)
		{
			if ((lbWhere.SelectedIndex > -1) && (lbWhere.SelectedItem.Text == "AND"))
			{
				lbWhere.SelectedItem.Text = "OR";
				lbWhere.SelectedItem.Value = lbWhere.Items.Count.ToString()+"~"+"OR";
			}
		}

		protected void btnOrderUp_Click(object sender, System.EventArgs e)
		{
			if (lbOrder.SelectedIndex > 0)
			{
				int indx =    lbOrder.SelectedIndex;
				ListItem theItem = lbOrder.Items[indx];
				lbOrder.Items.Remove(theItem);
				lbOrder.Items.Insert( indx-1, theItem);
			}
       
		}

		protected void btnOrderDown_Click(object sender, System.EventArgs e)
		{
			if ((lbOrder.SelectedIndex > -1)  && (lbOrder.SelectedIndex < lbOrder.Items.Count-1))
			{
				int indx =    lbOrder.SelectedIndex;
				ListItem theItem = lbOrder.Items[indx];
				lbOrder.Items.Remove(theItem);
				lbOrder.Items.Insert( indx+1, theItem);
			}
		}

		protected void btnOrderDelete_Click(object sender, System.EventArgs e)
		{
			if (lbOrder.SelectedIndex > -1)
			{
				int indx =    lbOrder.SelectedIndex;
				ListItem theItem = lbOrder.Items[indx];
				lbOrder.Items.Remove(theItem);
			}
       
		}

		protected void btnOrderAdd_Click(object sender, System.EventArgs e)
		{
			lbOrder.Items.Add(new ListItem(  lbFields.SelectedItem.Text, lbOrder.Items.Count.ToString() + "~" +lbFields.SelectedItem.Value));
       
		}

		protected void btnGenerate_Click(object sender, System.EventArgs e)
		{
			if (lbSelect.Items.Count > 0)
			{
				string sql ="";
				string sqlorder ="";

				sql = sql + "SELECT ";
				for (int i=0;i < lbSelect.Items.Count; i++)
				{
					sql = sql + "[" +lbSelect.Items[i].Value.Split('~')[1] + "] ";
					if ( i < lbSelect.Items.Count -1)
						sql = sql + ", ";
				}
				sql = sql + " ";

				sql = sql + " FROM [" +  DBTableDropDown.SelectedItem.Value + "] ";
            
				if (lbWhere.Items.Count > 0)
				{
					sql = sql + " WHERE ";
					for (int i=0;i < lbWhere.Items.Count; i++)
					{
						sql = sql + lbWhere.Items[i].Value.Split('~')[1] + " ";
					}
				}
				sql = sql + " ";
        
				if (lbOrder.Items.Count > 0)
				{
					sqlorder = sqlorder + " ORDER BY ";
					for (int i=0;i < lbOrder.Items.Count; i++)
					{
						sqlorder = sqlorder +  "[" +lbOrder.Items[i].Value.Split('~')[2] + "] ";
						if ( i < lbOrder.Items.Count -1)
							sqlorder = sqlorder + ", ";
					}
				}

				// lblSQL.Text = sql;
				Response.Redirect("AdHocReport.aspx?SQL="+Server.UrlEncode(sql)+"&ORDER="+Server.UrlEncode(sqlorder),true);

			}
		}

		protected void btnSelectAddAll_Click(object sender, System.EventArgs e)
		{
			lbSelect.Items.Clear();
			foreach (ListItem l in   lbFields.Items)
			{
				lbSelect.Items.Add(new ListItem(l.Text, l.Value));
			}
       
		}

	}
}
