using Cdy.Spider.DevelopCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.GEDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class GEAreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public GEAreasProvider()
        {
            Areas = new List<string>() { "AI", "AQ", "R", "SA", "SB", "SC", "I", "Q", "M", "T", "S", "G" };
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }
}
