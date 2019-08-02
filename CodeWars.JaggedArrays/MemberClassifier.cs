using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.JaggedArrays
{
    public class MemberClassifier
    {
        public static IEnumerable<string> OpenOrSenior(int[][] jaggedArray)
        {
            return jaggedArray.Select(member => member[0] >= 55 && member[1] > 7 ? "Senior" : "Open").ToArray();
        }
    }
}
