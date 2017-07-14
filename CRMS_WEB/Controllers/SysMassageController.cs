using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.DAO;
using CRMS_WEB.Models.DB;
using CRMS_WEB.Coverts;
using PagedList.Mvc;
using PagedList;

namespace CRMS_WEB.Controllers
{
    public class SysMassageController : Controller
    {
        //
        // GET: /SysMassage/

        public ActionResult CustomerMassage(int custID, int? page)
        {
            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            CustomerModel cModel = cDAO.FindById(custID);
            db.Close();
            ViewBag.CUSTOMER = cModel;

            db = new Database();
            NewsDAO newDAO = new NewsDAO(db);
            List<NewsModel> newModel = newDAO.FindByCustomerID(custID);
            db.Close();

            var numPage = page ?? 1;
            var data = newModel.ToPagedList(numPage, 20);
            ViewBag.ALL_MSG = data;

            return View();
        }
        public ActionResult CustomerMassage_MassageAddingpopup(int custID)
        {
            return View();
        }
        public ActionResult MassgeInserting(string massage, int custID)
        {
            NewsModel newModel = new NewsModel();
            newModel.CUSTOMER = new CustomerModel();
            newModel.CUSTOMER.CUST_NO = custID;
            newModel.NEWS_MSG = massage;
            newModel.ALERT_STATUS = "new";

            Database db = new Database();
            NewsDAO newDAO = new NewsDAO(db);
            newDAO.Add(newModel);
            db.Close();

            return RedirectToAction("CustomerMassage", "SysMassage", new { custID = custID });
        }
        public ActionResult ReadMassage(int c)
        {
            Database db = new Database();
            NewsDAO nDAO = new NewsDAO(db);
            HashSet<NewsModel> nModel = nDAO.FindAllSetByCustID(c);
            db.Close();

            ViewBag.NEWS = nModel;

            db = new Database();
            nDAO = new NewsDAO(db);
            nDAO.Readed(c);            
            db.Close();

            return View();
        }
        //Javascript ส่งค่ามาจ้าหน้า Template_cust
        public JsonResult DataLoading_BadgeMSG(int c)
        {
            Database db = new Database();
            NewsDAO newsDAO = new NewsDAO(db);
            int rs = newsDAO.CountNewNewsByCustID(c);
            db.Close();

            return Json(new {data = rs});
        }
    }
}
