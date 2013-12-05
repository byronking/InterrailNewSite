using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace InterrailPPRS.Reports
{
    public class DataReaderOleDB
    {
        string _sql = "";
        int _Rows = 0;
        DataTableReader _dr;
        bool _eof = false;   // End of file
        int _pos = 0;        // Position
        string _connection = "";

        public DataReaderOleDB(string connection)
        {
            _connection = connection;
        }

        public DataReaderOleDB(string sql, string connection)
        {
            this._sql = sql;
            this._connection = connection;
        }

        public int RecordCount
        {
            get { return this._Rows; }
        }

        public bool Read()
        {
            bool ret = this._dr.Read();
            _pos++;
            if (_pos == this._Rows) { _eof = true; }
            return ret;
        }

        public string Item(string name)
        {
            if (this._dr == null)
            {
                return "";
            }
            else
            {
                return System.Convert.ToString(this._dr[name]);
            }
        }

        public string Fields(int index)
        {
            if (this._dr == null)
            {
                return "";
            }
            else
            {
                return System.Convert.ToString(this._dr[index]);
            }

        }
        public string FieldName(int index)
        {
            if (this._dr == null)
            {
                return "";
            }
            else
            {
                return System.Convert.ToString(this._dr.GetName(index));
            }

        }
        public int FieldCount()
        {
            return this._dr.FieldCount;
        }
        public void Open()
        {
            getRS(this._sql);
        }

        public bool EOF
        {
            get { return _eof; }
        }

        public DataTableReader Requery()
        {
            return getRS(this._sql);
        }

        public DataTableReader getRS(string sql)
        {
            this._sql = sql;
            OleDbConnection sc = new OleDbConnection(_connection);
            sc.Open();
            OleDbCommand scom = new OleDbCommand(sql, sc);
            OleDbDataAdapter da = new OleDbDataAdapter(scom);
            DataSet ds = new DataSet();
            da.Fill(ds, "Data");

            this._Rows = ds.Tables[0].Rows.Count;
            this._pos = 0;
            if (this._Rows > 0) { this._eof = false; } else { this._eof = true; }

            sc.Close();

            this._dr = ds.Tables[0].CreateDataReader();
            sc.Close();
            return _dr;
        }
    }
}