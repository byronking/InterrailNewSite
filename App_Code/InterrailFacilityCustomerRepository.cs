using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailFacilityCustomerRepository
    {
        public List<InterrailFacilityCustomer> GetAllInterrailFacilityCustomers()
        {
            var facilityCustomersList = new List<InterrailFacilityCustomer>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetAllInterrailFacilityCustomers", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var facilityCustomer = new InterrailFacilityCustomer()
                    {
                        Id = reader.GetInt32(0),
                        FacilityId = reader.GetInt32(1),
                        CustomerCode = reader.GetString(2),
                        CustomerName = reader.GetString(3),
                        ContactName = reader.GetString(4),
                        ContactAddress1 = reader.GetString(5),
                        ContactAddress2 = reader.GetString(6),
                        ContactAddress3 = reader.GetString(7),
                        DefaultCustomer = reader.GetBoolean(8),
                        LastModifiedBy = reader.GetString(9),
                        LastModifiedOn = reader.GetDateTime(10),
                        Active = reader.GetBoolean(11)
                    };

                    facilityCustomersList.Add(facilityCustomer);
                }
            }

            return facilityCustomersList;
        }

        public InterrailFacilityCustomer GetFacilityCustomerById(int customerId)
        {
            InterrailFacilityCustomer facilityCustomer = null;

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetFacilityCustomerById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@facilityCustomerId", SqlDbType.Int).Value = customerId;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    facilityCustomer = new InterrailFacilityCustomer()
                    {
                        Id = reader.GetInt32(0),
                        FacilityId = reader.GetInt32(1),
                        CustomerCode = reader.GetString(2),
                        CustomerName = reader.GetString(3),
                        ContactName = reader.GetString(4),
                        ContactAddress1 = reader.GetString(5),
                        ContactAddress2 = reader.GetString(6),
                        ContactAddress3 = reader.GetString(7),
                        DefaultCustomer = reader.GetBoolean(8),
                        LastModifiedBy = reader.GetString(9),
                        LastModifiedOn = reader.GetDateTime(10),
                        Active = reader.GetBoolean(11)
                    };
                }
            }

            return facilityCustomer;
        }

        public int CreateNewFacilityCustomer(int facilityId, string customerCode, string customerName, string contactName, string contactAddress1,
            string contactAddress2, string contactAddress3, bool defaultCustomer, string lastModifiedBy, DateTime lastModifiedOn, bool active)
        {
            int newId = 0;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("CreateFacilityCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@facilityId", SqlDbType.Int).Value = facilityId;
                cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 30).Value = customerCode;
                cmd.Parameters.Add("@customerName", SqlDbType.VarChar, 250).Value = customerName;
                cmd.Parameters.Add("@contactName", SqlDbType.VarChar, 250).Value = contactName;
                cmd.Parameters.Add("@contactAddress1", SqlDbType.VarChar, 250).Value = contactAddress1;
                cmd.Parameters.Add("@contactAddress2", SqlDbType.VarChar, 250).Value = contactAddress2;
                cmd.Parameters.Add("@contactAddress3", SqlDbType.VarChar, 250).Value = contactAddress3;
                cmd.Parameters.Add("@defaultCustomer", SqlDbType.Bit).Value = defaultCustomer ? 1 : 0;
                cmd.Parameters.Add("@lastModifiedBy", SqlDbType.VarChar, 50).Value = lastModifiedBy;
                cmd.Parameters.Add("@lastModifiedOn", SqlDbType.DateTime).Value = lastModifiedOn;
                cmd.Parameters.Add("@active", SqlDbType.Bit).Value = active ? 1 : 0;

                var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                newId = (int)returnParameter.Value;
            }

            return newId;
        }

        public int UpdateFacilityCustomer(int id, int facilityId, string customerCode, string customerName, string contactName, string contactAddress1,
            string contactAddress2, string contactAddress3, bool defaultCustomer, string lastModifiedBy, DateTime lastModifiedOn, bool active)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("UpdateFacilityCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@facilityId", SqlDbType.Int).Value = facilityId;
                cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 10).Value = customerCode;
                cmd.Parameters.Add("@customerName", SqlDbType.VarChar, 30).Value = customerName;
                cmd.Parameters.Add("@contactName", SqlDbType.VarChar, 250).Value = contactName;
                cmd.Parameters.Add("@contactAddress1", SqlDbType.VarChar, 250).Value = contactAddress1;
                cmd.Parameters.Add("@contactAddress2", SqlDbType.VarChar, 250).Value = contactAddress2;
                cmd.Parameters.Add("@contactAddress3", SqlDbType.VarChar, 250).Value = contactAddress3;
                cmd.Parameters.Add("@defaultCustomer", SqlDbType.Bit).Value = defaultCustomer ? 1 : 0;
                cmd.Parameters.Add("@lastModifiedBy", SqlDbType.VarChar, 50).Value = lastModifiedBy;
                cmd.Parameters.Add("@lastModifiedOn", SqlDbType.DateTime).Value = lastModifiedOn;
                cmd.Parameters.Add("@active", SqlDbType.Bit).Value = active ? 1 : 0;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }

            return id;
        }
    }
}