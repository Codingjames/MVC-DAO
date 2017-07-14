using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.DAO;
using CRMS_WEB.Models.DB;
using PagedList.Mvc;
using PagedList;
using System.Net;

namespace CRMS_WEB.Controllers
{
    public class SysSmsController : Controller
    {
        //
        // GET: /SysSms/

        public ActionResult SmsCustomer(int custID, int? page)
        {
            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            CustomerModel cModel = cDAO.FindById(custID);
            db.Close();
            ViewBag.CUSTOMER = cModel;

            db = new Database();
            MassageDAO newDAO = new MassageDAO(db);
            List<MassageModel> newModel = newDAO.FindByCustomerID(custID);
            db.Close();

            var numPage = page ?? 1;
            var data = newModel.ToPagedList(numPage, 20);
            ViewBag.SMS = data;

            return View();

        }
        public ActionResult SendSms(string sms,int custID,string tel)
        {
            // insert sms into database
            MassageModel model = new MassageModel();
            model.CUSTOMER = new CustomerModel();
            model.CUSTOMER.CUST_NO = custID;
            model.MSG = sms;
            model.TEL = tel;

            Database db = new Database();
            MassageDAO mDAO = new MassageDAO(db);
            int i = mDAO.Add(model);
            db.Close();

           // after inserted  Send 
            if(i>0){
                this.SmsSender(sms, tel);
            }

            return Redirect("~/SysSms/SmsCustomer?custID=" + custID);
        }
        protected void SmsSender(string msg,string phone)//(object sender, EventArgs e)
        {
            // smsmkt
            //String Username = "james.black";
            //String Password = "Test@1234";
            //String SenderName = "ClickNext";

            // THSMS
            String Username = "shutjane";
            String Password = "ebed71";
            String to = phone;
            String from = "SMS";
            String Message = msg;

            //Byte[] ByteMsg = System.Text.Encoding.GetEncoding("TIS-620").GetBytes(Message);
            //Message = HttpUtility.UrlEncode(ByteMsg);

            //String ParamFormat = "?User={0}&Password={1}&Msnlist={2}&Msg={3}&Sender={4}";
            //String Parameters = String.Format(ParamFormat, Username, Password, PhoneList, Message, SenderName);

           // String ParamFormat = "?method=send&$USERNAME={0}&Password={1}&Msnlist={2}&Msg={3}&Sender={4}";
            String ParamFormat = "?method=send&username={0}&password={1}&from={2}&to={3}&message={4}";
            String Parameters = String.Format(ParamFormat, Username, Password, from, to, Message);
 
            String API_URL = "http://www.thsms.com/api/rest";

            WebClient webc = new WebClient();
            System.Diagnostics.Debug.WriteLine(" TEST  SMS" + API_URL + Parameters);
            String Result = webc.DownloadString(API_URL + Parameters);
            Response.Write(Result);
        }
   

}
}
