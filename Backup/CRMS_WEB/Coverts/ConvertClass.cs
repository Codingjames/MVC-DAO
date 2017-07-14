using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Coverts
{
    public class ConvertClass
    {
        public string strToDateTime(string date, string time)
        {
            string[] arrDate = date.Split('-');
            string day = arrDate[2] + "-" + arrDate[1] + "-" + (Int32.Parse(arrDate[0]) + 543);

            return day;
        }
        public string strToDateTime(string dateTime)
        {
            System.Diagnostics.Debug.WriteLine(" Date" + dateTime);
            string[] arrDate = dateTime.ToString().Split('/', '-', ' ');
            string d = arrDate[0] + "/" + arrDate[1] + "/" + (Int32.Parse(arrDate[2]) + 543) + " " + arrDate[3];  // พ.ศ.

            return d;
        }
        public int pageToNumrunning(int? paramPage, int numPage)
        {
            int p = 0, x = paramPage ?? 0;
            if (x > 1)
            {
                p = (x - 1) * numPage;
            }

            return p;
        }
        public int monthToNumberMonth(string month)
        {
            int x=0;
            switch (month)
            {
                case "มกราคม": x = 1; break;
                case "กุมภาพันธ์": x = 2; break;
                case "มีนาคม": x = 3; break;
                case "เมษายน": x = 4; break;
                case "พฤษภาคม": x = 5; break;
                case "มิถุนายน": x = 6; break;
                case "กรกฎาคม": x = 7; break;
                case "สิงหาคม": x = 8; break;
                case "กันยายน": x = 9; break;
                case "ตุลาคม": x = 10; break;
                case "พฤศจิกายน": x = 11; break;
                case "ธันวาคม": x = 12; break;
            }
            return x;
        }
        public string monthToStringMonth(int month)
        {
            string x = "";
            switch (month)
            {
                case 1: x = "มกราคม"; break;
                case 2: x = "กุมภาพันธ์"; break;
                case 3: x = "มีนาคม"; break;
                case 4: x = "เมษายน"; break;
                case 5: x = "พฤษภาคม"; break;
                case 6: x = "มิถุนายน"; break;
                case 7: x = "กรกฎาคม"; break;
                case 8: x = "สิงหาคม"; break;
                case 9: x = "กันยายน"; break;
                case 10: x = "ตุลาคม"; break;
                case 11: x = "พฤศจิกายน"; break;
                case 12: x = "ธันวาคม"; break;
            }
            return x;
        }
    }
}