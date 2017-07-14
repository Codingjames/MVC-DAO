using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class NewsDAO : DAO<NewsModel>
    {
        Database db;
        public NewsDAO(Database db)
        {
            this.db = db;
        }
        public int Add(NewsModel beans)
        {
            string sql = "INSERT INTO NEWS(CUST_NO,NEWS_MSG,ALERT_STATUS) VALUES("+beans.CUSTOMER.CUST_NO+",'"+beans.NEWS_MSG+"','"+beans.ALERT_STATUS+"');SELECT @@IDENTITY;";
            return db.add(sql);
        }

        public int Delete(NewsModel beans)
        {
            throw new NotImplementedException();
        }

        public int Update(NewsModel beans)
        {
            throw new NotImplementedException();
        }
        public int Readed(int c)
        {
            string sql = "UPDATE NEWS SET ALERT_NEWS=1 WHERE CUST_NO=" + c;
            return db.update(sql);
        }
        public List<NewsModel> FindAll()
        {
            throw new NotImplementedException();
        }
        public HashSet<NewsModel> FindAllSetByCustID(int c)
        {
            string sql = "SELECT * FROM NEWS WHERE CUST_NO ="+c+"  ORDER BY ALERT_NEWS ASC";
            System.Diagnostics.Debug.WriteLine("FindAllSetByCustID : ");
            HashSet<Dictionary<string, object>> dataSet = db.querySet(sql);
            HashSet<NewsModel> setModel = new HashSet<NewsModel>();

            foreach (var data in dataSet)
            {
                NewsModel model = new NewsModel();
                model = MappingBeans(data);
                
                setModel.Add(model);
            }

            return setModel;
        }
        public NewsModel FindById(NewsModel beans)
        {
            throw new NotImplementedException();
        }

        public NewsModel FindById(int id)
        {
            throw new NotImplementedException();
        }
        public List<NewsModel> FindByCustomerID(int custID)
        {
            string sql = "SELECT * FROM NEWS WHERE CUST_NO="+custID+" ORDER BY NEWS_ID DESC";
            
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<NewsModel> listModel = new List<NewsModel>();
            foreach(var data in listData){
                NewsModel model = new NewsModel();
                model = this.MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }
        public int CountNewNewsByCustID(int cid)
        {
            string sql = "SELECT COUNT(*) C FROM NEWS WHERE ALERT_NEWS IS NULL AND CUST_NO="+cid;
            Dictionary<string, object> data = db.querySingle(sql);
            
            return Int32.Parse(data["C"].ToString());
        }
        public NewsModel MappingBeans(Dictionary<string, object> map)
        {
            NewsModel model = new NewsModel();
            model.NEWS_ID = Int32.Parse(map["NEWS_ID"].ToString());
            model.NEWS_MSG = map["NEWS_MSG"].ToString();
            model.ALERT_STATUS = map["ALERT_NEWS"].ToString();
            model.NEWS_REG = DateTime.Parse(map["NEWS_REG"].ToString());

            Database db = new Database();
            CustomerDAO cDAO = new CustomerDAO(db);
            model.CUSTOMER = new CustomerModel();
            model.CUSTOMER = cDAO.FindById(Int32.Parse(map["CUST_NO"].ToString()));
            db.Close();

            return model;
        }
    }
}