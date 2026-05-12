using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using MIB_FILM_CLD_INT.Infrastructure;

namespace MIB_FILM_CLD_INT.Services
{
    public sealed class ProductivityIntegrationService(LegacyIntegrationDb db, ILogger<ProductivityIntegrationService> logger)
    {
        private const string PfractConnectionFile = "connectionStringPFRACT.txt";
        private const string MibConnectionFile = "connectionStringMIB.txt";
        private readonly LegacyIntegrationDb _db = db;
        private readonly ILogger<ProductivityIntegrationService> _logger = logger;

        public void Run()
        {
            DateTime start = DateTime.Now;
            SpMibProductionIntegration2();
            SpMibProductionAll2();
            PspMibLastUpdate("4", DateTime.Now.ToString("yyyy-MM", CultureInfo.InvariantCulture), start);
            PspMibLastUpdate("5", DateTime.Now.ToString("yyyy-MM", CultureInfo.InvariantCulture), start);
        }

        private void SpMibProductionAll2()
        {
            _db.ExecuteOracleStoredProcedureReader(PfractConnectionFile, "SP_MIB_PRODUCTION_ALL2", reader =>
            {
                while (reader.Read())
                {
                    PspProductionCrYieldMaintInt(
                        GetString(reader, "SDATE"),
                        GetString(reader, "FILMMAKINGMACHINECODE"),
                        GetDouble(reader, "BUDGET_MILLROLL_WEIGHT"),
                        GetDouble(reader, "BUDGET_EXTRD_WEIGHT"),
                        GetDouble(reader, "RESULT_MILLROLL_WEIGHT"),
                        GetDouble(reader, "RESULT_EXTRD_WEIGHT"),
                        GetDouble(reader, "BUDGET_SLTOUTPUT_WEIGHT"),
                        GetDouble(reader, "RESULT_SLTOUTPUT_WEIGHT"),
                        GetDouble(reader, "RESULT_MRCMSM_WEIGHT"),
                        GetDouble(reader, "BUDGET_PASS_WEIGHT_WITHJDG"),
                        GetDouble(reader, "RESULT_PASS_WEIGHT_WITHJDG"),
                        GetDouble(reader, "RESULT_INSPCMCM_WEIGHT_WITHJDG"),
                        GetDouble(reader, "RESULT_ALLEXTRD_WEIGHT"));

                    PspProductionBMixMaintInt(
                        GetString(reader, "SDATE"),
                        GetString(reader, "FILMMAKINGMACHINECODE"),
                        GetDouble(reader, "BUDGET_MAINBRM_WEIGHT"),
                        GetDouble(reader, "BUDGET_EXTRD_WEIGHT"),
                        GetDouble(reader, "RESULT_MAINBRM_WEIGHT"),
                        GetDouble(reader, "RESULT_ALLEXTRD_WEIGHT"));
                }
            });
        }

        private void SpMibProductionIntegration2()
        {
            _db.ExecuteOracleStoredProcedureReader(PfractConnectionFile, "SP_MIB_PRODUCTION_INTEGRATION2", reader =>
            {
                while (reader.Read())
                {
                    string yearMonth = GetString(reader, "SDATE");
                    string filmMakingMachineCode = GetString(reader, "FILMMAKINGMACHINECODE");

                    PspProductivityMaintInt(yearMonth, filmMakingMachineCode, GetDouble(reader, "PASS_QTY"));
                    PspProductionCrYieldMaintInt(
                        yearMonth,
                        filmMakingMachineCode,
                        GetDouble(reader, "BUDGET_MILLROLL_WEIGHT"),
                        GetDouble(reader, "BUDGET_EXTRD_WEIGHT"),
                        GetDouble(reader, "RESULT_MILLROLL_WEIGHT"),
                        GetDouble(reader, "RESULT_EXTRD_WEIGHT"),
                        GetDouble(reader, "BUDGET_SLTOUTPUT_WEIGHT"),
                        GetDouble(reader, "RESULT_SLTOUTPUT_WEIGHT"),
                        GetDouble(reader, "RESULT_MRCMSM_WEIGHT"),
                        GetDouble(reader, "BUDGET_PASS_WEIGHT_WITHJDG"),
                        GetDouble(reader, "RESULT_PASS_WEIGHT_WITHJDG"),
                        GetDouble(reader, "RESULT_INSPCMCM_WEIGHT_WITHJDG"),
                        GetDouble(reader, "RESULT_ALLEXTRD_WEIGHT"));
                    PspProductionBMixMaintInt(
                        yearMonth,
                        filmMakingMachineCode,
                        GetDouble(reader, "BUDGET_MAINBRM_WEIGHT"),
                        GetDouble(reader, "BUDGET_EXTRD_WEIGHT"),
                        GetDouble(reader, "RESULT_MAINBRM_WEIGHT"),
                        GetDouble(reader, "RESULT_ALLEXTRD_WEIGHT"));
                }
            });
        }

