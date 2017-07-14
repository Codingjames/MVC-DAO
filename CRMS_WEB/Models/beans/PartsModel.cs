using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.beans;

namespace CRMS_WEB.Models.beans
{
    public class PartsModel
    {
        public int PART_ID { get; set; }
        public PartsTypeModel TYPE { get; set; }
        public PartsBrandModel BRAND { get; set; }
        public DateTime PARTS_REG { get; set; }
    }
}