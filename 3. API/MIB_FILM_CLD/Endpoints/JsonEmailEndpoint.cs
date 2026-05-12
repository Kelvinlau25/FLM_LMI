using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using DBConnection;

namespace MIB_FILM_CLD.Endpoints
{
    public static class JsonEmailEndpoint
    {
        public static IResult HandleRequest(HttpContext context)
        {
            string pEmpno = context.Request.Query["EMPNO"];
            string pUuid = context.Request.Query["UUID"];
            string pName = context.Request.Query["NAME"];

            string result;
            if (pEmpno != "" && pEmpno != null && pUuid != "" && pUuid != null)
            {
                result = SendEmailVerify(pEmpno, pUuid, pName);
            }
            else
            {
                result = "2";
            }

            return Results.Content(result, "application/json");
        }

        private static string SendEmailVerify(string pEmpno, string pUuid, string pName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString.FILM_CLD))
            {
                SqlCommand cmd = new SqlCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "PSP_MIB_APPS_VERIFY_SEND";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("P_EMPNO", pEmpno)).Direction = ParameterDirection.Input;
                cmd.Parameters.Add(new SqlParameter("P_UUID", pUuid)).Direction = ParameterDirection.Input;
                cmd.Parameters.Add(new SqlParameter("P_NAME", pName)).Direction = ParameterDirection.Input;
                cmd.Parameters.Add(new SqlParameter("RETURN_VALUE", SqlDbType.VarChar, 1)).Direction = ParameterDirection.Output;
                cmd.ExecuteReader();
                conn.Close();
                
                string returnValue = cmd.Parameters["RETURN_VALUE"].Value.ToString();
                cmd.Dispose();
                return returnValue;
            }
        }
    }
}
