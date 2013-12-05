using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace InterrailPPRS.Reports
{
    public class DataWriterOleDb
    {

        string _sql = "";
        int _Rows = 0;
        DataTableReader _dr;
        bool _eof = false;   // End of file
        int _pos = 0;        // Position
        string _connection = "";
        OleDbConnection objConn;
        OleDbDataAdapter da;
        DataSet ds;

        public DataWriterOleDb(string connection)
        {
            _connection = connection;
        }

        public int RecordCount
        {
            get { return this._Rows; }
        }

        public void Fill(string sSQL){

            objConn  = new OleDbConnection(_connection);
            objConn.Open();

            // Create an instance of a DataAdapter.
            da = new OleDbDataAdapter(sSQL, objConn);

            // Create an instance of a DataSet, and retrieve data from the Authors table.
            ds = new DataSet("DataSet");
            da.Fill(ds);

            _Rows = ds.Tables[0].Rows.Count;

        }

        public void Fields(int field,string value)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            dr.BeginEdit();
            dr[field] = value;
            dr.EndEdit();
        }

        public void Fields(int row, int field,string value)
        {
            DataRow dr = ds.Tables[0].Rows[row];
            dr.BeginEdit();
            dr[field] = value;
            dr.EndEdit();
        }

        public DataColumnCollection Columns
        {
            get { return ds.Tables[0].Columns; }
        }

        public void Update()
        {
            da.Update(ds);
        }

        public void Close()
        {
            objConn.Close();
        }

    }
}