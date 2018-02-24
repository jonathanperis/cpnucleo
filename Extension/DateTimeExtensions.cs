using System;
using System.Globalization;

namespace dotnet_cpnucleo_pages.Extension
{
    public static class DateTimeExtensions 
    {
        private static readonly GregorianCalendar Gc = new GregorianCalendar();
        
        public static int GetWeekOfMonth(this DateTime input) 
        {
            DateTime first = new DateTime(input.Year, input.Month, 1);
            return input.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        private static int GetWeekOfYear(this DateTime input) 
        {
            return Gc.GetWeekOfYear(input, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
    }    
}