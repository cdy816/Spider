using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.SiemensDriver
{
    /// <summary>
    /// 西门子的地址数据信息，主要包含数据代码，DB块，偏移地址（偏移地址对于不是CT类型而已，是位为单位的），当处于写入时，Length无效<br />
    /// Address data information of Siemens, mainly including data code, DB block, offset address, when writing, Length is invalid
    /// </summary>
    // Token: 0x02000200 RID: 512
    public class S7AddressData : DeviceAddressDataBase
    {
        /// <summary>
        /// 获取或设置等待读取的数据的代码<br />
        /// Get or set the code of the data waiting to be read
        /// </summary>
        public byte DataCode { get; set; }

        /// <summary>
        /// 获取或设置PLC的DB块数据信息<br />
        /// Get or set PLC DB data information
        /// </summary>
        public ushort DbBlock { get; set; }

        /// <summary>
        /// 从指定的地址信息解析成真正的设备地址信息
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        public override void Parse(string address, ushort length)
        {
            S7AddressData operateResult = S7AddressData.ParseFrom(address, length);
            if (operateResult!=null)
            {
                base.AddressStart = operateResult.AddressStart;
                base.Length = operateResult.Length;
                this.DataCode = operateResult.DataCode;
                this.DbBlock = operateResult.DbBlock;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result;

            switch (DataCode)
            {
                case 31:
                    result = "T" + base.AddressStart.ToString();
                    break;
                case 30:
                    result = "C" + base.AddressStart.ToString();
                    break;
                case 6:
                    result = "AI" + S7AddressData.GetActualStringAddress(base.AddressStart);
                    break;
                case 7:
                    result = "AQ" + S7AddressData.GetActualStringAddress(base.AddressStart);
                    break;
                case 129:
                    result = "I" + S7AddressData.GetActualStringAddress(base.AddressStart);
                    break;
                case 130:
                    result = "Q" + S7AddressData.GetActualStringAddress(base.AddressStart);
                    break;
                case 131:
                    result = "M" + S7AddressData.GetActualStringAddress(base.AddressStart);
                    break;
                case 132:
                    result = "DB" + this.DbBlock.ToString() + "." + S7AddressData.GetActualStringAddress(base.AddressStart);
                    break;
                default:
                    result = base.AddressStart.ToString();
                    break;
            }           
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressStart"></param>
        /// <returns></returns>
        private static string GetActualStringAddress(int addressStart)
        {
            //bool flag = addressStart % 8 == 0;
            string result;
            if (addressStart % 8 == 0)
            {
                result = (addressStart / 8).ToString();
            }
            else
            {
                result = string.Format("{0}.{1}", addressStart / 8, addressStart % 8);
            }
            return result;
        }

        /// <summary>
        /// 计算特殊的地址信息<br />
        /// Calculate Special Address information
        /// </summary>
        /// <param name="address">字符串地址 -&gt; String address</param>
        /// <param name="isCT">是否是定时器和计数器的地址</param>
        /// <returns>实际值 -&gt; Actual value</returns>
        public static int CalculateAddressStarted(string address, bool isCT = false)
        {
            //bool flag = address.IndexOf('.') < 0;
            int result;
            if (address.IndexOf('.') < 0)
            {
                if (isCT)
                {
                    result = Convert.ToInt32(address);
                }
                else
                {
                    result = Convert.ToInt32(address) * 8;
                }
            }
            else
            {
                string[] array = address.Split(new char[]
                {
                    '.'
                });
                result = Convert.ToInt32(array[0]) * 8 + Convert.ToInt32(array[1]);
            }
            return result;
        }

        /// <summary>
        /// 从实际的西门子的地址里面解析出地址对象<br />
        /// Resolve the address object from the actual Siemens address
        /// </summary>
        /// <param name="address">西门子的地址数据信息</param>
        /// <returns>是否成功的结果对象</returns>
        // Token: 0x060028E3 RID: 10467 RVA: 0x000D1F08 File Offset: 0x000D0108
        public static S7AddressData ParseFrom(string address)
        {
            return S7AddressData.ParseFrom(address, 0);
        }

        /// <summary>
        /// 从实际的西门子的地址里面解析出地址对象<br />
        /// Resolve the address object from the actual Siemens address
        /// </summary>
        /// <param name="address">西门子的地址数据信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        public static S7AddressData ParseFrom(string address, ushort length)
        {
            S7AddressData s7AddressData = new S7AddressData();
            try
            {
                address = address.ToUpper();
                s7AddressData.Length = length;
                s7AddressData.DbBlock = 0;
                if (address.StartsWith("AI"))
                {
                    s7AddressData.DataCode = 6;
                    if (address.StartsWith("AIX") || address.StartsWith("AIB") || address.StartsWith("AIW") || address.StartsWith("AID"))
                    {
                        s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(3), false);
                    }
                    else
                    {
                        s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(2), false);
                    }
                }
                else
                {
                    if (address.StartsWith("AQ"))
                    {
                        s7AddressData.DataCode = 7;
                        if (address.StartsWith("AQX") || address.StartsWith("AQB") || address.StartsWith("AQW") || address.StartsWith("AQD"))
                        {
                            s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(3), false);
                        }
                        else
                        {
                            s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(2), false);
                        }
                    }
                    else
                    {
                        if (address[0] == 'I')
                        {
                            s7AddressData.DataCode = 129;
                            if (address.StartsWith("IX") || address.StartsWith("IB") || address.StartsWith("IW") || address.StartsWith("ID"))
                            {
                                s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(2), false);
                            }
                            else
                            {
                                s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1), false);
                            }
                        }
                        else
                        {
                            if (address[0] == 'Q')
                            {
                                s7AddressData.DataCode = 130;
                                if (address.StartsWith("QX") || address.StartsWith("QB") || address.StartsWith("QW") || address.StartsWith("QD"))
                                {
                                    s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(2), false);
                                }
                                else
                                {
                                    s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1), false);
                                }
                            }
                            else
                            {
                                if (address[0] == 'M')
                                {
                                    s7AddressData.DataCode = 131;
                                    if (address.StartsWith("MX") || address.StartsWith("MB") || address.StartsWith("MW") || address.StartsWith("MD"))
                                    {
                                        s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(2), false);
                                    }
                                    else
                                    {
                                        s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1), false);
                                    }
                                }
                                else
                                {
                                    if (address[0] == 'D' || address.Substring(0, 2) == "DB")
                                    {
                                        s7AddressData.DataCode = 132;
                                        string[] array = address.Split(new char[]
                                        {
                                            '.'
                                        });
                                        if (address[1] == 'B')
                                        {
                                            s7AddressData.DbBlock = Convert.ToUInt16(array[0].Substring(2));
                                        }
                                        else
                                        {
                                            s7AddressData.DbBlock = Convert.ToUInt16(array[0].Substring(1));
                                        }
                                        string text = address.Substring(address.IndexOf('.') + 1);
                                        //bool flag13 = text.StartsWith("DBX") || text.StartsWith("DBB") || text.StartsWith("DBW") || text.StartsWith("DBD");
                                        if (text.StartsWith("DBX") || text.StartsWith("DBB") || text.StartsWith("DBW") || text.StartsWith("DBD"))
                                        {
                                            text = text.Substring(3);
                                        }
                                        s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(text, false);
                                    }
                                    else
                                    {
                                        //bool flag14 = address[0] == 'T';
                                        if (address[0] == 'T')
                                        {
                                            s7AddressData.DataCode = 31;
                                            s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1), true);
                                        }
                                        else
                                        {
                                            //bool flag15 = address[0] == 'C';
                                            if (address[0] == 'C')
                                            {
                                                s7AddressData.DataCode = 30;
                                                s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1), true);
                                            }
                                            else
                                            {
                                                //bool flag16 = address[0] == 'V';
                                                if (address[0] != 'V')
                                                {
                                                    return null;
                                                }
                                                s7AddressData.DataCode = 132;
                                                s7AddressData.DbBlock = 1;
                                                //bool flag17 = address.StartsWith("VB") || address.StartsWith("VW") || address.StartsWith("VD") || address.StartsWith("VX");
                                                if (address.StartsWith("VB") || address.StartsWith("VW") || address.StartsWith("VD") || address.StartsWith("VX"))
                                                {
                                                    s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(2), false);
                                                }
                                                else
                                                {
                                                    s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1), false);
                                                }
                                            }
                                        }
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
            return s7AddressData;
        }
    }
}
