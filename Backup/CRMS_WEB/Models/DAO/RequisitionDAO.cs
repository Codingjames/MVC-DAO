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
    public class RequisitionDAO : DAO<RequisitionModel>
    {
        Database db;
        public RequisitionDAO(Database db)
        {
            this.db = db;
        }
        public int Add(RequisitionModel beans)
        {
            string sql = "INSERT INTO REQUISITION(REPAIR_NO,STOCK_ID,REQ_UNIT,APPROVE,USER_NO)"+
                " VALUES(" + beans.REPAIR_NO.REPAIR_NO + "," + beans.STOCK_NO.STOCK_ID + "," + beans.REQ_UNIT + ",'" + beans.APROVE + "',"+beans.STAFF.USER_NO+");SELECT @@IDENTITY";
            System.Diagnostics.Debug.WriteLine("RequisitionDAO SQL : " + sql);
            return db.add(sql);
        }

        public int Delete(RequisitionModel beans)
        {
            string sql = "DELETE FROM REQUISITION WHERE REQ_NO=" + beans.REQ_ID;
            return db.remove(sql);
        }

        public int Update(RequisitionModel beans)
        {
            string sql = "UPDATE REQUISITION SET REQ_UNIT=" + beans.REQ_UNIT + " WHERE REQ_NO=" + beans.REQ_ID;
            return db.update(sql);
        }
        // การอัพเดทข้อมูลลการอนุมัติ
        public int Approbation(RequisitionModel beans)
        {
            string sql = "UPDATE REQUISITION SET APPROVE='" + beans.APROVE + "' WHERE REQ_NO=" + beans.REQ_ID;
            return db.update(sql);
        }
        public List<RequisitionModel> FindAll()
        {
            string sql = "SELECT * FROM REQUISITION";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<RequisitionModel> listModel = new List<RequisitionModel>();
            foreach (var data in listData)
            {
                RequisitionModel rqModel = new RequisitionModel();
                rqModel = MappingBeans(data);

                listModel.Add(rqModel);
            }

            return listModel;
        }

        public RequisitionModel FindById(RequisitionModel beans)
        {
            string sql = "SELECT * FROM REQUISITION WHERE REQ_NO=" + beans.REQ_ID;
            Dictionary<string, object> data = db.querySingle(sql);
            RequisitionModel model = MappingBeans(data);

            return model;
        }

        public RequisitionModel FindById(int id)
        {
            string sql = "SELECT * FROM REQUISITION WHERE REQ_NO=" + id;
            Dictionary<string, object> data = db.querySingle(sql);
            RequisitionModel model = MappingBeans(data);

            return model;
        }
        // ค้นหาข้อมูลการ รหัสการซ่อม
        public List<RequisitionModel> FindByRepairID(int repairID)
        {
            string sql = "SELECT * FROM REQUISITION WHERE REPAIR_NO=" + repairID;
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<RequisitionModel> listModel = new List<RequisitionModel>();

            foreach (var data in listData)
            {
                RequisitionModel model = new RequisitionModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }
            return listModel;
        }
        public List<RequisitionModel> FindByMonthYear(string monthYear)
        {
            ConvertClass cv = new ConvertClass();
            string[] yM = monthYear.Split('-');
            string sql = "SELECT * FROM REQUISITION WHERE APPROVE='allow' AND DATEPART(month, REQ_DATE) =" + yM[1] + " AND DATEPART(yyyy, REQ_DATE) ="+yM[0];

            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<RequisitionModel> listModel = new List<RequisitionModel>();
            foreach (var data in listData)
            {
                RequisitionModel model = new RequisitionModel();
                model = MappingBeans(data);

                listModel.Add(model);
            }

            return listModel;
        }
        public RequisitionModel MappingBeans(Dictionary<string, object> map)
        {
            RequisitionModel model = new RequisitionModel();
            model.REQ_ID = Int32.Parse(map["REQ_NO"].ToString());

            Database db = new Database();
            RepairDAO rDAO = new RepairDAO(db);
            model.REPAIR_NO = new RepairModel();
            model.REPAIR_NO = rDAO.FindById(Int32.Parse(map["REPAIR_NO"].ToString()));
            db.Close();

            db = new Database();
            StockDAO sDAO = new StockDAO(db);
            model.STOCK_NO = new StockModel();
            model.STOCK_NO = sDAO.FindById(Int32.Parse(map["STOCK_ID"].ToString()));
            db.Close();

            db = new Database();
            UsersDAO uDAO = new UsersDAO(db);
            model.STAFF = new UsersModel();
            model.STAFF = uDAO.FindById(Int32.Parse(map["USER_NO"].ToString()));
            db.Close();

            model.REQ_UNIT = Int32.Parse(map["REQ_UNIT"].ToString());
            model.APROVE = map["APPROVE"].ToString();
            model.REQ_DATE = (DateTime)map["REQ_DATE"];


            return model;
        }
    }
}