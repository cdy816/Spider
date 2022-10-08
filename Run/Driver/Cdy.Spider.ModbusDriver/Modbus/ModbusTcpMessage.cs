using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.ModbusDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusTcpMessage : NetMessageBase, INetMessage
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
            byte[] headBytes = base.HeadBytes;
            int? num = (headBytes != null) ? new int?(headBytes.Length) : null;
            int protocolHeadBytesLength = this.ProtocolHeadBytesLength;
            //bool flag = num.GetValueOrDefault() >= protocolHeadBytesLength & num != null;
            int result;
            if (num.GetValueOrDefault() >= protocolHeadBytesLength & num != null)
            {
                int num2 = (int)base.HeadBytes[4] * 256 + (int)base.HeadBytes[5];
                if (num2 == 0)
                {
                    byte[] array = new byte[this.ProtocolHeadBytesLength - 1];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = base.HeadBytes[i + 1];
                    }
                    base.HeadBytes = array;
                    result = (int)base.HeadBytes[5] * 256 + (int)base.HeadBytes[6] - 1;
                }
                else
                {
                    result = Math.Min(num2 - 2, 300);
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public override bool CheckHeadBytesLegal(byte[] token)
        {
            bool isCheckMessageId = this.IsCheckMessageId;
            bool result;
            if (isCheckMessageId)
            {
                //bool flag = base.HeadBytes == null;
                if (base.HeadBytes == null)
                {
                    result = false;
                }
                else
                {
                    bool flag2 = base.SendBytes[0] != base.HeadBytes[0] || base.SendBytes[1] != base.HeadBytes[1];
                    result = (!flag2 && base.HeadBytes[2] == 0 && base.HeadBytes[3] == 0);
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHeadBytesIdentity()
        {
            return (int)base.HeadBytes[0] * 256 + (int)base.HeadBytes[1];
        }

        /// <summary>
        /// 获取或设置是否进行检查返回的消息ID和发送的消息ID是否一致，默认为true，也就是检查<br />
        /// Get or set whether to check whether the returned message ID is consistent with the sent message ID, the default is true, that is, check
        /// </summary>
        public bool IsCheckMessageId { get; set; } = true;
    }
}
