using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.beans
{
    public class NewsModel
    {
        public int NEWS_ID { get; set; }
        public CustomerModel CUSTOMER { get; set; }
        public string NEWS_MSG { get; set; }
        public DateTime NEWS_REG { get; set; }
        public string ALERT_STATUS { get; set; }
    }
}