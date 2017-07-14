using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class PartsDAO : DAO<PartsModel>
    {
        Database db;
        public PartsDAO(Database db)
        {
            this.db = db;
        }
        public int Add(PartsModel beans)
        {
            string sql = "INSERT INTO PARTS(PARTS_TYPE_ID,PARTS_BRAND_ID) VALUES(" + beans.TYPE.PART_TYPE_ID + "," + beans.BRAND.PART_BRAND_ID + ");SELECT @@IDENTITY;";
            System.Diagnostics.Debug.Write("TEST ADD :"+sql );
            return db.add(sql);
        }

        public int Delete(PartsModel beans)
        {
            string sql = "DELETE FROM PARTS WHERE PARTS_ID="+beans.PART_ID;
            return db.remove(sql);
        }
        public int DeleteByTypeAndBrand(PartsModel beans)
        {
            string sql = "DELETE FROM PARTS WHERE PARTS_TYPE_ID=" + beans.TYPE.PART_TYPE_ID + " AND PARTS_BRAND_ID="+beans.BRAND.PART_BRAND_ID;
            return db.remove(sql);
        }
        public int Update(PartsModel beans)
        {
            string sql = "UPDATE PARTS SET PARTS_BRAND_ID="+beans.BRAND.PART_BRAND_ID+" WHERE PARTS_ID="+beans.PART_ID;
            return db.update(sql);
        }

        public List<PartsModel> FindAll()
        {
            string sql = "SELECT * FROM PARTS";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<PartsModel> listModel = new List<PartsModel>();

            foreach(var data in listData){
                PartsModel model = new PartsModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }
            return listModel;
        }
        public List<PartsModel> FindByPartsTypeID(int pTypeID)
        {
            string sql = "SELECT * FROM PARTS WHERE PARTS_TYPE_ID="+pTypeID;
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<PartsModel> listModel = new List<PartsModel>();

            foreach (var data in listData)
            {
                PartsModel model = new PartsModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }
            return listModel;

        }
        public PartsModel FindById(PartsModel beans)
        {
            string sql = "SELECT * FROM PARTS WHERE PARTS_ID="+beans.PART_ID;
            Dictionary<string, object> data = db.querySingle(sql);
            PartsModel model = MappingBeans(data);

            return model;
        }

        public PartsModel FindById(int id)
        {
            string sql = "SELECT * FROM PARTS WHERE PARTS_ID=" +id;
            Dictionary<string, object> data = db.querySingle(sql);
            PartsModel model = MappingBeans(data);

            return model;
        }
        public Boolean HasField(PartsModel model)
        {
            string sql = "SELECT * FROM PARTS WHERE PARTS_TYPE_ID=" + model.TYPE.PART_TYPE_ID + " AND PRO_BRAND_ID=" + model.BRAND.PART_BRAND_ID;
            Dictionary<string, object> data = db.querySingle(sql);

            if (data.Count > 0)
                return true;
            else
                return false;
        }
        public PartsModel MappingBeans(Dictionary<string, object> map)
        {
            PartsModel model = new PartsModel();
            model.PART_ID = Int32.Parse(map["PARTS_ID"].ToString());

            Database db = new Database();
            PartsTypeDAO ptDAO = new PartsTypeDAO(db);
            model.TYPE = new PartsTypeModel();
            model.TYPE = ptDAO.FindById(Int32.Parse(map["PARTS_TYPE_ID"].ToString()));
            db.Close();

            db = new Database();
            PartsBrandDAO pbDAO = new PartsBrandDAO(db);
            model.BRAND = new PartsBrandModel();
            model.BRAND = pbDAO.FindById(Int32.Parse(map["PARTS_BRAND_ID"].ToString()));
            db.Close();

            model.PARTS_REG = (DateTime)map["PARTS_REG"];
            return model;
        }
    }
}