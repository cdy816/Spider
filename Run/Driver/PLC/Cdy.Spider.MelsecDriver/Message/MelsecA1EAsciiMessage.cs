using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver.Message
{
    /// <summary>
    /// 三菱的A兼容1E帧ASCII协议解析规则
    /// </summary>
    public class MelsecA1EAsciiMessage : NetMessageBase, INetMessage
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
        /// <returns></returns>
        public int GetContentLengthByHeadBytes()
        {
            bool flag = HeadBytes[2] == 53 && HeadBytes[3] == 66;
            int result;
            if (flag)
            {
                result = 4;
            }
            else
            {
                bool flag2 = HeadBytes[2] == 48 && HeadBytes[3] == 48;
                if (flag2)
                {
                    int num = Convert.ToInt32(Encoding.ASCII.GetString(SendBytes, 20, 2), 16);
                    bool flag3 = num == 0;
                    if (flag3)
                    {
                        num = 256;
                    }
                    switch (HeadBytes[1])
                    {
                        case 48:
                            result = num % 2 == 1 ? num + 1 : num;
                            break;
                        case 49:
                            result = num * 4;
                            break;
                        case 50:
                        case 51:
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
            return flag && HeadBytes[0] - SendBytes[0] == 8;
        }
    }
}
