using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.IO;
using CRMS_WEB.Models.DB;
using CRMS_WEB.Models.DAO;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Coverts;
using Microsoft.Reporting.WebForms;
using PagedList.Mvc;
using PagedList;
using CRMS_WEB.Models.Ext;

namespace CRMS_WEB.Controllers
{
    public class ServicesController : Controller
    {
        //
        // GET: /Services/

        CultureInfo culture = CultureInfo.CurrentCulture;
        public ActionResult RepairDescriptionBilling(int repID)
        {
            string t = "pdf";
            LocalReport lr = new LocalReport();
            string path = Server.MapPath("~/Reportor/ExportBill.rdlc");

            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }

            Database db = new Database();
            RequisitionDAO rqDAO = new RequisitionDAO(db);
            List<RequisitionModel> reqModel = rqDAO.FindByRepairID(repID).Where(r => r.APROVE.Contains("allow")).ToList();
            //HashSet<RequisitionModel> reqModel = rqDAO.FindByRepairID(repID).Where(r => r.APROVE.Contains("allow")).ToList();
            db.Close();

            var data = reqModel.Select(r => new
            {
                REQ_DATE = r.REQ_DATE.ToString("dd MMMM yyyy H:mm"),
                REQ_DETAIL = r.STOCK_NO.PART.TYPE.PART_TYPE_NAME+" "+r.STOCK_NO.PART.BRAND.PART_BRAND_NAME+" "+r.STOCK_NO.STOCK_INFO,
                REQ_UNIT = r.REQ_UNIT,
                REQ_PRICE = r.STOCK_NO.PRICE
            }).ToList();


            db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            RepairModel rModel = rDAO.FindById(repID);
            db.Close();

            ReportParameter[] pr = new ReportParameter[]
            {
                new ReportParameter("NAME",rModel.CUSTOMER.C_NAME+" "+rModel.CUSTOMER.C_LASTNAME),
                new ReportParameter("ADDR",rModel.CUSTOMER.C_ADDRESS),
                new ReportParameter("TEL",rModel.CUSTOMER.C_TEL+" "+(rModel.CUSTOMER.C_EMAIL.Equals("")?" ":" Email :"+rModel.CUSTOMER.C_EMAIL)),
                new ReportParameter("REP_ID",rModel.REPAIR_NO.ToString()),
                new ReportParameter("REP_PRODUCT",rModel.PRODUCT.TYPE.PRO_TYPE_NAME+" "+rModel.PRODUCT.BANRD.PRO_BAND_NAME+" "+rModel.MODEL+" สี :"+rModel.COLOR),
                new ReportParameter("DAMAGE",rModel.DAMAGE)
            };

            ReportDataSource rds = new ReportDataSource("DataSetRepairing", data);
            lr.SetParameters(pr);
            lr.DataSources.Add(rds);

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
        public ActionResult Index(string SearchText, string statusText, int? page)
        {
            Database db = new Database();
            RepairDAO repDAO = new RepairDAO(db);
            List<RepairModel> repModel = repDAO.FindAll();
            db.Close();

            var data = from d in repModel select d;
            if (!String.IsNullOrEmpty(statusText))
            {
                // ค้นหาจาก List ด้วยคำสั่ง Where
                data = data.Where(d => d.STATUS.STATUS_ID.ToString().Contains(statusText)
                    ).ToList(); // END Where
            }
            if (!String.IsNullOrEmpty(SearchText))
            {
                // ค้นหาจาก List ด้วยคำสั่ง Where
                data = data.Where(d => d.REPAIR_NO.ToString().Contains(SearchText)
                    || d.CUSTOMER.C_NAME.Contains(SearchText)

                    ).ToList(); // END Where
            }
            var pageNumber = page ?? 1;
            ViewBag.REPAIR = data.ToPagedList(pageNumber, 15);

            return View();
        }

