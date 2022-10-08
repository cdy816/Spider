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
    public class A1AreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public A1AreasProvider()
        {
            Areas = new List<string>() { "B", "C", "D", "F", "CC", "CS", "CN", "R", "S", "M", "W", "X", "Y" };
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }
}
