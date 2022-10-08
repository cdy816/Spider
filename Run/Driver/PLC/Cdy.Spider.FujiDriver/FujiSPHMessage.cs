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
    public class FujiSPHMessage: NetMessageBase, INetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolHeadBytesLength
        {
            get
            {
                return 20;
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
                result = (int)BitConverter.ToUInt16(base.HeadBytes, 18);
            }
            return result;
        }
    }
}
