using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using MIB_FILM_CLD_INT.Infrastructure;

namespace MIB_FILM_CLD_INT.Services
{
    public sealed class SelfEfficiencyIntegrationService(LegacyIntegrationDb db)
    {
        private const string MibConnectionFile = "connectionStringMIB.txt";
        private static readonly string[] FilmMakingMachineCodes = ["F1", "F2", "F3"];
        private readonly LegacyIntegrationDb _db = db;

        public void Run4Days()
        {
            Run(DateTime.Now.AddDays(-4), includePview2061: false);
        }

        public void RunYtd()
        {
            Run(DateTime.Now.AddDays(-1), includePview2061: true);
        }

        private void Run(DateTime dateParam, bool includePview2061)
        {
            DateTime start = DateTime.Now;
            DataTable prodData = GetProdData();

            for (int i = 0; i <= 2; i++)
            {
                dateParam = dateParam.AddDays(i);
                string yearMonth = dateParam.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                string day = dateParam.Day.ToString(CultureInfo.InvariantCulture);

                foreach (string filmMakingMachineCode in FilmMakingMachineCodes)
                {
                    Pview3141(yearMonth, filmMakingMachineCode, day);
                    PviewFm(yearMonth, filmMakingMachineCode, day);
                    PviewFm2(yearMonth, filmMakingMachineCode, day);
                    PviewHopper(yearMonth, filmMakingMachineCode, day);
                    Pview3015(yearMonth, filmMakingMachineCode, day);

                    if (includePview2061)
                    {
                        Pview2061(yearMonth, filmMakingMachineCode, day);
                    }

                    DataTable typeData = GetTypeData(filmMakingMachineCode);

                    foreach (DataRow prodRow in prodData.Rows)
                    {
                        foreach (DataRow typeRow in typeData.Rows)
                        {
                            string prod = Convert.ToString(prodRow["PROD"], CultureInfo.InvariantCulture) ?? string.Empty;
                            string type = Convert.ToString(typeRow["TYPE"], CultureInfo.InvariantCulture) ?? string.Empty;
                            string thick = Convert.ToString(typeRow["THICK"], CultureInfo.InvariantCulture) ?? string.Empty;

                            CalcSummary(yearMonth, filmMakingMachineCode, prod, type, thick);
                            CalcRawUsage(yearMonth, filmMakingMachineCode, prod, type, thick);
                            CalcRawComp(yearMonth, filmMakingMachineCode, prod, type, thick);
                        }
                    }

                    CalcSummaryTtl(yearMonth, filmMakingMachineCode);

                    foreach (DataRow prodRow in prodData.Rows)
                    {
                        string prod = Convert.ToString(prodRow["PROD"], CultureInfo.InvariantCulture) ?? string.Empty;
                        CalcRawCalc(yearMonth, filmMakingMachineCode, prod);
                    }

                    CalcWaste(yearMonth, filmMakingMachineCode, "C");
                    CalcWaste(yearMonth, filmMakingMachineCode, "B");
                    CalcQty(yearMonth, filmMakingMachineCode);
                }
            }

            PspMibLastUpdate("3", DateTime.Now.ToString("yyyy-MM", CultureInfo.InvariantCulture), start);
        }

        private DataTable GetProdData()
        {
            return _db.ExecuteSqlDataTable(MibConnectionFile, "PSP_SELFEF_PROD_SEL");
        }

        private DataTable GetTypeData(string filmMakingMachineCode)
        {
            return _db.ExecuteSqlDataTable(
                MibConnectionFile,
                "PSP_SELFEF_TYPE_SEL",
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode));
        }

        private void CalcQty(string yearMonth, string filmMakingMachineCode)
        {
            ExecuteYearMonthMachine("PSP_SELFEF_CALC_QTY_INT", yearMonth, filmMakingMachineCode);
        }

