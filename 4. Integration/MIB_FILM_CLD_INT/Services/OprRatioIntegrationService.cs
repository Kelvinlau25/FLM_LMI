using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using MIB_FILM_CLD_INT.Infrastructure;

namespace MIB_FILM_CLD_INT.Services
{
    public sealed class OprRatioIntegrationService(LegacyIntegrationDb db)
    {
        private const string PfractConnectionFile = "connectionStringPFRACT.txt";
        private const string MibConnectionFile = "connectionStringMIB.txt";
        private readonly LegacyIntegrationDb _db = db;

        public void Run()
        {
            DateTime start = DateTime.Now;
            SpMibOprRatioIntegration();
            OprRatioTransDateIntegration();
            PspMibLastUpdate("1", DateTime.Now.ToString("yyyy-MM", CultureInfo.InvariantCulture), start);
        }

        private void OprRatioTransDateIntegration()
        {
            const string sql = "select TO_CHAR(t.TRANS_TIME, 'yyyy-MM-dd HH24:mi') AS TRANS_TIME, t.FILMMAKINGMACHINECODE from PVIEW_GET_TRANS_DATE t";
            _db.ExecuteOracleTextReader(PfractConnectionFile, sql, reader =>
            {
                while (reader.Read())
                {
                    PspSalesOprRatioTransDateMaintInt(
                        GetString(reader, "TRANS_TIME"),
                        GetString(reader, "FILMMAKINGMACHINECODE"));
                }
            });
        }

        private void SpMibOprRatioIntegration()
        {
            _db.ExecuteOracleStoredProcedureReader(PfractConnectionFile, "SP_MIB_OPR_RATIO_INTEGRATION", reader =>
            {
                while (reader.Read())
                {
                    PspSalesOprRatioMaintInt(
                        GetString(reader, "YEAR_MONTH"),
                        GetString(reader, "FILMMAKINGMACHINECODE"),
                        GetDouble(reader, "DAY_HOURS"),
                        GetDouble(reader, "USEDTIME"));
                }
            });
        }

        private void PspSalesOprRatioMaintInt(string yearMonth, string filmMakingMachineCode, double dayHours, double usedTime)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_SALES_OPR_RATIO_MAINT_INT",
                Varchar("P_YEAR_MONTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                Decimal("P_DAY_HOURS", dayHours),
                Decimal("P_USED_TIME", usedTime));
        }

        private void PspSalesOprRatioTransDateMaintInt(string transDate, string filmMakingMachineCode)
        {
            DateTime parsedDate = DateTime.ParseExact(transDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_SALES_OPR_RATIO_TRANS_DATE_MAINT_INT",
                DateTimeParameter("P_TRANS_DATE", parsedDate),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode));
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

        private static SqlParameter DateTimeParameter(string name, DateTime value)
        {
            return new SqlParameter(name, SqlDbType.DateTime)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }
    }
}
