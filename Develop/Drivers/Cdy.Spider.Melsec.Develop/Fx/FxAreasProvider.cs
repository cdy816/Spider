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
    public class FxAreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public FxAreasProvider()
        {
            Areas = new List<string>() { "C", "CS", "CN", "D", "M", "R", "S", "T", "TS", "TN", "Y", "X"};
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }
}
