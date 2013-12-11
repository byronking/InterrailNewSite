using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class IRGRailCarrierRepository
    {
        public List<IRGRailCarrier> GetAllInterrailCarriers()
        {
            var carriersList = new List<IRGRailCarrier>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetAllInterrailCarriers", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var carrier = new IRGRailCarrier()
                    {
                        Id = reader.GetInt32(0),
                        RailCarrierCode = reader.GetString(1),
                        RailCarrierName = reader.GetString(2),
                        LastModifiedBy = reader.GetString(3),
                        LastModifiedOn = reader.GetDateTime(4),
                    };

                    carriersList.Add(carrier);
                }
            }

            return carriersList;
        }

        public IRGRailCarrier GetInterrailCarrierByCode(string railCarrierCode)
        {
            IRGRailCarrier carrier = null;

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetInterrailCarrierByCode", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@railCarrierCode", SqlDbType.Char, 10).Value = railCarrierCode;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    carrier = new IRGRailCarrier()
                    {
                        Id = reader.GetInt32(0),
                        RailCarrierCode = reader.GetString(1),
                        RailCarrierName = reader.GetString(2),
                        LastModifiedBy = reader.GetString(3),
                        LastModifiedOn = reader.GetDateTime(4),
                    };
                }
            }

            return carrier;
        }
    }
}