        private void Pview3141(string yearMonth, string filmMakingMachineCode, string day)
        {
            ExecuteYearMonthMachineDay("PSP_SELFEF_CALC_PVIEW3141_INT", yearMonth, filmMakingMachineCode, day);
        }

        private void PviewFm(string yearMonth, string filmMakingMachineCode, string day)
        {
            ExecuteYearMonthMachineDay("PSP_SELFEF_CALC_PVIEWFM_INT", yearMonth, filmMakingMachineCode, day);
        }

        private void PviewFm2(string yearMonth, string filmMakingMachineCode, string day)
        {
            ExecuteYearMonthMachineDay("PSP_SELEFF_CALC_PVIEWFM2_INT", yearMonth, filmMakingMachineCode, day);
        }

        private void PviewHopper(string yearMonth, string filmMakingMachineCode, string day)
        {
            ExecuteYearMonthMachineDay("PSP_SELFEF_CALC_PVIEWHOPPER_INT", yearMonth, filmMakingMachineCode, day);
        }

        private void Pview3015(string yearMonth, string filmMakingMachineCode, string day)
        {
            ExecuteYearMonthMachineDay("PSP_SELFEF_CALC_PVIEW3015_INT", yearMonth, filmMakingMachineCode, day);
        }

        private void Pview2061(string yearMonth, string filmMakingMachineCode, string day)
        {
            ExecuteYearMonthMachineDay("PSP_SELFEF_CALC_PVIEW2061_INT", yearMonth, filmMakingMachineCode, day);
        }

        private void CalcSummary(string yearMonth, string filmMakingMachineCode, string prod, string type, string thick)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_SELFEF_CALC_SUMMARY_INT",
                Varchar("P_YEARMTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                Varchar("P_PROD", 10, prod),
                Varchar("P_TYPE", 10, type),
                Varchar("P_THICK", 10, thick));
        }

        private void CalcRawUsage(string yearMonth, string filmMakingMachineCode, string prod, string type, string thick)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_SELFEF_CALC_RAWUSAGE_INT",
                Varchar("P_YEARMTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                Varchar("P_PROD", 10, prod),
                Varchar("P_TYPE", 10, type),
                Varchar("P_THICK", 10, thick));
        }

        private void CalcRawComp(string yearMonth, string filmMakingMachineCode, string prod, string type, string thick)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_SELFEF_CALC_COMP_INT",
                Varchar("P_YEARMTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                Varchar("P_PROD", 10, prod),
                Varchar("P_TYPE", 10, type),
                Varchar("P_THICK", 10, thick));
        }

        private void CalcSummaryTtl(string yearMonth, string filmMakingMachineCode)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_SELFEF_CALC_SUMMARYTTL_INT",
                Varchar("P_YEARMONTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode));
        }

        private void CalcRawCalc(string yearMonth, string filmMakingMachineCode, string prod)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_SELFEF_CALC_RAWCALC_INT",
                Varchar("P_YEARMONTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                Varchar("P_PROD", 10, prod));
        }

        private void CalcWaste(string yearMonth, string filmMakingMachineCode, string bcType)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                "PSP_SELFEF_CALC_WASTE_INT",
                Varchar("P_YEARMTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                Varchar("P_BCTYPE", 1, bcType));
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

        private void ExecuteYearMonthMachine(string storedProcedureName, string yearMonth, string filmMakingMachineCode)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                storedProcedureName,
                Varchar("P_YEARMTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode));
        }

        private void ExecuteYearMonthMachineDay(string storedProcedureName, string yearMonth, string filmMakingMachineCode, string day)
        {
            _db.ExecuteSqlNonQuery(
                MibConnectionFile,
                storedProcedureName,
                Varchar("P_YEARMTH", 10, yearMonth),
                Varchar("P_FMMCCODE", 3, filmMakingMachineCode),
                Varchar("P_DAY", 2, day));
        }

        private static SqlParameter Varchar(string name, int size, string value)
        {
            return new SqlParameter(name, SqlDbType.VarChar, size)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }
    }
}
