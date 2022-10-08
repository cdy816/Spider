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
    public class SPHAreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public SPHAreasProvider()
        {
            Areas = new List<string>() { "I", "Q", "M" };
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }
}
