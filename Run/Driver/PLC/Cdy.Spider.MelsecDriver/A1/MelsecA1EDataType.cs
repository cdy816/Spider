using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 三菱PLC的数据类型，此处包含了几个常用的类型
    /// </summary>
    public class MelsecA1EDataType
    {
        /// <summary>
        /// 如果您清楚类型代号，可以根据值进行扩展
        /// </summary>
        /// <param name="code">数据类型的代号</param>
        /// <param name="type">0或1，默认为0</param>
        /// <param name="asciiCode">ASCII格式的类型信息</param>
        /// <param name="fromBase">指示地址的多少进制的，10或是16</param>
        public MelsecA1EDataType(ushort code, byte type, string asciiCode, int fromBase)
        {
            DataCode = code;
            AsciiCode = asciiCode;
            FromBase = fromBase;
            bool flag = type < 2;
            if (flag)
            {
                DataType = type;
            }
        }

        /// <summary>
        /// 类型的代号值（软元件代码，用于区分软元件类型，如：D，R）
        /// </summary>
        public ushort DataCode { get; private set; } = 0;

        /// <summary>
        /// 数据的类型，0代表按字，1代表按位
        /// </summary>
        public byte DataType { get; private set; } = 0;

        /// <summary>
        /// 当以ASCII格式通讯时的类型描述
        /// </summary>
        public string AsciiCode { get; private set; }

        /// <summary>
        /// 指示地址是10进制，还是16进制的
        /// </summary>
        public int FromBase { get; private set; }

        /// <summary>
        /// X输入寄存器
        /// </summary>
        public static readonly MelsecA1EDataType X = new MelsecA1EDataType(22560, 1, "X*", 16);

        /// <summary>
        /// Y输出寄存器
        /// </summary>
        public static readonly MelsecA1EDataType Y = new MelsecA1EDataType(22816, 1, "Y*", 16);

        /// <summary>
        /// M中间寄存器
        /// </summary>
        public static readonly MelsecA1EDataType M = new MelsecA1EDataType(19744, 1, "M*", 10);

        /// <summary>
        /// S状态寄存器
        /// </summary>
        public static readonly MelsecA1EDataType S = new MelsecA1EDataType(21280, 1, "S*", 10);

        /// <summary>
        /// F报警器
        /// </summary>
        public static readonly MelsecA1EDataType F = new MelsecA1EDataType(17952, 1, "F*", 10);

        /// <summary>
        /// B连接继电器
        /// </summary>
        public static readonly MelsecA1EDataType B = new MelsecA1EDataType(16928, 1, "B*", 16);

        /// <summary>
        /// TS定时器触点
        /// </summary>
        public static readonly MelsecA1EDataType TS = new MelsecA1EDataType(21587, 1, "TS", 10);

        /// <summary>
        /// TC定时器线圈
        /// </summary>
        public static readonly MelsecA1EDataType TC = new MelsecA1EDataType(21571, 1, "TC", 10);

        /// <summary>
        /// TN定时器当前值
        /// </summary>
        public static readonly MelsecA1EDataType TN = new MelsecA1EDataType(21582, 0, "TN", 10);

        /// <summary>
        /// CS计数器触点
        /// </summary>
        public static readonly MelsecA1EDataType CS = new MelsecA1EDataType(17235, 1, "CS", 10);

        /// <summary>
        /// CC计数器线圈
        /// </summary>
        public static readonly MelsecA1EDataType CC = new MelsecA1EDataType(17219, 1, "CC", 10);

        /// <summary>
        /// CN计数器当前值
        /// </summary>
        public static readonly MelsecA1EDataType CN = new MelsecA1EDataType(17230, 0, "CN", 10);

        /// <summary>
        /// D数据寄存器
        /// </summary>
        public static readonly MelsecA1EDataType D = new MelsecA1EDataType(17440, 0, "D*", 10);

        /// <summary>
        /// W链接寄存器
        /// </summary>
        public static readonly MelsecA1EDataType W = new MelsecA1EDataType(22304, 0, "W*", 16);

        /// <summary>
        /// R文件寄存器
        /// </summary>
        public static readonly MelsecA1EDataType R = new MelsecA1EDataType(21024, 0, "R*", 10);
    }
}