        private void PspProductionCrYieldMaintInt(string yearMonth, string filmMakingMachineCode, double budgetMillRollWeight, double budgetExtrdWeight, double resultMillRollWeight, double resultExtrdWeight, double budgetSltOutputWeight, double resultSltOutputWeight, double resultMrcmsmWeight, double budgetPassWeightWithJdg, double resultPassWeightWithJdg, double resultInsWeightWithJdg, double resultAllExtrdWeight)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_PRODUCTION_CR_YIELD_MAINT_INT",
                Varchar("P_YEAR_MONTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                Decimal("P_BUDGET_MILLROLL_WEIGHT", budgetMillRollWeight),
                Decimal("P_BUDGET_EXTRD_WEIGHT", budgetExtrdWeight),
                Decimal("P_RESULT_MILLROLL_WEIGHT", resultMillRollWeight),
                Decimal("P_RESULT_EXTRD_WEIGHT", resultExtrdWeight),
                Decimal("P_BUDGET_SLTOUTPUT_WEIGHT", budgetSltOutputWeight),
                Decimal("P_RESULT_SLTOUTPUT_WEIGHT", resultSltOutputWeight),
                Decimal("P_RESULT_MRCMSM_WEIGHT", resultMrcmsmWeight),
                Decimal("P_BUDGET_PASS_WEIGHT_WITHJDG", budgetPassWeightWithJdg),
                Decimal("P_RESULT_PASS_WEIGHT_WITHJDG", resultPassWeightWithJdg),
                Decimal("P_RESULT_INS_WEIGHT_WITHJDG", resultInsWeightWithJdg),
                Decimal("P_RESULT_ALLEXTRD_WEIGHT", resultAllExtrdWeight));
        }

        private void PspProductionBMixMaintInt(string yearMonth, string filmMakingMachineCode, double budgetMainBrmWeight, double budgetExtrdWeight, double resultMainBrmWeight, double resultAllExtrdWeight)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_PRODUCTION_B_MIX_MAINT_INT",
                Varchar("P_YEAR_MONTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                Decimal("P_BUDGET_MAINBRM_WEIGHT", budgetMainBrmWeight),
                Decimal("P_BUDGET_EXTRD_WEIGHT", budgetExtrdWeight),
                Decimal("P_RESULT_MAINBRM_WEIGHT", resultMainBrmWeight),
                Decimal("P_RESULT_ALLEXTRD_WEIGHT", resultAllExtrdWeight));
        }

        private void PspProductivityMaintInt(string yearMonth, string filmMakingMachineCode, double passQty)
        {
            try
            {
                _db.ExecuteSqlNonQuery(
                    MibConnectionFile,
                    "PSP_PRODUCTIVITY_MAINT_INT",
                    Varchar("P_YEAR_MONTH", 10, yearMonth),
                    Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                    Decimal("P_PASS_QTY", passQty));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute PSP_PRODUCTIVITY_MAINT_INT.");
            }
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
