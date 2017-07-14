using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class ProductTypeDAO : DAO<ProductTypeModel>
    {
        Database db;
        public ProductTypeDAO(Database db)
        {
            this.db = db;
        }
        public int Add(ProductTypeModel beans)
        {
            string sql = "INSERT INTO PRODUCT_TYPE(PRO_TYPE_NAME) VALUES('"+beans.PRO_TYPE_NAME+"') ; SELECT @@IDENTITY;";
            return db.add(sql);
        }

        public int Delete(ProductTypeModel beans)
        {
            string sql = "DELETE FROM PRODUCT_TYPE WHERE PRO_TYPE_ID="+beans.PRO_TYP_ID;
            return db.remove(sql);
        }

        public int Update(ProductTypeModel beans)
        {
            string sql = "UPDATE PRODUCT_TYPE SET PRO_TYPE_NAME='"+beans.PRO_TYPE_NAME+"' WHERE PRO_TYPE_ID="+beans.PRO_TYP_ID;
            return db.update(sql);
        }

        public List<ProductTypeModel> FindAll()
        {
            string sql = "SELECT * FROM PRODUCT_TYPE";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<ProductTypeModel> listModel = new List<ProductTypeModel>();
            
            foreach(var data in listData)
            {
                ProductTypeModel model = new ProductTypeModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }

        public ProductTypeModel FindById(ProductTypeModel beans)
        {
            string sql = "SELECT * FROM PRODUCT_TYPE WHERE PRO_TYPE_ID = " + beans.PRO_TYP_ID;
            Dictionary<string, object> data = db.querySingle(sql);
            ProductTypeModel model = MappingBeans(data);
            return model;
        }

        public ProductTypeModel FindById(int id)
        {
            string sql = "SELECT * FROM PRODUCT_TYPE WHERE PRO_TYPE_ID = "+id;
            Dictionary<string, object> data = db.querySingle(sql);
            ProductTypeModel model = MappingBeans(data);
            return model;
        }

        public ProductTypeModel MappingBeans(Dictionary<string, object> map)
        {
            ProductTypeModel model = new ProductTypeModel();
            model.PRO_TYP_ID = Int32.Parse(map["PRO_TYPE_ID"].ToString());
            model.PRO_TYPE_NAME = map["PRO_TYPE_NAME"].ToString();

            return model;
        }
    }
}