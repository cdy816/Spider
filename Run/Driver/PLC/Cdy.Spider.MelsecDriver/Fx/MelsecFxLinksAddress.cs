using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 三菱的FxLinks协议信息
    /// </summary>
    // Token: 0x020001FE RID: 510
    public class MelsecFxLinksAddress : DeviceAddressDataBase
    {
        /// <summary>
        /// 当前的地址类型信息
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static MelsecFxLinksAddress ParseFrom(string address)
        {
            return ParseFrom(address, 0);
        }

        /// <summary>
        /// 从三菱FxLinks协议里面解析出实际的地址信息
        /// </summary>
        /// <param name="address">三菱的地址信息</param>
        /// <param name="length">读取的长度信息</param>
        /// <returns>解析结果信息</returns>
        public static MelsecFxLinksAddress ParseFrom(string address, ushort length)
        {
            MelsecFxLinksAddress melsecFxLinksAddress = new MelsecFxLinksAddress();
            melsecFxLinksAddress.Length = length;
            try
            {
                char c2 = address[0];
                if (c2 <= 'Y')
                {
                    if (c2 <= 'D')
                    {
                        if (c2 == 'C')
                        {
                            if (address[1] == 'S' || address[1] == 's')
                            {
                                melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
                                melsecFxLinksAddress.TypeCode = "CS";
                                return melsecFxLinksAddress;
                            }
                            else if (address[1] == 'N' || address[1] == 'n')
                            {
                                melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
                                melsecFxLinksAddress.TypeCode = "CN";
                                return melsecFxLinksAddress;
                            }
                            else return null;
                        }
                        if (c2 != 'D')
                        {
                            return null;
                        }
                        melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
                        melsecFxLinksAddress.TypeCode = "D";
                        return melsecFxLinksAddress;
                    }
                    else
                    {
                        if (c2 == 'M')
                        {
                            melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
                            melsecFxLinksAddress.TypeCode = "M";
                            return melsecFxLinksAddress;
                        }
                        switch (c2)
                        {
                            case 'R':
                                melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
                                melsecFxLinksAddress.TypeCode = "R";
                                return melsecFxLinksAddress;
                            case 'S':
                                melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
                                melsecFxLinksAddress.TypeCode = "S";
                                return melsecFxLinksAddress;
                            case 'T':
                                if (address[1] == 'S' || address[1] == 's')
                                {
                                    melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
                                    melsecFxLinksAddress.TypeCode = "TS";
                                    return melsecFxLinksAddress;
                                }
                                else if (address[1] == 'N' || address[1] == 'n')
                                {
                                    melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
                                    melsecFxLinksAddress.TypeCode = "TN";
                                    return melsecFxLinksAddress;
                                }
                                return null;
                            case 'U':
                            case 'V':
                            case 'W':
                                return null;
                            case 'X':
                                melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 8);
                                melsecFxLinksAddress.TypeCode = "X";
                                return melsecFxLinksAddress;
                            case 'Y':
                                melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 8);
                                melsecFxLinksAddress.TypeCode = "Y";
                                return melsecFxLinksAddress;
                            default:
                                return null;
                        }
                    }
                }
                else if (c2 <= 'd')
                {
                    if (c2 == 'c')
                    {
                        if (address[1] == 'S' || address[1] == 's')
                        {
                            melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
                            melsecFxLinksAddress.TypeCode = "CS";
                            return melsecFxLinksAddress;
                        }
                        else if (address[1] == 'N' || address[1] == 'n')
                        {
                            melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
                            melsecFxLinksAddress.TypeCode = "CN";
                            return melsecFxLinksAddress;
                        }
                        else return null;
                    }
                    if (c2 != 'd')
                    {
                        return null;
                    }
                    melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
                    melsecFxLinksAddress.TypeCode = "D";
                    return melsecFxLinksAddress;
                }
                else
                {
                    if (c2 == 'm')
                    {
                        melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
                        melsecFxLinksAddress.TypeCode = "M";
                        return melsecFxLinksAddress;
                    }
                    switch (c2)
                    {
                        case 'r':
                            melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
                            melsecFxLinksAddress.TypeCode = "R";
                            return melsecFxLinksAddress;
                        case 's':
                            melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
                            melsecFxLinksAddress.TypeCode = "S";
                            return melsecFxLinksAddress;
                        case 't':
                            if (address[1] == 'S' || address[1] == 's')
                            {
                                melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
                                melsecFxLinksAddress.TypeCode = "TS";
                                return melsecFxLinksAddress;
                            }
                            else if (address[1] == 'N' || address[1] == 'n')
                            {
                                melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
                                melsecFxLinksAddress.TypeCode = "TN";
                                return melsecFxLinksAddress;
                            }
                            return null;
                        case 'u':
                        case 'v':
                        case 'w':
                            return null;
                        case 'x':
                            melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 8);
                            melsecFxLinksAddress.TypeCode = "X";
                            return melsecFxLinksAddress;
                        case 'y':
                            melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 8);
                            melsecFxLinksAddress.TypeCode = "Y";
                            return melsecFxLinksAddress;
                        default:
                            return null;
                    }
                }
                
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}