        public ActionResult EditRepair()
        {
            int repairID = Int32.Parse(Request.Params["repID"]);

            // ค้นข้อมูลของรายการซ่อมเพื่อแก้ไข
            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            RepairModel lsRModel = rDAO.FindById(repairID);
            db.Close();
            ViewData["REPAIR"] = lsRModel;

            // รายการประเภทสินค้า
            db = new Database();
            ProductTypeDAO ptDAO = new ProductTypeDAO(db);
            List<ProductTypeModel> lPTModel = ptDAO.FindAll();
            db.Close();
            ViewData["PRODUCTTYPE"] = lPTModel;

            // รายการยี่ห้อสินค้า
            db = new Database();
            ProductDAO pDAO = new ProductDAO(db);
            List<ProductModel> pModel = pDAO.FindByTypeID(lsRModel.PRODUCT.TYPE.PRO_TYP_ID);
            db.Close();
            ViewData["BRAND"] = pModel;

            //รายการผู้รับผิดชอบ
            db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            List<UsersModel> uModel = uDAO.FindAll();
            db.Close();
            ViewData["STAFF"] = uModel;

            return View();
        }
        public ActionResult RepairDelete()
        {
            int repID = Int32.Parse(Request.Params["repID"]);
            RepairModel rModel = new RepairModel();
            rModel.REPAIR_NO = repID;

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            rDAO.Delete(rModel);
            db.Close();

            return Redirect("~/Services");
        }
        public ActionResult RepairUpdate()
        {
            int repID = Int32.Parse(Request.Params["repID"]);
            int product_id = Int32.Parse(Request.Params["product_id"]);
            int staff_id = Int32.Parse(Request.Params["staff_id"]);
            string model = Request.Params["model"];
            string color = Request.Params["color"];
            string damage = Request.Params["damage"];
            string consign = Request.Params["consign"];

            RepairModel rModel = new RepairModel();
            rModel.REPAIR_NO = repID;
            rModel.PRODUCT = new ProductModel();
            rModel.PRODUCT.PRODUCT_NO = product_id;
            rModel.STAFF = new UsersModel();
            rModel.STAFF.USER_NO = staff_id;
            rModel.MODEL = model;
            rModel.DAMAGE = damage;
            rModel.DATE_CONSIGN = consign;

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            rDAO.Update(rModel);
            db.Close();

            return Redirect("~/Services");
        }
        public ActionResult RepairDescription()
        {
            //รับพารามิเตอร์
            int repairID = Int32.Parse(Request.Params["repID"]);

            RepairDetailModel rdModel = new RepairDetailModel();
            /////////////
            RepairModel rModel = new RepairModel();
            rModel.REPAIR_NO = repairID;
            rModel.ALERT_STATUS = "0";  // หมายถึงดูแล้ว

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            rDAO.AlertStatus(rModel);
            db.Close();

            //////////////////////////
            db = new Database();
            rDAO = new RepairDAO(db);
            rdModel.REPAIR = rDAO.FindById(repairID);
            db.Close();

            db = new Database();
            RequisitionDAO rqDAO = new RequisitionDAO(db);
            rdModel.STOCK = rqDAO.FindByRepairID(rdModel.REPAIR.REPAIR_NO);
            db.Close();

            ViewData["Repair"] = rdModel;

            // รายการสถานะ
            db = new Database();
            RepStatusDAO rtDAO = new RepStatusDAO(db);
            List<RepStatusModel> rtModel = rtDAO.FindAll();
            ViewData["STATUS"] = rtModel;
            db.Close();

            db = new Database();
            RepairDAO rpDAO = new RepairDAO(db);
            RepairModel rpModel = rpDAO.FindById(repairID);
            db.Close();

            return View();
        }
        public ActionResult RepairDescription_WithdrawEditPopup(int reqID)
        {
            Database db = new Database();
            RequisitionDAO rqDAO = new RequisitionDAO(db);
            RequisitionModel reqModel = rqDAO.FindById(reqID);
            db.Close();
            ViewBag.REQ = reqModel;

            return View();
        }
        public ActionResult WithdrawUpdate(int reqID, int repID, int reqUnit)
        {
            RequisitionModel reqModel = new RequisitionModel();
            reqModel.REQ_ID = reqID;
            reqModel.REQ_UNIT = reqUnit;

            Database db = new Database();
            RequisitionDAO reqDAO = new RequisitionDAO(db);
            reqDAO.Update(reqModel);
            db.Close();

            return Redirect("~/Services/RepairDescription?repid=" + repID);
        }
        public ActionResult Services(string userUID)
        {
            if (!String.IsNullOrEmpty(userUID))
            {
                userUID = userUID.Trim();

                Database db = new Database();
                CustomerDAO cDAO = new CustomerDAO(db);
                CustomerModel cModel = cDAO.FindByUid(userUID);
                db.Close();

                /// ถ้ามีข้อมูลให้เก็บไว้ที่ วิวดาต้าเพื่อนำไปแสดง <-- james
                if (cModel != null)
                {
                    ViewData["CUST"] = cModel;

                    db = new Database();
                    ProductTypeDAO ptDAO = new ProductTypeDAO(db);
                    List<ProductTypeModel> listPTModel = ptDAO.FindAll();
                    db.Close();

                    ViewData["PROTYPE"] = listPTModel;
                }
                else
                {
                    ViewData["CUST"] = null;
                }

            }
            return View();
        }
        public ActionResult ConfirmInsertServices()
        {
            //รับ พารามิเตอร์ที่ส่งเข้ามา
            int custID = Int32.Parse(Request.Params["custid"]);
            int productID = Int32.Parse(Request.Params["IDproduct"]);

            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            CustomerModel cModel = cDAO.FindById(custID);
            db.Close();



            /// ถ้ามีข้อมูลให้เก็บไว้ที่ วิวดาต้าเพื่อนำไปแสดง <-- james
            if (cModel != null)
            {
                // หน้าคอนเฟิม เป็นหน้าที่มีพารามิเตอร์ค่อนข้่างครบ ให้ค้นจากข้อมูลหลักเลย
                // ค้นหาจากตารางโปรดัก Product
                db = new Database();
                ProductDAO pDAO = new ProductDAO(db);
                ProductModel pModel = pDAO.FindById(productID);
                db.Close();

                db = new Database();
                UsersDAO uDAO = new UsersDAO(db);
                List<UsersModel> uModel = uDAO.FindAll();
                db.Close();

                ViewData["USER"] = uModel;
                ViewData["CUST"] = cModel;
                ViewData["PRODUCT"] = pModel;
            }
            else
            {
                ViewData["CUST"] = null;
            }

            return View();
        }

