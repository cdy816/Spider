using Cdy.Spider.DevelopCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.Melsec.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class A3AreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public A3AreasProvider()
        {
            Areas = new List<string>() { "X", "Y", "M", "S", "SM", "SB", "SD", "SW", "SS", "SC", "SN", "STS", "STC", "L", "LSTS", "LSTC", "LSTN", "LTS", "LTC", "LCS", "LCC", "LTN", "LCN", "F", "V", "B", "CN", "CS", "CC", "D", "DX", "DY", "W", "R", "Z", "ZR", "TN", "TS", "TC" };
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }
}
