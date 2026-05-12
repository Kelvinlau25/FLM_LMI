using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using DBConnection;

namespace MIB_FILM_CLD.Endpoints
{
    public static class VerifyEndpoint
    {
        public static IResult HandleRequest(HttpContext context)
        {
            string pVerifyId = context.Request.Query["VERIFYID"];
            string htmlContent;

            if (pVerifyId != null && pVerifyId != "")
            {
                htmlContent = ProcessVerification(pVerifyId);
            }
            else
            {
                htmlContent = "Invalid Verify ID.";
            }

            return Results.Content(htmlContent, "text/html");
        }

        private static string ProcessVerification(string pVerifyId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString.FILM_CLD))
            {
                SqlCommand cmd = new SqlCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "PSP_MIB_APPS_VERIFY_RECEIVE";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("P_VERIFY_ID", pVerifyId)).Direction = ParameterDirection.Input;
                cmd.Parameters.Add(new SqlParameter("HTML_RETURN", SqlDbType.VarChar, 1000)).Direction = ParameterDirection.Output;
                cmd.ExecuteReader();
                conn.Close();
                
                string htmlReturn = cmd.Parameters["HTML_RETURN"].Value.ToString();
                cmd.Dispose();
                return htmlReturn;
            }
        }
    }
}
