using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.SiemensDriver
{
    /// <summary>
    /// 西门子S7协议的消息解析规则
    /// </summary>
    // Token: 0x020001EC RID: 492
    public class S7Message : NetMessageBase, INetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolHeadBytesLength
        {
            get
            {
                return 4;
            }
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
                result = base.HeadBytes[0] == 3 && base.HeadBytes[1] == 0;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetContentLengthByHeadBytes()
        {
            byte[] headBytes = base.HeadBytes;
            //bool flag = headBytes != null && headBytes.Length >= 4;
            int result;
            if (headBytes != null && headBytes.Length >= 4)
            {
                int num = (int)base.HeadBytes[2] * 256 + (int)base.HeadBytes[3] - 4;
                if (num < 0)
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
