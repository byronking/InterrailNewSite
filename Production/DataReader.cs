﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

using Interrial.PPRS.Dal.TypedListClasses;
using Interrial.PPRS.Dal.EntityClasses;
using Interrial.PPRS.Dal.FactoryClasses;
using Interrial.PPRS.Dal.CollectionClasses;
using Interrial.PPRS.Dal.HelperClasses;
using Interrial.PPRS.Dal;

using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;

namespace InterrailPPRS.Production
{
    public class DataReader
    {
        string _sql = "";
        int _Rows = 0;
        DataTableReader _dr;
        bool _eof = false;   // End of file
        int _pos = 0;        // Position
        bool _lastReadSuccess = false;

        public DataReader()
        {

        }

        public DataReader(string sql)
        {
            this._sql = sql;
        }

        public int RecordCount
        {
            get { return this._Rows; }
        }

        public bool Read()
        {
            _lastReadSuccess = this._dr.Read();
            _pos++;
            if (_pos == this._Rows) { _eof = true; }
            return _lastReadSuccess;
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

        public void Open()
        {
            getRS(this._sql);
        }

        public bool LastReadSuccess
        {
            get { return _lastReadSuccess; }
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
            SqlConnection sc = new SqlConnection(HttpContext.Current.Session["dbPath"].ToString());
            sc.Open();
            SqlCommand scom = new SqlCommand(sql, sc);
            SqlDataAdapter da = new SqlDataAdapter(scom);
            DataSet ds = new DataSet();
            da.Fill(ds, "Data");

            this._Rows =  ds.Tables[0].Rows.Count;
            this._pos = 0;
            if (this._Rows > 0) { this._eof = false; } else { this._eof = true; }

            sc.Close();

            this._dr = ds.Tables[0].CreateDataReader();
            return _dr;
        }
    }
  

}