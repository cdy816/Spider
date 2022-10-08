using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    public interface IReadWriteA3C
    {
        byte Station { get; set; }
        bool SumCheck { get; set; }
        int Format { get; set; }

        byte[] ReadFromCoreServer(byte[] send);
    }

    public interface IReadWriteMc
    {
        /// <summary>
        /// 网络号，通常为0<br />
        /// Network number, usually 0
        /// </summary>
        /// <remarks>
        /// 依据PLC的配置而配置，如果PLC配置了1，那么此处也填0，如果PLC配置了2，此处就填2，测试不通的话，继续测试0
        /// </remarks>
        byte NetworkNumber { get; set; }

        /// <summary>
        /// 网络站号，通常为0<br />
        /// Network station number, usually 0
        /// </summary>
        /// <remarks>
        /// 依据PLC的配置而配置，如果PLC配置了1，那么此处也填0，如果PLC配置了2，此处就填2，测试不通的话，继续测试0
        /// </remarks>

        byte NetworkStationNumber { get; set; }

        /// <summary>
        /// 当前MC协议的分析地址的方法，对传入的字符串格式的地址进行数据解析。<br />
        /// The current MC protocol's address analysis method performs data parsing on the address of the incoming string format.
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <returns>解析后的数据信息</returns>
        McAddressData McAnalysisAddress(string address, ushort length);


        IByteTransform ByteTransform { get; set; }

        /// <summary>
        /// 当前的MC协议的格式类型<br />
        /// The format type of the current MC protocol
        /// </summary>
        McType McType { get; }

        /// <summary>
        /// 从PLC反馈的数据中提取出实际的数据内容，需要传入反馈数据，是否位读取
        /// </summary>
        /// <param name="response">反馈的数据内容</param>
        /// <param name="isBit">是否位读取</param>
        /// <returns>解析后的结果对象</returns>
        // Token: 0x06000DFA RID: 3578
        byte[] ExtractActualData(byte[] response, bool isBit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        byte[] ReadFromCoreServer(byte[] send);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        byte[] Read(string address, ushort length,out bool res);
    }

    /// <summary>
    /// MC协议的类型
    /// </summary>
    public enum McType
    {
        /// <summary>
        /// 基于二进制的MC协议
        /// </summary>
        McBinary,
        /// <summary>
        /// 基于ASCII格式的MC协议
        /// </summary>
        MCAscii,
        /// <summary>
        /// 基于R系列的二进制的MC协议
        /// </summary>
        McRBinary,
        /// <summary>
        /// 基于R系列的ASCII格式的MC协议
        /// </summary>
        McRAscii
    }
}
