using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;
namespace CRMS_WEB.Models.DAO
{
    public class PartsBrandDAO : DAO<PartsBrandModel>
    {
        Database db;
        public PartsBrandDAO(Database db)
        {
            this.db = db;
        }
        public int Add(PartsBrandModel beans)
        {
            string sql = "INSERT INTO PARTS_BRAND(PARTS_BRAND_NAME) VALUES('" + beans.PART_BRAND_NAME + "');SELECT @@IDENTITY;";
            return db.add(sql);
        }

        public int Delete(PartsBrandModel beans)
        {
            string sql = "DELETE FROM PARTS_BRAND WHERE PARTS_BRAND_ID="+beans.PART_BRAND_ID;
            return db.remove(sql);
        }

        public int Update(PartsBrandModel beans)
        {
            string sql = "UPDATE PARTS_BRAND SET PARTS_BRAND_NAME='"+beans.PART_BRAND_NAME+"' WHERE PARTS_BRAND_ID="+beans.PART_BRAND_ID;
            return db.update(sql);
        }

        public List<PartsBrandModel> FindAll()
        {
            string sql = "SELECT * FROM PARTS_BRAND";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<PartsBrandModel> listModel = new List<PartsBrandModel>();

            foreach(var data in listData)
            {
                PartsBrandModel model = new PartsBrandModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }
        public List<PartsBrandModel> FindByBrandName(string pbName)
        {
            string sql = "SELECT * FROM PARTS_BRAND WHERE PARTS_BRAND_NAME='"+pbName+"'";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<PartsBrandModel> listModel = new List<PartsBrandModel>();

            foreach (var data in listData)
            {
                PartsBrandModel model = new PartsBrandModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }
        public PartsBrandModel FindById(PartsBrandModel beans)
        {
            string sql = "SELECT * FROM PARTS_BRAND WHERE PARTS_BRAND_ID="+beans.PART_BRAND_ID;
            Dictionary<string, object> data = db.querySingle(sql);
            PartsBrandModel model = new PartsBrandModel();
            model = MappingBeans(data);
            return model;
        }

        public PartsBrandModel FindById(int id)
        {
            string sql = "SELECT * FROM PARTS_BRAND WHERE PARTS_BRAND_ID=" +id;
            Dictionary<string, object> data = db.querySingle(sql);
            PartsBrandModel model = new PartsBrandModel();
            model = MappingBeans(data);
            return model;
        }

        public PartsBrandModel MappingBeans(Dictionary<string, object> map)
        {
            PartsBrandModel model = new PartsBrandModel();
            model.PART_BRAND_ID = Int32.Parse(map["PARTS_BRAND_ID"].ToString());
            model.PART_BRAND_NAME = map["PARTS_BRAND_NAME"].ToString();

            return model;
        }
    }
}