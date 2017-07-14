using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.DAO;
using CRMS_WEB.Models.DB;
using CRMS_WEB.Coverts;
using CRMS_WEB.Models.Ext;
using System.Net;

namespace CRMS_WEB.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        public ActionResult RegistrationInserting()
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
            int cID = cDAO.Add(cModel);
            db.Close();

            return RedirectToAction("CheckLogin_Customer", "Login", new { custID = cID });
        }
        public ActionResult CheckLogin_Customer(int custID)
        {
           
            ///  ล็อกอิน ลูกค้า
            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            CustomerModel cModel = cDAO.FindById(custID);
            db.Close();

          
            // userModel ไม่มีค่า
            // เช็คค่าของ CustomerModel
           if (cModel != null)
            {
                Session["ID"] = cModel.CUST_NO;
                Session["PRIO"] = cModel.PRIORITY.PRIO_ID;
                Session["LEVEL"] = cModel.PRIORITY.PRIO_NAME;
                Session["NAME"] = cModel.C_NAME + " " + cModel.C_LASTNAME;
                Session.Timeout = 1800;

                return Redirect("~/Main/C"); // C = Customer
            } // end else if
            // ค่าเป็นค่าว่างท้งหมดให้กลับไปหน้า Login
            else
            {
                return RedirectToAction("Alert", new { msg = "ข้อมูลเข้าสูระบบไม่ถูกต้อง", link = "../Login" });
            } // จบ else


        }
        public ActionResult CheckLogin()
        {
            string username = Request.Params["Username"].ToLower().Trim();
            string password = Request.Params["Password"];

            /// ล็อกอิน staff
            Database db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            UsersModel uModel = uDAO.GetLogin(username, password);
            db.Close();

            ///  ล็อกอิน ลูกค้า
            db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            CustomerModel cModel = cDAO.GetLogin(username, password);
            db.Close();

            // userModel มีค่า
            if (uModel != null)
            {
                Session["ID"] = uModel.USER_NO;
                Session["PRIO"] = uModel.PRIORITY.PRIO_ID;
                Session["LEVEL"] = uModel.PRIORITY.PRIO_NAME;
                Session["NAME"] = uModel.NAME + " " + uModel.LASTNAME;
                Session.Timeout = 1800;

                return Redirect("~/Main/Staff");
            }
            // userModel ไม่มีค่า
            // เช็คค่าของ CustomerModel
            else if (cModel != null)
            {
                Session["ID"] = cModel.CUST_NO;
                Session["PRIO"] = cModel.PRIORITY.PRIO_ID;
                Session["LEVEL"] = cModel.PRIORITY.PRIO_NAME;
                Session["NAME"] = cModel.C_NAME + " " + cModel.C_LASTNAME;
                Session.Timeout = 1800;

                return Redirect("~/Main/C"); // C = Customer
            } // end else if
            // ค่าเป็นค่าว่างท้งหมดให้กลับไปหน้า Login
            else
            {
                return RedirectToAction("Alert", new { msg = "ข้อมูลเข้าสูระบบไม่ถูกต้อง", link = "../Login" });
            } // จบ else


        }
        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("~/Home");

        }
        public ActionResult Alert()
        {
            string msg = Request.Params["msg"];
            string link = Request.Params["link"];

            return View();
        }
        protected void Page_Load()//(object sender, EventArgs e)
        {
            String Username = "james.black";
            String Password = "Test@1234";
            String PhoneList = "0956182415;";
            String Message = "เจนกูส่งจากเว็บ ถ้าสนไลน์มานะ ASP.NET test";
            String SenderName = "ClickNext";

            Byte[] ByteMsg = System.Text.Encoding.GetEncoding("TIS-620").GetBytes(Message);
            Message = HttpUtility.UrlEncode(ByteMsg);

            String ParamFormat = "?User={0}&Password={1}&Msnlist={2}&Msg={3}&Sender={4}";
            String Parameters = String.Format(ParamFormat, Username, Password, PhoneList, Message, SenderName);
            String API_URL = "http://member.smsmkt.com/SMSLink/SendMsg/index.php";

            WebClient webc = new WebClient();
            String Result = webc.DownloadString(API_URL + Parameters);
            Response.Write(Result);
        }
    }
}
