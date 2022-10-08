using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.FujiDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class FujiSPHAddress : DeviceAddressDataBase
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public byte TypeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BitIndex { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 从实际的Fuji的地址里面解析出地址对象<br />
        /// Resolve the address object from the actual Fuji address
        /// </summary>
        /// <param name="address">富士的地址数据信息</param>
        /// <returns>是否成功的结果对象</returns>
        public static FujiSPHAddress ParseFrom(string address)
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
        public static FujiSPHAddress ParseFrom(string address, ushort length)
        {
            FujiSPHAddress fujiSPHAddress = new FujiSPHAddress();
            try
            {
                string[] array2 = address.SplitDot();
                char c2 = address[0];
                if (c2 <= 'Q')
                {
                    if (c2 == 'I')
                    {
                       
                        fujiSPHAddress.TypeCode = 1;
                        fujiSPHAddress.AddressStart = Convert.ToInt32(array2[0].Substring(1));
                        if (array2.Length > 1)
                        {
                            fujiSPHAddress.BitIndex = DataExtend.CalculateBitStartIndex(array2[1]);
                        }
                        return fujiSPHAddress;
                    }
                    if (c2 != 'M')
                    {
                        if (c2 != 'Q')
                        {
                            return null;
                        }
                        fujiSPHAddress.TypeCode = 1;
                        fujiSPHAddress.AddressStart = Convert.ToInt32(array2[0].Substring(1));
                        if (array2.Length > 1)
                        {
                            fujiSPHAddress.BitIndex = DataExtend.CalculateBitStartIndex(array2[1]);
                        }
                        return fujiSPHAddress;
                    }
                }
                else
                {
                    if (c2 == 'i')
                    {
                        fujiSPHAddress.TypeCode = 1;
                        fujiSPHAddress.AddressStart = Convert.ToInt32(array2[0].Substring(1));
                        if (array2.Length > 1)
                        {
                            fujiSPHAddress.BitIndex = DataExtend.CalculateBitStartIndex(array2[1]);
                        }
                        return fujiSPHAddress;
                    }
                    if (c2 != 'm')
                    {
                        if (c2 != 'q')
                        {
                            return null;
                        }
                        fujiSPHAddress.TypeCode = 1;
                        fujiSPHAddress.AddressStart = Convert.ToInt32(array2[0].Substring(1));
                        if (array2.Length > 1)
                        {
                            fujiSPHAddress.BitIndex = DataExtend.CalculateBitStartIndex(array2[1]);
                        }
                        return fujiSPHAddress;
                    }
                }
                string[] array = address.SplitDot();
                int num = int.Parse(array[0].Substring(1));
                if (num == 1)
                {
                    fujiSPHAddress.TypeCode = 2;
                }
                else
                {
                    if (num == 3)
                    {
                        fujiSPHAddress.TypeCode = 4;
                    }
                    else if(num!=10)
                    {
                        return null;
                    }
                    else 
                    {
                        
                        fujiSPHAddress.TypeCode = 8;
                    }
                }
                fujiSPHAddress.AddressStart = Convert.ToInt32(array[1]);
                if (array.Length > 2)
                {
                    fujiSPHAddress.BitIndex = DataExtend.CalculateBitStartIndex(array[2]);
                }
                return fujiSPHAddress;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
