using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class RepStatusDAO : DAO<RepStatusModel>
    {
        Database db;
        public RepStatusDAO(Database db)
        {
            this.db = db;
        }
        public int Add(RepStatusModel beans)
        {
            throw new NotImplementedException();
        }

        public int Delete(RepStatusModel beans)
        {
            throw new NotImplementedException();
        }

        public int Update(RepStatusModel beans)
        {
            throw new NotImplementedException();
        }

        public List<RepStatusModel> FindAll()
        {
            string sql = "SELECT * FROM REP_STATUS";
            List<Dictionary<string, object>> listData = db.queryList(sql);
            List<RepStatusModel> listModel = new List<RepStatusModel>();
            foreach(var data in listData)
            {
                RepStatusModel model = new RepStatusModel();
                model = MappingBeans(data);
                listModel.Add(model);
            }

            return listModel;
        }

        public RepStatusModel FindById(RepStatusModel beans)
        {
            string sql = "SELECT * FROM REP_STATUS WHERE REP_STATUS_ID=" + beans.STATUS_ID;
            Dictionary<string, object> data = db.querySingle(sql);
            RepStatusModel model = null;

            if (data.Count > 0)
            {
                model = MappingBeans(data);
            }
            return model;
        }

        public RepStatusModel FindById(int id)
        {
            string sql = "SELECT * FROM REP_STATUS WHERE REP_STATUS_ID="+id;
            Dictionary<string, object> data = db.querySingle(sql);
            RepStatusModel model = null;

            if (data.Count > 0)
            {
                model = MappingBeans(data);
            }
            return model;
        }

        public RepStatusModel MappingBeans(Dictionary<string, object> map)
        {
            RepStatusModel rsModel = new RepStatusModel();
            rsModel.STATUS_ID = Int32.Parse(map["REP_STATUS_ID"].ToString());
            rsModel.STATUS_NAME = map["REP_STATUS_NAME"].ToString();

            return rsModel;

        }
    }
}