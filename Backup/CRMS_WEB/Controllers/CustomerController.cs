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
namespace CRMS_WEB.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        public ActionResult Index()
        {
            return View();
        }
        //เฉพาะลูกค้า
        public ActionResult Profile(int c)
        {
            CustomerViewModel cvModel = new CustomerViewModel();

            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            cvModel.CUSTOMER = cDAO.FindById(c);
            db.Close();

            db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            cvModel.WAIT_REP = rDAO.CountByCustID_WaitRep(c);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            cvModel.NOW_REP = rDAO.CountByCustID_NowRep(c);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            cvModel.SUCCESS_REP = rDAO.CountByCustID_SuccessRep(c);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            cvModel.CANNOT_REP = rDAO.CountByCustID_CannotRep(c);
            db.Close();
            ViewBag.C = cvModel;

            return View();
        }
        // เฉพาะลูกค้า
        public ActionResult MyProfileEditing(int c)
        {
            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            CustomerModel model = cDAO.FindById(c);
            db.Close();

            ViewBag.C = model;
            return View();
        }
        public ActionResult ManageCustomer(string SearchText, int? page)
        {

            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            List<CustomerModel> lsCModel = cDAO.FindAll();
            db.Close();
            ViewBag.Total = lsCModel.Count;

            var data = from d in lsCModel select d;
            if (!String.IsNullOrEmpty(SearchText))
            {
                data = data.Where(
                    d => d.CUST_UID.Contains(SearchText)
                        || d.C_NAME.Contains(SearchText)
                        || d.C_LASTNAME.Contains(SearchText)
                        || d.C_TEL.Contains(SearchText)
                    );
            }

            var numPage = page ?? 1;
            ViewBag.Customer = data.ToPagedList(numPage, 20);

            return View();
        }
        public ActionResult AddCustomer()
        {

            return View();
        }
        public ActionResult ConfirmAddCustomer()
        {
            return View();
        }
        public ActionResult InsertCustomer()
        {
            string userName = Request.Params["username"];
            string password = Request.Params["password"];
            string name = Request.Params["name"];
            string lname = Request.Params["lname"];
            string tel = Request.Params["tel"];
            string email = Request.Params["email"];
            string address = Request.Params["addr"];

            CustomerModel cModel = new CustomerModel();
            cModel.CUST_UID = UID.ranByLen(8);
            cModel.USERNAME = userName;
            cModel.PASSWORD = password;
            cModel.C_NAME = name;
            cModel.C_LASTNAME = lname;
            cModel.C_ADDRESS = address;
            cModel.C_TEL = tel;
            cModel.C_EMAIL = email;
            cModel.PRIORITY = new PriorityModel();
            cModel.PRIORITY.PRIO_ID = 3;



            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            cDAO.Add(cModel);
            db.Close();

            return RedirectToAction("ManageCustomer");
        }
        public ActionResult DeleteCustomer(int custID)
        {
            try
            {
                CustomerModel cModel = new CustomerModel();
                cModel.CUST_NO = custID;
                Database db = new Database();
                CustomerDAO cDAO = new CustomerDAO(db);
                cDAO.Delete(cModel);
                db.Close();

                return Redirect("~/Customer/ManageCustomer");
            }
            catch
            {
                return RedirectToAction("Alert", "Customer", new { link = "../Customer/ManageCustomer", massage = "ไม่สามารถลบได้ ผู้ใช้นี้มีการอ้างถึงจากรายการอื่น" });
            }

        }
        public ActionResult CustomerEditing(int custID)
        {
            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            CustomerModel cModel = cDAO.FindById(custID);
            db.Close();
            ViewBag.CUST = cModel;

            return View();
        }
        public ActionResult CustomerUpdating()
        {
            int custID = Int32.Parse(Request.Params["custID"]);
            string password = Request.Params["password"] == "" ? "" : Request.Params["password"];
            string name = Request.Params["name"];
            string lname = Request.Params["lname"];
            string tel = Request.Params["tel"];
            string email = Request.Params["email"];
            string address = Request.Params["addr"];

            CustomerModel cModel = new CustomerModel();
            cModel.CUST_NO = custID;
            cModel.PASSWORD = password;
            cModel.C_NAME = name;
            cModel.C_LASTNAME = lname;
            cModel.C_ADDRESS = address;
            cModel.C_TEL = tel;
            cModel.C_EMAIL = email;

            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            cDAO.Update(cModel);
            db.Close();

            if (!String.IsNullOrEmpty(Request.Params["swicth"]))
            {
                db = new Database();
                cDAO = new CustomerDAO(db);
                cModel = new CustomerModel();
                cModel = cDAO.FindById(custID);
                db.Close();

                Session["ID"] = cModel.CUST_NO;
                Session["PRIO"] = cModel.PRIORITY.PRIO_ID;
                Session["LEVEL"] = cModel.PRIORITY.PRIO_NAME;
                Session["NAME"] = cModel.C_NAME + " " + cModel.C_LASTNAME;
                Session.Timeout = 1800;

                if (!String.IsNullOrEmpty(password))
                {
                    Session.Clear();
                    return Redirect("~/Login");
                }
                else
                {
                    return Redirect("~/Customer/MyProfileEditing?c=" + custID);
                }
            }
            else
            {
                return Redirect("~/Customer/ManageCustomer");
            }
        }
        public ActionResult Alert(string link, string massage)
        {

            ViewBag.LINK = link;
            ViewBag.MSG = massage;

            return View();

        }
        public ActionResult CustomerProfile(int custID, int? page, string SearchText)
        {
            CustomerViewModel cvModel = new CustomerViewModel();

            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            cvModel.CUSTOMER = cDAO.FindById(custID);
            db.Close();

            db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            cvModel.REPAIRING = rDAO.FindByCustID(custID);
            db.Close();
            ViewBag.CUSTOMER = cvModel;

            db = new Database();
            rDAO = new RepairDAO(db);
            cvModel.WAIT_REP = rDAO.CountByCustID_WaitRep(custID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            cvModel.NOW_REP = rDAO.CountByCustID_NowRep(custID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            cvModel.SUCCESS_REP = rDAO.CountByCustID_SuccessRep(custID);
            db.Close();

            db = new Database();
            rDAO = new RepairDAO(db);
            cvModel.CANNOT_REP = rDAO.CountByCustID_CannotRep(custID);
            db.Close();

            var data = from r in cvModel.REPAIRING select r;

            if (!String.IsNullOrEmpty(SearchText))
            {
                if (!SearchText.Equals("0"))
                {
                    data = data.Where(
                   d => d.STATUS.STATUS_ID.ToString().Contains(SearchText)
                   );
                }
            }

            var numPage = page ?? 1;
            ViewBag.REPAIRING = data.ToPagedList(numPage, 20);

            return View();
        }
    }
}
