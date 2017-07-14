using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;


namespace CRMS_WEB.Models.DAO
{
    public class StockDAO : DAO<StockModel>
    {
        Database db = new Database();
        public StockDAO(Database db)
        {
            this.db = db;
        }
        public int Add(StockModel beans)
        {
            string sql = "INSERT INTO STOCK(PART_ID,STOCK_INFO,UNIT,PRICE) VALUES(" + beans.PART.PART_ID + ",'" + beans.STOCK_INFO + "'," + beans.UNIT + "," + beans.PRICE + "); SELECT @@IDENTITY;";
            System.Diagnostics.Debug.WriteLine("Add :"+sql);
            return db.add(sql);
        }

        public int Delete(StockModel beans)
        {
            string sql = "DELETE FROM STOCK WHERE STOCK_ID=" + beans.STOCK_ID;
            return db.remove(sql);
        }

        public int Update(StockModel beans)
        {
            string sql = "UPDATE STOCK SET PART_ID=" + beans.PART.PART_ID + ", STOCK_INFO='" + beans.STOCK_INFO + "', UNIT=" + beans.UNIT + ", PRICE=" + beans.PRICE + " WHERE STOCK_ID=" + beans.STOCK_ID;
            System.Diagnostics.Debug.WriteLine("Update sql : "+sql);
            return db.update(sql);
        }

        public List<StockModel> FindAll()
        {
            string sql = "SELECT * FROM STOCK";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<StockModel> listModel = new List<StockModel>();
            foreach (var data in listData)
            {
                StockModel sModel = new StockModel();
                sModel = MappingBeans(data);
                listModel.Add(sModel);
            }

            return listModel;
        }

        public StockModel FindById(StockModel beans)
        {
            string sql = "SELECT * FROM STOCK WHERE STOCK_ID="+beans.STOCK_ID;
            Dictionary<string, object> data = db.querySingle(sql);
            StockModel sModel = MappingBeans(data);

            return sModel;

        }

        public StockModel FindById(int id)
        {
            string sql = "SELECT * FROM STOCK WHERE STOCK_ID=" + id;
            Dictionary<string, object> data = db.querySingle(sql);
            StockModel sModel = MappingBeans(data);

            return sModel;
        }
        public int StockDeduction(StockModel beans)
        {
            string sql = "UPDATE STOCK SET UNIT="+beans.UNIT+" WHERE STOCK_ID="+beans.STOCK_ID;
            return db.update(sql);
        }
        public StockModel MappingBeans(Dictionary<string, object> map)
        {
            StockModel sModel = new StockModel();
            sModel.STOCK_ID = Int32.Parse(map["STOCK_ID"].ToString());

            Database db = new Database();
            PartsDAO pDAO = new PartsDAO(db);
            sModel.PART = new PartsModel();
            sModel.PART = pDAO.FindById(Int32.Parse(map["PART_ID"].ToString()));
            db.Close();

            sModel.STOCK_INFO = map["STOCK_INFO"].ToString();
            sModel.UNIT = Int32.Parse(map["UNIT"].ToString());
            sModel.PRICE = Int32.Parse(map["PRICE"].ToString());
            sModel.STOCK_REG = (DateTime)map["STOCK_REG"];
            return sModel;
        }
    }
}