using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OmronDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class OmronFinsAddress : DeviceAddressDataBase
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 进行位操作的指令
        /// </summary>
        public byte BitCode { get; set; }

        /// <summary>
        /// 进行字操作的指令
        /// </summary>
        public byte WordCode { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 从指定的地址信息解析成真正的设备地址信息
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        public override void Parse(string address, ushort length)
        {
            var res = ParseFrom(address, length, out bool result);
            if(result)
            {
                base.AddressStart = res.AddressStart;
                base.Length = res.Length;
                this.BitCode = res.BitCode;
                this.WordCode = res.WordCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static int CalculateBitIndex(string address)
        {
            string[] array = address.SplitDot();
            int num = (int)(ushort.Parse(array[0]) * 16);
            if (array.Length > 1)
            {
                num += array[1].CalculateBitStartIndex();
            }
            return num;
        }

        /// <summary>
        /// 从实际的欧姆龙的地址里面解析出地址对象<br />
        /// Resolve the address object from the actual Omron address
        /// </summary>
        /// <param name="address">欧姆龙的地址数据信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        // Token: 0x060028D9 RID: 10457 RVA: 0x000D15FC File Offset: 0x000CF7FC
        public static OmronFinsAddress ParseFrom(string address, ushort length,out bool result)
        {
            OmronFinsAddress omronFinsAddress = new OmronFinsAddress();
            try
            {
                result = true;
                omronFinsAddress.Length = length;
                if (address.StartsWith("DR") || address.StartsWith("dr"))
                {
                    omronFinsAddress.WordCode = 188;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(2)) + 8192;
                }
                else if (address.StartsWith("IR") || address.StartsWith("ir"))
                {
                    omronFinsAddress.WordCode = 220;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(2)) + 4096;
                }
                else if (address.StartsWith("DM") || address.StartsWith("dm"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.DM.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.DM.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(2));
                }
                else if (address.StartsWith("TIM") || address.StartsWith("tim"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.TIM.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.TIM.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(3));
                }
                else if (address.StartsWith("CNT") || address.StartsWith("cnt"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.TIM.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.TIM.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(3)) + 524288;
                }
                else if (address.StartsWith("CIO") || address.StartsWith("cio"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.CIO.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.CIO.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(3));
                }
                else if (address.StartsWith("WR") || address.StartsWith("wr"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.WR.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.WR.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(2));
                }
                else if (address.StartsWith("HR") || address.StartsWith("hr"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.HR.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.HR.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(2));
                }
                else if (address.StartsWith("AR") || address.StartsWith("ar"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.AR.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.AR.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(2));
                }
                else if(address.StartsWith("EM") || address.StartsWith("em") || address.StartsWith("E") || address.StartsWith("e"))
                {
                    string[] array = address.SplitDot();
                    int num = Convert.ToInt32(array[0].Substring((address[1] == 'M' || address[1] == 'm') ? 2 : 1), 16);
                    bool flag11 = num < 16;
                    if (flag11)
                    {
                        omronFinsAddress.BitCode = (byte)(32 + num);
                        omronFinsAddress.WordCode = (byte)(160 + num);
                    }
                    else
                    {
                        omronFinsAddress.BitCode = (byte)(224 + num - 16);
                        omronFinsAddress.WordCode = (byte)(96 + num - 16);
                    }
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(address.IndexOf('.') + 1));
                }
                else if(address.StartsWith("D") || address.StartsWith("d"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.DM.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.DM.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(1));
                }
                else if(address.StartsWith("C") || address.StartsWith("c"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.CIO.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.CIO.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(1));
                }
                else if(address.StartsWith("W") || address.StartsWith("w"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.WR.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.WR.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(1));
                }
                else if(address.StartsWith("H") || address.StartsWith("h"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.HR.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.HR.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(1));
                }
                else if(address.StartsWith("A") || address.StartsWith("a"))
                {
                    omronFinsAddress.BitCode = OmronFinsDataType.AR.BitCode;
                    omronFinsAddress.WordCode = OmronFinsDataType.AR.WordCode;
                    omronFinsAddress.AddressStart = OmronFinsAddress.CalculateBitIndex(address.Substring(1));
                }
                else
                {
                    throw new Exception($"NotSupportedDataType {address}");
                }
                    
                    
            }
            catch (Exception)
            {
                result = false;
            }
            return omronFinsAddress;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
