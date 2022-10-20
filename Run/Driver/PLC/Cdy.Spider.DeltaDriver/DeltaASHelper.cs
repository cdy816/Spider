using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.DeltaDriver
{
    public class DeltaASHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static int ParseDeltaBitAddress(string address)
        {
            int num = address.IndexOf('.');
            bool flag = num > 0;
            int result;
            if (flag)
            {
                result = Convert.ToInt32(address.Substring(0, num)) * 16 + DataExtend.CalculateBitStartIndex(address.Substring(num + 1));
            }
            else
            {
                result = Convert.ToInt32(address) * 16;
            }
            return result;
        }

        /// <summary>
        /// 根据台达AS300的PLC的地址，解析出转换后的modbus协议信息，适用AS300系列，当前的地址仍然支持站号指定，例如s=2;D100<br />
        /// According to the PLC address of Delta AS300, the converted modbus protocol information is parsed, 
        /// and it is applicable to AS300 series. The current address still supports station number designation, for example, s=2;D100
        /// </summary>
        /// <param name="address">台达plc的地址信息</param>
        /// <param name="modbusCode">原始的对应的modbus信息</param>
        /// <returns>还原后的modbus地址</returns>
        public static string ParseDeltaASAddress(string address, byte modbusCode)
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
                    bool flag2 = address.StartsWith("SM") || address.StartsWith("sm");
                    if (flag2)
                    {
                        return (str + (Convert.ToInt32(address.Substring(2)) + 16384).ToString());
                    }
                    bool flag3 = address.StartsWith("HC") || address.StartsWith("hc");
                    if (flag3)
                    {
                        return (str + (Convert.ToInt32(address.Substring(2)) + 64512).ToString());
                    }
                    bool flag4 = address.StartsWith("S") || address.StartsWith("s");
                    if (flag4)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1)) + 20480).ToString());
                    }
                    bool flag5 = address.StartsWith("X") || address.StartsWith("x");
                    if (flag5)
                    {
                        return (str + "x=2;" + (DeltaASHelper.ParseDeltaBitAddress(address.Substring(1)) + 24576).ToString());
                    }
                    bool flag6 = address.StartsWith("Y") || address.StartsWith("y");
                    if (flag6)
                    {
                        return (str + (DeltaASHelper.ParseDeltaBitAddress(address.Substring(1)) + 40960).ToString());
                    }
                    bool flag7 = address.StartsWith("T") || address.StartsWith("t");
                    if (flag7)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1)) + 57344).ToString());
                    }
                    bool flag8 = address.StartsWith("C") || address.StartsWith("c");
                    if (flag8)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1)) + 61440).ToString());
                    }
                    bool flag9 = address.StartsWith("M") || address.StartsWith("m");
                    if (flag9)
                    {
                        return (str + Convert.ToInt32(address.Substring(1)).ToString());
                    }
                    bool flag10 = address.StartsWith("D") && address.Contains(".");
                    if (flag10)
                    {
                        return (str + address);
                    }
                }
                else
                {
                    bool flag11 = address.StartsWith("SR") || address.StartsWith("sr");
                    if (flag11)
                    {
                        return (str + (Convert.ToInt32(address.Substring(2)) + 49152).ToString());
                    }
                    bool flag12 = address.StartsWith("HC") || address.StartsWith("hc");
                    if (flag12)
                    {
                        return (str + (Convert.ToInt32(address.Substring(2)) + 64512).ToString());
                    }
                    bool flag13 = address.StartsWith("D") || address.StartsWith("d");
                    if (flag13)
                    {
                        return (str + Convert.ToInt32(address.Substring(1)).ToString());
                    }
                    bool flag14 = address.StartsWith("X") || address.StartsWith("x");
                    if (flag14)
                    {
                        return (str + "x=4;" + (Convert.ToInt32(address.Substring(1)) + 32768).ToString());
                    }
                    bool flag15 = address.StartsWith("Y") || address.StartsWith("y");
                    if (flag15)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1)) + 40960).ToString());
                    }
                    bool flag16 = address.StartsWith("C") || address.StartsWith("c");
                    if (flag16)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1)) + 61440).ToString());
                    }
                    bool flag17 = address.StartsWith("T") || address.StartsWith("t");
                    if (flag17)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1)) + 57344).ToString());
                    }
                    bool flag18 = address.StartsWith("E") || address.StartsWith("e");
                    if (flag18)
                    {
                        return (str + (Convert.ToInt32(address.Substring(1)) + 65024).ToString());
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
    }
}
