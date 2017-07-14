using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.beans
{
    public class UserProfileModel
    {
        public UsersModel USERS { get; set; }
        public List<RepairModel> REPAIR {get;set;}
        public int TOTAL_WORK { get; set; }  // รายการซ่อมทั้งหมด
        public int SUCCESS_WORK { get; set; }  // รายการซ่อมสำเร็จ
        public int NOW_WORK { get; set; }  // รายการซ่อม กำลังซ่อม
        public int WAIT_WORK { get; set; }  // รายการซ่อม รอ
        public int FAIL_WORK { get; set; }  // รายการซ่อม ล้มเหลว
    }
}