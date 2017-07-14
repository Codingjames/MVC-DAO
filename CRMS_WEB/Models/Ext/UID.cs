using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMS_WEB.Models.Ext
{
    public class UID
    {
        private static Random random = new Random();
        public static string ranByLen(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public Guid DateTimeToGuid(DateTime dateTime)
        {
            Guid guid = Guid.NewGuid();
            string guidEnd = guid.ToString().Substring(guid.ToString().IndexOf("-"), guid.ToString().Length - guid.ToString().IndexOf("-"));
            string guidStart = dateTime.Day.ToString() + dateTime.Month.ToString() + dateTime.Year.ToString();
            guidStart = guidStart.PadRight(8, '0');
            guid = new Guid(guidStart + guidEnd);

            return guid;
        }
    
    }
}