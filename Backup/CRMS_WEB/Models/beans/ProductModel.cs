using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.beans
{
    public class ProductModel
    {
        public int PRODUCT_NO { get; set; }
        public ProductTypeModel TYPE { get; set; }
        public ProductBrandModel BANRD { get; set; }
        public DateTime PRODUCT_REG { get; set; }
    }
}