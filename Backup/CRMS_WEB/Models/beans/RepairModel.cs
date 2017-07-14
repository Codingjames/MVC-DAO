using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.beans
{
    public class RepairModel
    {
        public int REPAIR_NO { get; set; }
        public CustomerModel CUSTOMER { get; set; }
        public UsersModel STAFF { get; set; }
        public ProductModel PRODUCT { get; set; }
        public RepStatusModel STATUS { get; set; }
        public string MODEL { get; set; }
        public string COLOR { get; set; }
        public string DAMAGE { get; set; }
        public string IMAGES { get; set; }
        public string REPAIR_DETAIL { get; set; }
        public string DATE_ASSIGN { get; set; }   
        public string DATE_CONSIGN { get; set; }   ///นัดส่งคืน
        public string ALERT_STATUS { get; set;}
        public UsersModel RECIPIENT_ID { get; set; }
        public DateTime REPAIR_REG { get; set; }



        public string ALERT_CUST { get; set; }
    }
}