using Cdy.Spider.DevelopCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.BeckhoffDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class BeckhoffAreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public BeckhoffAreasProvider()
        {
            Areas = new List<string>() { "I", "S", "IG", "Q" };
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }
}
