using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.beans
{
    public class MassageModel
    {
        public int SMS_ID { get; set; }
        public CustomerModel CUSTOMER { get; set; }
        public string MSG { get; set; }
        public DateTime SMS_REG { get; set; }
        public string ALERT_STATUS { get; set; }
        public string TEL { get; set; }
    }
}