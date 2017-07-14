using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;

namespace CRMS_WEB.Models.beans
{
    public class StockModel
    {
        public int STOCK_ID { get; set; }
        public PartsModel PART { get; set; }
        public string STOCK_INFO { get; set; }
        public int UNIT { get; set; }
        public float PRICE { get; set; }
        public DateTime STOCK_REG { get; set; }
    }
}