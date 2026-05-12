using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using MIB_FILM_CLD_INT.Infrastructure;

namespace MIB_FILM_CLD_INT.Services
{
    public sealed class SalesOrderIntegrationService(LegacyIntegrationDb db)
    {
        private const string XxtryConnectionFile = "connectionStringXXTRY.txt";
        private const string MibConnectionFile = "connectionStringMIB.txt";
        private readonly LegacyIntegrationDb _db = db;

        public void Run()
        {
            DateTime start = DateTime.Now;
            SpMibSalesOrderSituation();
            PspMibLastUpdate("2", DateTime.Now.ToString("yyyy-MM", CultureInfo.InvariantCulture), start);
        }

        private void SpMibSalesOrderSituation()
        {
            _db.ExecuteOracleStoredProcedureReader(XxtryConnectionFile, "SP_MIB_SALES_ORDER_SITUATION", reader =>
            {
                while (reader.Read())
                {
                    PspSalesSalesOrderMaintInt(
                        GetString(reader, "YearMth"),
                        GetString(reader, "Prod_line"),
                        GetString(reader, "REGION_CD"),
                        GetDouble(reader, "BUDGET"),
                        GetDouble(reader, "FORECAST_QTY"),
                        GetDouble(reader, "DS_QTY"),
                        GetDouble(reader, "LA_QTY"),
                        GetDouble(reader, "BUDGET_AMT"),
                        GetDouble(reader, "FORECAST_AMT"),
                        GetDouble(reader, "DS_AMT"),
                        GetDouble(reader, "LA_AMT"));
                }
            });
        }

        private void PspSalesSalesOrderMaintInt(string yearMonth, string prodLine, string regionCode, double budget, double forecastQty, double dsQty, double laQty, double budgetAmount, double forecastAmount, double dsAmount, double laAmount)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_SALES_SALES_ORDER_MAINT_INT",
                Varchar("P_YEARMTH", 25, yearMonth),
                Varchar("P_PROD_LINE", 3, prodLine),
                Varchar("P_REGION_CD", 20, regionCode),
                Decimal("P_BUDGET", budget),
                Decimal("P_FORECAST_QTY", forecastQty),
                Decimal("P_DS_QTY", dsQty),
                Decimal("P_LA_QTY", laQty),
                Decimal("P_BUDGET_AMT", budgetAmount),
                Decimal("P_FORECAST_AMT", forecastAmount),
                Decimal("P_DS_AMT", dsAmount),
                Decimal("P_LA_AMT", laAmount));
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
