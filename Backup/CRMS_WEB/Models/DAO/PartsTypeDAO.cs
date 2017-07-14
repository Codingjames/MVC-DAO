using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class PartsTypeDAO : DAO<PartsTypeModel>
    {
        Database db;
        public PartsTypeDAO(Database db)
        {
            this.db = db;
        }
        public int Add(PartsTypeModel beans)
        {
            string sql = "INSERT INTO PARTS_TYPE(PARTS_TYPE_NAME) VALUES('"+beans.PART_TYPE_NAME+"');SELECT @@IDENTITY;";
            return db.add(sql);
        }

        public int Delete(PartsTypeModel beans)
        {
            string sql = "DELETE FROM PARTS_TYPE WHERE PARTS_TYPE_ID="+beans.PART_TYPE_ID;
            return db.remove(sql);
        }

        public int Update(PartsTypeModel beans)
        {
            string sql = "UPDATE PARTS_TYPE SET PARTS_TYPE_NAME='"+beans.PART_TYPE_NAME+"' WHERE PARTS_TYPE_ID="+beans.PART_TYPE_ID;
            return db.update(sql);
        }

        public List<PartsTypeModel> FindByTypeName(string typeName)
        {
            string sql = "SELECT * FROM PARTS_TYPE WHERE PARTS_TYPE_NAME='"+typeName+"' ORDER BY PARTS_TYPE_NAME ASC";
            List<Dictionary<string,object>> listData = db.queryList(sql);
            List<PartsTypeModel> listModel = new List<PartsTypeModel>();

            foreach(var data in listData)
            {
                PartsTypeModel model = new PartsTypeModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }
        public List<PartsTypeModel> FindAll()
        {
            string sql = "SELECT * FROM PARTS_TYPE ORDER BY PARTS_TYPE_NAME ASC";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<PartsTypeModel> listModel = new List<PartsTypeModel>();

            foreach (var data in listData)
            {
                PartsTypeModel model = new PartsTypeModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }
        public PartsTypeModel FindById(PartsTypeModel beans)
        {
            string sql = "SELECT * FROM PARTS_TYPE WHERE PARTS_TYPE_ID="+beans.PART_TYPE_ID;
            Dictionary<string, object> data = db.querySingle(sql);
            PartsTypeModel model = MappingBeans(data);
            return model;
        }

        public PartsTypeModel FindById(int id)
        {
            string sql = "SELECT * FROM PARTS_TYPE WHERE PARTS_TYPE_ID=" + id;
            Dictionary<string, object> data = db.querySingle(sql);
            PartsTypeModel model = MappingBeans(data);
            return model;
        }

        public PartsTypeModel MappingBeans(Dictionary<string, object> map)
        {
            PartsTypeModel model = new PartsTypeModel();
            model.PART_TYPE_ID = Int32.Parse(map["PARTS_TYPE_ID"].ToString());
            model.PART_TYPE_NAME = map["PARTS_TYPE_NAME"].ToString();
            return model;
        }
    }
}