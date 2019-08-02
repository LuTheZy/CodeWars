using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.CenturyCalculator
{
    public class CenturyConverter
    {
        public static int CalculateCentury(int year)
        {
            return (year / 100) + ((year % 100 == 0) ? 0 : 1);
        }
    }
}
