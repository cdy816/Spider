using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.FujiDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class FujiSPBMessage: NetMessageBase, INetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolHeadBytesLength
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetContentLengthByHeadBytes()
        {
            bool flag = base.HeadBytes == null;
            int result;
            if (flag)
            {
                result = 0;
            }
            else
            {
                result = Convert.ToInt32(Encoding.ASCII.GetString(base.HeadBytes, 3, 2), 16) * 2 + 2;
            }
            return result;
        }
    }
}
