using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.DAO;
using CRMS_WEB.Models.DB;
using CRMS_WEB.Models.Ext;
using PagedList.Mvc;
using PagedList;
using CRMS_WEB.Coverts;
using Microsoft.Reporting.WebForms;

namespace CRMS_WEB.Controllers
{
    public class RequisitionController : Controller
    {
        //
        // GET: /Requisition/
        public ActionResult Alert(string link, string massage)
        {
            ViewBag.LINK = link;
            ViewBag.MSG = massage;

            return View();
        }
        public ActionResult approbationManagement(string SearchText, int? page)
        {
            Database db = new Database();
            RequisitionDAO rqDAO = new RequisitionDAO(db);
            List<RequisitionModel> rqModel = rqDAO.FindAll();
            db.Close();
            ViewBag.Total = rqModel.Count;

            var data = from a in rqModel select a;
            if (!String.IsNullOrEmpty(SearchText))
            {
                if (SearchText.Contains("อนุมัติแล้ว"))
                {
                    SearchText = "allow";
                }
                else if (SearchText.Contains("รออนุมัติ"))
                {
                    SearchText = "wait";
                }
                else if (SearchText.Contains("ไม่อนุมัติ"))
                {
                    SearchText = "not";
                }

                data = data.Where(
                    a => a.REPAIR_NO.DAMAGE.Contains(SearchText)
                        || a.STOCK_NO.PART.BRAND.PART_BRAND_NAME.Contains(SearchText)
                        || a.STOCK_NO.PART.TYPE.PART_TYPE_NAME.Contains(SearchText)
                        || a.STOCK_NO.STOCK_INFO.Contains(SearchText)
                        || a.APROVE.Contains(SearchText)
                    );
            }

            var numPage = page ?? 1;
            ViewBag.Approbation = data.ToPagedList(numPage, 20);
            return View();
        }
        public ActionResult Approbation_popup(int reqID)
        {
            Database db = new Database();
            RequisitionDAO reqDAO = new RequisitionDAO(db);
            RequisitionModel reqModel = reqDAO.FindById(reqID);
            db.Close();
            ViewBag.REQ = reqModel;

            return View();
        }
        // อัพเดทเมื่อสถานะเป็น allow ให่้หักสต็อกเลย
        public ActionResult Approbation_popupUpdate(string approve, int reqID, int repID)
        {



            RequisitionModel reqModel = new RequisitionModel();

            // ค้นเอา stock_id จากการเบิก
            Database db = new Database();
            RequisitionDAO reqDAO = new RequisitionDAO(db);
            reqModel = new RequisitionModel();
            reqModel = reqDAO.FindById(reqID);
            db.Close();

            // แล้วเอามาค้นเอาข้อมูลสต๊อก
            db = new Database();
            StockDAO sDAO = new StockDAO(db);
            StockModel sModel = sDAO.FindById(reqModel.STOCK_NO.STOCK_ID);
            db.Close();

            RequisitionModel rqModel = new RequisitionModel();
            rqModel.REQ_ID = reqID;
            rqModel.APROVE = approve;

            db = new Database();
            reqDAO = new RequisitionDAO(db);
            int ap = reqDAO.Approbation(rqModel);
            db.Close();

            if (approve.Equals("allow") && !reqModel.APROVE.Equals("allow"))
            {
              

                if (ap > 0)
                {
                    // ของที่เบิก ต้องมีจำนวน น้อยกว่าเท่ากับของที่อยู่ในสต๊อก
                    if (reqModel.REQ_UNIT <= sModel.UNIT)
                    {
                        int stkMinus = sModel.UNIT - reqModel.REQ_UNIT;

                        // รับค่าที่ทำการหักสต๊อกแล้ว
                        sModel = new StockModel();
                        sModel.STOCK_ID = reqModel.STOCK_NO.STOCK_ID;
                        sModel.UNIT = stkMinus;

                        System.Diagnostics.Debug.WriteLine("sModel.STOCK_ID :" + sModel.STOCK_ID);

                        db = new Database();
                        sDAO = new StockDAO(db);
                        sDAO.StockDeduction(sModel);
                        db.Close();

                    }
                    else
                    {
                        return RedirectToAction("Alert", "Requisition", new { link = "../Requisition/approbationManagement", massage = "ไม่สามารถเบิกมากกว่าจำนวนของที่มีในคลังได้" });
                    }
                }

            }
            else if ((approve.Equals("wait") || approve.Equals("not")) && reqModel.APROVE.Equals("allow"))
            {
                int stkMinus = sModel.UNIT + reqModel.REQ_UNIT;

                // รับค่าที่ทำการหักสต๊อกแล้ว
                sModel = new StockModel();
                sModel.STOCK_ID = reqModel.STOCK_NO.STOCK_ID;
                sModel.UNIT = stkMinus;

                db = new Database();
                sDAO = new StockDAO(db);
                sDAO.StockDeduction(sModel);
                db.Close();
            }


            RepairModel rModel = new RepairModel();
            rModel.REPAIR_NO = repID;
            rModel.ALERT_STATUS = "1";

            db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            rDAO.AlertStatus(rModel);
            db.Close();

            return Redirect("~/Requisition/approbationManagement");
        }
        public ActionResult WithdrawDeleting(int reqID, int repID)
        {
            RequisitionModel reqModel = new RequisitionModel();
            reqModel.REQ_ID = reqID;

            Database db = new Database();
            RequisitionDAO reqDAO = new RequisitionDAO(db);
            reqDAO.Delete(reqModel);
            db.Close();

            return Redirect("/Services/RepairDescription?repid=" + repID);
        }
        public ActionResult ApproveDeleting()
        {
            return Redirect("~/Requisition/approbationManagement");
        }
        public ActionResult RequisitionReportView(int? page)
        {
            string month = Request.Params["month"];
            ConvertClass cv = new ConvertClass();
            if (String.IsNullOrEmpty(month))
            {
                month = "0-0";
            }
            List<RequisitionModel> rqModel = new List<RequisitionModel>();

            Database db = new Database();
            RequisitionDAO rqDAO = new RequisitionDAO(db);
            rqModel = rqDAO.FindByMonthYear(month);
            db.Close();
            ViewBag.TOTAL = rqModel.Count;
            var pageNum = page ?? 1;

            ViewBag.DATA = rqModel.ToPagedList(pageNum, 20);
            return View();
        }
        public ActionResult RequisitionExportReport(string t)
        {
            string month = Request.Params["month"];
            List<RequisitionModel> rqModel = new List<RequisitionModel>();

            Database db = new Database();
            RequisitionDAO rqDAO = new RequisitionDAO(db);
            rqModel = rqDAO.FindByMonthYear(month);
            db.Close();

            LocalReport lr = new LocalReport();
            string path = Server.MapPath("~/Reportor/RequisitionReport.rdlc");

            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }

