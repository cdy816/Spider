using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface INetMessage
    {
        /// <summary>
        /// 消息头的指令长度，第一次接受数据的长度<br />
        /// Instruction length of the message header, the length of the first received data
        /// </summary>
        /// <remarks>
        /// 当最高位字节的最高位为1时，第0和1位为校验的字符数量，第二高位字节表示结束字符之后的剩余字符长度信息，因为一个int占用四个字节，所以最多可以判断2个结束的字符信息。<br />
        /// When the highest bit of the highest-order byte is 1, the 0th and 1st bits are the number of characters to be checked, 
        /// and the second high-order byte indicates the length information of the remaining characters after the end character. 
        /// Because one int occupies four bytes, the maximum It is possible to judge the character information of 2 ends.
        /// </remarks>
        int ProtocolHeadBytesLength { get; }

        /// <summary>
        /// 从当前的头子节文件中提取出接下来需要接收的数据长度<br />
        /// Extract the length of the data to be received from the current header file
        /// </summary>
        /// <remarks>
        /// 如果剩余字节的长度小于0，则表示消息头数据还没有接收完整，还需要接收一定的长度(返回值的绝对值)，然后再判断剩余字节长度是否小于0，直到结果大于等于0为止，最多判断的次数为16次，超过16次将返回失败<br />
        /// If the length of the remaining bytes is less than 0, it means that the message header data has not been received completely, and a certain length (absolute value of the return value) needs to be received, 
        /// and then it is judged whether the length of the remaining bytes is less than 0 until the result is greater than or equal to 0, 
        /// the maximum number of judgments is 16, more than 16 times will return failure
        /// </remarks>
        /// <returns>返回接下来的数据内容长度</returns>
        int GetContentLengthByHeadBytes();

        /// <summary>
        /// 在接收头报文的时候，返回前置无效的报文头字节长度，默认为0，不处理<br />
        /// When receiving a header message, return the header byte length of the invalid header, the default is 0, and no processing is performed.
        /// </summary>
        /// <param name="headByte">接收到的头子节信息</param>
        /// <returns>头子节中无效的字节长度信息</returns>
        int PependedUselesByteLength(byte[] headByte);

        /// <summary>
        /// 检查头子节的合法性<br />
        /// Check the legitimacy of the head subsection
        /// </summary>
        /// <param name="token">特殊的令牌，有些特殊消息的验证</param>
        /// <returns>是否成功的结果</returns>
        bool CheckHeadBytesLegal(byte[] token);

        /// <summary>
        /// 获取头子节里的消息标识<br />
        /// Get the message ID in the header subsection
        /// </summary>
        /// <returns>消息标识</returns>
        int GetHeadBytesIdentity();

        /// <summary>
        /// 消息头字节<br />
        /// Message header byte
        /// </summary>
        byte[] HeadBytes { get; set; }

        /// <summary>
        /// 消息内容字节<br />
        /// Message content byte
        /// </summary>
        byte[] ContentBytes { get; set; }

        /// <summary>
        /// 发送的字节信息<br />
        /// Byte information sent
        /// </summary>
        byte[] SendBytes { get; set; }
    }
}
