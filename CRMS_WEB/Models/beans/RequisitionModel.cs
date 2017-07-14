using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;

namespace CRMS_WEB.Models.beans
{
    public class RequisitionModel
    {
        public int REQ_ID { get; set; }
        public UsersModel STAFF { get; set; }
        public RepairModel REPAIR_NO { get; set; }
        public StockModel STOCK_NO { get; set; }
        public int REQ_UNIT { get; set;}
        public string APROVE { get; set; }
        public DateTime REQ_DATE { get; set; }

    }
}