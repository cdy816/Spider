using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.Common
{
    /// <summary>
    /// 消息类的基类
    /// </summary>
    public class NetMessageBase
    {

        public byte[] HeadBytes { get; set; }


        public byte[] ContentBytes { get; set; }


        public byte[] SendBytes { get; set; }


        public virtual int PependedUselesByteLength(byte[] headByte)
        {
            return 0;
        }


        public virtual int GetHeadBytesIdentity()
        {
            return 0;
        }

        public virtual bool CheckHeadBytesLegal(byte[] token)
        {
            bool flag = this.HeadBytes == null;
            return !flag;
        }
    }

    /// <summary>
    /// 专门用于接收指定字符结尾的网络消息
    /// </summary>
    public class SpecifiedCharacterMessage : NetMessageBase, INetMessage
    {
        /// <summary>
        /// 使用固定的一个字符结尾作为当前的报文接收条件，来实例化一个对象<br />
        /// Instantiate an object using a fixed end of one character as the current message reception condition
        /// </summary>
        /// <param name="endCode">结尾的字符</param>
        public SpecifiedCharacterMessage(byte endCode)
        {
            byte[] array = new byte[4];
            array[3] = (byte)(array[3] | 128);
            array[3] = (byte)(array[3] | 1);
            array[1] = endCode;
            this.protocolHeadBytesLength = BitConverter.ToInt32(array, 0);
        }

        /// <summary>
        /// 使用固定的两个个字符结尾作为当前的报文接收条件，来实例化一个对象<br />
        /// Instantiate an object using a fixed two-character end as the current message reception condition
        /// </summary>
        /// <param name="endCode1">第一个结尾的字符</param>
        /// <param name="endCode2">第二个结尾的字符</param>
        public SpecifiedCharacterMessage(byte endCode1, byte endCode2)
        {
            byte[] array = new byte[4];
            array[3] = (byte)(array[3] | 128);
            array[3] = (byte)(array[3] | 2);
            array[1] = endCode1;
            array[0] = endCode2;
            this.protocolHeadBytesLength = BitConverter.ToInt32(array, 0);
        }

        /// <summary>
        /// 获取或设置在结束字符之后剩余的固定字节长度，有些则还包含两个字节的校验码，这时该值就需要设置为2。<br />
        /// Gets or sets the remaining fixed byte length after the end character, and some also contain a two-byte check code. In this case, the value needs to be set to 2.
        /// </summary>
        public byte EndLength
        {
            get
            {
                return BitConverter.GetBytes(this.protocolHeadBytesLength)[2];
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(this.protocolHeadBytesLength);
                bytes[2] = value;
                this.protocolHeadBytesLength = BitConverter.ToInt32(bytes, 0);
            }
        }

        public int ProtocolHeadBytesLength
        {
            get
            {
                return this.protocolHeadBytesLength;
            }
        }


        public int GetContentLengthByHeadBytes()
        {
            return 0;
        }

        public override bool CheckHeadBytesLegal(byte[] token)
        {
            return true;
        }


        private int protocolHeadBytesLength = -1;
    }
}
