using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DBModel;
using MstMainModel;

namespace PAB.Repository
{
    public class MMRepo
    {
        DatabaseModel.Database db = new DatabaseModel.Database();


        #region OPR RATIO MASTER MAINTENANCE

        public DataTable OPR_RATIO_TARGET_SEL(MM_OPR_RATIO_TARGET MM_OPR_RATIO_TARGET)
        {
            db.OpenConnection();
            db.command.CommandText = "PSP_LMI_MM_OPR_TARGET_SEL";
            db.command.CommandType = CommandType.StoredProcedure;
            db.command.CommandTimeout = 0;
            db.command.Parameters.Clear();
            db.command.Parameters.Add(new SqlParameter("@P_YEAR", MM_OPR_RATIO_TARGET.YEAR)).Direction = System.Data.ParameterDirection.Input;
            db.command.Parameters.Add(new SqlParameter("@P_MONTH", MM_OPR_RATIO_TARGET.MONTH)).Direction = System.Data.ParameterDirection.Input;
            db.reader = db.command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(db.reader);

            db.CloseReader();
            db.CloseConnection();

            return dt;
        }

        public string OPR_RATIO_TARGET_MAINT(OPR_RATIO_TARGET OPR_RATIO_TARGET)
        {
            try
            {
                string returnValue = "";
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_OPR_TARGET_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_YEAR", OPR_RATIO_TARGET.YEAR)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_MONTH", OPR_RATIO_TARGET.MONTH)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_ITEM", OPR_RATIO_TARGET.ITEM)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TARGET", OPR_RATIO_TARGET.TARGET_BUDGET)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_UPDATED_BY", OPR_RATIO_TARGET.UPDATED_BY)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@MSG", SqlDbType.VarChar, 250)).Direction = System.Data.ParameterDirection.Output;
                db.ExecuteNonQuery();

                returnValue = db.command.Parameters["@MSG"].Value.ToString();
                db.CloseConnection();

                return returnValue;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }


        #endregion

        #region FILM PRODUCTION HOLIDAY
        public DataTable GET_PRODUCTION_HOLIDAY(string year, string month)
        {
            try
            {
                DataTable dataTable = new DataTable();

                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_PRODUCTION_HOLIDAY";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_YEAR", year)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_MONTH", month)).Direction = System.Data.ParameterDirection.Input;
                
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int MAINT_PRODUCTION_HOLIDAY(string action, DateTime holidayDate, string createdBy)
        {
            try
            {
                int result = 0;

                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_PRODUCTION_HOLIDAY_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_ACTION", action)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_DATE", holidayDate)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATED_BY", createdBy)).Direction = System.Data.ParameterDirection.Input;



                var readr = db.command.ExecuteReader();

                while (readr.Read())
                {
                    result = (int)readr["RESULT"];
                }
                db.CloseConnection();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM SALES ORDER SITUATION GROUP CREATION
        public DataTable GET_SALES_GROUP()
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_GET_SALES_GROUP";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public DataTable GET_RPT_GROUP()
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_GET_RPT_GROUP";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int SALES_GROUP_MAINT(string id, string prodLine, string groupName, string other, string rptGroup, string recType, string createdBy, string createdLoc)
        {
            int result = 0;
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SALES_GROUP_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_ID", id)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_PRODLINE", prodLine)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_GROUPNAME", groupName)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_OTHER", other)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_RPTGROUP", rptGroup)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_RECTYPE", recType)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdBy)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDLOC", createdLoc)).Direction = System.Data.ParameterDirection.Input;


                var readr = db.command.ExecuteReader();

                while (readr.Read())
                {
                    result = (int)readr["RESULT"];
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
        #endregion

        #region FILM MACHINE STOPPAGE
        public DataTable GET_FILM_MACHINE_STOPPAGE(DateTime period)
        {
            try
            {
                DataTable dataTable = new DataTable();

                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_OPR_RATIO_STOPPAGE_LST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;

                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_FILM_MACHINE_STOPPAGE(string period, string item, string target, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_OPR_RATIO_STOPPAGE_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_ITEM", item)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TARGET", target)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM LMI PRODUCTIVITY TARGET SETTING
        public DataTable GET_PRODUCTIVITY_TARGET(string period)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_PRODUCTIVITY_TGT_LST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;

                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_PRODUCTIVITY_TARGET(string period, string fmmcode, decimal target, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_PRODUCTIVITY_TGT_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmcode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TARGET", target)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM STOCK CONTROL LIST BUDGET SETTING
        public DataTable GET_STOCK_CONTROL_BUDGET(string period)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_STOCK_CONTROL_BDG_LST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;

                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_STOCK_CONTROL_BUDGET(string period, string prodLine, decimal budget, decimal target, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_STOCK_CONTROL_BDG_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_PROD_LINE", prodLine)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_BUDGET", budget)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TARGET", target)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM FMMCCODE A LIST BUDGET SETTING
        public DataTable GET_FMMCCODE_A(string period)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_FMMCCODE_A_LIST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;

                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_FMMCCODE_A(string period, string fmmccode, string prod, decimal budget, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_FMMCCODE_A_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_PROD", prod)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_BUDGET", budget)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM FMMCCODE OTHER LIST SETTING
        public DataTable GET_FMMCCODE_O(string period)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_FMMCCODE_O_LIST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;

                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_FMMCCODE_O(string period, string fmmccode, decimal budget, decimal waste, decimal fixd, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_FMMCCODE_O_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_BUDGET", budget)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_WASTE", waste)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FIXED", fixd)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM DAILY ACT TYPE OTHER LIST SETTING
        public DataTable GET_DAILY_ACT_TYPE_O(string period, string fmmccode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_ACT_TYPE_O_LIST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_DAILY_ACT_TYPE_O(string period, string fmmccode, string thick, string type, decimal cract, decimal bmixact, decimal speedPlan, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_ACT_TYPE_O_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_THICK", thick)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TYPE", type)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CRACT", cract)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_BMIXACT", bmixact)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_SPEEDPLAN", speedPlan)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM DAILY BUDGET TIME LIST SETTING
        public DataTable GET_DAILY_BUDGET_TIME(string period, string fmmccode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_D_BUDGET_T_LIST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_DAILY_BUDGET_TIME(string period, string fmmccode, decimal prod, decimal utility, decimal test, decimal type, decimal filter, decimal film, decimal mctrouble, decimal cleaning, decimal others, decimal sd, decimal wait, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_D_BUDGET_T_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_PROD", prod)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_UTILITY", utility)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TEST", test)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TYPE", type)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FILTER", filter)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FILM", film)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_MCTROUBLE", mctrouble)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CLEANING", cleaning)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_OTHERS", others)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_SD", sd)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_WAIT", wait)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM TARGET
        public DataTable GET_TARGET(string period)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_SELFEF_TARGET_SEL";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
            
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_TARGET(string period, string fmmccode, decimal target, string rectype, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_SELFEF_TARGET_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TARGET", target)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_RECTYPE", rectype)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM DAILY PLAN BUDGET LIST SETTING
        public DataTable GET_DAILY_PLAN_BUDGET(string period, string fmmccode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_PLAN_BUDGET_LIST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_DAILY_PLAN_BUDGET(string period, string fmmccode, string thick, string type, decimal plan, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_PLAN_BUDGE_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_THICK", thick)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TYPE", type)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_PLAN", plan)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM DAILY PLAN TYPE LIST SETTING
        public DataTable GET_DAILY_PLAN_TYPE(string period, string fmmccode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_PLAN_TYPE_LIST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_DAILY_PLAN_TYPE(string period, string fmmccode, string thick, string type, decimal plan, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_PLAN_TYPE_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_THICK", thick)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TYPE", type)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_PLAN", plan)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM PROD LIST SETTING
        public DataTable GET_PROD()
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_PROD_LIST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_PROD(string prod, string rectype, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_PROD_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PROD", prod)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_RECTYPE", rectype)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM TYPE LIST SETTING
        public DataTable GET_TYPE(string fmmccode)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_TYPE_LIST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_TYPE(string fmmccode, string seq, string type, string thick, string rectype, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_TYPE_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_SEQ", seq)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TYPE", type)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_THICK", thick)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_RECTYPE", rectype)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region FILM RAW BALANCE LIST SETTING
        public DataTable GET_RAW_BALANCE(string period, string fmmccode, string prod)
        {
            try
            {
                DataTable dataTable = new DataTable();
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_RAW_BAL_LIST";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_PROD", prod)).Direction = System.Data.ParameterDirection.Input;
                var readr = db.command.ExecuteReader();
                dataTable.Load(readr);
                db.CloseConnection();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UPDATE_RAW_BALANCE(string period, string fmmccode, string prod, string thick, string type, decimal plan, string createdby)
        {
            try
            {
                db.OpenConnection();
                db.command.CommandText = "PSP_LMI_MM_SELFEF_RAW_BAL_MAINT";
                db.command.CommandType = CommandType.StoredProcedure;
                db.command.CommandTimeout = 0;
                db.command.Parameters.Clear();
                db.command.Parameters.Add(new SqlParameter("@P_PERIOD", period)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_FMMCCODE", fmmccode)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_PROD", prod)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_THICK", thick)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_TYPE", type)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_PLAN", plan)).Direction = System.Data.ParameterDirection.Input;
                db.command.Parameters.Add(new SqlParameter("@P_CREATEDBY", createdby)).Direction = System.Data.ParameterDirection.Input;
                db.ExecuteNonQuery();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}