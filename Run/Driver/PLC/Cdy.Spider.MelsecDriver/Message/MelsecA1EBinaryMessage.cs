using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver.Message
{
    /// <summary>
    /// 三菱的A兼容1E帧协议解析规则
    /// </summary>
    public class MelsecA1EBinaryMessage : NetMessageBase, INetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolHeadBytesLength
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetContentLengthByHeadBytes()
        {
            bool flag = HeadBytes[1] == 91;
            int result;
            if (flag)
            {
                result = 2;
            }
            else
            {
                bool flag2 = HeadBytes[1] == 0;
                if (flag2)
                {
                    switch (HeadBytes[0])
                    {
                        case 128:
                            result = SendBytes[10] != 0 ? (SendBytes[10] + 1) / 2 : 128;
                            break;
                        case 129:
                            result = SendBytes[10] * 2;
                            break;
                        case 130:
                        case 131:
                            result = 0;
                            break;
                        default:
                            result = 0;
                            break;
                    }
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public override bool CheckHeadBytesLegal(byte[] token)
        {
            bool flag = HeadBytes != null;
            return flag && HeadBytes[0] - SendBytes[0] == 128;
        }
    }
}
