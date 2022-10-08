using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver.Message
{
    /// <summary>
    /// 三菱的Qna兼容3E帧协议解析规则
    /// </summary>
    public class MelsecQnA3EBinaryMessage : NetMessageBase, INetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolHeadBytesLength
        {
            get
            {
                return 9;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetContentLengthByHeadBytes()
        {
            return BitConverter.ToUInt16(HeadBytes, 7);
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
                bool flag2 = HeadBytes[0] == 208 && HeadBytes[1] == 0;
                result = flag2;
            }
            return result;
        }
    }
}