        public ActionResult InsertServices()
        {

            int custID = Int32.Parse(Request.Params["custid"]);
            int userNo = Int32.Parse(Request.Params["user_no"]);
            int productID = Int32.Parse(Request.Params["productid"]);
            int recipientID = Int32.Parse(Request.Params["recipient_id"]);
            string model = Request.Params["model"];
            string color = Request.Params["color"];
            string damage = Request.Params["damage"];
            string consign = Request.Params["consign"];
            //string time = Request.Params["time"];
            ConvertClass cv = new ConvertClass();

            RepairModel rModel = new RepairModel();
            rModel.CUSTOMER = new CustomerModel();              // CUST_NO
            rModel.CUSTOMER.CUST_NO = custID;

            rModel.STAFF = new UsersModel();
            rModel.STAFF.USER_NO = userNo;                      // USER_NO

            rModel.PRODUCT = new ProductModel();
            rModel.PRODUCT.PRODUCT_NO = productID;              //PRODUCT_NO

            rModel.STATUS = new RepStatusModel();
            rModel.STATUS.STATUS_ID = 1;                            //REP_STATUS_ID

            rModel.MODEL = model;                                   // MODEL
            rModel.COLOR = color;                                   // COLOR
            rModel.DAMAGE = damage;                                 // DAMAGE
            rModel.DATE_CONSIGN = consign; //consign + " " + time;             // DATE_CONSIGN
            rModel.DATE_ASSIGN = DateTime.Now.ToString();            // DATE_ASSIGN วันรับทำรายการ
            rModel.RECIPIENT_ID = new UsersModel();
            rModel.RECIPIENT_ID.USER_NO = (int)Session["ID"];

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            Session["REPBILL"] = rDAO.Add(rModel);          ///     Session           / / /  
            db.Close();

            return RedirectToAction("", "Services");

        }


