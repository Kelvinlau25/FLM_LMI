using MstMainModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MIB_FILM_CLD_MM_MVC.Extensions;
using MIB_FILM_CLD_MM_MVC.Filters;
using PAB.Helper_Code.Objects;
using PAB.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PAB.Controllers
{

    public class MstmainController : Controller
    {
        public CommonRepo CommonRepo = new CommonRepo();
        public MMRepo MMRepo = new MMRepo();

        #region OPR RATIO MASTER MAINTENANCE
        [SessionExpireFilter]
        public ActionResult OPR_RATIO_TARGET()
        {
            MM_OPR_RATIO_TARGET MM_OPR_RATIO_TARGET = new MM_OPR_RATIO_TARGET();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_OPR_RATIO_TARGET.YEAR == null)
                MM_OPR_RATIO_TARGET.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_OPR_RATIO_TARGET.MONTH == null)
                MM_OPR_RATIO_TARGET.MONTH = DateTime.Now.ToString("MM");

            DataTable dtOPRRatio = MMRepo.OPR_RATIO_TARGET_SEL(MM_OPR_RATIO_TARGET);
            MM_OPR_RATIO_TARGET.LIST_OPR_RATIO_TARGET = CommonRepo.ConvertToList<OPR_RATIO_TARGET>(dtOPRRatio);

            return View(MM_OPR_RATIO_TARGET);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult OPR_RATIO_TARGET(MM_OPR_RATIO_TARGET model)
        {
            MM_OPR_RATIO_TARGET MM_OPR_RATIO_TARGET = new MM_OPR_RATIO_TARGET();
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            DataTable dtOPRRatio = MMRepo.OPR_RATIO_TARGET_SEL(model);
            MM_OPR_RATIO_TARGET.LIST_OPR_RATIO_TARGET = CommonRepo.ConvertToList<OPR_RATIO_TARGET>(dtOPRRatio);

            return PartialView("_OprRatioTargetPartial",MM_OPR_RATIO_TARGET);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult OPR_RATIO_TARGET_MAINT(OPR_RATIO_TARGET OPR_RATIO_TARGET)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");
            //Debug use only
            //OPR_RATIO_TARGET.UPDATED_BY = "9145";
            OPR_RATIO_TARGET.UPDATED_BY = ACLUser.EMP_NO;
            OPR_RATIO_TARGET.UPDATED_LOC = HttpContext.Connection.RemoteIpAddress?.ToString();

            return Json(MMRepo.OPR_RATIO_TARGET_MAINT(OPR_RATIO_TARGET));
        }

        #endregion

        #region FILM PRODUCTION HOLIDAY
        [SessionExpireFilter]
        public ActionResult FILM_PRODUCTION_HOLIDAYS()
        {
            MM_FILM_PRODUCTION_HOLIDAYS MM_FILM_PRODUCTION_HOLIDAY = new MM_FILM_PRODUCTION_HOLIDAYS();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_FILM_PRODUCTION_HOLIDAY.YEAR == null)
                MM_FILM_PRODUCTION_HOLIDAY.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_FILM_PRODUCTION_HOLIDAY.MONTH == null)
                MM_FILM_PRODUCTION_HOLIDAY.MONTH = DateTime.Now.ToString("MM");

            DataTable filmProdHoliday = MMRepo.GET_PRODUCTION_HOLIDAY(MM_FILM_PRODUCTION_HOLIDAY.YEAR, MM_FILM_PRODUCTION_HOLIDAY.MONTH);
            MM_FILM_PRODUCTION_HOLIDAY.LIST_FILM_PRODUCTION_HOLIDAYS = CommonRepo.ConvertToList<FILM_PRODUCTION_HOLIDAYS>(filmProdHoliday);

            return View(MM_FILM_PRODUCTION_HOLIDAY);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_PRODUCTION_HOLIDAYS(MM_FILM_PRODUCTION_HOLIDAYS model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            DataTable filmProdHoliday = MMRepo.GET_PRODUCTION_HOLIDAY(model.YEAR, model.MONTH);
            model.LIST_FILM_PRODUCTION_HOLIDAYS = CommonRepo.ConvertToList<FILM_PRODUCTION_HOLIDAYS>(filmProdHoliday);

            return PartialView("_FilmProductionHolidaysPartial", model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_PRODUCTION_HOLIDAYS_MAINT(MM_FILM_PRODUCTION_HOLIDAYS model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");
            //Debug use only
            //OPR_RATIO_TARGET.UPDATED_BY = "9145";
            var pUser = ACLUser.EMP_NO;
            int result = 0;
            if (ModelState.IsValid)
            {
                result = MMRepo.MAINT_PRODUCTION_HOLIDAY(model.ACTION, model.DATETIME, pUser);
            }

            if (model.YEAR != null && model.MONTH != null) {
                DateTime dtstart = new DateTime(Int32.Parse(model.YEAR), Int32.Parse(model.MONTH), 1,0,0,0);
                DateTime dtend = new DateTime(Int32.Parse(model.YEAR), Int32.Parse(model.MONTH), DateTime.DaysInMonth(Int32.Parse(model.YEAR), Int32.Parse(model.MONTH)),23,59,59);
                if (model.DATETIME >= dtstart && model.DATETIME <= dtend)
                {
                   
                }
                else {
                    HttpContext.Response.StatusCode = 500;
                    return Json(new { status = "error", message = "Invalid date selecting! Please select date between " + dtstart.ToString("dd-MMM-yyyy") + " to " + dtend.ToString("dd-MMM-yyyy") });
                }
            }

            MM_FILM_PRODUCTION_HOLIDAYS MM_FILM_PRODUCTION_HOLIDAY = new MM_FILM_PRODUCTION_HOLIDAYS();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_FILM_PRODUCTION_HOLIDAY.YEAR == null)
                MM_FILM_PRODUCTION_HOLIDAY.YEAR = model.DATETIME.ToString("yyyy");

            if (MM_FILM_PRODUCTION_HOLIDAY.MONTH == null)
                MM_FILM_PRODUCTION_HOLIDAY.MONTH = model.DATETIME.ToString("MM");

            DataTable filmProdHoliday = MMRepo.GET_PRODUCTION_HOLIDAY(MM_FILM_PRODUCTION_HOLIDAY.YEAR, MM_FILM_PRODUCTION_HOLIDAY.MONTH);
            MM_FILM_PRODUCTION_HOLIDAY.LIST_FILM_PRODUCTION_HOLIDAYS = CommonRepo.ConvertToList<FILM_PRODUCTION_HOLIDAYS>(filmProdHoliday);


            ViewBag.Result = "Data saved successfully";

            return PartialView("_FilmProductionHolidaysPartial", MM_FILM_PRODUCTION_HOLIDAY);
        }

        [HttpGet]
        [SessionExpireFilter]
        public ActionResult FILM_PRODUCTION_HOLIDAYS_MAINT(string ACTION, DateTime DATETIME)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");
            //Debug use only
            //OPR_RATIO_TARGET.UPDATED_BY = "9145";
            var pUser = ACLUser.EMP_NO;
            int result = 0;
            if (ModelState.IsValid)
            {
                result = MMRepo.MAINT_PRODUCTION_HOLIDAY(ACTION, DATETIME, pUser);
            }
            return Json(result);
        }
        #endregion

        #region FILM MIB MACHINE STOPPAGE SETTING
        [SessionExpireFilter]
        public ActionResult FILM_MACHINE_STOPPAGE()
        {
            MM_OPR_RATIO_STOPPAGE FILM_MACHINE_STOPPAGE = new MM_OPR_RATIO_STOPPAGE();

            var dateTime = DateTime.Now;
            FILM_MACHINE_STOPPAGE.PERIOD = dateTime;

            DataTable filmMacStop = MMRepo.GET_FILM_MACHINE_STOPPAGE(dateTime);
            FILM_MACHINE_STOPPAGE.LIST_OPR_RATIO_STOPPAGE = CommonRepo.ConvertToList<LMI_OPR_RATIO_STOPPAGE>(filmMacStop);

            return View(FILM_MACHINE_STOPPAGE);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_MACHINE_STOPPAGE(MM_OPR_RATIO_STOPPAGE model)
        {
            DataTable filmMacStop = MMRepo.GET_FILM_MACHINE_STOPPAGE(model.PERIOD);
            model.LIST_OPR_RATIO_STOPPAGE = CommonRepo.ConvertToList<LMI_OPR_RATIO_STOPPAGE>(filmMacStop);

            return PartialView("_FilmMachineStoppagePartial",model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_MACHINE_STOPPAGE_MAINT([FromBody] MM_OPR_RATIO_STOPPAGE model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            foreach (var m in model.LIST_OPR_RATIO_STOPPAGE)
            {
                //Debug use only
                //var updatedBy = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_FILM_MACHINE_STOPPAGE(model.PERIOD.ToString("yyyy-MM-dd"),m.ITEM,m.STOPPAGE_MINUTE.ToString(),updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM LMI PRODUCTIVITY TARGET SETTING
        [SessionExpireFilter]
        public ActionResult FILM_LMI_PRODUCTIVITY_TARGET()
        {
            MM_PRODUCTIVITY_TARGET MM_FILM_PRODUCTIVITY_TARGET = new MM_PRODUCTIVITY_TARGET();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_FILM_PRODUCTIVITY_TARGET.YEAR == null)
                MM_FILM_PRODUCTIVITY_TARGET.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_FILM_PRODUCTIVITY_TARGET.MONTH == null)
                MM_FILM_PRODUCTIVITY_TARGET.MONTH = DateTime.Now.ToString("MM");

            string period = "";

            period = String.Concat(MM_FILM_PRODUCTIVITY_TARGET.YEAR,"-", MM_FILM_PRODUCTIVITY_TARGET.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_PRODUCTIVITY_TARGET(period);
            MM_FILM_PRODUCTIVITY_TARGET.LIST_LMI_PRODUCTIVITY_TARGET = CommonRepo.ConvertToList<LMI_PRODUCTIVITY_TARGET>(filmProdHoliday);

            return View(MM_FILM_PRODUCTIVITY_TARGET);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_LMI_PRODUCTIVITY_TARGET([FromBody] MM_PRODUCTIVITY_TARGET model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            string period = "";

            period = String.Concat(model.YEAR, "-", model.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_PRODUCTIVITY_TARGET(period);
            model.LIST_LMI_PRODUCTIVITY_TARGET = CommonRepo.ConvertToList<LMI_PRODUCTIVITY_TARGET>(filmProdHoliday);

            return PartialView("_FilmLmiProductivityTargetPartial",model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_LMI_PRODUCTIVITY_TARGET_MAINT([FromBody] List<LMI_PRODUCTIVITY_TARGET> model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            foreach(var m in model)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_PRODUCTIVITY_TARGET(m.SDATE.ToString("yyyy-MM-dd"), "F1", m.TARGET1, updatedBy);
                MMRepo.UPDATE_PRODUCTIVITY_TARGET(m.SDATE.ToString("yyyy-MM-dd"), "F2", m.TARGET2, updatedBy);
                MMRepo.UPDATE_PRODUCTIVITY_TARGET(m.SDATE.ToString("yyyy-MM-dd"), "F3", m.TARGET3, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM SALES ORDER SITUATION GROUP CREATION
        [SessionExpireFilter]
        public ActionResult SALES_ORDER_SITUATION_GROUP()
        {
            MM_SALES_ORDER_GROUP MM_SALES_ORDER_GROUP = new MM_SALES_ORDER_GROUP();

            DataTable rptGroup = MMRepo.GET_RPT_GROUP();
            var rptGroupList= CommonRepo.ConvertToList<LMI_RPT_SALES_ORDER_GROUP>(rptGroup).Select( x => x.RPT_GROUP).ToList();

            var prodLineList = new List<string>();
            prodLineList.Add("F1");
            prodLineList.Add("F2");
            prodLineList.Add("F3");
            prodLineList.Add("MFP");
            prodLineList.Sort();

            ViewBag.ProdLineDDL = prodLineList;

            rptGroupList.Add("-");

            ViewBag.ReportGroupDDL = rptGroupList;

            DataTable salesGroup = MMRepo.GET_SALES_GROUP();
            MM_SALES_ORDER_GROUP.LIST_LMI_SALES_ORDER_GROUP = CommonRepo.ConvertToList<LMI_SALES_ORDER_GROUP>(salesGroup);

            return View(MM_SALES_ORDER_GROUP);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult SALES_ORDER_SITUATION_GROUP_MAINT(MM_SALES_ORDER_GROUP model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");
            //Debug use only
            //OPR_RATIO_TARGET.UPDATED_BY = "9145";
            var updatedBy = ACLUser.EMP_NO;

            string _other = model.RPT_GROUP == "-" ? "1" : "0";
            string _ipLoc = HttpContext.Connection.RemoteIpAddress?.ToString();


            model.PROD_LINE = model.PROD_LINE == null ? "" : model.PROD_LINE;
            model.GROUP_NAME = model.GROUP_NAME == null ? "" : model.GROUP_NAME;
            model.RPT_GROUP = model.RPT_GROUP == null ? "" : model.RPT_GROUP;

            int result = MMRepo.SALES_GROUP_MAINT(model.ID_LMI_SALES_GROUP.ToString(), model.PROD_LINE, model.GROUP_NAME, _other, model.RPT_GROUP, model.ACTION, updatedBy, _ipLoc);


            MM_SALES_ORDER_GROUP MM_SALES_ORDER_GROUP = new MM_SALES_ORDER_GROUP();

            DataTable rptGroup = MMRepo.GET_RPT_GROUP();
            var rptGroupList = CommonRepo.ConvertToList<LMI_RPT_SALES_ORDER_GROUP>(rptGroup).Select(x => x.RPT_GROUP).ToList();

            var prodLineList = new List<string>();
            prodLineList.Add("F1");
            prodLineList.Add("F2");
            prodLineList.Add("F3");
            prodLineList.Add("MFP");
            prodLineList.Sort();

            ViewBag.ProdLineDDL = prodLineList;

            rptGroupList.Add("-");

            ViewBag.ReportGroupDDL = rptGroupList;

            DataTable salesGroup = MMRepo.GET_SALES_GROUP();
            MM_SALES_ORDER_GROUP.LIST_LMI_SALES_ORDER_GROUP = CommonRepo.ConvertToList<LMI_SALES_ORDER_GROUP>(salesGroup);

            if(result == 1)
            {
                ViewBag.ResultSaved = "Record successfully saved!";
            }
            else if (result == 2)
            {
                ViewBag.ResultSaved = "Product Line "+ model.PROD_LINE + " has duplicate Report Group "+ model.RPT_GROUP;

            }
            else
            {
                ViewBag.ResultSaved = "Error when updating the record";

            }

            return PartialView("_SalesOrderSituationGroupPartial", MM_SALES_ORDER_GROUP);
        }
        #endregion

        #region FILM STOCK CONTROL LIST BUDGET SETTING
        [SessionExpireFilter]
        public ActionResult STOCK_CONTROL_LIST_BUDGET()
        {
            MM_STOCK_CONTROL_LIST_BUDGET MM_STOCK_CONTROL_LIST_BUDGET = new MM_STOCK_CONTROL_LIST_BUDGET();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_STOCK_CONTROL_LIST_BUDGET.YEAR == null)
                MM_STOCK_CONTROL_LIST_BUDGET.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_STOCK_CONTROL_LIST_BUDGET.MONTH == null)
                MM_STOCK_CONTROL_LIST_BUDGET.MONTH = DateTime.Now.ToString("MM");

            string period = "";

            period = String.Concat(MM_STOCK_CONTROL_LIST_BUDGET.YEAR, "-", MM_STOCK_CONTROL_LIST_BUDGET.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_STOCK_CONTROL_BUDGET(period);
            MM_STOCK_CONTROL_LIST_BUDGET.LIST_LMI_STOCK_CONTROL_LIST_BUDGET = CommonRepo.ConvertToList<LMI_STOCK_CONTROL_LIST_BUDGET>(filmProdHoliday);

            return View(MM_STOCK_CONTROL_LIST_BUDGET);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult STOCK_CONTROL_LIST_BUDGET(MM_STOCK_CONTROL_LIST_BUDGET model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            string period = "";

            period = String.Concat(model.YEAR, "-", model.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_STOCK_CONTROL_BUDGET(period);
            model.LIST_LMI_STOCK_CONTROL_LIST_BUDGET = CommonRepo.ConvertToList<LMI_STOCK_CONTROL_LIST_BUDGET>(filmProdHoliday);

            return PartialView("_StockControlListBudgetPartial",model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult STOCK_CONTROL_LIST_BUDGET_MAINT([FromBody] List<LMI_STOCK_CONTROL_LIST_BUDGET> model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            foreach (var m in model)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_STOCK_CONTROL_BUDGET(m.SDATE, m.PROD_LINE, m.BUDGET, m.TARGET, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM FMMCCODE A LIST BUDGET SETTING
        [SessionExpireFilter]
        public ActionResult FMMCCODE_BUDGET()
        {
            MM_FMMCCODE_BUDGET MM_FMMCCODE_BUDGET = new MM_FMMCCODE_BUDGET();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_FMMCCODE_BUDGET.YEAR == null)
                MM_FMMCCODE_BUDGET.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_FMMCCODE_BUDGET.MONTH == null)
                MM_FMMCCODE_BUDGET.MONTH = DateTime.Now.ToString("MM");

            string period = "";

            period = String.Concat(MM_FMMCCODE_BUDGET.YEAR, "-", MM_FMMCCODE_BUDGET.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_FMMCCODE_A(period);
            MM_FMMCCODE_BUDGET.LIST_LMI_FMMCCODE_BUDGET = CommonRepo.ConvertToList<LMI_FMMCCODE_BUDGET>(filmProdHoliday);

            return View(MM_FMMCCODE_BUDGET);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FMMCCODE_BUDGET(MM_FMMCCODE_BUDGET model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            string period = "";

            period = String.Concat(model.YEAR, "-", model.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_FMMCCODE_A(period);
            model.LIST_LMI_FMMCCODE_BUDGET = CommonRepo.ConvertToList<LMI_FMMCCODE_BUDGET>(filmProdHoliday);

            return PartialView("_FmmccodeBudgetPartial",model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FMMCCODE_BUDGET_MAINT([FromBody] List<LMI_FMMCCODE_BUDGET> model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            foreach (var m in model)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_FMMCCODE_A(m.YEAR_MONTH, "F1", m.PROD, m.BUDGET1, updatedBy);
                MMRepo.UPDATE_FMMCCODE_A(m.YEAR_MONTH, "F2", m.PROD, m.BUDGET1, updatedBy);
                MMRepo.UPDATE_FMMCCODE_A(m.YEAR_MONTH, "F3", m.PROD, m.BUDGET1, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM FMMCCODE OTHER LIST SETTING
        [SessionExpireFilter]
        public ActionResult FMMCCODE_OTHER()
        {
            MM_FMMCCODE_BUDGET_OTHER MM_FMMCCODE_BUDGET_OTHER = new MM_FMMCCODE_BUDGET_OTHER();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_FMMCCODE_BUDGET_OTHER.YEAR == null)
                MM_FMMCCODE_BUDGET_OTHER.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_FMMCCODE_BUDGET_OTHER.MONTH == null)
                MM_FMMCCODE_BUDGET_OTHER.MONTH = DateTime.Now.ToString("MM");

            string period = "";

            period = String.Concat(MM_FMMCCODE_BUDGET_OTHER.YEAR, "-", MM_FMMCCODE_BUDGET_OTHER.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_FMMCCODE_O(period);
            MM_FMMCCODE_BUDGET_OTHER.LIST_LMI_FMMCCODE_BUDGET_OTHER = CommonRepo.ConvertToList<LMI_FMMCCODE_BUDGET_OTHER>(filmProdHoliday);

            return View(MM_FMMCCODE_BUDGET_OTHER);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FMMCCODE_OTHER(MM_FMMCCODE_BUDGET_OTHER model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            string period = "";

            period = String.Concat(model.YEAR, "-", model.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_FMMCCODE_O(period);
            model.LIST_LMI_FMMCCODE_BUDGET_OTHER = CommonRepo.ConvertToList<LMI_FMMCCODE_BUDGET_OTHER>(filmProdHoliday);

            return PartialView("_FmmccodeOtherPartial", model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FMMCCODE_OTHER_MAINT([FromBody] List<LMI_FMMCCODE_BUDGET_OTHER> model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            foreach (var m in model)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_FMMCCODE_O(m.YEAR_MONTH, m.FMMCCODE, m.B_BUDGET, m.C_WASTE, m.FIXED_COST, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM DAILY ACT TYPE OTHER LIST SETTING
        [SessionExpireFilter]
        public ActionResult DAILY_ACT_TYPE_OTHER()
        {
            MM_DAILY_ACT_TYPE_OTHER MM_DAILY_ACT_TYPE_OTHER = new MM_DAILY_ACT_TYPE_OTHER();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_DAILY_ACT_TYPE_OTHER.YEAR == null)
                MM_DAILY_ACT_TYPE_OTHER.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_DAILY_ACT_TYPE_OTHER.MONTH == null)
                MM_DAILY_ACT_TYPE_OTHER.MONTH = DateTime.Now.ToString("MM");

            if (MM_DAILY_ACT_TYPE_OTHER.FMMCCODE == null)
                MM_DAILY_ACT_TYPE_OTHER.FMMCCODE = "F1";

            string period = "";
            string defaultFMMCCODE = "F1";

            period = String.Concat(MM_DAILY_ACT_TYPE_OTHER.YEAR, "-", MM_DAILY_ACT_TYPE_OTHER.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_DAILY_ACT_TYPE_O(period, defaultFMMCCODE);
            MM_DAILY_ACT_TYPE_OTHER.LIST_LMI_DAILY_ACT_TYPE_OTHER = CommonRepo.ConvertToList<LMI_DAILY_ACT_TYPE_OTHER>(filmProdHoliday);

            return View(MM_DAILY_ACT_TYPE_OTHER);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult DAILY_ACT_TYPE_OTHER(MM_DAILY_ACT_TYPE_OTHER model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            if (model.FMMCCODE == null)
                model.FMMCCODE = model.FMMCCODE;

            string period = "";
            string defaultFMMCCODE = model.FMMCCODE;

            period = String.Concat(model.YEAR, "-", model.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_DAILY_ACT_TYPE_O(period, defaultFMMCCODE);
            model.LIST_LMI_DAILY_ACT_TYPE_OTHER = CommonRepo.ConvertToList<LMI_DAILY_ACT_TYPE_OTHER>(filmProdHoliday);

            return PartialView("_DailyActTypeOtherPartial", model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult DAILY_ACT_TYPE_OTHER_MAINT([FromBody] MM_DAILY_ACT_TYPE_OTHER model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            string strPeriod = String.Concat(model.YEAR, "-", model.MONTH);
            string strFmmccode = model.FMMCCODE;
            foreach (var m in model.LIST_LMI_DAILY_ACT_TYPE_OTHER)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_DAILY_ACT_TYPE_O(strPeriod, strFmmccode, m.THICK, m.TYPE, m.CR_ACT, m.BMIX_ACT, m.SPEED_PLAN, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM DAILY BUDGET TIME LIST SETTING
        [SessionExpireFilter]
        public ActionResult DAILY_BUDGET_TIME()
        {
            MM_DAILY_BUDGET_TIME MM_DAILY_BUDGET_TIME = new MM_DAILY_BUDGET_TIME();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_DAILY_BUDGET_TIME.YEAR == null)
                MM_DAILY_BUDGET_TIME.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_DAILY_BUDGET_TIME.MONTH == null)
                MM_DAILY_BUDGET_TIME.MONTH = DateTime.Now.ToString("MM");

            if (MM_DAILY_BUDGET_TIME.FMMCCODE == null)
                MM_DAILY_BUDGET_TIME.FMMCCODE = "F1";

            string period = "";
            string defaultFMMCCODE = "F1";

            period = String.Concat(MM_DAILY_BUDGET_TIME.YEAR, "-", MM_DAILY_BUDGET_TIME.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_DAILY_BUDGET_TIME(period, defaultFMMCCODE);
            MM_DAILY_BUDGET_TIME.LIST_LMI_DAILY_BUDGET_TIME = CommonRepo.ConvertToList<LMI_DAILY_BUDGET_TIME>(filmProdHoliday);

            return View(MM_DAILY_BUDGET_TIME);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult DAILY_BUDGET_TIME(MM_DAILY_BUDGET_TIME model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            if (model.FMMCCODE == null)
                model.FMMCCODE = model.FMMCCODE;

            string period = "";

            period = String.Concat(model.YEAR, "-", model.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_DAILY_BUDGET_TIME(period, model.FMMCCODE);
            model.LIST_LMI_DAILY_BUDGET_TIME = CommonRepo.ConvertToList<LMI_DAILY_BUDGET_TIME>(filmProdHoliday);

            return PartialView("_DailyBudgetTimePartial", model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult DAILY_BUDGET_TIME_MAINT([FromBody] MM_DAILY_BUDGET_TIME model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            string strPeriod = String.Concat(model.YEAR, "-", model.MONTH);
            string strFmmccode = model.FMMCCODE;
            foreach (var m in model.LIST_LMI_DAILY_BUDGET_TIME)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_DAILY_BUDGET_TIME(m.SDATE.ToString("yyyy-MM-dd"), strFmmccode, m.PROD_DAYS, m.UTILITY_DAYS, m.TEST_DAYS, m.TYPE_CHANGE, m.FITLER_CHANGE, m.FILM_BREAK, m.MC_TROUBLE, m.CLEANING, m.OTHERS, m.SD, m.WAIT, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM DAILY PLAN BUDGET LIST SETTING
        [SessionExpireFilter]
        public ActionResult DAILY_PLAN_BUDGET()
        {
            MM_DAILY_PLAN_BUDGET MM_DAILY_PLAN_BUDGET = new MM_DAILY_PLAN_BUDGET();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_DAILY_PLAN_BUDGET.YEAR == null)
                MM_DAILY_PLAN_BUDGET.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_DAILY_PLAN_BUDGET.MONTH == null)
                MM_DAILY_PLAN_BUDGET.MONTH = DateTime.Now.ToString("MM");

            if (MM_DAILY_PLAN_BUDGET.FMMCCODE == null)
                MM_DAILY_PLAN_BUDGET.FMMCCODE = "F1";

            string period = "";
            string defaultFMMCCODE = "F1";

            period = String.Concat(MM_DAILY_PLAN_BUDGET.YEAR, "-", MM_DAILY_PLAN_BUDGET.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_DAILY_PLAN_BUDGET(period, defaultFMMCCODE);
            MM_DAILY_PLAN_BUDGET.LIST_LMI_DAILY_PLAN_BUDGET = CommonRepo.ConvertToList<LMI_DAILY_PLAN_BUDGET>(filmProdHoliday);

            return View(MM_DAILY_PLAN_BUDGET);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult DAILY_PLAN_BUDGET(MM_DAILY_PLAN_BUDGET model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            if (model.FMMCCODE == null)
                model.FMMCCODE = model.FMMCCODE;

            string period = "";
            string defaultFMMCCODE = model.FMMCCODE;

            period = String.Concat(model.YEAR, "-", model.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_DAILY_PLAN_BUDGET(period, defaultFMMCCODE);
            model.LIST_LMI_DAILY_PLAN_BUDGET = CommonRepo.ConvertToList<LMI_DAILY_PLAN_BUDGET>(filmProdHoliday);

            return PartialView("_DailyPlanBudgetPartial", model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult DAILY_PLAN_BUDGET_MAINT([FromBody] MM_DAILY_PLAN_BUDGET model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            string strPeriod = String.Concat(model.YEAR, "-", model.MONTH);
            string strFmmccode = model.FMMCCODE;
            foreach (var m in model.LIST_LMI_DAILY_PLAN_BUDGET)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_DAILY_PLAN_BUDGET(strPeriod, strFmmccode, m.THICK, m.TYPE, m.PLAN, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM DAILY PLAN TYPE LIST SETTING
        [SessionExpireFilter]
        public ActionResult DAILY_PLAN_TYPE()
        {
            MM_DAILY_PLAN_TYPE MM_DAILY_PLAN_TYPE = new MM_DAILY_PLAN_TYPE();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_DAILY_PLAN_TYPE.YEAR == null)
                MM_DAILY_PLAN_TYPE.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_DAILY_PLAN_TYPE.MONTH == null)
                MM_DAILY_PLAN_TYPE.MONTH = DateTime.Now.ToString("MM");

            if (MM_DAILY_PLAN_TYPE.DAY == null)
                MM_DAILY_PLAN_TYPE.DAY = DateTime.Now.ToString("dd");

            if (MM_DAILY_PLAN_TYPE.FMMCCODE == null)
                MM_DAILY_PLAN_TYPE.FMMCCODE = "F1";

            string period = "";
            string defaultFMMCCODE = "F1";

            period = String.Concat(MM_DAILY_PLAN_TYPE.YEAR, "-", MM_DAILY_PLAN_TYPE.MONTH, "-", MM_DAILY_PLAN_TYPE.DAY);
            DataTable filmProdHoliday = MMRepo.GET_DAILY_PLAN_TYPE(period, defaultFMMCCODE);
            MM_DAILY_PLAN_TYPE.LIST_LMI_DAILY_PLAN_TYPE = CommonRepo.ConvertToList<LMI_DAILY_PLAN_TYPE>(filmProdHoliday);

            return View(MM_DAILY_PLAN_TYPE);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult DAILY_PLAN_TYPE(MM_DAILY_PLAN_TYPE model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            if (model.DAY == null)
                model.DAY = model.DAY;

            if (model.FMMCCODE == null)
                model.FMMCCODE = model.FMMCCODE;

            string period = "";
            string defaultFMMCCODE = model.FMMCCODE;

            period = String.Concat(model.YEAR, "-", model.MONTH,"-",model.DAY);
            DataTable filmProdHoliday = MMRepo.GET_DAILY_PLAN_TYPE(period, defaultFMMCCODE);
            model.LIST_LMI_DAILY_PLAN_TYPE = CommonRepo.ConvertToList<LMI_DAILY_PLAN_TYPE>(filmProdHoliday);

            return PartialView("_DailyPlanTypePartial", model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult DAILY_PLAN_TYPE_MAINT([FromBody] MM_DAILY_PLAN_TYPE model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            string strPeriod = String.Concat(model.YEAR, "-", model.MONTH, "-", model.DAY);
            string strFmmccode = model.FMMCCODE;
            foreach (var m in model.LIST_LMI_DAILY_PLAN_TYPE)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_DAILY_PLAN_TYPE(strPeriod, strFmmccode, m.THICK, m.TYPE, m.PLAN, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM PROD LIST SETTING
        [SessionExpireFilter]
        public ActionResult FILM_PROD_LIST()
        {
            MM_PROD MM_PROD = new MM_PROD();
            DataTable filmProdHoliday = MMRepo.GET_PROD();
            MM_PROD.LIST_LMI_PROD = CommonRepo.ConvertToList<LMI_PROD>(filmProdHoliday);
            ViewBag.Message = TempData["Message"];
            return View(MM_PROD);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_PROD_LIST_MAINT(MM_PROD model)
        {
            MM_PROD MM_PROD = new MM_PROD();

            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");
            //Debug use only
            //OPR_RATIO_TARGET.UPDATED_BY = "9145";
            var updatedBy = ACLUser.EMP_NO;

            MMRepo.UPDATE_PROD(model.PROD, model.REC_TYPE, updatedBy);
            TempData["Message"] = "OK";
            return RedirectToAction("FILM_PROD_LIST");
        }
        #endregion

        #region FILM TYPE LIST SETTING
        [SessionExpireFilter]
        public ActionResult FILM_TYPE_LIST(string FMMCCODE)
        {
            MM_TYPE MM_TYPE = new MM_TYPE();

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;

            string defaultFMMCCODE = "F1";

            if (String.IsNullOrEmpty(FMMCCODE))
            {
                if (MM_TYPE.FMMCCODE == null)
                    MM_TYPE.FMMCCODE = "F1";
            }
            else
            {
                MM_TYPE.FMMCCODE = FMMCCODE;
                defaultFMMCCODE = FMMCCODE;
            }
            
            DataTable filmProdHoliday = MMRepo.GET_TYPE(defaultFMMCCODE);
            MM_TYPE.LIST_LMI_TYPE = CommonRepo.ConvertToList<LMI_TYPE>(filmProdHoliday);

            return View(MM_TYPE);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_TYPE_LIST_MAINT(MM_TYPE model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            string strFmmccode = model.FMMCCODE;
            //Debug use only
            //OPR_RATIO_TARGET.UPDATED_BY = "9145";
            var updatedBy = ACLUser.EMP_NO;
            MMRepo.UPDATE_TYPE(strFmmccode, model.SEQ, model.TYPE, model.THICK, model.REC_TYPE, updatedBy);


            MM_TYPE MM_TYPE = new MM_TYPE();

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            ViewBag.FmmccodeDDL = fmmccodeList;

            DataTable filmProdHoliday = MMRepo.GET_TYPE(strFmmccode);
            MM_TYPE.LIST_LMI_TYPE = CommonRepo.ConvertToList<LMI_TYPE>(filmProdHoliday);

            return Json(new { RESULT = "OK", FMMCODE = strFmmccode });
            //return PartialView("_FilmTypeListPartial", MM_TYPE);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_TYPE_LIST_MAINT2(MM_TYPE model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            string strFmmccode = model.FMMCCODE;

            foreach(var item in model.LIST_LMI_TYPE)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_TYPE(strFmmccode, item.PLAN.ToString(), item.TYPE, item.THICK, model.REC_TYPE, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM RAW BALANCE LIST SETTING
        [SessionExpireFilter]
        public ActionResult FILM_RAW_BALANCE()
        {
            MM_RAW_BALANCE MM_RAW_BALANCE = new MM_RAW_BALANCE();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            // TODO: prod list
            var prodList = new List<string>();

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;
            ViewBag.ProdDDL = prodList;

            if (MM_RAW_BALANCE.YEAR == null)
                MM_RAW_BALANCE.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_RAW_BALANCE.MONTH == null)
                MM_RAW_BALANCE.MONTH = DateTime.Now.ToString("MM");

            if (MM_RAW_BALANCE.FMMCCODE == null)
                MM_RAW_BALANCE.FMMCCODE = "F1";

            string period = "";
            string defaultFMMCCODE = "F1";

            period = String.Concat(MM_RAW_BALANCE.YEAR, "-", MM_RAW_BALANCE.MONTH);

            DataTable prodTable = MMRepo.GET_PROD();

            var prod = CommonRepo.ConvertToList<LMI_PROD>(prodTable);
            prodList.AddRange(prod.Select(p => p.PROD).ToList());

            var strProd = prodList.FirstOrDefault();

            if (MM_RAW_BALANCE.PROD == null)
                MM_RAW_BALANCE.PROD = strProd;

            DataTable filmProdHoliday = MMRepo.GET_RAW_BALANCE(period, defaultFMMCCODE, strProd);
            MM_RAW_BALANCE.LIST_LMI_RAW_BALANCE = CommonRepo.ConvertToList<LMI_RAW_BALANCE>(filmProdHoliday);

            return View(MM_RAW_BALANCE);
        }

        [HttpPost]
        public ActionResult FILM_RAW_BALANCE(MM_RAW_BALANCE model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            var fmmccodeList = new List<string>();
            fmmccodeList.Add("F1");
            fmmccodeList.Add("F2");
            fmmccodeList.Add("F3");

            // TODO: prod list
            var prodList = new List<string>();

            ViewBag.FmmccodeDDL = fmmccodeList;
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;
            ViewBag.ProdDDL = prodList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            if (model.FMMCCODE == null)
                model.FMMCCODE = model.FMMCCODE;

            string period = "";
            string defaultFMMCCODE = model.FMMCCODE;

            period = String.Concat(model.YEAR, "-", model.MONTH);

            DataTable prodTable = MMRepo.GET_PROD();

            var prod = CommonRepo.ConvertToList<LMI_PROD>(prodTable);
            prodList.AddRange(prod.Select(p => p.PROD).ToList());

            var strProd = model.PROD;

            if (model.PROD == null)
                model.PROD = strProd;

            DataTable filmProdHoliday = MMRepo.GET_RAW_BALANCE(period, defaultFMMCCODE, strProd);
            model.LIST_LMI_RAW_BALANCE = CommonRepo.ConvertToList<LMI_RAW_BALANCE>(filmProdHoliday);

            return PartialView("_FilmRawBalancePartial", model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_RAW_BALANCE_MAINT([FromBody] MM_RAW_BALANCE model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            string strPeriod = String.Concat(model.YEAR, "-", model.MONTH);
            string strFmmccode = model.FMMCCODE;
            foreach (var m in model.LIST_LMI_RAW_BALANCE)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_RAW_BALANCE(strPeriod, strFmmccode, model.PROD, m.THICK, m.TYPE, m.PLAN, updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion

        #region FILM TARGET SETTING
        [SessionExpireFilter]
        public ActionResult FILM_TARGET()
        {
            MM_LMI_TARGET MM_LMI_TARGET = new MM_LMI_TARGET();

            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }
            
            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (MM_LMI_TARGET.YEAR == null)
                MM_LMI_TARGET.YEAR = DateTime.Now.ToString("yyyy");

            if (MM_LMI_TARGET.MONTH == null)
                MM_LMI_TARGET.MONTH = DateTime.Now.ToString("MM");

            string period = "";

            period = String.Concat(MM_LMI_TARGET.YEAR, "-", MM_LMI_TARGET.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_TARGET(period);
            MM_LMI_TARGET.LIST_LMI_TARGET = CommonRepo.ConvertToList<LMI_TARGET>(filmProdHoliday);

            return View(MM_LMI_TARGET);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_TARGET(MM_LMI_TARGET model)
        {
            var yearList = new List<int>();
            for (int i = 1; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year - i);
            }
            for (int i = 0; i < 6; i++)
            {
                yearList.Add(DateTime.Now.Year + i);
            }
            yearList.Sort();

            var monthList = new List<string>();
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString("0#"));
            }

            ViewBag.YearDDL = yearList;
            ViewBag.MonthDDL = monthList;

            if (model.YEAR == null)
                model.YEAR = model.YEAR;

            if (model.MONTH == null)
                model.MONTH = model.MONTH;

            string period = "";

            period = String.Concat(model.YEAR, "-", model.MONTH);
            DataTable filmProdHoliday = MMRepo.GET_TARGET(period);
            model.LIST_LMI_TARGET = CommonRepo.ConvertToList<LMI_TARGET>(filmProdHoliday);

            return PartialView("_FilmTargetPartial",model);
        }

        [HttpPost]
        [SessionExpireFilter]
        public ActionResult FILM_TARGET_MAINT([FromBody] MM_LMI_TARGET model)
        {
            ACL_UserObj ACLUser = HttpContext.Session.GetObject<ACL_UserObj>("AclUser");

            string strPeriod = String.Concat(model.YEAR, "-", model.MONTH);

            foreach (var m in model.LIST_LMI_TARGET)
            {
                //Debug use only
                //OPR_RATIO_TARGET.UPDATED_BY = "9145";
                var updatedBy = ACLUser.EMP_NO;
                MMRepo.UPDATE_TARGET(strPeriod, m.FMMCCODE, m.TARGET,"1", updatedBy);
            }

            return Json(new { RESULT = "OK" });
        }
        #endregion
    }
}
