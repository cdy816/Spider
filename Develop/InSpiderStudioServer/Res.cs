using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InSpiderStudioServer
{
    public class Res
    {
        public static string Get(string name)
        {
            return Properties.Resources.ResourceManager.GetString(name, Thread.CurrentThread.CurrentCulture);
        }
    }
}
