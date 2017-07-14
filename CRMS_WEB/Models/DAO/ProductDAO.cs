using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class ProductDAO : DAO<ProductModel>
    {
        Database db;
        public ProductDAO(Database db)
        {
            this.db = db;
        }
        public int Add(ProductModel beans)
        {
            string sql = "INSERT INTO PRODUCT(PRO_TYPE_ID,PRO_BRAND_ID) VALUES("+beans.TYPE.PRO_TYP_ID+","+beans.BANRD.PRO_BRAND_ID+");SELECT @@IDENTITY;";
            System.Diagnostics.Debug.WriteLine("ADD PRODUCT SQL:"+sql);
            return db.add(sql);
        }

        public int Delete(ProductModel beans)
        {
            string sql = "DELETE FROM PRODUCT WHERE PRODUCT_NO=" + beans.PRODUCT_NO;
            return db.remove(sql);
        }
          public int DeleteByTypeIdAndBrandId(ProductModel beans)
        {
            string sql = "DELETE FROM PRODUCT WHERE PRO_TYPE_ID=" + beans.TYPE.PRO_TYP_ID+" AND PRO_BRAND_ID="+beans.BANRD.PRO_BRAND_ID;
            return db.remove(sql);
        }
        
        public int Update(ProductModel beans)
        {
            string sql = "UPDATE PRODUCT SET PRO_TYPE_ID="+beans.TYPE.PRO_TYP_ID+ ", PRO_BRAND_ID="+beans.BANRD.PRO_BRAND_ID+" WHERE PRODUCT_NO="+beans.PRODUCT_NO;
            return db.update(sql);
        }

        public List<ProductModel> FindAll()
        {
            string sql = "SELECT * FROM PRODUCT";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<ProductModel> listModel = new List<ProductModel>();
            foreach(var data in listData)
            {
                ProductModel model = new ProductModel();
                model = MappingBeans(data);
            }
            return listModel;
        }

        public ProductModel FindById(ProductModel beans)
        {
            string sql = "SELECT * FROM PRODUCT WHERE PRODUCT_NO="+beans.PRODUCT_NO;
            Dictionary<string, object> data = db.querySingle(sql);
            ProductModel model = this.MappingBeans(data);
            return model;
        }

        public ProductModel FindById(int id)
        {
            string sql = "SELECT * FROM PRODUCT WHERE PRODUCT_NO=" +id;
            Dictionary<string, object> data = db.querySingle(sql);
            ProductModel model = this.MappingBeans(data);
            return model;
        }
        public List<ProductModel> FindByTypeID(int typeID)
        {
            string sql = "SELECT * FROM PRODUCT WHERE PRO_TYPE_ID="+typeID;

            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<ProductModel> listModel = new List<ProductModel>();

            foreach (var data in listData)
            {
                ProductModel model = new ProductModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }
            return listModel;
        }
        public Boolean HasField(ProductModel model)
        {
            string sql = "SELECT * FROM PRODUCT WHERE PRO_TYPE_ID="+model.TYPE.PRO_TYP_ID+" AND PRO_BRAND_ID="+model.BANRD.PRO_BRAND_ID;
            Dictionary<string, object> data = db.querySingle(sql);

            if (data.Count > 0)
                return true;
            else
                return false;
        }
        public ProductModel MappingBeans(Dictionary<string, object> map)
        {
           
            ProductModel model = new ProductModel();
            model.PRODUCT_NO = Int32.Parse(map["PRODUCT_NO"].ToString());
            model.PRODUCT_REG = (DateTime)map["PRODUCT_REG"];
            
            Database db = new Database();
            ProductTypeDAO ptDAO = new ProductTypeDAO(db);
            model.TYPE = new ProductTypeModel();
            model.TYPE = ptDAO.FindById(Int32.Parse(map["PRO_TYPE_ID"].ToString()));
            db.Close();

            db = new Database();
            ProductBrandDAO pbDAO = new ProductBrandDAO(db);
            model.BANRD = new ProductBrandModel();
            model.BANRD = pbDAO.FindById(Int32.Parse(map["PRO_BRAND_ID"].ToString()));
            db.Close();

            return model;
        }
    }
}