        public ActionResult BillServicesAssign_Printing(string type, int repID)
        {
            LocalReport locRep = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/Reportor/"), "BillServicesAssign_Printing.rdlc");

            System.Diagnostics.Debug.WriteLine("TEST PATH FILE :" + path);
            if (System.IO.File.Exists(path))
            {
                locRep.ReportPath = path;
            }
            else
            {
                return Redirect("~/Services");
            }

            Database db = new Database();
            RepairDAO repDAO = new RepairDAO(db);
            RepairModel repModel = repDAO.FindById(repID);
            db.Close();

            ReportParameter[] pr = new ReportParameter[]
            {
                new ReportParameter("CDETAIL",repModel.CUSTOMER.C_NAME+"  "+repModel.CUSTOMER.C_LASTNAME+(!repModel.CUSTOMER.C_TEL.Equals("")?" เบอร์โทร : "+repModel.CUSTOMER.C_TEL:" ")+(!repModel.CUSTOMER.C_ADDRESS.Equals("")?" ที่อยู่ : "+repModel.CUSTOMER.C_ADDRESS:" ") ),
                new ReportParameter("STAFF",repModel.STAFF.NAME+" "+repModel.STAFF.LASTNAME),
                new ReportParameter("PRODUCT",repModel.PRODUCT.TYPE.PRO_TYPE_NAME+" "+repModel.PRODUCT.BANRD.PRO_BAND_NAME+" "+repModel.MODEL+" "+repModel.COLOR),
                new ReportParameter("DAMAGE",repModel.DAMAGE),
                new ReportParameter("ASSIGN_DATE",repModel.DATE_ASSIGN),
                new ReportParameter("CONSIGN_DATE",repModel.DATE_CONSIGN.ToString()),
                new ReportParameter("RECIPIENT",repModel.RECIPIENT_ID.NAME+" "+repModel.RECIPIENT_ID.LASTNAME),
                new ReportParameter("REPID",repModel.REPAIR_NO.ToString())
            };

            locRep.SetParameters(pr);
            locRep.GetDefaultPageSettings();
            locRep.DisplayName = repModel.REPAIR_NO.ToString();

            string reportType = type;
            string mimeType;
            string encoding;
            string fileNameExtension;


            string deviceInfoA4 =
            "<DeviceInfo>" +
            "  <OutputFormat>" + type + "</OutputFormat>" +
            "  <PageWidth>17.78cm</PageWidth>" +
            "  <PageHeight>12.7cm</PageHeight>" +
            "  <MarginTop>1cm</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = locRep.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);

        }
        public JsonResult DataLoading_ProductType()
        {
            int proTypeID = Int32.Parse(Request.Params["protypeid"]);

            Database db = new Database();
            ProductDAO pDAO = new ProductDAO(db);
            List<ProductModel> pModel = pDAO.FindByTypeID(proTypeID);
            db.Close();
            var result = "";

            if (pModel.Count > 0)
            {
                foreach (var pro in pModel)
                {
                    result += "<option value='" + pro.PRODUCT_NO + "'>" + pro.BANRD.PRO_BAND_NAME + "</option>";
                }
            }
            else
            {
                result = "<option value=''>----ไม่มีข้อมูล----</option>";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateDetail()
        {
            int repID = Int32.Parse(Request.Params["repID"]);
            int repStatusID = Int32.Parse(Request.Params["repStatusID"]);
            string repDetail = Request.Params["repDetail"];
            string chkpic = Request.Params["oldPic"].ToString();
            string fileName = "";

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            RepairModel rModel = rDAO.FindById(repID);
            db.Close();
            if (Request.Files[0] != null)
            {
                var file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    UID uid = new UID();
                    fileName = uid.DateTimeToGuid(DateTime.Now).ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img_repair/"), fileName);
                    file.SaveAs(path);

                    // ลบไฟล์เก่าทิ้ง
                    if (System.IO.File.Exists(Server.MapPath("~/Content/img_repair/") + rModel.IMAGES))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/img_repair/") + rModel.IMAGES);
                    }
                }
                else
                {
                    fileName = rModel.IMAGES;
                }
            }
            else
            {
                fileName = rModel.IMAGES;
            }

            RepairModel model = new RepairModel();
            model.REPAIR_NO = repID;
            model.STATUS = new RepStatusModel();
            model.STATUS.STATUS_ID = repStatusID;
            model.REPAIR_DETAIL = repDetail;
            model.IMAGES = fileName;
            model.ALERT_STATUS = "1";

            db = new Database();
            rDAO = new RepairDAO(db);
            rDAO.UpdateDetail(model);
            db.Close();

            return Redirect("~/Services/RepairDescription?repID=" + repID);
        }
        public ActionResult WithdrawParts(string SearchText, int? page)
        {

            //เรียกข้อมูลจากฐานข้อมูล
            Database db = new Database();
            StockDAO stkDAO = new StockDAO(db);
            List<StockModel> stkModel = stkDAO.FindAll();
            db.Close();
            ViewBag.Total = stkModel.Count;

            var data = from d in stkModel select d;
            // ค้นหา
            if (!String.IsNullOrEmpty(SearchText))
            {
                SearchText.Trim();
                //ค้นหา
                data = data.Where(
                    d => d.STOCK_ID.ToString().Contains(SearchText)
                        || d.PART.TYPE.PART_TYPE_NAME.Contains(SearchText) || d.PART.TYPE.PART_TYPE_NAME.Contains(SearchText.ToUpper()) || d.PART.TYPE.PART_TYPE_NAME.Contains(SearchText.ToLower())
                        || d.PART.BRAND.PART_BRAND_NAME.Contains(SearchText) || d.PART.BRAND.PART_BRAND_NAME.Contains(SearchText.ToLower()) || d.PART.BRAND.PART_BRAND_NAME.Contains(SearchText.ToUpper())
                        || d.PRICE.ToString().Contains(SearchText)
                        || d.STOCK_INFO.Contains(SearchText.ToUpper()) || d.STOCK_INFO.Contains(SearchText.ToLower()) || d.STOCK_INFO.Contains(SearchText)
                    ).ToList();
            }

            var numPage = page ?? 1;
            ViewBag.Stock = data.ToPagedList(numPage, 20);

            return View();
        }
        //ปุ่มอยู่หน้า WithdrawParts
        public ActionResult WithdrawPartsAdding_popup(int repID, int stockID)
        {
            Database db = new Database();
            StockDAO sDAO = new StockDAO(db);
            StockModel sModel = sDAO.FindById(stockID);
            db.Close();
            ViewBag.Stock = sModel;

            return View();
        }
        public ActionResult WithdrawPartsInserting(int wdUnit, int repID, int stockID, int userID)
        {
            RequisitionModel rqModel = new RequisitionModel();
            rqModel.REPAIR_NO = new RepairModel();
            rqModel.REPAIR_NO.REPAIR_NO = repID;
            rqModel.STOCK_NO = new StockModel();
            rqModel.STOCK_NO.STOCK_ID = stockID;
            rqModel.REQ_UNIT = wdUnit;
            rqModel.APROVE = "wait";
            rqModel.STAFF = new UsersModel();
            rqModel.STAFF.USER_NO = userID;

            Database db = new Database();
            RequisitionDAO rqDAO = new RequisitionDAO(db);
            rqDAO.Add(rqModel);
            db.Close();

            return Redirect("~/Services/RepairDescription?repID=" + repID);
        }
        public ActionResult RepairingReportView(int? page)
        {
            string month = Request.Params["month"];
            ConvertClass cv = new ConvertClass();
            if (String.IsNullOrEmpty(month))
            {
                month = "0-0";
            }

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            List<RepairModel> rModel = rDAO.FindByMonthYear(month);
            db.Close();
            var numPage = page ?? 1;
            ViewBag.REPAIR = rModel.ToPagedList(numPage, 20);
            return View();
        }
        public ActionResult RepairingExportReport(string t)
        {
            string month = Request.Params["month"];
            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            List<RepairModel> rModel = rDAO.FindByMonthYear(month);
            db.Close();

            LocalReport lr = new LocalReport();
            string path = Server.MapPath("~/Reportor/RepairingReport.rdlc");

            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }

