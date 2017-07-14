using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.beans
{
    public class CustomerModel
    {
        public int CUST_NO { get; set; }
        public string CUST_UID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string C_NAME { get; set; }
        public string C_LASTNAME { get; set; }
        public string C_ADDRESS { get; set; }
        public string C_TEL { get; set; }
        public string C_EMAIL { get; set; }
        public PriorityModel PRIORITY { get; set; }
        public DateTime CUST_REG { get; set; }

        public int TOTAL_REPAIR { get; set; }  // รายการซ่อมทั้งหมดของคนๆ นั้น

    }
}