using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class InterrailUserRepository
    {
        public List<InterrailUser> GetAllInterrailUsers()
        {
            var usersList = new List<InterrailUser>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("GetAllInterrailUsers", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = startIndex;
                //cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    var user = new InterrailUser()
                    {
                        ID = reader.GetInt32(0),
                        UserId = reader.GetString(1),
                        UserName = reader.GetString(2),
                        Password = reader.GetString(3),
                        UserLongName = reader.GetString(4),
                        UserType = reader.GetString(5),
                        LastModifiedBy = reader.GetString(6),
                        LastModifiedOn = reader.GetDateTime(7)
                    };

                    usersList.Add(user);
                }
            }

            return usersList;
        }

        public List<InterrailUser> SearchInterrailUsers(string searchCriteria)
        {
            var usersList = new List<InterrailUser>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
            using (SqlCommand cmd = new SqlCommand("SearchInterrailUsers", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@searchCriteria", SqlDbType.VarChar, 50).Value = searchCriteria;
                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var user = new InterrailUser()
                    {
                        ID = reader.GetInt32(0),
                        UserId = reader.GetString(1),
                        UserName = reader.GetString(2),
                        Password = reader.GetString(3),
                        UserLongName = reader.GetString(4),
                        UserType = reader.GetString(5),
                        LastModifiedBy = reader.GetString(6),
                        LastModifiedOn = reader.GetDateTime(7)
                    };

                    usersList.Add(user);
                }
            }

            return usersList;
        }
    }
}