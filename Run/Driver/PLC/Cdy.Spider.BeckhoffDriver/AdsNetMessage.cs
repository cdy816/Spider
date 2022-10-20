using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.BeckhoffDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class AdsNetMessage : NetMessageBase, INetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolHeadBytesLength => 6;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetContentLengthByHeadBytes()
        {
            byte[] headBytes = base.HeadBytes;
            int result;
            if (headBytes != null && headBytes.Length >= 6)
            {
                int num = BitConverter.ToInt32(base.HeadBytes, 2);
                if (num > 10000)
                {
                    num = 10000;
                }
                else if (num < 0)
                {
                    num = 0;
                }
                result = num;
            }
            else
            {
                result = 0;
            }
            return result;
        }
    }
}
