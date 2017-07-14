using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.beans
{
    public class CustomerViewModel
    {
        public CustomerModel CUSTOMER { get; set; }
        public List<RepairModel> REPAIRING { get; set; }
        public int SUCCESS_REP { get; set; }
        public int NOW_REP { get; set; }
        public int WAIT_REP { get; set; }
        public int CANNOT_REP { get; set; }
    }
}