            var data = rModel.Select(r => new
            {
                C_UID = r.CUSTOMER.CUST_UID,
                C_NAME = r.CUSTOMER.C_NAME + " " + r.CUSTOMER.C_LASTNAME,
                REP_DET = r.PRODUCT.TYPE.PRO_TYPE_NAME + " " + r.PRODUCT.BANRD.PRO_BAND_NAME + " " + r.MODEL,
                DAMAGE = r.DAMAGE,
                STAFF = r.STAFF.NAME + " " + r.STAFF.LASTNAME
            }).ToList();

            ConvertClass cv = new ConvertClass();
            string[] x = month.Split('-');

            ReportParameter rpm = new ReportParameter()
            {
                Name = "MONTH",
                Values = { cv.monthToStringMonth(Int32.Parse(x[1])) + " " + x[0] }  // ชื่อรายงาน
            };
            ReportDataSource rds = new ReportDataSource("DataSetRepairing", data);
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
        public JsonResult DataLoading_BadgeAlertBell(int c)
        {
            Database db = new Database();
            RepairDAO repDAO = new RepairDAO(db);
            int result = repDAO.CountAlertByCustID(c);
            db.Close();

            return Json(new { data = result });
        }
        public ActionResult ReadUpdateRepair(int c,int?page)
        {
            List<RepairModel> listModel = new List<RepairModel>();

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            listModel = rDAO.FindUpdateByCustID(c).OrderBy(d => d.ALERT_CUST).ToList();
            db.Close();

            var numpage = page ?? 1;
            ViewBag.REP = listModel.ToPagedList(numpage,20);
            return View();
        }
        public ActionResult ReadRepairDescription(int repairID)
        {
            RepairDetailModel rdModel = new RepairDetailModel();
            /////////////
            RepairModel rModel = new RepairModel();
            rModel.REPAIR_NO = repairID;
            rModel.ALERT_CUST = "0";  // หมายถึงดูแล้ว

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            rDAO.AlertCust(rModel);
            db.Close();

            //////////////////////////
            db = new Database();
            rDAO = new RepairDAO(db);
            rdModel.REPAIR = rDAO.FindById(repairID);
            db.Close();

            db = new Database();
            RequisitionDAO rqDAO = new RequisitionDAO(db);
            rdModel.STOCK = rqDAO.FindByRepairID(rdModel.REPAIR.REPAIR_NO);
            db.Close();

            ViewData["Repair"] = rdModel;

            // รายการสถานะ
            db = new Database();
            RepStatusDAO rtDAO = new RepStatusDAO(db);
            List<RepStatusModel> rtModel = rtDAO.FindAll();
            ViewData["STATUS"] = rtModel;
            db.Close();

            db = new Database();
            RepairDAO rpDAO = new RepairDAO(db);
            RepairModel rpModel = rpDAO.FindById(repairID);
            db.Close();

            return View();

        }
        public ActionResult ListCustService(int c, string SearchText, string statusText, int? page)
        {
            Database db = new Database();
            RepairDAO repDAO = new RepairDAO(db);
            List<RepairModel> repModel = repDAO.FindByCustID(c);
            db.Close();

            var data = from d in repModel select d;
            if (!String.IsNullOrEmpty(statusText))
            {
                // ค้นหาจาก List ด้วยคำสั่ง Where
                data = data.Where(d => d.STATUS.STATUS_ID.ToString().Contains(statusText)
                    ).ToList(); // END Where
            }
            if (!String.IsNullOrEmpty(SearchText))
            {
                // ค้นหาจาก List ด้วยคำสั่ง Where
                data = data.Where(d => d.REPAIR_NO.ToString().Contains(SearchText)
                    || d.CUSTOMER.C_NAME.Contains(SearchText)

                    ).ToList(); // END Where
            }
            var pageNumber = page ?? 1;
            ViewBag.REPAIR = data.ToPagedList(pageNumber, 15);

            return View();
        }
        public ActionResult CustServiceDetail(int repID)
        {

            RepairDetailModel rdModel = new RepairDetailModel();
            /////////////
            RepairModel rModel = new RepairModel();
            rModel.REPAIR_NO = repID;
            rModel.ALERT_STATUS = "1";  // หมายถึงดูแล้ว

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            rDAO.AlertStatus(rModel);
            db.Close();

            //////////////////////////
            db = new Database();
            rDAO = new RepairDAO(db);
            rdModel.REPAIR = rDAO.FindById(repID);
            db.Close();

            db = new Database();
            RequisitionDAO rqDAO = new RequisitionDAO(db);
            rdModel.STOCK = rqDAO.FindByRepairID(rdModel.REPAIR.REPAIR_NO);
            db.Close();

            ViewData["Repair"] = rdModel;

            // รายการสถานะ
            db = new Database();
            RepStatusDAO rtDAO = new RepStatusDAO(db);
            List<RepStatusModel> rtModel = rtDAO.FindAll();
            ViewData["STATUS"] = rtModel;
            db.Close();

            db = new Database();
            RepairDAO rpDAO = new RepairDAO(db);
            RepairModel rpModel = rpDAO.FindById(repID);
            db.Close();

            return View();
        }
        public ActionResult CustServiceDetailUpdateDamage(int repID, string repDetail)
        {
            RepairModel rModel = new RepairModel();
            rModel.REPAIR_NO = repID;
            rModel.DAMAGE = repDetail;
            rModel.ALERT_STATUS = "1";

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            rDAO.UpdateDamageRepair(rModel);
            db.Close();

            

            return Redirect("~/Services/CustServiceDetail?repID=" + repID);
        }
    }
}
