using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public class TeamsRepository
    {
        public List<InterrailTeam> GetTeamById(int teamId)
        {
            var teamList = new List<InterrailTeam>();

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("GetTeamById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@teamId", SqlDbType.Int).Value = teamId;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var team = new InterrailTeam()
                        {
                            Id = Convert.ToInt32(reader["Id"].ToString()),
                            TeamName = reader["TeamName"].ToString(),
                            FacilityId = Convert.ToInt32(reader["FacilityId"].ToString()),
                            TeamMembers = reader["TeamMembers"].ToString(),
                            LastModifiedBy = reader["LastModifiedBy"].ToString(),
                            LastModifiedOn = Convert.ToDateTime(reader["LastModifiedOn"]),
                            Active = Convert.ToBoolean(reader["Active"])
                        };

                        teamList.Add(team);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return teamList;
        }

        public void InsertNewTeam(InterrailTeam team)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("InsertNewTeam", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public void UpdateExistingTeam(InterrailTeam team)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("UpdateTeamById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = team.Id;
                    cmd.Parameters.Add("@teamName", SqlDbType.VarChar).Value = team.TeamName;
                    cmd.Parameters.Add("@teamMembers", SqlDbType.VarChar).Value = team.TeamMembers;
                    cmd.Parameters.Add("@lastModifiedBy", SqlDbType.VarChar).Value = team.LastModifiedBy;
                    cmd.Parameters.Add("@lastModifiedOn", SqlDbType.DateTime).Value = team.LastModifiedOn;
                    cmd.Parameters.Add("@active", SqlDbType.Bit).Value = team.Active ? 1 : 0;
                    cmd.Connection.Open();

                    var reader = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public int SaveNewTeam(InterrailTeam team)
        {
            var newId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["MM_Main_STRING"]))
                using (SqlCommand cmd = new SqlCommand("SaveNewTeam", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@facilityId", SqlDbType.Int).Value = team.FacilityId;
                    cmd.Parameters.Add("@teamName", SqlDbType.VarChar).Value = team.TeamName;
                    cmd.Parameters.Add("@teamMembers", SqlDbType.VarChar).Value = team.TeamMembers;
                    cmd.Parameters.Add("@lastModifiedBy", SqlDbType.VarChar).Value = team.LastModifiedBy;
                    cmd.Parameters.Add("@lastModifiedOn", SqlDbType.DateTime).Value = team.LastModifiedOn;
                    cmd.Parameters.Add("@active", SqlDbType.Bit).Value = team.Active ? 1 : 0;
                    cmd.Connection.Open();

                    var returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    var reader = cmd.ExecuteNonQuery();
                    newId = (int)returnParameter.Value;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return newId;
        }
    }
}