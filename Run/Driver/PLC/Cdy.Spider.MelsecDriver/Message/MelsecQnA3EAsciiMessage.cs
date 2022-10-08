using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver.Message
{
    /// <summary>
    /// 基于MC协议的Qna兼容3E帧协议的ASCII通讯消息机制
    /// </summary>
    public class MelsecQnA3EAsciiMessage : NetMessageBase, INetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolHeadBytesLength
        {
            get
            {
                return 18;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetContentLengthByHeadBytes()
        {
            byte[] bytes = new byte[]
            {
                HeadBytes[14],
                HeadBytes[15],
                HeadBytes[16],
                HeadBytes[17]
            };
            return Convert.ToInt32(Encoding.ASCII.GetString(bytes), 16);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public override bool CheckHeadBytesLegal(byte[] token)
        {
            bool flag = HeadBytes == null;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = HeadBytes[0] == 68 && HeadBytes[1] == 48 && HeadBytes[2] == 48 && HeadBytes[3] == 48;
                result = flag2;
            }
            return result;
        }
    }
}
