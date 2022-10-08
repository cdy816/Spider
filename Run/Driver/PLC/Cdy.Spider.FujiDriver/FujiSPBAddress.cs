using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.FujiDriver
{
    public class FujiSPBAddress : DeviceAddressDataBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BitIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetWordAddress()
        {
            return this.TypeCode + FujiSPBHelper.AnalysisIntegerAddress(base.AddressStart);
        }

        public string GetWriteBoolAddress()
        {
            int num = base.AddressStart * 2;
            int num2 = this.BitIndex;
            if (num2 >= 8)
            {
                num++;
                num2 -= 8;
            }
            return string.Format("{0}{1}{2:X2}", this.TypeCode, FujiSPBHelper.AnalysisIntegerAddress(num), num2);
        }

        /// <summary>
        /// 按照位为单位获取相关的索引信息
        /// </summary>
        /// <returns>位数据信息</returns>
        public int GetBitIndex()
        {
            return base.AddressStart * 16 + this.BitIndex;
        }

        /// <summary>
        /// 从实际的Fuji的地址里面解析出地址对象<br />
        /// Resolve the address object from the actual Fuji address
        /// </summary>
        /// <param name="address">富士的地址数据信息</param>
        /// <returns>是否成功的结果对象</returns>
        // Token: 0x060028A8 RID: 10408 RVA: 0x000CE850 File Offset: 0x000CCA50
        public static FujiSPBAddress ParseFrom(string address)
        {
            return ParseFrom(address, 0);
        }

        /// <summary>
        /// 从实际的Fuji的地址里面解析出地址对象<br />
        /// Resolve the address object from the actual Fuji address
        /// </summary>
        /// <param name="address">富士的地址数据信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        public static FujiSPBAddress ParseFrom(string address, ushort length)
        {
            FujiSPBAddress fujiSPBAddress = new FujiSPBAddress();
            fujiSPBAddress.Length = length;
            try
            {
                fujiSPBAddress.BitIndex = DataExtend.GetBitIndexInformation(ref address);
                char c = address[0];
                char c2 = c;
                if (c2 <= 'Y')
                {
                    if (c2 <= 'D')
                    {
                        if (c2 == 'C')
                        {
                            if (address[1] == 'N' || address[1] == 'n')
                            {
                                fujiSPBAddress.TypeCode = "0B";
                                fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(2), 10);
                                return fujiSPBAddress;
                            }
                            else if (address[1] == 'C' || address[1] == 'c')
                            {
                                fujiSPBAddress.TypeCode = "05";
                                fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(2), 10);
                                return fujiSPBAddress;
                            }
                            else
                            {

                                return null;
                            }
                        }
                        if (c2 != 'D')
                        {
                            return null;
                        }
                        fujiSPBAddress.TypeCode = "0C";
                        fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                        return fujiSPBAddress;
                    }
                    else
                    {
                        if (c2 == 'L')
                        {
                            fujiSPBAddress.TypeCode = "03";
                            fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                            return fujiSPBAddress;
                        }
                        if (c2 == 'M')
                        {
                            fujiSPBAddress.TypeCode = "02";
                            fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                            return fujiSPBAddress;
                        }
                        switch (c2)
                        {
                            case 'R':
                                fujiSPBAddress.TypeCode = "0D";
                                fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                                return fujiSPBAddress;
                            case 'S':
                            case 'U':
                            case 'V':
                                return null;
                            case 'T':
                                if (address[1] == 'N' || address[1] == 'n')
                                {
                                    fujiSPBAddress.TypeCode = "0A";
                                    fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(2), 10);
                                    return fujiSPBAddress;
                                }
                                else if (address[1] == 'C' || address[1] == 'c')
                                {
                                    fujiSPBAddress.TypeCode = "04";
                                    fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(2), 10);
                                    return fujiSPBAddress;
                                }
                                else
                                {
                                    return null;
                                }
                            case 'W':
                                fujiSPBAddress.TypeCode = "0E";
                                fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                                return fujiSPBAddress;
                            case 'X':
                                break;
                            case 'Y':
                                fujiSPBAddress.TypeCode = "00";
                                fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                                return fujiSPBAddress;
                            default:
                                return null;
                        }
                    }
                }
                else if (c2 <= 'd')
                {
                    if (c2 == 'c')
                    {
                        if (address[1] == 'N' || address[1] == 'n')
                        {
                            fujiSPBAddress.TypeCode = "0B";
                            fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(2), 10);
                            return fujiSPBAddress;
                        }
                        else if (address[1] == 'C' || address[1] == 'c')
                        {
                            fujiSPBAddress.TypeCode = "05";
                            fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(2), 10);
                            return fujiSPBAddress;
                        }
                        else
                        {

                            return null;
                        }
                    }
                    if (c2 != 'd')
                    {
                        return null;
                    }
                    fujiSPBAddress.TypeCode = "0C";
                    fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                    return fujiSPBAddress;
                }
                else
                {
                    if (c2 == 'l')
                    {
                        fujiSPBAddress.TypeCode = "03";
                        fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                        return fujiSPBAddress;
                    }
                    if (c2 == 'm')
                    {
                        fujiSPBAddress.TypeCode = "02";
                        fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                        return fujiSPBAddress;
                    }
                    switch (c2)
                    {
                        case 'r':
                            fujiSPBAddress.TypeCode = "0D";
                            fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                            return fujiSPBAddress;
                        case 's':
                        case 'u':
                        case 'v':
                            return null;
                        case 't':
                            if (address[1] == 'N' || address[1] == 'n')
                            {
                                fujiSPBAddress.TypeCode = "0A";
                                fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(2), 10);
                                return fujiSPBAddress;
                            }
                            else if (address[1] == 'C' || address[1] == 'c')
                            {
                                fujiSPBAddress.TypeCode = "04";
                                fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(2), 10);
                                return fujiSPBAddress;
                            }
                            else
                            {
                                return null;
                            }
                        case 'w':
                            fujiSPBAddress.TypeCode = "0E";
                            fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                            return fujiSPBAddress;
                        case 'x':
                            break;
                        case 'y':
                            fujiSPBAddress.TypeCode = "00";
                            fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                            return fujiSPBAddress;
                        default:
                            return null;
                    }
                }
                fujiSPBAddress.TypeCode = "01";
                fujiSPBAddress.AddressStart = (int)Convert.ToUInt16(address.Substring(1), 10);
                return fujiSPBAddress;
           
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
