using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.DAO;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        
        // ฟังชั้นไว้เรียกฟังชั่น จาว่าสคริป อีกที 
        public ActionResult Alert(string link, string massage)
        {
            ViewBag.LINK = link;
            ViewBag.MSG = massage;
            return View();
        }
        public ActionResult ManageProduct()
        {
            Database db = new Database();
            ProductTypeDAO ptDAO = new ProductTypeDAO(db);
            List<ProductTypeModel> lisPTModel = ptDAO.FindAll();
            db.Close();

            ViewData["PRODUCT_TYPE"] = lisPTModel;

            return View();
        }
        // เมื่อคลิกปุ่มเรดิโอ จะโหลดข้อมูลส่วนนี้ไปแสดง
        public ActionResult ManageProduct_DataLoading()
        {
            int typeID = Int32.Parse(Request.Params["typeID"]);

            Database db = new Database();
            ProductBrandDAO pbDAO = new ProductBrandDAO(db);
            List<ProductBrandModel> listPbModel = pbDAO.FindAll();
            ViewData["BRAND"] = listPbModel;
            db.Close();

            db = new Database();
            ProductDAO proDAO = new ProductDAO(db);
            List<ProductModel> proModel = proDAO.FindByTypeID(typeID);
            ViewData["PRODUCT"] = proModel;
            db.Close();

            return View();
        }
        public ActionResult AddProductType()
        {
            string proTypeName = Request.Params["protype_name"].Trim();
            ProductTypeModel ptModel = new ProductTypeModel();
            ptModel.PRO_TYPE_NAME = proTypeName;

            Database db = new Database();
            ProductTypeDAO ptDAO = new ProductTypeDAO(db);
            int id = ptDAO.Add(ptModel);
            db.Close();

            System.Diagnostics.Debug.WriteLine("LAST ID PRODUCT_TYPE : " + id);
            if (id > 0)
            {
                return RedirectToAction("Alert", "Product", new { link = "~/Product/ManageProduct", massage = "เพิ่มข้อมูลเรียบร้อย" });

            }
            else
            {
                return RedirectToAction("ManageProduct", "Product");
            }


        }
        public ActionResult AddProductBrand()
        {
            string proBrandName = Request.Params["probrand_name"].Trim();
            ProductBrandModel pbModel = new ProductBrandModel();
            pbModel.PRO_BAND_NAME = proBrandName;

            Database db = new Database();
            ProductBrandDAO pbDAO = new ProductBrandDAO(db);
            int id = pbDAO.Add(pbModel);
            db.Close();

            if (id > 0)
            {
                return RedirectToAction("Alert", "Product", new {link= "~/Product/ManageProduct",massage="เพิ่มข้อมูลเรียบร้อย"});
            }
            else
            {
                return RedirectToAction("ManageProduct", "Product");
            }
        }
        public ActionResult addProduct()
        {
            Database db;
            int typeID = Int32.Parse(Request.Params["protype_id"]);
            string[] brandID = Request.Form.GetValues("probrand_id");

            ProductTypeModel ptModel = new ProductTypeModel();
            ptModel.PRO_TYP_ID = typeID;

            for (int i = 0; i < brandID.Length; i++)
            {
                ProductBrandModel pbModel = new ProductBrandModel();
                pbModel.PRO_BRAND_ID = Int32.Parse(brandID[i]);

                ProductModel pModel = new ProductModel();
                pModel.BANRD = pbModel;
                pModel.TYPE = ptModel;

                // ค้นข้อมูลมา ตรวจสอบค่าก่อนจะเพิ่มลงฐานข้อมูล
                db = new Database();
                ProductDAO pDAO = new ProductDAO(db);
                bool tf = pDAO.HasField(pModel);                // ค้นหาจาก Type and brand
                db.Close();

                // ถ้าไม่มีให้เพิ่มได้ ถ้ามีไม่ต้องทำอะไร
                if (!tf)
                {
                    db = new Database();
                    pDAO = new ProductDAO(db);
                    pDAO.Add(pModel);
                    db.Close();
                }
            }

            return RedirectToAction("ManageProduct", "Product");
        }
        // เมื่อคลิ๊ก จะส่งค่า Brand Product CheckBox  มาลบที่ฐานข้อมูล 
        public JsonResult Uncheck()
        {
            int brandID = Int32.Parse(Request.Params["brand_id"]);
            int typeID = Int32.Parse(Request.Params["type_id"]);

            ProductModel pModel = new ProductModel();
            pModel.BANRD = new ProductBrandModel();
            pModel.BANRD.PRO_BRAND_ID = brandID;

            pModel.TYPE = new ProductTypeModel();
            pModel.TYPE.PRO_TYP_ID = typeID;

            Database db = new Database();
            ProductDAO pDAO = new ProductDAO(db);
            pDAO.DeleteByTypeIdAndBrandId(pModel);
            db.Close();

            return Json(new { errMsg = "test" });
        }

        public ActionResult ProductBrandManagement()
        {
            Database db = new Database();
            ProductBrandDAO pbDAO = new ProductBrandDAO(db);
            List<ProductBrandModel> pbModel = pbDAO.FindAll();
            db.Close();

            ViewBag.ProductBrand = pbModel;
            return View();
        }
        public ActionResult ProductBrandDeleting(int brandID)
        {
            try
            {
                ProductBrandModel pbModel = new ProductBrandModel();
                pbModel.PRO_BRAND_ID = brandID;

                Database db = new Database();
                ProductBrandDAO pbDAO = new ProductBrandDAO(db);
                pbDAO.Delete(pbModel);
                db.Close();
                return Redirect("~/Product/ProductBrandManagement");
            }
            catch
            {
                return RedirectToAction("Alert", "Product", new { link = "ProductBrandManagement", massage = "มีรายการที่อ้างถึงข้อมูลนี้อยู่ ไม่สามารถลบได้" });
            }
        }
        public ActionResult ProductBrandUpdating(string proBrand_name, int proBrand_id)
        {
            ProductBrandModel pbModel = new ProductBrandModel();
            pbModel.PRO_BRAND_ID = proBrand_id;
            pbModel.PRO_BAND_NAME = proBrand_name;

            Database db = new Database();
            ProductBrandDAO pbDAO = new ProductBrandDAO(db);
            pbDAO.Update(pbModel);
            db.Close();

            return Redirect("~/Product/ProductBrandManagement");
        }
        public ActionResult ProductTypeManagement()
        {
            Database db = new Database();
            ProductTypeDAO ptDAO = new ProductTypeDAO(db);
            List<ProductTypeModel> ptModel = ptDAO.FindAll();
            db.Close();

            ViewBag.ProductType = ptModel;
            return View();
        }
        public ActionResult ProductTypeUpdating(int proType_id, string proType_name)
        {
            ProductTypeModel ptModel = new ProductTypeModel();
            ptModel.PRO_TYP_ID = proType_id;
            ptModel.PRO_TYPE_NAME = proType_name;

            Database db = new Database();
            ProductTypeDAO ptDAO = new ProductTypeDAO(db);
            ptDAO.Update(ptModel);
            db.Close();

            return Redirect("~/Product/ProductTypeManagement");
        }
        public ActionResult ProductTypeDeleting(int typeID)
        {
            try
            {
                ProductTypeModel ptModel = new ProductTypeModel();
                ptModel.PRO_TYP_ID = typeID;

                Database db = new Database();
                ProductTypeDAO ptDAO = new ProductTypeDAO(db);
                ptDAO.Delete(ptModel);
                db.Close();

                return Redirect("~/Product/ProductTypeManagement");
            }
            catch
            {
                return RedirectToAction("alert", "Product", new { link = "ProductTypeManagement", massage = "มีรายการที่อ้างถึงข้อมูลนี้อยู่ ไม่สามารถลบได้" });               
            }

            
        }
        public JsonResult ProductBrandName_DataLoading(int pBrandID)
        {
            Database db = new Database();
            ProductBrandDAO pbDAO = new ProductBrandDAO(db);
            ProductBrandModel pbModel = pbDAO.FindById(pBrandID);
            db.Close();

            return Json(new { brandID=pbModel.PRO_BRAND_ID , brandName = pbModel.PRO_BAND_NAME });
        }
        public JsonResult ProductTypeName_DataLoading(int proTypeID)
        {
            Database db = new Database();
            ProductTypeDAO ptDAO = new ProductTypeDAO(db);
            ProductTypeModel ptModel = ptDAO.FindById(proTypeID);
            db.Close();

            return Json(new { typeName = ptModel.PRO_TYPE_NAME, typeID = ptModel.PRO_TYP_ID });
        }

    }
}
