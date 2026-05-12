using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using DBConnection;

namespace MIB_FILM_CLD.Endpoints
{
    public static class JsonFilmEndpoint
    {
        public static IResult HandleRequest(HttpContext context)
        {
            string pType = context.Request.Query["TYPE"];
            string pUuid = context.Request.Query["UUID"];
            string callback = context.Request.Query["CALLBACK"];

            DataTable dtResult = GetFilmMobileData(pType, pUuid);
            string jsonResponse = SerializeToJson(dtResult, callback);

            return Results.Content(jsonResponse, "application/json");
        }

        private static DataTable GetFilmMobileData(string pType, string pUuid)
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString.FILM_CLD))
            {
                SqlCommand cmd = new SqlCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "MIB_MOBILE_GET_DATA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@P_TYPE", pType)).Direction = ParameterDirection.Input;
                cmd.Parameters.Add(new SqlParameter("@P_UUID", pUuid)).Direction = ParameterDirection.Input;
                dtResult.Load(cmd.ExecuteReader());
                conn.Close();
                cmd.Dispose();
            }
            return dtResult;
        }

        private static string SerializeToJson(DataTable dt, string callback)
        {
            // PRESERVE EXACT LEGACY SERIALIZATION LOGIC
            string jsonString = "";
            jsonString = jsonString + "{\"" + callback + "\":[";

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                jsonString = jsonString + "{";
                for (int ii = 0; ii <= dt.Columns.Count - 1; ii++)
                {
                    if (dt.Columns[ii].DataType.Name == "String")
                    {
                        string zz = "";
                        if (dt.Rows[i][ii].ToString() == "true" || dt.Rows[i][ii].ToString() == "false")
                            zz = dt.Rows[i][ii].ToString();
                        else
                            zz = "\"" + dt.Rows[i][ii].ToString() + "\"";

                        jsonString = jsonString + "\"" + dt.Columns[ii].ColumnName + "\":" + zz;
                    }
                    else
                    {
                        jsonString = jsonString + "\"" + dt.Columns[ii].ColumnName + "\":" + dt.Rows[i][ii].ToString();
                    }

                    if (ii != dt.Columns.Count - 1)
                    {
                        jsonString = jsonString + ",";
                    }
                }

                if (i == dt.Rows.Count - 1)
                    jsonString = jsonString + "}";
                else
                    jsonString = jsonString + "},";
            }
            jsonString = jsonString + "]}";
            return jsonString;
        }
    }
}
