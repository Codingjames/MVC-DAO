using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.DAO;
using CRMS_WEB.Models.DB;
using CRMS_WEB.Models.Ext;
namespace CRMS_WEB.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            HashSet<RepairModel> repSet = rDAO.FindSetForShow();
            db.Close();

            ViewBag.REP = repSet;
            return View();
        }
        public ActionResult ServiceList()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
    }
}
