using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace InterrailPPRS.App_Code
{
    public static class GenericHelper
    {
        public static DateTime GetDateTimeValue(this SqlDataReader reader, int columnIndex)
        {
            if (!reader.IsDBNull(columnIndex))
            {
                return reader.GetDateTime(columnIndex);
            }
            else
            {
                return Convert.ToDateTime("12/12/9999");
            }
        }

        public static string GetLoggedOnUser()
        {
            return WindowsIdentity.GetCurrent().Name;
        }
    }
}