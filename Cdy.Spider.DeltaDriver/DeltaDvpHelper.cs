using Cdy.Spider.Common;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.DeltaDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class DeltaDvpHelper
    {
        /// <summary>
        /// 根据台达PLC的地址，解析出转换后的modbus协议信息，适用DVP系列，当前的地址仍然支持站号指定，例如s=2;D100<br />
        /// According to the address of Delta PLC, the converted modbus protocol information is parsed out, applicable to DVP series, 
        /// the current address still supports station number designation, such as s=2;D100
        /// </summary>
        /// <param name="address">台达plc的地址信息</param>
        /// <param name="modbusCode">原始的对应的modbus信息</param>
        /// <returns>还原后的modbus地址</returns>
        // Token: 0x06001285 RID: 4741 RVA: 0x00076554 File Offset: 0x00074754
        public static string ParseDeltaDvpAddress(string address, byte modbusCode)
        {
            string result;
            try
            {
                string str = string.Empty;
                var operateResult = DataExtend.ExtractParameter(ref address, "s",out bool res);
                if (res)
                {
                    str = string.Format("s={0};", operateResult);
                }
                bool flag = modbusCode == 1 || modbusCode == 15 || modbusCode == 5;
                if (flag)
                {
                    bool flag2 = address.StartsWith("S") || address.StartsWith("s");
                    if (flag2)
                    {
                        return (str + Convert.ToInt32(address.Substring(1)).ToString());
                    }
                    bool flag3 = address.StartsWith("X") || address.StartsWith("x");
                    if (flag3)
                    {
                        return (str + "x=2;" + (Convert.ToInt32(address.Substring(1), 8) + 1024).ToString());
                    }
                    bool flag4 = address.StartsWith("Y") || address.StartsWith("y");
                    if (flag4)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1), 8) + 1280).ToString());
                    }
                    bool flag5 = address.StartsWith("T") || address.StartsWith("t");
                    if (flag5)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1)) + 1536).ToString());
                    }
                    bool flag6 = address.StartsWith("C") || address.StartsWith("c");
                    if (flag6)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1)) + 3584).ToString());
                    }
                    bool flag7 = address.StartsWith("M") || address.StartsWith("m");
                    if (flag7)
                    {
                        int num = Convert.ToInt32(address.Substring(1));
                        bool flag8 = num >= 1536;
                        if (flag8)
                        {
                            return (str + (num - 1536 + 45056).ToString());
                        }
                        return (str + (num + 2048).ToString());
                    }
                    else
                    {
                        bool flag9 = address.StartsWith("D") && address.Contains(".");
                        if (flag9)
                        {
                            return (str + address);
                        }
                    }
                }
                else
                {
                    bool flag10 = address.StartsWith("D") || address.StartsWith("d");
                    if (flag10)
                    {
                        int num2 = Convert.ToInt32(address.Substring(1));
                        bool flag11 = num2 >= 4096;
                        if (flag11)
                        {
                            return (str + (num2 - 4096 + 36864).ToString());
                        }
                        return (str + (num2 + 4096).ToString());
                    }
                    else
                    {
                        bool flag12 = address.StartsWith("C") || address.StartsWith("c");
                        if (flag12)
                        {
                            int num3 = Convert.ToInt32(address.Substring(1));
                            bool flag13 = num3 >= 200;
                            if (flag13)
                            {
                                return (str + (num3 - 200 + 3784).ToString());
                            }
                            return (str + (num3 + 3584).ToString());
                        }
                        else
                        {
                            bool flag14 = address.StartsWith("T") || address.StartsWith("t");
                            if (flag14)
                            {
                                return (str + (Convert.ToInt32(address.Substring(1)) + 1536).ToString());
                            }
                        }
                    }
                }
                result = null;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// 读取台达PLC的bool变量，重写了读M地址时，跨区域读1536地址时，将会分割多次读取
        /// </summary>
        /// <param name="readBoolFunc">底层基础的读取方法</param>
        /// <param name="address">PLC的地址信息</param>
        /// <param name="length">读取的长度信息</param>
        /// <returns>读取的结果</returns>
        public static bool[] ReadBool(Func<string, ushort, bool[]> readBoolFunc, string address, ushort length)
        {
            string str = string.Empty;
           var operateResult = DataExtend.ExtractParameter(ref address, "s",out bool res);
            if (res)
            {
                str = string.Format("s={0};", operateResult);
            }
            //bool flag = address.StartsWith("M");
            if (address.StartsWith("M"))
            {
                int num;
                //bool flag2 = int.TryParse(address.Substring(1), out num);
                if (int.TryParse(address.Substring(1), out num))
                {
                    //bool flag3 = num < 1536 && num + (int)length > 1536;
                    if (num < 1536 && num + (int)length > 1536)
                    {
                        ushort num2 = (ushort)(1536 - num);
                        ushort arg = (ushort)(length - num2);
                      var operateResult2 = readBoolFunc(str + address, num2);
                        if (operateResult2==null)
                        {
                            return null;
                        }
                        var operateResult3 = readBoolFunc(str + "M1536", arg);
                        if (operateResult3==null)
                        {
                            return null;
                        }
                        return (DataExtend.SpliceArray<bool>(new bool[][]
                        {
                            operateResult2,
                            operateResult3
                        }));
                    }
                }
            }
            return readBoolFunc(address, length);
        }

        /// <summary>
        /// 写入台达PLC的bool数据，当发现是M类型的数据，并且地址出现跨1536时，进行切割写入操作
        /// </summary>
        /// <param name="writeBoolFunc">底层的写入操作方法</param>
        /// <param name="address">PLC的起始地址信息</param>
        /// <param name="value">等待写入的数据信息</param>
        /// <returns>是否写入成功</returns>
        public static object Write(Func<string, bool[], object> writeBoolFunc, string address, bool[] value)
        {
            string str = string.Empty;
            var operateResult = DataExtend.ExtractParameter(ref address, "s",out bool res);
            if (res)
            {
                str = string.Format("s={0};", operateResult);
            }
            if (address.StartsWith("M"))
            {
                int num;
                if (int.TryParse(address.Substring(1), out num))
                {
                    if (num < 1536 && num + value.Length > 1536)
                    {
                        ushort length = (ushort)(1536 - num);
                        var operateResult2 = writeBoolFunc(str + address, value.SelectBegin((int)length));
                        if (operateResult2==null)
                        {
                            return operateResult2;
                        }
                        var operateResult3 = writeBoolFunc(str + "M1536", value.RemoveBegin((int)length));
                        if (operateResult3==null)
                        {
                            return operateResult3;
                        }
                        return true;
                    }
                }
            }
            return writeBoolFunc(address, value);
        }

        /// <summary>
        /// 读取台达PLC的原始字节变量，重写了读D地址时，跨区域读4096地址时，将会分割多次读取
        /// </summary>
        /// <param name="readFunc">底层基础的读取方法</param>
        /// <param name="address">PLC的地址信息</param>
        /// <param name="length">读取的长度信息</param>
        /// <returns>读取的结果</returns>
        public static byte[] Read(Func<string, ushort, byte[]> readFunc, string address, ushort length)
        {
            string str = string.Empty;
            var operateResult = DataExtend.ExtractParameter(ref address, "s",out bool res);
            if (res)
            {
                str = string.Format("s={0};", operateResult);
            }
            bool flag = address.StartsWith("D");
            if (flag)
            {
                int num;
                bool flag2 = int.TryParse(address.Substring(1), out num);
                if (flag2)
                {
                    bool flag3 = num < 4096 && num + (int)length > 4096;
                    if (flag3)
                    {
                        ushort num2 = (ushort)(4096 - num);
                        ushort arg = (ushort)(length - num2);
                        var operateResult2 = readFunc(str + address, num2);
                        if (operateResult2==null)
                        {
                            return operateResult2;
                        }
                        var operateResult3 = readFunc(str + "D4096", arg);
                        if (operateResult3==null)
                        {
                            return operateResult3;
                        }
                        return (DataExtend.SpliceArray<byte>(new byte[][]
                        {
                            operateResult2,
                            operateResult3
                        }));
                    }
                }
            }
            return readFunc(address, length);
        }

        /// <summary>
        /// 写入台达PLC的原始字节数据，当发现是D类型的数据，并且地址出现跨4096时，进行切割写入操作
        /// </summary>
        /// <param name="writeFunc">底层的写入操作方法</param>
        /// <param name="address">PLC的起始地址信息</param>
        /// <param name="value">等待写入的数据信息</param>
        /// <returns>是否写入成功</returns>
        // Token: 0x06001289 RID: 4745 RVA: 0x00076D10 File Offset: 0x00074F10
        public static object Write(Func<string, byte[], object> writeFunc, string address, byte[] value)
        {
            string str = string.Empty;
            var operateResult = DataExtend.ExtractParameter(ref address, "s",out bool res);
            if (res)
            {
                str = string.Format("s={0};", operateResult);
            }
            //bool flag = address.StartsWith("D");
            if (address.StartsWith("D"))
            {
                int num;
                //bool flag2 = int.TryParse(address.Substring(1), out num);
                if (int.TryParse(address.Substring(1), out num))
                {
                    //bool flag3 = num < 4096 && num + value.Length / 2 > 4096;
                    if (num < 4096 && num + value.Length / 2 > 4096)
                    {
                        ushort num2 = (ushort)(4096 - num);
                        var operateResult2 = writeFunc(str + address, value.SelectBegin((int)(num2 * 2)));
                        if (operateResult2==null)
                        {
                            return null;
                        }
                        var operateResult3 = writeFunc(str + "D4096", value.RemoveBegin((int)(num2 * 2)));
                        if (operateResult3==null)
                        {
                            return null;
                        }
                        return true;
                    }
                }
            }
            return writeFunc(address, value);
        }
    }
}
