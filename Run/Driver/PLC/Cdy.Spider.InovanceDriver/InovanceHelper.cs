using Cdy.Spider.Common;
using Cdy.Spider.ModbusDriver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.InovanceDriver
{
    /// <summary>
    /// 汇川的系列枚举信息
    /// </summary>
    public enum InovanceSeries
    {
        /// <summary>
        /// 适用于AM400、 AM400_800、 AC800 等系列
        /// </summary>
        AM,
        /// <summary>
        /// 适用于H3U, XP 等系列
        /// </summary>
        H3U,
        /// <summary>
        /// 适用于H5U 系列
        /// </summary>
        H5U
    }

    public class InovanceHelper
    {
        private static int CalculateStartAddress(string address)
        {
            //bool flag = address.IndexOf('.') < 0;
            int result;
            if (address.IndexOf('.') < 0)
            {
                result = int.Parse(address);
            }
            else
            {
                string[] array = address.Split(new char[]
                {
                    '.'
                }, StringSplitOptions.RemoveEmptyEntries);
                result = int.Parse(array[0]) * 8 + int.Parse(array[1]);
            }
            return result;
        }

        /// <summary>
        /// 按照字节读取汇川M地址的数据，地址示例： MB100，MB101，需要注意的是，MB100 及 MB101 的地址是 MW50 的数据。<br />
        /// Read the data of Inovance M address according to the byte, address example: MB100, MB101, it should be noted that the addresses of MB100 and MB101 are the data of MW50.
        /// </summary>
        /// <param name="modbus">汇川的PLC对象</param>
        /// <param name="address">地址信息</param>
        /// <returns>读取的结果数据</returns>
        public static byte ReadByte(IModbus modbus, string address,out bool res)
        {
            int num = 0;
            bool flag = address.StartsWith("MB") || address.StartsWith("mb");
            if (flag)
            {
                num = Convert.ToInt32(address.Substring(2));
            }
            else
            {
                bool flag2 = address.StartsWith("M") || address.StartsWith("m");
                if (flag2)
                {
                    num = Convert.ToInt32(address.Substring(1));
                }
                else
                {
                    res = false;
                    return 0;
                }
            }
            var operateResult = modbus.Read("MW" + (num / 2).ToString(), 1,out res);
            if (operateResult==null)
            {
                res = false;
                return 0;
            }
            else
            {
                res = false;
                return ((num % 2 == 0) ? operateResult[1] : operateResult[0]);
            }
        }


        /// <summary>
        /// 根据汇川PLC的地址，解析出转换后的modbus协议信息，适用AM,H3U,H5U系列的PLC<br />
        /// According to the address of Inovance PLC, analyze the converted modbus protocol information, which is suitable for AM, H3U, H5U series PLC
        /// </summary>
        /// <param name="series">PLC的系列</param>
        /// <param name="address">汇川plc的地址信息</param>
        /// <param name="modbusCode">原始的对应的modbus信息</param>
        /// <returns>Modbus格式的地址</returns>
        public static string PraseInovanceAddress(InovanceSeries series, string address, byte modbusCode)
        {
            if (series == InovanceSeries.AM)
            {
                return InovanceHelper.PraseInovanceAMAddress(address, modbusCode);
            }
            else if (series == InovanceSeries.H3U)
            {
                return InovanceHelper.PraseInovanceH3UAddress(address, modbusCode);
            }
            else if (series == InovanceSeries.H5U)
            {
                return InovanceHelper.PraseInovanceH5UAddress(address, modbusCode);
            }
            else
            {
                return null;
            }
        }

        public static string PraseInovanceAMAddress(string address, byte modbusCode)
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
                if (address.StartsWith("QX") || address.StartsWith("qx"))
                {
                    result = (str + InovanceHelper.CalculateStartAddress(address.Substring(2)).ToString());
                }
                else if (address.StartsWith("Q") || address.StartsWith("q"))
                {
                    result = (str + InovanceHelper.CalculateStartAddress(address.Substring(1)).ToString());
                }
                else if (address.StartsWith("IX") || address.StartsWith("ix"))
                {
                    result = (str + "x=2;" + InovanceHelper.CalculateStartAddress(address.Substring(2)).ToString());
                }
                else if (address.StartsWith("I") || address.StartsWith("i"))
                {
                    result = (str + "x=2;" + InovanceHelper.CalculateStartAddress(address.Substring(1)).ToString());
                }
                else if (address.StartsWith("MW") || address.StartsWith("mw"))
                {
                    result = (str + address.Substring(2));
                }
                else if (address.StartsWith("MX") || address.StartsWith("mx"))
                {
                    if (modbusCode == 1 || modbusCode == 15 || modbusCode == 5)
                    {
                        bool flag8 = address.IndexOf('.') > 0;
                        if (flag8)
                        {
                            string[] array = address.Substring(2).Split(new char[]
                            {
                                                    '.'
                            }, StringSplitOptions.RemoveEmptyEntries);
                            int num = Convert.ToInt32(array[0]);
                            int num2 = Convert.ToInt32(array[1]);
                            result = (str + (num / 2).ToString() + "." + (num % 2 * 8 + num2).ToString());
                        }
                        else
                        {
                            int num3 = Convert.ToInt32(address.Substring(2));
                            result = (str + (num3 / 2).ToString() + ".0");
                        }
                    }
                    else
                    {
                        int num4 = Convert.ToInt32(address.Substring(2));
                        result = (str + (num4 / 2).ToString());
                    }
                }
                else
                {
                    if (address.StartsWith("M") || address.StartsWith("m"))
                    {
                        result = (str + address.Substring(1));
                    }
                    else
                    {
                        if (modbusCode == 1 || modbusCode == 15 || modbusCode == 5)
                        {
                            if (address.StartsWith("SMX") || address.StartsWith("smx"))
                            {
                                return (str + string.Format("x={0};", (int)(modbusCode + 48)) + InovanceHelper.CalculateStartAddress(address.Substring(3)).ToString());
                            }
                            if (address.StartsWith("SM") || address.StartsWith("sm"))
                            {
                                return (str + string.Format("x={0};", (int)(modbusCode + 48)) + InovanceHelper.CalculateStartAddress(address.Substring(2)).ToString());
                            }
                        }
                        else
                        {
                            if (address.StartsWith("SDW") || address.StartsWith("sdw"))
                            {
                                return (str + string.Format("x={0};", (int)(modbusCode + 48)) + address.Substring(3));
                            }
                            if (address.StartsWith("SD") || address.StartsWith("sd"))
                            {
                                return (str + string.Format("x={0};", (int)(modbusCode + 48)) + address.Substring(2));
                            }
                        }
                        result = null;
                    }
                }

            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        private static int CalculateH3UStartAddress(string address)
        {
            //bool flag = address.IndexOf('.') < 0;
            int result;
            if (address.IndexOf('.') < 0)
            {
                result = Convert.ToInt32(address, 8);
            }
            else
            {
                string[] array = address.Split(new char[]
                {
                    '.'
                }, StringSplitOptions.RemoveEmptyEntries);
                result = Convert.ToInt32(array[0], 8) * 8 + int.Parse(array[1]);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="modbusCode"></param>
        /// <returns></returns>
        public static string PraseInovanceH3UAddress(string address, byte modbusCode)
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
                    if (address.StartsWith("X") || address.StartsWith("x"))
                    {
                        return  (str + (InovanceHelper.CalculateH3UStartAddress(address.Substring(1)) + 63488).ToString());
                    }
                    if (address.StartsWith("Y") || address.StartsWith("y"))
                    {
                        return  (str + (InovanceHelper.CalculateH3UStartAddress(address.Substring(1)) + 64512).ToString());
                    }
                    if (address.StartsWith("SM") || address.StartsWith("sm"))
                    {
                        return  (str + (Convert.ToInt32(address.Substring(2)) + 9216).ToString());
                    }
                    if (address.StartsWith("S") || address.StartsWith("s"))
                    {
                        return  (str + (Convert.ToInt32(address.Substring(1)) + 57344).ToString());
                    }
                    if (address.StartsWith("T") || address.StartsWith("t"))
                    {
                        return  (str + (Convert.ToInt32(address.Substring(1)) + 61440).ToString());
                    }
                    if (address.StartsWith("C") || address.StartsWith("c"))
                    {
                        return  (str + (Convert.ToInt32(address.Substring(1)) + 62464).ToString());
                    }
                    if (address.StartsWith("M") || address.StartsWith("m"))
                    {
                        int num = Convert.ToInt32(address.Substring(1));
                        if (num >= 8000)
                        {
                            return  (str + (num - 8000 + 8000).ToString());
                        }
                        return  (str + num.ToString());
                    }
                }
                else
                {
                    if (address.StartsWith("D") || address.StartsWith("d"))
                    {
                        return  (str + Convert.ToInt32(address.Substring(1)).ToString());
                    }
                    if (address.StartsWith("SD") || address.StartsWith("sd"))
                    {
                        return  (str + (Convert.ToInt32(address.Substring(2)) + 9216).ToString());
                    }
                    if (address.StartsWith("R") || address.StartsWith("r"))
                    {
                        return  (str + (Convert.ToInt32(address.Substring(1)) + 12288).ToString());
                    }
                    if (address.StartsWith("T") || address.StartsWith("t"))
                    {
                        return  (str + (Convert.ToInt32(address.Substring(1)) + 61440).ToString());
                    }
                    if (address.StartsWith("C") || address.StartsWith("c"))
                    {
                        int num2 = Convert.ToInt32(address.Substring(1));
                        if (num2 >= 200)
                        {
                            return  (str + ((num2 - 200) * 2 + 63232).ToString());
                        }
                        return  (str + (num2 + 62464).ToString());
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
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="modbusCode"></param>
        /// <returns></returns>
        public static string PraseInovanceH5UAddress(string address, byte modbusCode)
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
                    bool flag2 = address.StartsWith("X") || address.StartsWith("x");
                    if (flag2)
                    {
                        return (str + (InovanceHelper.CalculateH3UStartAddress(address.Substring(1)) + 63488).ToString());
                    }
                    bool flag3 = address.StartsWith("Y") || address.StartsWith("y");
                    if (flag3)
                    {
                        return (str + (InovanceHelper.CalculateH3UStartAddress(address.Substring(1)) + 64512).ToString());
                    }
                    bool flag4 = address.StartsWith("S") || address.StartsWith("s");
                    if (flag4)
                    {
                        return  (str + (Convert.ToInt32(address.Substring(1)) + 57344).ToString());
                    }
                    bool flag5 = address.StartsWith("B") || address.StartsWith("b");
                    if (flag5)
                    {
                        return  (str + (Convert.ToInt32(address.Substring(1)) + 12288).ToString());
                    }
                    bool flag6 = address.StartsWith("M") || address.StartsWith("m");
                    if (flag6)
                    {
                        return  (str + Convert.ToInt32(address.Substring(1)).ToString());
                    }
                }
                else
                {
                    bool flag7 = address.StartsWith("D") || address.StartsWith("d");
                    if (flag7)
                    {
                        return  (str + Convert.ToInt32(address.Substring(1)).ToString());
                    }
                    bool flag8 = address.StartsWith("R") || address.StartsWith("r");
                    if (flag8)
                    {
                        return  (str + (Convert.ToInt32(address.Substring(1)) + 12288).ToString());
                    }
                }
                result = null;
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }
}
