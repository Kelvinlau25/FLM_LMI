using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;
using PAB.Helper_Code.Objects;
using PAB.Helpers;

namespace MstMainModel
{
    public class MM_OPR_RATIO_TARGET
    {
        [Required(ErrorMessage = "The Year field is required.")]
        public string YEAR { get; set; }
        [Required(ErrorMessage = "The Month field is required.")]
        public string MONTH { get; set; }
        public List<OPR_RATIO_TARGET> LIST_OPR_RATIO_TARGET { get; set; }
    }

    public class OPR_RATIO_TARGET
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string ITEM { get; set; }
        public double TARGET_BUDGET { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_LOC { get; set; }
    }

    public class MM_FILM_PRODUCTION_HOLIDAYS
    {
        public string ACTION { get; set; }
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime DATETIME { get; set; }
        public List<FILM_PRODUCTION_HOLIDAYS> LIST_FILM_PRODUCTION_HOLIDAYS { get; set; }
    }

    public class FILM_PRODUCTION_HOLIDAYS
    {
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime HOLIDAY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_DATE { get; set; }
    }

    public class MM_OPR_RATIO_STOPPAGE
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime PERIOD { get; set; } 
        public List<LMI_OPR_RATIO_STOPPAGE> LIST_OPR_RATIO_STOPPAGE { get; set; }
    }

    public class LMI_OPR_RATIO_STOPPAGE
    {
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime SDATE { get; set; }
        public string ITEM { get; set; }
        public decimal STOPPAGE_MINUTE { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_DATE { get; set; }
    }

    public class MM_PRODUCTIVITY_TARGET
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public List<LMI_PRODUCTIVITY_TARGET> LIST_LMI_PRODUCTIVITY_TARGET { get; set; }
    }

    public class LMI_PRODUCTIVITY_TARGET
    {
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime SDATE { get; set; }
        public decimal TARGET1 { get; set; }
        public decimal TARGET2 { get; set; }
        public decimal TARGET3 { get; set; }
    }

    public class MM_STOCK_CONTROL_LIST_BUDGET
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public List<LMI_STOCK_CONTROL_LIST_BUDGET> LIST_LMI_STOCK_CONTROL_LIST_BUDGET { get; set; }
    }

    public class LMI_STOCK_CONTROL_LIST_BUDGET
    {
        public string SDATE { get; set; }
        public string PROD_LINE { get; set; }
        public decimal BUDGET { get; set; }
        public decimal TARGET { get; set; }
    }

    public class MM_FMMCCODE_BUDGET
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public List<LMI_FMMCCODE_BUDGET> LIST_LMI_FMMCCODE_BUDGET { get; set; }
    }

    public class LMI_FMMCCODE_BUDGET
    {
        public string YEAR_MONTH { get; set; }
        public string PROD { get; set; }
        public decimal BUDGET1 { get; set; }
        public decimal BUDGET2 { get; set; }
        public decimal BUDGET3 { get; set; }
    }

    public class MM_FMMCCODE_BUDGET_OTHER
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public List<LMI_FMMCCODE_BUDGET_OTHER> LIST_LMI_FMMCCODE_BUDGET_OTHER { get; set; }
    }

    public class LMI_FMMCCODE_BUDGET_OTHER
    {
        public string YEAR_MONTH { get; set; }
        public string FMMCCODE { get; set; }
        public decimal B_BUDGET { get; set; }
        public decimal C_WASTE { get; set; }
        public decimal FIXED_COST { get; set; }
    }

    public class MM_DAILY_ACT_TYPE_OTHER
    {
        public string ACTIONTYPE { get; set; }
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string FMMCCODE { get; set; }
        public List<LMI_DAILY_ACT_TYPE_OTHER> LIST_LMI_DAILY_ACT_TYPE_OTHER { get; set; }
    }

    public class LMI_DAILY_ACT_TYPE_OTHER
    {
        public int SEQ { get; set; }
        public string THICK { get; set; }
        public string TYPE { get; set; }
        public decimal CR_ACT { get; set; }
        public decimal BMIX_ACT { get; set; }
        public decimal SPEED_PLAN { get; set; }
    }

    public class MM_DAILY_BUDGET_TIME
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string FMMCCODE { get; set; }
        public List<LMI_DAILY_BUDGET_TIME> LIST_LMI_DAILY_BUDGET_TIME { get; set; }
    }

    public class LMI_DAILY_BUDGET_TIME
    {
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime SDATE { get; set; }
        public decimal PROD_DAYS { get; set; }
        public decimal UTILITY_DAYS { get; set; }
        public decimal TEST_DAYS { get; set; }
        public decimal TYPE_CHANGE { get; set; }
        public decimal FITLER_CHANGE { get; set; }
        public decimal FILM_BREAK { get; set; }
        public decimal MC_TROUBLE { get; set; }
        public decimal CLEANING { get; set; }
        public decimal OTHERS { get; set; }
        public decimal SD { get; set; }
        public decimal WAIT { get; set; }
    }

    public class MM_LMI_TARGET
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public List<LMI_TARGET> LIST_LMI_TARGET { get; set; }
    }

    public class LMI_TARGET
    {
        public decimal TARGET { get; set; }
        public string FMMCCODE { get; set; }
    }

    public class MM_DAILY_PLAN_TYPE
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string DAY { get; set; }
        public string FMMCCODE { get; set; }
        public List<LMI_DAILY_PLAN_TYPE> LIST_LMI_DAILY_PLAN_TYPE { get; set; }
    }
    
    public class LMI_DAILY_PLAN_TYPE
    {
        public int SEQ { get; set; }
        public string THICK { get; set; }
        public string TYPE { get; set; }
        public decimal PLAN { get; set; }
    }

    public class MM_DAILY_PLAN_BUDGET
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string FMMCCODE { get; set; }
        public List<LMI_DAILY_PLAN_BUDGET> LIST_LMI_DAILY_PLAN_BUDGET { get; set; }
    }

    public class LMI_DAILY_PLAN_BUDGET
    {
        public int SEQ { get; set; }
        public string THICK { get; set; }
        public string TYPE { get; set; }
        public decimal PLAN { get; set; }
    }

    public class MM_PROD
    {
        public string PROD { get; set; }
        public string REC_TYPE { get; set; }
        public List<LMI_PROD> LIST_LMI_PROD { get; set; }
    }

    public class LMI_PROD
    {
        public string PROD { get; set; }
    }

    public class MM_TYPE
    {
        public string FMMCCODE { get; set; }
        public string SEQ { get; set; }
        public string THICK { get; set; }
        public string TYPE { get; set; }
        public string REC_TYPE { get; set; }
        public List<LMI_TYPE> LIST_LMI_TYPE { get; set; }
    }

    public class LMI_TYPE
    {
        public string THICK { get; set; }
        public string TYPE { get; set; }
        public int PLAN { get; set; }
    }

    public class MM_RAW_BALANCE
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string PROD { get; set; }
        public string FMMCCODE { get; set; }
        public List<LMI_RAW_BALANCE> LIST_LMI_RAW_BALANCE { get; set; }
    }

    public class LMI_RAW_BALANCE
    {
        public string THICK { get; set; }
        public string TYPE { get; set; }
        public decimal PLAN { get; set; }
    }

    public class MM_SALES_ORDER_GROUP
    {
        public string ACTION { get; set; }
        public string PROD_LINE { get; set; }
        public string GROUP_NAME { get; set; }
        public string RPT_GROUP { get; set; }
        public string ID_LMI_SALES_GROUP { get; set; }
        public List<LMI_SALES_ORDER_GROUP> LIST_LMI_SALES_ORDER_GROUP { get; set; }
    }

    public class LMI_SALES_ORDER_GROUP
    {
        public int ID_LMI_SALES_GROUP { get; set; }
        public string PROD_LINE { get; set; }
        public string GROUP_NAME { get; set; }
        public string RPT_GROUP { get; set; }
    }

    public class LMI_RPT_SALES_ORDER_GROUP
    {
        public string RPT_GROUP { get; set; }
    }

    public class DB : DatabaseModel.Database
    {
        public DB()
        {

        }
    }
}