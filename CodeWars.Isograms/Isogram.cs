using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Isograms
{
    public class Isogram
    {
        public static bool IsIsogram(string str)
        {
            List<char> charList = new List<char>();
            bool isIsogram = false;
            foreach (var item in str.ToCharArray())
            {
                if (!charList.Contains(item))
                {
                    charList.Add(item);
                    isIsogram = true;
                }
                else
                {
                    isIsogram = false;
                    break;
                }
            }
            return isIsogram;
        }
    }
}
