using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using MIB_FILM_CLD_INT.Infrastructure;

namespace MIB_FILM_CLD_INT.Services
{
    public sealed class StockControlIntegrationService(LegacyIntegrationDb db)
    {
        private const string XxtryConnectionFile = "connectionStringXXTRY.txt";
        private const string MibConnectionFile = "connectionStringMIB.txt";
        private readonly LegacyIntegrationDb _db = db;

        public void Run()
        {
            DateTime start = DateTime.Now;
            SpMibStockControlList();
            PspMibLastUpdate("3", DateTime.Now.ToString("yyyy-MM", CultureInfo.InvariantCulture), start);
        }

        private void SpMibStockControlList()
        {
            _db.ExecuteOracleStoredProcedureReader(XxtryConnectionFile, "SP_MIB_STOCK_CONTROL_LIST", reader =>
            {
                while (reader.Read())
                {
                    PspInvStockControlMaintInt(
                        GetString(reader, "SDATE"),
                        GetString(reader, "PROD_LINE"),
                        GetString(reader, "PROD_GROUP"),
                        GetDouble(reader, "SV"),
                        GetDouble(reader, "PICK"),
                        GetDouble(reader, "HOLD"),
                        GetDouble(reader, "NOINCOMING"),
                        GetDouble(reader, "ASG"),
                        GetDouble(reader, "ATP"),
                        GetDouble(reader, "NORECEIVING"));
                }
            });
        }

        private void PspInvStockControlMaintInt(string sdate, string prodLine, string prodGroup, double sv, double pick, double hold, double noIncoming, double asg, double atp, double noReceiving)
        {
            string formattedSdate = sdate.Length >= 6
                ? sdate[..4] + "-" + sdate.Substring(4)
                : sdate;

            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_INV_STOCK_CONTROL_MAINT_INT",
                Varchar("P_SDATE", 7, formattedSdate),
                Varchar("P_PROD_LINE", 3, prodLine),
                Varchar("P_PROD_GROUP", 20, prodGroup),
                Decimal("P_SV", sv),
                Decimal("P_PICK", pick),
                Decimal("P_HOLD", hold),
                Decimal("P_NO_INC", noIncoming),
                Decimal("P_ASG", asg),
                Decimal("P_ATP", atp),
                Decimal("P_NORECEIVING", noReceiving));
        }

        private void PspMibLastUpdate(string chartId, string date, DateTime start)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_MIB_LAST_UPDATE",
                Varchar("P_CHART_ID", 50, chartId),
                Varchar("P_DATE", 50, date),
                Varchar("P_UPDATE_DATE", 50, DateTime.Now.ToString("dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture)),
                Varchar("P_START_DATE", 50, start.ToString("dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
        }

        private static string GetString(OracleDataReader reader, string columnName)
        {
            return Convert.ToString(reader[columnName], CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static double GetDouble(OracleDataReader reader, string columnName)
        {
            object value = reader[columnName];
            if (value == DBNull.Value)
            {
                return 0d;
            }

            string text = Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
            return string.IsNullOrWhiteSpace(text) ? 0d : double.Parse(text, CultureInfo.InvariantCulture);
        }

        private static SqlParameter Varchar(string name, int size, string value)
        {
            return new SqlParameter(name, SqlDbType.VarChar, size)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        private static SqlParameter Decimal(string name, double value)
        {
            return new SqlParameter(name, SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }
    }
}
