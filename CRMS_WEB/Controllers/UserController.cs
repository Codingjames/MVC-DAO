using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.DAO;
using CRMS_WEB.Models.DB;
using CRMS_WEB.Models.Ext;
using System.Threading;
using PagedList.Mvc;
using PagedList;
using CRMS_WEB.Coverts;
using Microsoft.Reporting.WebForms;

namespace CRMS_WEB.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public JsonResult IsDuplicateUser(string userName)
        {
            string result = "fail";
            int u = 0, c = 0;
            userName = userName.Trim().ToLower();

            UsersModel uModel = new UsersModel();
            uModel.USERNAME = userName;

            CustomerModel cModel = new CustomerModel();
            cModel.USERNAME = userName;

            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            u = uDAO.FindByUserName(uModel).Count;
            db.Close();

            db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            c = cDAO.FindByUserName(cModel).Count;
            db.Close();

            if (c >= 1 || u >= 1)
            {
                result = "pass";
            }
            Thread.Sleep(2000);
            return Json(new { result });
        }
        public ActionResult ManagerProfile(int userID,string SearchText,int? page)
        {
            UserProfileModel upModel = new UserProfileModel();

            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            upModel.USERS = uDAO.FindById(userID);
            db.Close();

            db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            upModel.REPAIR = rDAO.FindByUserID(upModel.USERS.USER_NO);
            db.Close();

            var data = from d in upModel.REPAIR select d;
            if(!String.IsNullOrEmpty(SearchText)){
                if(!SearchText.Equals("0")){
                    data = data.Where(
                        d => d.STATUS.STATUS_ID.ToString().Contains(SearchText)
                        );
                }
            }

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.TOTAL_WORK = rDAO.CountByUserID(userID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.WAIT_WORK = rDAO.CountByUserIDWait(userID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.NOW_WORK = rDAO.CountByUserIDNowWorking(userID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.SUCCESS_WORK = rDAO.CountByUserIDSuccess(userID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.FAIL_WORK = rDAO.CountByUserIDFail(userID);
            db.Close();

            var numpage = page ?? 1;

            ViewBag.REPAIR = data.ToPagedList(numpage, 20);
            ViewBag.PROFILE = upModel;
            return View();
        }
        public ActionResult EmployeeProfile(int userID, string SearchText, int? page)
        {
            UserProfileModel upModel = new UserProfileModel();

            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            upModel.USERS = uDAO.FindById(userID);
            db.Close();

            db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            upModel.REPAIR = rDAO.FindByUserID(upModel.USERS.USER_NO);
            db.Close();

            var data = from d in upModel.REPAIR select d;
            if (!String.IsNullOrEmpty(SearchText))
            {
                if (!SearchText.Equals("0"))
                {
                    data = data.Where(
                        d => d.STATUS.STATUS_ID.ToString().Contains(SearchText)
                        );
                }
            }

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.TOTAL_WORK = rDAO.CountByUserID(userID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.WAIT_WORK = rDAO.CountByUserIDWait(userID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.NOW_WORK = rDAO.CountByUserIDNowWorking(userID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.SUCCESS_WORK = rDAO.CountByUserIDSuccess(userID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            upModel.FAIL_WORK = rDAO.CountByUserIDFail(userID);
            db.Close();

            var numpage = page ?? 1;

            ViewBag.REPAIR = data.ToPagedList(numpage, 20);
            ViewBag.PROFILE = upModel;
            return View();
        }

        public ActionResult ManageManager()
        {
            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            List<UsersModel> lisUsrModel = new List<UsersModel>();
            lisUsrModel = uDAO.FindAll();
            db.Close();
            ViewData["USERS"] = lisUsrModel;

            return View();
        }
        ///
        /// เพิ่มข้อมูลผู้จัดการระบบ <-- james
        public ActionResult AddManager()
        {
            return View();
        }
        /// 
        /// // confirm data before insert into databasee <-- james
        public ActionResult ConfirmAddManager()
        {
            return View();
        }
        /// 
        /// ///เพิ่มลงดาต้าเบส  <-- james
        public ActionResult InsertManager()
        {

            UsersModel uModel = new UsersModel();
            uModel.USERNAME = Request.Params["username"].Trim().ToLower();
            uModel.PASSWORD = Request.Params["password"];
            uModel.ID_CARD = Request.Params["ID_CARD"].Trim();
            uModel.NAME = Request.Params["name"];
            uModel.LASTNAME = Request.Params["lname"];
            uModel.TEL = Request.Params["tel"];
            uModel.EMAIL = Request.Params["email"].Trim();
            uModel.ADDRESS = Request.Params["addr"].Trim();
            uModel.START_WORK = Convert.ToDateTime(Request.Params["start_work"]);
            uModel.SALARY = Request.Params["salary"].Trim();
            uModel.USER_UID = UID.ranByLen(8);
            uModel.PRIORITY = new PriorityModel();
            uModel.PRIORITY.PRIO_ID = 1;

            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            uDAO.Add(uModel);
            db.Close();


            return RedirectToAction("ManageManager");
        }

        /// <summary>
        /// 
        /// // หน้า แสดงข้อมูลพนักงาน เมนู เพิ่ม ลบ แก้ไข  <-- james
        public ActionResult ManageEmployees(string SearchText, int? page)
        {
            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            List<UsersModel> empModel = uDAO.FindAllEmployee();
            db.Close();
            ViewBag.Total = empModel.Count;

            var data = from d in empModel select d;
            if (!String.IsNullOrEmpty(SearchText))
            {
                data = data.Where(
                    d => d.USER_UID.Contains(SearchText)
                        || d.NAME.Contains(SearchText)
                        || d.LASTNAME.Contains(SearchText)
                        || d.TEL.Contains(SearchText)
                    );
            }
            var numPage = page ?? 1;
            ViewBag.EMP = data.ToPagedList(numPage, 20);

            return View();
        }
        /// <summary>
        /// ////>>>>>> เพิ่มพนักงาน  <-- james 
        public ActionResult AddEmployees()
        {

            return View();
        }
        public ActionResult ConfirmAddEmployees()
        {

            return View();
        }
        /// เพิ่มข้อมูลพนักงาน
        /// 
        public ActionResult InsertEmployees()
        {

            UsersModel uModel = new UsersModel();
            uModel.USERNAME = Request.Params["username"].Trim().ToLower();
            uModel.PASSWORD = Request.Params["password"];
            uModel.ID_CARD = Request.Params["ID_CARD"].Trim();
            uModel.NAME = Request.Params["name"];
            uModel.LASTNAME = Request.Params["lname"];
            uModel.TEL = Request.Params["tel"].Trim();
            uModel.EMAIL = Request.Params["email"].Trim();
            uModel.ADDRESS = Request.Params["addr"].Trim();
            uModel.START_WORK = Convert.ToDateTime(Request.Params["start_work"]);
            uModel.SALARY = Request.Params["salary"].Trim();
            uModel.USER_UID = UID.ranByLen(8);
            uModel.PRIORITY = new PriorityModel();
            uModel.PRIORITY.PRIO_ID = 2;

            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            uDAO.Add(uModel);
            db.Close();


            return RedirectToAction("ManageEmployees");
        }
        public ActionResult UserDeleting(string who, int userID)
        {
            UsersModel uModel = new UsersModel();
            uModel.USER_NO = userID;

            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            uDAO.Delete(uModel);
            db.Close();

            return Redirect("~/User/Manage" + who);
        }
        public ActionResult UserEditing(string who, int userID)
        {
            ViewBag.WHO = who;

            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            UsersModel uModel = uDAO.FindById(userID);
            db.Close();
            ViewBag.USER = uModel;

            return View();
        }
        public ActionResult UserUpdating(string who)
        {
            UsersModel uModel = new UsersModel();
            uModel.USER_NO = Int32.Parse(Request.Params["userID"]);
            uModel.PASSWORD = Request.Params["password"]!=""?Request.Params["password"]:"";
            uModel.ID_CARD = Request.Params["ID_CARD"].Trim();
            uModel.NAME = Request.Params["name"];
            uModel.LASTNAME = Request.Params["lname"];
            uModel.TEL = Request.Params["tel"].Trim();
            uModel.EMAIL = Request.Params["email"].Trim();
            uModel.ADDRESS = Request.Params["addr"].Trim();
            uModel.START_WORK = Convert.ToDateTime(Request.Params["start_work"]);
            uModel.SALARY = Request.Params["salary"].Trim();

            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            uDAO.Update(uModel);
            db.Close();

            return Redirect("~/User/Manage"+who);
        }
        // รายงานผลงานพนักงาน
        public ActionResult EmployeesWorkingReportView(int? page)
        {
            string month = Request.Params["month"];

            if (!String.IsNullOrEmpty(month))
            {
                List<UsersModel> lusModel = new List<UsersModel>();
                
                Database db = new Database();
                UsersDAO usDAO = new UsersDAO(db);
                lusModel = usDAO.FindByMonthYear(month);
                db.Close();

                foreach (var u in lusModel)
                {
                    
                    db = new Database();
                    RepairDAO rDAO = new RepairDAO(db);
                    u.REPAIR = new WorkingEmployeesViewModel();
                    u.REPAIR.SUCCESS_REP = rDAO.CountByUserIDSuccess(u.USER_NO,month);
                    db.Close();

                    db = new Database();
                    rDAO = new RepairDAO(db);
                    u.REPAIR.WAIT_REP = rDAO.CountByUserIDWait(u.USER_NO, month);
                    db.Close();

                    db = new Database();
                    rDAO = new RepairDAO(db);
                    u.REPAIR.NOW_REP = rDAO.CountByUserIDNowWorking(u.USER_NO, month);
                    db.Close();

                    db = new Database();
                    rDAO = new RepairDAO(db);
                    u.REPAIR.CANNOT_REP = rDAO.CountByUserIDFail(u.USER_NO, month);
                    db.Close();
                }

                var numage = page ?? 1;
                ViewBag.USER = lusModel.ToPagedList(numage,20);
            }
            return View();
        }
        public ActionResult ExportEmployeesWorkingReport(string month)
        {
            List<UsersModel> lusModel = new List<UsersModel>();

            Database db = new Database();
            UsersDAO usDAO = new UsersDAO(db);
            lusModel = usDAO.FindByMonthYear(month);
            db.Close();

            foreach (var u in lusModel)
            {

                db = new Database();
                RepairDAO rDAO = new RepairDAO(db);
                u.REPAIR = new WorkingEmployeesViewModel();
                u.REPAIR.SUCCESS_REP = rDAO.CountByUserIDSuccess(u.USER_NO);
                db.Close();

                db = new Database();
                rDAO = new RepairDAO(db);
                u.REPAIR.WAIT_REP = rDAO.CountByUserIDWait(u.USER_NO);
                db.Close();

                db = new Database();
                rDAO = new RepairDAO(db);
                u.REPAIR.NOW_REP = rDAO.CountByUserIDNowWorking(u.USER_NO);
                db.Close();

                db = new Database();
                rDAO = new RepairDAO(db);
                u.REPAIR.CANNOT_REP = rDAO.CountByUserIDFail(u.USER_NO);
                db.Close();
            }
           

            string type = "excel";
            LocalReport lr = new LocalReport();
            string path = Server.MapPath("~/Reportor/EmployeeWorkingReport.rdlc");

            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }

            var data = lusModel.Select(d => new { 
                UID = d.USER_UID,
                NAME = d.NAME+" "+d.LASTNAME,
                WAIT = d.REPAIR.WAIT_REP,
                NOW = d.REPAIR.NOW_REP,
                SUCCESS = d.REPAIR.SUCCESS_REP,
                CANNOT = d.REPAIR.CANNOT_REP
            
            }).ToList();

            ConvertClass cv = new ConvertClass();
            string[] x = month.Split('-');

            ReportParameter rpm = new ReportParameter()
            {
                Name = "MONTH",
                Values = { cv.monthToStringMonth(Int32.Parse(x[1])) + " " + x[0] }  // ชื่อรายงาน
            };
            ReportDataSource rds = new ReportDataSource("WorkingDataSetReport", data);
            lr.SetParameters(rpm);
            lr.DataSources.Add(rds);

            lr.DisplayName = "รายงานผลงานประจำ" + cv.monthToStringMonth(Int32.Parse(x[1])) + " " + x[0];  // ชื่อรายงาน
            string reportType = type;
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfoA4 =
           "<DeviceInfo>" +
           "  <OutputFormat>" + type + "</OutputFormat>" +
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
