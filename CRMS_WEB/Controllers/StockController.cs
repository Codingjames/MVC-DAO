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
    public class StockController : Controller
    {
        //
        // GET: /Stock/

        public ActionResult Index(string SearchText, int? page)
        {
            Database db = new Database();
            StockDAO stkDAO = new StockDAO(db);
            List<StockModel> stkModel = stkDAO.FindAll();

            var data = from d in stkModel select d;

            // ค้นหา
            if(!String.IsNullOrEmpty(SearchText)){
                //ค้นหา
                data = data.Where(
                    d=> d.STOCK_ID.ToString().Contains(SearchText)
                        || d.PART.TYPE.PART_TYPE_NAME.Contains(SearchText) || d.PART.TYPE.PART_TYPE_NAME.Contains(SearchText.ToUpper()) || d.PART.TYPE.PART_TYPE_NAME.Contains(SearchText.ToLower())
                        || d.PART.BRAND.PART_BRAND_NAME.Contains(SearchText) || d.PART.BRAND.PART_BRAND_NAME.Contains(SearchText.ToLower()) || d.PART.BRAND.PART_BRAND_NAME.Contains(SearchText.ToUpper())
                        || d.PRICE.ToString().Contains(SearchText)                 
                        || d.STOCK_INFO.Contains(SearchText.ToUpper()) || d.STOCK_INFO.Contains(SearchText.ToLower()) ||d.STOCK_INFO.Contains(SearchText)
                    ).ToList();
            }

            var numPage = page ?? 1;
            ViewBag.Stock = data.ToPagedList(numPage,20);
            db.Close();
            return View();
        }
        // ปุ่มกดป๊อบอัพหน้า Index
        public ActionResult StockAdding_Popup()
        {
            Database db = new Database();
            PartsTypeDAO ptDAO = new PartsTypeDAO(db);
            List<PartsTypeModel> ptModel = ptDAO.FindAll();
            db.Close();

            ViewBag.PartsType = ptModel;
            return View();
        }
       
        // รับค่าจาก StockAdding_Popup
        public ActionResult StockInserting(int partsID, string stockInfo, int unit,int price)
        {
            StockModel sModel = new StockModel();
            sModel.PART = new PartsModel();
            sModel.PART.PART_ID = partsID;
            sModel.STOCK_INFO = stockInfo;
            sModel.UNIT = unit;
            sModel.PRICE = price;

            Database db = new Database();
            StockDAO sDAO = new StockDAO(db);
            sDAO.Add(sModel);
            db.Close();

            return Redirect("~/Stock");
        }

        //ปุ่มเรียกใช้อยู่หน้า index/stock
        public ActionResult stockEdeting_Popup(int stockID)
        {
            // ตัวที่จะแก้ไข
            Database db = new Database();
            StockDAO sDAO = new StockDAO(db);
            StockModel sModel = sDAO.FindById(stockID);
            db.Close();
            ViewBag.Stock = sModel;

            // ทำตัวเลือกประเภท
            db = new Database();
            PartsTypeDAO ptDAO = new PartsTypeDAO(db);
            List<PartsTypeModel> ptModel = ptDAO.FindAll();
            db.Close();
            ViewBag.PartsType = ptModel;

            //ทำตัวเลือกยี่ห้อ
            db = new Database();
            PartsDAO pDAO = new PartsDAO(db);
            List<PartsModel> pModel = pDAO.FindByPartsTypeID(sModel.PART.TYPE.PART_TYPE_ID);
            db.Close();
            ViewBag.PartsBrand = pModel;

            return View();
        }
        public ActionResult StockUpdating(int partsID, string stockInfo, int unit, int price,int stockID)
        {
            System.Diagnostics.Debug.WriteLine("StockUpdating :" + stockID + " partsID" + partsID + " stockInfo" + stockInfo + " unit" + unit + " price" + price);
            StockModel sModel = new StockModel();
            sModel.STOCK_ID = stockID;
            sModel.PART = new PartsModel();
            sModel.PART.PART_ID = partsID;
            sModel.STOCK_INFO = stockInfo;
            sModel.UNIT = unit;
            sModel.PRICE = price;

            Database db = new Database();
            StockDAO sDAO = new StockDAO(db);
            sDAO.Update(sModel);
            db.Close();

            return Redirect("~/Stock");
        }
        public ActionResult StockDeleting(int stockID)
        {
            try
            {
                StockModel sModel = new StockModel();
                sModel.STOCK_ID = stockID;

                Database db = new Database();
                StockDAO sDAO = new StockDAO(db);
                sDAO.Delete(sModel);
                db.Close();

                return Redirect("~/Stock");
            }
            catch
            {
                return RedirectToAction("alert", "Stock", new { link = "", massage = "มีรายการที่อ้างถึงข้อมูลนี้อยู่ ไม่สามารถลบได้" });

            }
        }
        public JsonResult StockAdding_PopupSelectDataLoading(int idValue)
        {
            Database db = new Database();
            PartsDAO pDAO = new PartsDAO(db);
            List<PartsModel> pModel = pDAO.FindByPartsTypeID(idValue);
            db.Close();

            var data = "";
            if(pModel.Count > 0){
                data = "<option value=''>---- เลือกยี่ห้อ ----</option>";
                foreach(var brand in pModel){
                    data += "<option value='"+brand.PART_ID+"'>"+brand.BRAND.PART_BRAND_NAME+"</option>";
                }
            }
            else
            {
                data = "<option value=''>---- ไม่มีข้อมูล ----</option>";
            }


            return Json(new { data });
        }
        public ActionResult PartsBrandManagement()
        {
            Database db = new Database();
            PartsBrandDAO pbDAO = new PartsBrandDAO(db);
            List<PartsBrandModel> pbModel = pbDAO.FindAll();
            ViewBag.Brand = pbModel;
            db.Close();
            return View();
        }
        public ActionResult PartsBrandUpdating(int partsBrand_id, string partsBrand_name)
        {
            PartsBrandModel pbModel = new PartsBrandModel();
            pbModel.PART_BRAND_ID = partsBrand_id;
            pbModel.PART_BRAND_NAME = partsBrand_name;

            Database db = new Database();
            PartsBrandDAO pbDAO = new PartsBrandDAO(db);
            pbDAO.Update(pbModel);
            db.Close();

            return Redirect("~/Stock/PartsBrandManagement");
        }
        public ActionResult PartsBrandDeleting(int brandID)
        {
            try
            {
                PartsBrandModel pbModel = new PartsBrandModel();
                pbModel.PART_BRAND_ID = brandID;

                Database db = new Database();
                PartsBrandDAO pbDAO = new PartsBrandDAO(db);
                pbDAO.Delete(pbModel);

                db.Close();
                return Redirect("~/Stock/PartsBrandManagement");
            }
            catch (Exception ex)
            {
                return RedirectToAction("alert", "Stock", new { link = "PartsBrandManagement", massage = "มีรายการที่อ้างถึงข้อมูลนี้อยู่ ไม่สามารถลบได้"});
            }
           
        }
        /// เพิ่มยี่ห้อ
        /// <returns>ไปหน้าเดิม</returns>
        public ActionResult PartsBrandInserting(string partsbrand_name)
        {
            Database db = new Database();
            PartsBrandDAO pbDAO = new PartsBrandDAO(db);
            List<PartsBrandModel> pbModel = pbDAO.FindByBrandName(partsbrand_name);
            db.Close();

            if(pbModel.Count > 0)
            {
                return RedirectToAction("alert", "Stock", new { link = "StockManagement", massage = "ไม่สามารถเพิ่มข้อมูลได้มีข้อมูลนี้แล้ว กรุณาตรวจสอบอีกครั้ง !" });
            }
            else
            {
                PartsBrandModel pbMD = new PartsBrandModel();
                pbMD.PART_BRAND_NAME = partsbrand_name;

                db = new Database();
                pbDAO = new PartsBrandDAO(db);
                pbDAO.Add(pbMD);
                db.Close();
                return RedirectToAction("alert", "Stock", new { link = "StockManagement", massage = "เพิ่มข้อมูลสำเร็จแล้ว." });                
            }

        }
        public JsonResult PartsBrandName_DataLoading(int pBrandID)
        {
            Database db = new Database();
            PartsBrandDAO pbDAO = new PartsBrandDAO(db);
            PartsBrandModel pbModel = pbDAO.FindById(pBrandID);
            db.Close();

            return Json(new { brandName = pbModel.PART_BRAND_NAME, brandID = pbModel.PART_BRAND_ID });
        }
        public ActionResult PartsTypeManagement()
        {
            Database db = new Database();
            PartsTypeDAO ptDAO = new PartsTypeDAO(db);
            List<PartsTypeModel> ptModel = ptDAO.FindAll();
            db.Close();

            ViewBag.PartsType = ptModel;

            return View();
        }
        /// เพิ่มชนิด ของ อะไหล่
        /// <returns>ไปหน้าเดิม</returns>
        public ActionResult PartsTypeInserting(string partsType_name)
        {
            Database db = new Database();
            PartsTypeDAO ptDAO = new PartsTypeDAO(db);
            List<PartsTypeModel> ptModel = ptDAO.FindByTypeName(partsType_name);
            db.Close();
            System.Diagnostics.Debug.WriteLine("Values of partsType_name :" + partsType_name);

             //9ตรวจสอบว่ามีหรือไม่ ถ้ามีใน DB แล้ว ไม่ให้เพิ่ม
            if (ptModel.Count > 0)
            {
                return RedirectToAction("alert", "Stock", new { link = "StockManagement", massage="ไม่สามารถเพิ่มข้อมูลได้มีข้อมูลนี้แล้ว กรุณาตรวจสอบอีกครั้ง !" });
            }
            else
            {
                PartsTypeModel model = new PartsTypeModel();
                model.PART_TYPE_NAME = partsType_name;

                db = new Database();
                ptDAO = new PartsTypeDAO(db);
                ptDAO.Add(model);
                db.Close();
                return RedirectToAction("alert", "Stock", new { link = "StockManagement", massage = "เพิ่มข้อมูลสำเร็จแล้ว." });

            }
        }
        public ActionResult PartsTypeUpdating(string partsType_name, int partsType_id)
        {
            PartsTypeModel model = new PartsTypeModel();
            model.PART_TYPE_ID = partsType_id;
            model.PART_TYPE_NAME = partsType_name;

            Database db = new Database();
            PartsTypeDAO ptDAO = new PartsTypeDAO(db);
            ptDAO.Update(model);
            db.Close();

            return Redirect("~/Stock/PartsTypeManagement");
        }
        public ActionResult PartsTypeDeleting(int typeID)
        {
            PartsTypeModel model = new PartsTypeModel();
            model.PART_TYPE_ID = typeID;

            Database db = new Database();
            PartsTypeDAO ptDAO = new PartsTypeDAO(db);
            ptDAO.Delete(model);
            db.Close();

            return Redirect("~/Stock/PartsTypeManagement");
        }
       
       
        public ActionResult StockManagement()
        {
            Database db = new Database();
            PartsTypeDAO ptDAO = new PartsTypeDAO(db);
            List<PartsTypeModel> ptModel = ptDAO.FindAll();
            db.Close();

            ViewBag.PartsType = ptModel;

            return View();
        }
        public ActionResult PartsInserting(int partsTypeID,int[] partsBrandID )
        {
            Database db;
            PartsTypeModel ptModel = new PartsTypeModel();
            ptModel.PART_TYPE_ID = partsTypeID;

            for (int i = 0; i < partsBrandID.Length; i++)
            {
                PartsBrandModel pbModel = new PartsBrandModel();
                pbModel.PART_BRAND_ID = partsBrandID[i];

                PartsModel pModel = new PartsModel();
                pModel.TYPE = ptModel;
                pModel.BRAND = pbModel;

                // ค้นข้อมูลมา ตรวจสอบค่าก่อนจะเพิ่มลงฐานข้อมูล
                db = new Database();
                PartsDAO pDAO = new PartsDAO(db);
                bool hasRow = pDAO.HasField(pModel);
                db.Close();

                // ถ้าไม่มีให้เพิ่มได้ ถ้ามีไม่ต้องทำอะไร
                if(!hasRow){
                    db = new Database();
                    pDAO = new PartsDAO(db);
                    pDAO.Add(pModel);
                    db.Close();
                }

            }
            
            return Redirect("~/Stock/StockManagement");
        }
        /// รับค่าจากหน้า  PartsTypeManagement โดย javascript function getEditType ส่งมา
        /// <param name="ptypeID">PartsTypeID ที่ส่งมาค้น</param>
        /// <returns>ผลลัพธ์ที่ค้นหา</returns>
        public JsonResult PartsTypeName_DataLoading(int ptypeID)
        {
            Database db = new Database();
            PartsTypeDAO ptDAO = new PartsTypeDAO(db);
            PartsTypeModel ptModel = ptDAO.FindById(ptypeID);
            db.Close();

            return Json(new { typeName = ptModel.PART_TYPE_NAME,typeID = ptModel.PART_TYPE_ID });
        }
        public ActionResult StockManagement_DataLoading(int typeID)
        {
            Database db = new Database();
            PartsBrandDAO pbDAO = new PartsBrandDAO(db);
            List<PartsBrandModel> pbModel = pbDAO.FindAll();
            ViewBag.PartsBrand = pbModel;
            db.Close();

            db = new Database();
            PartsDAO pDAO = new PartsDAO(db);
            List<PartsModel> tModel = pDAO.FindByPartsTypeID(typeID);
            ViewBag.Parts = tModel;
            db.Close();

            return View();
        }
        public ActionResult alert(string link,string massage)
        {
            ViewBag.MSG = massage;
            ViewBag.LINK = "../Stock/"+link;
            return View();
        }
        public JsonResult Uncheck(int type_id, int brand_id)
        {
            PartsTypeModel ptModel = new PartsTypeModel();
            ptModel.PART_TYPE_ID = type_id;

            PartsBrandModel pbModel = new PartsBrandModel();
            pbModel.PART_BRAND_ID = brand_id;

            PartsModel pModel = new PartsModel();
            pModel.TYPE = ptModel;
            pModel.BRAND = pbModel;

            Database db = new Database();
            PartsDAO pDAO = new PartsDAO(db);
            int x = pDAO.DeleteByTypeAndBrand(pModel);
            db.Close();
            return Json(new { errMsg = "test" });
        }
    }
}
