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
    public class RepairDAO : DAO<RepairModel>
    {
        Database db;
        ConvertClass cv = new ConvertClass();

        public RepairDAO(Database db)
        {
            this.db = db;
        }
        public int Add(RepairModel beans)
        {
            // ผู้ทำรายการ
            // string format = "yyyy-MM-dd HH:MM:ss"; 
            string sql = "INSERT INTO REPAIR(CUST_NO,USER_NO,PRODUCT_NO,REP_STATUS_ID,MODEL,COLOR,DAMAGE,DATE_CONSIGN,DATE_ASSIGN,RECIPIENT_ID)";
            sql += "VALUES(" + beans.CUSTOMER.CUST_NO + "," + beans.STAFF.USER_NO + "," + beans.PRODUCT.PRODUCT_NO + "," + beans.STATUS.STATUS_ID + ",'" + beans.MODEL + "',"
                + "'" + beans.COLOR + "','" + beans.DAMAGE + "','" + cv.strToDateTime(beans.DATE_CONSIGN.ToString()) + "','" + beans.DATE_ASSIGN + "','" + beans.RECIPIENT_ID.USER_NO + "');SELECT @@IDENTITY;";

            System.Diagnostics.Debug.WriteLine("REPAIR_DAO.ADD TEST SQL" + sql);
            return db.add(sql);
        }
         public int AlertStatus(RepairModel beans)
        {
            //ว่าง = เพิ่มเข้าใหม่ให้ใช้สถานะ New, 0 = คลิ๊กเข้าดูแล้ว,1 = ทักครั้งที่มีการปรับปรุงการซ่อม ให้ปรับเป้นสถานะนี้
            string sql = "UPDATE REPAIR SET ALERT_STATUS='" + beans.ALERT_STATUS + "' WHERE REPAIR_NO=" + beans.REPAIR_NO;
            return db.update(sql);
        }
         public int AlertCust(RepairModel beans)
         {
             string sql = "UPDATE REPAIR SET ALERT_CUST='" + beans.ALERT_CUST + "' WHERE REPAIR_NO=" + beans.REPAIR_NO;
             return db.update(sql);
         }
        public int Delete(RepairModel beans)
        {
            string sql = "DELETE FROM REPAIR WHERE REPAIR_NO=" + beans.REPAIR_NO;

            return db.remove(sql);
        }

        public int Update(RepairModel beans)
        {
            string sql = "UPDATE REPAIR SET USER_NO="+beans.STAFF.USER_NO+", PRODUCT_NO="+beans.PRODUCT.PRODUCT_NO+","+
                " MODEL='"+beans.MODEL+"', COLOR='"+beans.COLOR+"', DAMAGE='"+beans.DAMAGE+"',DATE_CONSIGN='"+beans.DATE_CONSIGN+"'"+
                " WHERE REPAIR_NO="+beans.REPAIR_NO;
            return db.update(sql);
        }
        public int UpdateDetail(RepairModel beans)
        {
           string sql = "UPDATE REPAIR SET REP_STATUS_ID="+beans.STATUS.STATUS_ID+",REPAIR_DETAIL='"+beans.REPAIR_DETAIL+"', IMAGES='"+beans.IMAGES+"' "+
            " WHERE REPAIR_NO=" + beans.REPAIR_NO;
            return db.update(sql);
        }
        public int UpdateDamageRepair(RepairModel beans)
        {
            string sql = "UPDATE REPAIR SET DAMAGE='"+beans.DAMAGE +"',ALERT_STATUS='"+beans.ALERT_STATUS+"' WHERE REPAIR_NO="+beans.REPAIR_NO;
            System.Diagnostics.Debug.WriteLine("UpdateDamageRepair : =" + sql);
            return db.update(sql);
        }
        public int UpdateOldOrder()
        {
            string sql = "UPDATE REPAIR SET ALERT_STATUS='0'";
            return db.update(sql);
        }
        public List<RepairModel> FindAll()
        {
            string sql = "SELECT * FROM REPAIR ORDER BY REP_STATUS_ID ASC";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<RepairModel> listModel = new List<RepairModel>();

            foreach (var data in listData)
            {
                RepairModel model = new RepairModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }

        public RepairModel FindById(RepairModel beans)
        {
            string sql = "SELECT * FROM REPAIR WHERE REPAIR_NO=" + beans.REPAIR_NO;

            Dictionary<string, object> data = db.querySingle(sql);
            RepairModel model = null;
            if (data.Count > 0)
            {
                model = MappingBeans(data);
            }

            return model;
        }

        public RepairModel FindById(int id)
        {
            string sql = "SELECT * FROM REPAIR WHERE REPAIR_NO=" + id;

            Dictionary<string, object> data = db.querySingle(sql);
            RepairModel model = null;
            if (data.Count > 0)
            {
                model = MappingBeans(data);
            }

            return model;
        }
        public List<RepairModel> FindByCustID(int custID)
        {
            string sql = "SELECT * FROM REPAIR WHERE CUST_NO="+custID;
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<RepairModel> listModel = new List<RepairModel>();

            foreach (var data in listData)
            {
                RepairModel model = new RepairModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }

            return listModel;
        }
        public List<RepairModel> FindUpdateByCustID(int custID)
        {
            string sql = "SELECT * FROM REPAIR WHERE ALERT_CUST='1' AND CUST_NO=" + custID;
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<RepairModel> listModel = new List<RepairModel>();

            foreach (var data in listData)
            {
                RepairModel model = new RepairModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }

            return listModel;
        }
        public int CountByCustID(int custID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE CUST_NO=" + custID;
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }
        public List<RepairModel> FindByUserID(int UserID)
        {
            string sql = "SELECT * FROM REPAIR WHERE USER_NO=" + UserID;
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<RepairModel> listModel = new List<RepairModel>();

            foreach (var data in listData)
            {
                RepairModel model = new RepairModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }

            return listModel;
        }
        public HashSet<RepairModel> FindSetByUserID(int UserID)
        {
            string sql = "SELECT * FROM REPAIR WHERE USER_NO=" + UserID;
            HashSet<Dictionary<string, object>> listData = db.querySet(sql);
            HashSet<RepairModel> listModel = new HashSet<RepairModel>();

            foreach (var data in listData)
            {
                RepairModel model = new RepairModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }

            return listModel;
        }
        public int CountByUserID(int userID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE USER_NO=" + userID;
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }
        public int CountByUserIDNowWorking(int userID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=2 AND USER_NO=" + userID;
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }
        public int CountByUserIDSuccess(int userID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=3 AND USER_NO=" + userID;
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }
        public int CountByUserIDWait(int userID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=1 AND USER_NO=" + userID;
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }
        public int CountByUserIDFail(int userID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=4 AND USER_NO=" + userID;
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }
        public int CountByUserIDNowWorking(int userID, string monthYear)
        {
            ConvertClass cv = new ConvertClass();
            string[] yM = monthYear.Split('-');

            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=2 AND USER_NO=" + userID + " AND (DATEPART(year, REPAIR_REG)=" + yM[0] + " AND  DATEPART(month, REPAIR_REG)=" + yM[1] + ")";
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }
        public int CountByUserIDSuccess(int userID, string monthYear)
        {
            ConvertClass cv = new ConvertClass();
            string[] yM = monthYear.Split('-');

            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=3 AND USER_NO=" + userID + " AND (DATEPART(year, REPAIR_REG)=" + yM[0] + " AND  DATEPART(month, REPAIR_REG)="+yM[1]+")";
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }
        public int CountByUserIDWait(int userID, string monthYear)
        {
            ConvertClass cv = new ConvertClass();
            string[] yM = monthYear.Split('-');
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=1 AND USER_NO=" + userID + " AND (DATEPART(year, REPAIR_REG)=" + yM[0] + " AND  DATEPART(month, REPAIR_REG)=" + yM[1] + ")";
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }
        public int CountByUserIDFail(int userID, string monthYear)
        {
            ConvertClass cv = new ConvertClass();
            string[] yM = monthYear.Split('-');

            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=4 AND USER_NO=" + userID + " AND (DATEPART(year, REPAIR_REG)=" + yM[0] + " AND  DATEPART(month, REPAIR_REG)=" + yM[1] + ")";
            Dictionary<string, object> listData = db.querySingle(sql);

            return Int32.Parse(listData["C"].ToString());
        }


        public List<RepairModel> FindByMonthYear(string monthYear)
        {
            ConvertClass cv = new ConvertClass();
            string[] yM = monthYear.Split('-');
            string sql = "SELECT * FROM REPAIR WHERE DATEPART(month, REPAIR_REG)=" + yM[1] + " AND DATEPART(year, REPAIR_REG) =" + yM[0];
            
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<RepairModel> listModel = new List<RepairModel>();
            foreach (var data in listData)
            {
                RepairModel model = new RepairModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }

            return listModel;
        }

        public HashSet<RepairModel> FindSetForShow()
        {
            string sql = "SELECT TOP(3) * FROM REPAIR WHERE REP_STATUS_ID = 3";
            HashSet<Dictionary<string, object>> dataSet = db.querySet(sql);
            HashSet<RepairModel> modelSet = new HashSet<RepairModel>();
            
            foreach(var data in dataSet){
                RepairModel model = new RepairModel();
                model = MappingBeans(data);

                modelSet.Add(model);
            }
            return modelSet;
        }

        public int CountByCustID_WaitRep(int custID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=1 AND CUST_NO=" + custID;
            Dictionary<string, object> data = db.querySingle(sql);
            return Int32.Parse(data["C"].ToString());
        }
        public int CountByCustID_NowRep(int custID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=2 AND CUST_NO=" + custID;
            Dictionary<string, object> data = db.querySingle(sql);
            return Int32.Parse(data["C"].ToString());
        }
        public int CountByCustID_SuccessRep(int custID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=3 AND CUST_NO=" + custID;
            Dictionary<string, object> data = db.querySingle(sql);
            return Int32.Parse(data["C"].ToString());
        }
        public int CountByCustID_CannotRep(int custID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE REP_STATUS_ID=4 AND CUST_NO=" + custID;
            Dictionary<string, object> data = db.querySingle(sql);
            return Int32.Parse(data["C"].ToString());
        }
        //alert bell
        public int CountAlertByCustID(int custID)
        {
            string sql = "SELECT COUNT(*) C FROM REPAIR WHERE ALERT_CUST='1'  AND CUST_NO =" + custID;
            System.Diagnostics.Debug.WriteLine("CountAlertByCustID : "+sql);
            Dictionary<string, object> data = db.querySingle(sql);
            return Int32.Parse(data["C"].ToString());
        }

        public RepairModel MappingBeans(Dictionary<string, object> map)
        {
            try
            {
                RepairModel model = new RepairModel();
                model.REPAIR_NO = Int32.Parse(map["REPAIR_NO"].ToString());

                model.CUSTOMER = new CustomerModel();
                Database db = new Database();
                CustomerDAO cDAO = new CustomerDAO(db);
                model.CUSTOMER = cDAO.FindById(Int32.Parse(map["CUST_NO"].ToString()));
                db.Close();

                model.STAFF = new UsersModel();
                db = new Database();
                UsersDAO uDAO = new UsersDAO(db);
                model.STAFF = uDAO.FindById(Int32.Parse(map["USER_NO"].ToString()));
                db.Close();

                model.PRODUCT = new ProductModel();
                db = new Database();
                ProductDAO pDAO = new ProductDAO(db);
                model.PRODUCT = pDAO.FindById(Int32.Parse(map["PRODUCT_NO"].ToString()));
                db.Close();

                model.STATUS = new RepStatusModel();
                db = new Database();
                RepStatusDAO rsDAO = new RepStatusDAO(db);
                model.STATUS = rsDAO.FindById(Int32.Parse(map["REP_STATUS_ID"].ToString()));
                db.Close();

                model.MODEL = map["MODEL"].ToString();
                model.COLOR = map["COLOR"].ToString();
                model.DAMAGE = map["DAMAGE"].ToString();
                model.IMAGES = map["IMAGES"].ToString();
                model.REPAIR_DETAIL = map["REPAIR_DETAIL"].ToString();
                model.DATE_ASSIGN = map["DATE_ASSIGN"].ToString();
                model.DATE_CONSIGN = map["DATE_CONSIGN"].ToString();
                model.ALERT_STATUS = map["ALERT_STATUS"].ToString();
                model.ALERT_CUST = map["ALERT_CUST"].ToString();

                model.RECIPIENT_ID = new UsersModel();
                db = new Database();
                uDAO = new UsersDAO(db);
                model.RECIPIENT_ID = uDAO.FindById(Int32.Parse(map["RECIPIENT_ID"].ToString()));
                db.Close();

                model.REPAIR_REG = DateTime.Parse(map["REPAIR_REG"].ToString());

                return model;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERR : "+e.Message);
                return null;
            }
        }
    }
}