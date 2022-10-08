using Cdy.Spider.DevelopCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.Fuji.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class SPBAreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public SPBAreasProvider()
        {
            Areas = new List<string>() { "Y", "C", "D", "L", "M", "R", "T", "TN", "TC", "CN", "CC", "W" };
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }
}
