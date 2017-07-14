using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.beans
{
    public class RepairDetailModel
    {
        public RepairModel REPAIR { get; set; }
        public List<RequisitionModel> STOCK {get;set;}
    }
}