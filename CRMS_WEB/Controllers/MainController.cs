using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.DB;
using CRMS_WEB.Models.DAO;
using PagedList.Mvc;
using PagedList;

namespace CRMS_WEB.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Staff(string SearchText, string statusText, int? page)
        {
            int userNo = (int)Session["ID"];

            Database db = new Database();
            RepairDAO repDAO = new RepairDAO(db);
            HashSet<RepairModel> repModel = repDAO.FindSetByUserID(userNo);
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
        public ActionResult C()
        {
            return View();
        }

    }
}