            var data = rqModel.Select(d => new
            {
                DAMAGE = d.REPAIR_NO.DAMAGE,
                SPARE = d.STOCK_NO.PART.TYPE.PART_TYPE_NAME + " " + d.STOCK_NO.PART.BRAND.PART_BRAND_NAME + " " + d.STOCK_NO.STOCK_INFO,
                UNIT = d.REQ_UNIT,
                PRICE = d.STOCK_NO.PRICE,
                WITHDRAWAL = d.STAFF.NAME + " " + d.STAFF.LASTNAME
            }).ToList();

            ConvertClass cv = new ConvertClass();
            string[] x = month.Split('-');

            ReportParameter rpm = new ReportParameter()
            {
                Name = "MONTH",
                Values = { cv.monthToStringMonth(Int32.Parse(x[1])) + " " + x[0] }  // ชื่อรายงาน
            };
            ReportDataSource rds = new ReportDataSource("DataSetRQ", data);
            lr.SetParameters(rpm);
            lr.DataSources.Add(rds);

            lr.DisplayName = "รายงานประจำ" + cv.monthToStringMonth(Int32.Parse(x[1])) + " " + x[0];  // ชื่อรายงาน
            string reportType = t;
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfoA4 =
           "<DeviceInfo>" +
           "  <OutputFormat>" + t + "</OutputFormat>" +
           "  <PageWidth>21cm</PageWidth>" +
           "  <PageHeight>29.7cm</PageHeight>" +
           "  <MarginTop>1cm</MarginTop>" +
           "  <MarginLeft>0.5in</MarginLeft>" +
           "  <MarginRight>0.5in</MarginRight>" +
           "  <MarginBottom>0.5in</MarginBottom>" +
           "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }
    }
}
