using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.GEDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class GeSRTPAddress : DeviceAddressDataBase
    {
        public byte DataCode { get; set; }

        public override void Parse(string address, ushort length)
        {
            GeSRTPAddress operateResult = GeSRTPAddress.ParseFrom(address, length, false);
            if (operateResult!=null)
            {
                base.AddressStart = operateResult.AddressStart;
                base.Length = operateResult.Length;
                this.DataCode = operateResult.DataCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="isBit"></param>
        /// <returns></returns>
        public static GeSRTPAddress ParseFrom(string address, bool isBit)
        {
            return GeSRTPAddress.ParseFrom(address, 0, isBit);
        }

        /// <summary>
        /// 从GE的地址里，解析出实际的带数据码的  地址信息，起始地址会自动减一，和实际的地址相匹配
        /// </summary>
        /// <param name="address">实际的地址数据</param>
        /// <param name="length">读取的长度信息</param>
        /// <param name="isBit">是否位操作</param>
        /// <returns>是否成功的GE地址对象</returns>
        public static GeSRTPAddress ParseFrom(string address, ushort length, bool isBit)
        {
            GeSRTPAddress geSRTPAddress = new GeSRTPAddress();
            try
            {
                geSRTPAddress.Length = length;
                //bool flag = address.StartsWith("AI") || address.StartsWith("ai");
                if (address.StartsWith("AI") || address.StartsWith("ai"))
                {
                    if (isBit)
                    {
                        return null;
                    }
                    geSRTPAddress.DataCode = 10;
                    geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
                }
                else
                {
                    //bool flag2 = address.StartsWith("AQ") || address.StartsWith("aq");
                    if (address.StartsWith("AQ") || address.StartsWith("aq"))
                    {
                        if (isBit)
                        {
                            return null;
                        }
                        geSRTPAddress.DataCode = 12;
                        geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
                    }
                    else
                    {
                        //bool flag3 = address.StartsWith("R") || address.StartsWith("r");
                        if (address.StartsWith("R") || address.StartsWith("r"))
                        {
                            if (isBit)
                            {
                                return null;
                            }
                            geSRTPAddress.DataCode = 8;
                            geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(1));
                        }
                        else
                        {
                            //bool flag4 = address.StartsWith("SA") || address.StartsWith("sa");
                            if (address.StartsWith("SA") || address.StartsWith("sa"))
                            {
                                geSRTPAddress.DataCode = (isBit ? (byte)78 : (byte)24);
                                geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
                            }
                            else
                            {
                                //bool flag5 = address.StartsWith("SB") || address.StartsWith("sb");
                                if (address.StartsWith("SB") || address.StartsWith("sb"))
                                {
                                    geSRTPAddress.DataCode = (isBit ? (byte)80 : (byte)26);
                                    geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
                                }
                                else
                                {
                                    //bool flag6 = address.StartsWith("SC") || address.StartsWith("sc");
                                    if (address.StartsWith("SC") || address.StartsWith("sc"))
                                    {
                                        geSRTPAddress.DataCode = (isBit ? (byte)82 : (byte)28);
                                        geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
                                    }
                                    else
                                    {
                                        //bool flag7 = address[0] == 'I' || address[0] == 'i';
                                        if (address[0] == 'I' || address[0] == 'i')
                                        {
                                            geSRTPAddress.DataCode = (isBit ? (byte)70 : (byte)16);
                                        }
                                        else
                                        {
                                            //bool flag8 = address[0] == 'Q' || address[0] == 'q';
                                            if (address[0] == 'Q' || address[0] == 'q')
                                            {
                                                geSRTPAddress.DataCode = (isBit ? (byte)72 : (byte)18);
                                            }
                                            else
                                            {
                                                //bool flag9 = address[0] == 'M' || address[0] == 'm';
                                                if (address[0] == 'M' || address[0] == 'm')
                                                {
                                                    geSRTPAddress.DataCode = (isBit ? (byte)76 : (byte)22);
                                                }
                                                else
                                                {
                                                    //bool flag10 = address[0] == 'T' || address[0] == 't';
                                                    if (address[0] == 'T' || address[0] == 't')
                                                    {
                                                        geSRTPAddress.DataCode = (isBit ? (byte)74 : (byte)20);
                                                    }
                                                    else
                                                    {
                                                        //bool flag11 = address[0] == 'S' || address[0] == 's';
                                                        if (address[0] == 'S' || address[0] == 's')
                                                        {
                                                            geSRTPAddress.DataCode = (isBit ? (byte)84 : (byte)30);
                                                        }
                                                        else
                                                        {
                                                            bool flag12 = address[0] == 'G' || address[0] == 'g';
                                                            if (!flag12)
                                                            {
                                                                return null;
                                                            }
                                                            geSRTPAddress.DataCode = (isBit ? (byte)86 : (byte)56);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(1));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            bool flag13 = geSRTPAddress.AddressStart == 0;
            GeSRTPAddress result;
            if (geSRTPAddress.AddressStart == 0)
            {
                return null;
            }
            else
            {
                //bool flag14 = geSRTPAddress.AddressStart > 0;
                if (geSRTPAddress.AddressStart > 0)
                {
                    GeSRTPAddress geSRTPAddress2 = geSRTPAddress;
                    int addressStart = geSRTPAddress2.AddressStart;
                    geSRTPAddress2.AddressStart = addressStart - 1;
                }
                return geSRTPAddress;
            }
        }
    }
}
