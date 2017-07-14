using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class ProductBrandDAO : DAO<ProductBrandModel>
   {
        Database db;
        public ProductBrandDAO(Database db)
        {
            this.db = db;
        }
        public int Add(ProductBrandModel beans)
        {
            string sql = "INSERT INTO PRODUCT_BRAND(PRO_BRAND_NAME) VALUES('"+beans.PRO_BAND_NAME+"');SELECT @@IDENTITY;";

            return db.add(sql);
        }

        public int Delete(ProductBrandModel beans)
        {
            string sql = "DELETE FROM PRODUCT_BRAND WHERE PRO_BRAND_ID"+beans.PRO_BRAND_ID;
            return db.remove(sql);
        }

        public int Update(ProductBrandModel beans)
        {
            string sql = "UPDATE PRODUCT_BRAND SET PRO_BRAND_NAME='"+beans.PRO_BAND_NAME+"' WHERE PRO_BRAND_ID="+beans.PRO_BRAND_ID;
            return db.update(sql);
        }

        public List<ProductBrandModel> FindAll()
        {
            string sql = "SELECT * FROM PRODUCT_BRAND ORDER BY PRO_BRAND_NAME ASC";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<ProductBrandModel> listModel = new List<ProductBrandModel>();

            foreach(var data in listData)
            {
                ProductBrandModel model = new ProductBrandModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }
            return listModel;
        }

        public ProductBrandModel FindById(ProductBrandModel beans)
        {
            string sql = "SELECT * FROM PRODUCT_BRAND WHERE PRO_BRAND_ID="+beans.PRO_BRAND_ID;
            Dictionary<string, object> data = db.querySingle(sql);
            ProductBrandModel model = MappingBeans(data);
            return model;
        }

        public ProductBrandModel FindById(int id)
        {
            string sql = "SELECT * FROM PRODUCT_BRAND WHERE PRO_BRAND_ID=" + id;
            Dictionary<string, object> data = db.querySingle(sql);
            ProductBrandModel model = MappingBeans(data);
            return model;
        }

        public ProductBrandModel MappingBeans(Dictionary<string, object> map)
        {
            ProductBrandModel model = new ProductBrandModel();
            model.PRO_BRAND_ID = Int32.Parse(map["PRO_BRAND_ID"].ToString());
            model.PRO_BAND_NAME = map["PRO_BRAND_NAME"].ToString();

            return model;
        }
    }
}