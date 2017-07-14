using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;
using CRMS_WEB.Models.beans.IMPL;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.DAO
{
    public class PriorityDAO : DAO<PriorityModel>
    {
        Database db;
        public PriorityDAO(Database db)
        {
            this.db = db;
        }
        public int Add(PriorityModel beans)
        {
            throw new NotImplementedException();
        }

        public int Delete(PriorityModel beans)
        {
            throw new NotImplementedException();
        }

        public int Update(PriorityModel beans)
        {
            throw new NotImplementedException();
        }

        public List<PriorityModel> FindAll()
        {
            throw new NotImplementedException();
        }

        public PriorityModel FindById(PriorityModel beans)
        {
            string sql = "SELECT * FROM PRIORITY WHERE PRIO_ID="+beans.PRIO_ID;
            Dictionary<string, object> data = db.querySingle(sql);
            PriorityModel model = MappingBeans(data);

            return model;
        }

        public PriorityModel FindById(int id)
        {
            string sql = "SELECT * FROM PRIORITY WHERE PRIO_ID=" + id;
            Dictionary<string, object> data = db.querySingle(sql);
            PriorityModel model = MappingBeans(data);

            return model;
        }

        public PriorityModel MappingBeans(Dictionary<string, object> map)
        {
            PriorityModel model = new PriorityModel();
            model.PRIO_ID = Int32.Parse(map["PRIO_ID"].ToString());
            model.PRIO_NAME = map["PRIO_NAME"].ToString();
            return model;
        }
    }
}