using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class MassageDAO : DAO<MassageModel>
    {
        Database db;
        public MassageDAO(Database db)
        {
            this.db = db;
        }
        public int Add(MassageModel beans)
        {
            string sql = "INSERT INTO MASSAGE(CUST_NO,MSG,TEL) VALUES("+beans.CUSTOMER.CUST_NO+",'"+beans.MSG+"','"+beans.TEL+"');SELECT @@IDENTITY;";
                return db.add(sql);
        }

        public int Delete(MassageModel beans)
        {
            string sql = "DELETE FROM MASSAGE WHERE SMS_ID="+beans.SMS_ID;
            return db.remove(sql);
        }

        public int Update(MassageModel beans)
        {
            throw new NotImplementedException();
        }

        public List<MassageModel> FindAll()
        {
            string sql = "SELECT * FROM MASSAGE";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<MassageModel> listModel = new List<MassageModel>();
            foreach(var data in listData){
                MassageModel model = new MassageModel();
                model = this.MappingBeans(data);

                listModel.Add(model);

            }
            return listModel;
        }

        public MassageModel FindById(MassageModel beans)
        {
            throw new NotImplementedException();
        }

        public MassageModel FindById(int id)
        {
            throw new NotImplementedException();
        }
        public List<MassageModel> FindByCustomerID(int custID)
        {
            string sql = "SELECT * FROM MASSAGE WHERE CUST_NO="+custID+" ORDER BY SMS_ID DESC";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<MassageModel> listModel = new List<MassageModel>();
            foreach(var data in listData){
                MassageModel model = new MassageModel();
                model = this.MappingBeans(data);

                listModel.Add(model);
            }

            return listModel;
        }
        
        public MassageModel MappingBeans(Dictionary<string, object> map)
        {
            MassageModel model = new MassageModel();
            model.SMS_ID = Int32.Parse(map["SMS_ID"].ToString());
            model.MSG = map["MSG"].ToString();
            model.TEL = map["TEL"].ToString();
            model.SMS_REG = DateTime.Parse(map["SMS_REG"].ToString());
           // model.ALERT_STATUS = map["ALERT_STATUS"].ToString();

            return model;
        }
    }
}