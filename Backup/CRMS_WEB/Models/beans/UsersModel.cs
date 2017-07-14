using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.beans
{
    public class UsersModel
    {
        public int USER_NO { get; set; }
        public string USER_UID { get; set; }
        public string ID_CARD { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string NAME { get; set; }
        public string LASTNAME { get; set; }
        public string ADDRESS { get; set; }
        public string TEL { get; set; }
        public string EMAIL { get; set; }
        public string SALARY { get; set; }
        public DateTime START_WORK { get; set; }
        public DateTime RESIGN_DATE { get; set; }
        public PriorityModel PRIORITY { get; set; }
        public DateTime USER_REG { get; set; }
        public WorkingEmployeesViewModel REPAIR {get;set;}
    }
}