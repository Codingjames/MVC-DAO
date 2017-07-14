using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;
using CRMS_WEB.Coverts;

namespace CRMS_WEB.Models.DAO
{
    /// <summary>
    /// 15 field of table
    /// </summary>
    public class UsersDAO : DAO<UsersModel>
    {
        Database db;
        public UsersDAO(Database db)
        {
            this.db = db;
        }
        public int Add(UsersModel beans)
        {
            string sql = " INSERT INTO USERS(ID_CARD,USER_UID,USERNAME,PASSWORD,NAME,LASTNAME,ADDRESS,TEL,EMAIL,SALARY,START_WORK,PRIORITY_ID)" +
                         " VALUES('" + beans.ID_CARD + "','" + beans.USER_UID + "','" + beans.USERNAME + "','" + beans.PASSWORD + "','" + beans.NAME + "','" + beans.LASTNAME + "','" + beans.ADDRESS + "','" + beans.TEL + "','" + beans.EMAIL + "'," + beans.SALARY + ",'" + beans.START_WORK + "','" + beans.PRIORITY.PRIO_ID + "');SELECT @@IDENTITY;";
            System.Diagnostics.Debug.WriteLine("  "+sql);
            return db.add(sql);
        }

        public int Delete(UsersModel beans)
        {
            string sql = "DELETE FROM USERS WHERE USER_NO=" + beans.USER_NO;
            return db.remove(sql);
        }

        public int Update(UsersModel beans)
        {
            string pwd = beans.PASSWORD==""?"":",PASSWORD=\'"+beans.PASSWORD+"\'";
            string sql = "UPDATE USERS SET NAME='" + beans.NAME + "',LASTNAME='" + beans.LASTNAME + "',ADDRESS='" +beans.ADDRESS + "',"+
                " TEL='" + beans.TEL + "',EMAIL='" + beans.EMAIL + "',SALARY=" + beans.SALARY +", START_WORK='"+beans.START_WORK+"' "+
                " " + pwd +
                " WHERE USER_NO=" + beans.USER_NO;

            System.Diagnostics.Debug.WriteLine("sql update test :"+sql);
            return db.update(sql);
        }

        public List<UsersModel> FindAll()
        {
            string sql = "SELECT * FROM USERS";
            List<Dictionary<string, object>> listData = db.queryList(sql);

            List<UsersModel> listModel = new List<UsersModel>();
            foreach (var data in listData)
            {
                UsersModel model = new UsersModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }
            return listModel;

        }

        public UsersModel FindById(UsersModel beans)
        {
            string sql = "SELECT * FROM USERS WHERE USER_NO=" + beans.USER_NO;
            Dictionary<string, object> data = db.querySingle(sql);
            UsersModel model = MappingBeans(data);
            return model;
        }

        public UsersModel FindById(int id)
        {
            string sql = "SELECT * FROM USERS WHERE USER_NO=" + id;
            Dictionary<string, object> data = db.querySingle(sql);
            UsersModel model = MappingBeans(data);
            return model;
        }
        public List<UsersModel> FindByUserName(UsersModel beans)
        {
            string sql = "SELECT * FROM USERS WHERE USERNAME='"+beans.USERNAME+"'";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<UsersModel> listModel = new List<UsersModel>();

            foreach(var data in listData){
                UsersModel model = new UsersModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }

            return listModel;
        }
        public List<UsersModel> FindAllEmployee()
        {
            string sql = "SELECT * FROM USERS WHERE PRIORITY_ID=2";
            List<Dictionary<string, object>> listData = db.queryList(sql);

            List<UsersModel> listModel = new List<UsersModel>();
            foreach (var data in listData)
            {
                UsersModel model = new UsersModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }
        public UsersModel GetLogin(string userName, string password)
        {
            string sql = "SELECT * FROM USERS WHERE USERNAME='" + userName + "' AND PASSWORD='" + password + "'";
            System.Diagnostics.Debug.WriteLine("GetLogin SQL : "+sql);
            Dictionary<string, object> data = db.querySingle(sql);
            UsersModel model = null;
            if (data.Count > 0)
            {
                model = MappingBeans(data);
            }
            return model;
        }
        public List<UsersModel> FindByMonthYear(string monthYear)
        {
            ConvertClass cv = new ConvertClass();
            string[] yM = monthYear.Split('-');
            string sql = "SELECT * FROM USERS WHERE DATEPART(month, USER_REG) <=" + yM[1] + " AND DATEPART(year, USER_REG)<=" + yM[0] + " AND DATEPART(month, START_WORK)<=" + yM[1] + " AND DATEPART(year, START_WORK)<=" + yM[0];

            System.Diagnostics.Debug.WriteLine("TEST SQL MONTH : "+sql);
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<UsersModel> listModel = new List<UsersModel>();
            foreach (var data in listData)
            {
                UsersModel model = new UsersModel();
                model = this.MappingBeans(data);

                listModel.Add(model);
            }

            return listModel;
        }
      
        public UsersModel MappingBeans(Dictionary<string, object> map)
        {
            UsersModel model = new UsersModel();
            model.USER_NO = Int32.Parse(map["USER_NO"].ToString());
            model.USER_UID = map["USER_UID"].ToString();
            model.USERNAME = map["USERNAME"].ToString();
            model.PASSWORD = map["PASSWORD"].ToString();
            model.ID_CARD = map["ID_CARD"].ToString();
            model.NAME = map["NAME"].ToString();
            model.LASTNAME = map["LASTNAME"].ToString();
            model.ADDRESS = map["ADDRESS"].ToString();
            model.TEL = map["TEL"].ToString();
            model.EMAIL = map["EMAIL"].ToString();
            model.SALARY = map["SALARY"].ToString();
            model.START_WORK = (DateTime)map["START_WORK"];
            model.USER_REG = (DateTime)map["USER_REG"];

            Database db = new Database();
            PriorityDAO pioDAO = new PriorityDAO(db);
            model.PRIORITY = new PriorityModel();
            model.PRIORITY = pioDAO.FindById(Int32.Parse(map["PRIORITY_ID"].ToString()));
            db.Close();

            return model;
        }
    }
}