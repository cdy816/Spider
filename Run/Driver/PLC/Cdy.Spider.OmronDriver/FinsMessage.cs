using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OmronDriver
{
    /// <summary>
    /// 用于欧姆龙通信的Fins协议的消息解析规则
    /// </summary>
    public class FinsMessage : NetMessageBase, INetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolHeadBytesLength
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetContentLengthByHeadBytes()
        {
            int num = BitConverter.ToInt32(new byte[]
            {
                base.HeadBytes[7],
                base.HeadBytes[6],
                base.HeadBytes[5],
                base.HeadBytes[4]
            }, 0);
            if (num > 10000)
            {
                num = 10000;
            }
            if (num < 0)
            {
                num = 0;
            }
            return num;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public override bool CheckHeadBytesLegal(byte[] token)
        {
            bool result;
            if (base.HeadBytes == null)
            {
                result = false;
            }
            else
            {
                result = base.HeadBytes[0] == 70 && base.HeadBytes[1] == 73 && base.HeadBytes[2] == 78 && base.HeadBytes[3] == 83;
            }
            return result;
        }
    }
}
