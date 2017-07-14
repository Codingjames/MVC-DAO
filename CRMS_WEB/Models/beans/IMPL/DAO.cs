using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMS_WEB.Models.DB;

namespace CRMS_WEB.Models.beans.IMPL
{
   interface DAO<T> where T: class
   {
       int Add(T beans);
       int Delete(T beans);
       int Update(T beans);
       List<T> FindAll();
       T FindById(T beans);
       T FindById(int id);
       T MappingBeans(Dictionary<String, Object> map);

   }
}