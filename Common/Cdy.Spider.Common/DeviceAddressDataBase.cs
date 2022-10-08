using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceAddressDataBase
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 数字的起始地址，也就是偏移地址
        /// </summary>
        public int AddressStart { get; set; }

        /// <summary>
        /// 读取的数据长度，单位是字节还是字取决于设备方
        /// </summary>
        public ushort Length { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 从指定的地址信息解析成真正的设备地址信息<br />
        /// Parsing from the specified address information into real device address information
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        public virtual void Parse(string address, ushort length)
        {
            this.AddressStart = int.Parse(address);
            this.Length = length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        public virtual void Parse(string address)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.AddressStart.ToString();
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
