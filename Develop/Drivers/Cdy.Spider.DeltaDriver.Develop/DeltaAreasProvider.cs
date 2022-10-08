using Cdy.Spider.DevelopCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.DeltaDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class DeltaAreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public DeltaAreasProvider()
        {
            Areas = new List<string>() { "S", "X", "Y", "T", "C", "M", "D" };
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }
}
