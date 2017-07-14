using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class CustomerDAO : DAO<CustomerModel>
    {
        Database db;
        public CustomerDAO(Database db)
        {
            this.db = db;
        }
        public int Add(CustomerModel beans)
        {
            string sql = " INSERT INTO CUSTOMER(CUST_UID,USERNAME,[PASSWORD],C_NAME,C_LASTNAME,C_ADDRESS,C_TEL,C_EMAIL,PRIORITY_ID) " +
                         " VALUES('" + beans.CUST_UID + "','" + beans.USERNAME + "','" + beans.PASSWORD + "','" + beans.C_NAME + "','" + beans.C_LASTNAME + "','" + beans.C_ADDRESS + "','" + beans.C_TEL + "','" + beans.C_EMAIL + "'," + beans.PRIORITY.PRIO_ID + ");SELECT @@IDENTITY;";
            System.Diagnostics.Debug.WriteLine("TEST SQL ADD : " + sql);
            return db.add(sql);
        }

        public int Delete(CustomerModel beans)
        {
            string sql = "DELETE FROM CUSTOMER WHERE CUST_NO=" + beans.CUST_NO;
            return db.remove(sql);
        }

        public int Update(CustomerModel beans)
        {
            string pwd = beans.PASSWORD == "" ? "" : ",PASSWORD=\'"+beans.PASSWORD+"\'";
            string sql = " UPDATE CUSTOMER SET C_NAME='" + beans.C_NAME + "', C_LASTNAME ='" + beans.C_LASTNAME + "',"+
                " C_ADDRESS='" + beans.C_ADDRESS + "', C_TEL='" + beans.C_TEL + "', C_EMAIL='" + beans.C_EMAIL + "'"+
                pwd+" WHERE CUST_NO=" + beans.CUST_NO;
            return db.update(sql);
        }

        public List<CustomerModel> FindAll()
        {
            string sql = "SELECT * FROM CUSTOMER";

            List<Dictionary<string, object>> listData = db.queryList(sql);

            List<CustomerModel> listModel = new List<CustomerModel>();
            foreach (var data in listData)
            {
                CustomerModel model = new CustomerModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }

        public CustomerModel FindById(CustomerModel beans)
        {
            string sql = "SELECT * FROM CUSTOMER WHERE CUST_NO=" + beans.CUST_NO;
            Dictionary<string, object> data = db.querySingle(sql);
            CustomerModel model = MappingBeans(data);

            return model;
        }

        public CustomerModel FindById(int id)
        {
            string sql = "SELECT * FROM CUSTOMER WHERE CUST_NO=" + id;
            Dictionary<string, object> data = db.querySingle(sql);
            CustomerModel model = MappingBeans(data);

            return model;
        }
        public List<CustomerModel> FindByUserName(CustomerModel beans)
        {
            string sql = "SELECT * FROM CUSTOMER WHERE USERNAME='"+beans.USERNAME+"'";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<CustomerModel> listModel = new List<CustomerModel>();
            
            foreach(var data in listData){
                CustomerModel model = new CustomerModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }
            return listModel;
        }
        public CustomerModel FindByUid(string uid)
        {
            string sql = "SELECT * FROM CUSTOMER WHERE CUST_UID='" + uid+"'";
            Dictionary<string, object> data = db.querySingle(sql);
            CustomerModel model = null;

            if (data.Count > 0)
            {
                model = MappingBeans(data);
            }


            return model;
        }
        public CustomerModel GetLogin(string userName, string password)
        {
            string sql = "SELECT * FROM CUSTOMER WHERE USERNAME='"+userName+"' AND PASSWORD='"+password+"'" ;
            System.Diagnostics.Debug.WriteLine("SQL Login = "+sql);
            Dictionary<string, object> data = db.querySingle(sql);
            CustomerModel model =null ;
            if(data.Count > 0)
            {
                model = MappingBeans(data);
            }
            return model;
        }
        public CustomerModel MappingBeans(Dictionary<string, object> map)
        {
            CustomerModel model = new CustomerModel();
            model.CUST_NO = Int32.Parse(map["CUST_NO"].ToString());
            model.CUST_UID = map["CUST_UID"].ToString();
            model.USERNAME = map["USERNAME"].ToString();
            model.PASSWORD = map["PASSWORD"].ToString();
            model.C_NAME = map["C_NAME"].ToString();
            model.C_LASTNAME = map["C_LASTNAME"].ToString();
            model.C_ADDRESS = map["C_ADDRESS"].ToString();
            model.C_TEL = map["C_TEL"].ToString();
            model.C_EMAIL = map["C_EMAIL"].ToString();
            model.CUST_REG = (DateTime)map["CUST_REG"];

            Database db = new Database();
            PriorityDAO pioDAO = new PriorityDAO(db);
            model.PRIORITY = new PriorityModel();
            model.PRIORITY = pioDAO.FindById(Int32.Parse(map["PRIORITY_ID"].ToString()));
            db.Close();

            db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            model.TOTAL_REPAIR = rDAO.CountByCustID(Int32.Parse(map["CUST_NO"].ToString()));
            db.Close();

            return model;

        }
    }
}