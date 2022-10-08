using Cdy.Spider.DevelopCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.InovanceDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class InovanceAreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public InovanceAreasProvider()
        {
            Areas = new List<string>() { "Q", "QX", "I", "IX", "M", "MX", "MW", "X", "Y", "T", "S", "SM", "SD","C","D","B" };
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }
}
