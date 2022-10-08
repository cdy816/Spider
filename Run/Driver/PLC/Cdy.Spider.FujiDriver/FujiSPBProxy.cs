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
    public class FujiSPBProxy : SerialDeviceProxyBase, IReadWriteDevice
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        public FujiSPBProxy(DriverRunnerBase driver):base(driver)
        {
            base.ByteTransform = new RegularByteTransform();
            base.WordLength = 1;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public byte Station { get; set; } = 1;
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public override bool CheckDataReceiveCompletely(MemoryStream ms)
        {
            var bdatas = ms.ToArray();
            return CheckAsciiReceiveDataComplete(bdatas,bdatas.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modbusAscii"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool CheckAsciiReceiveDataComplete(byte[] modbusAscii, int length)
        {
            bool flag = length > 5;
            return flag && (modbusAscii[0] == 58 && modbusAscii[length - 2] == 13) && modbusAscii[length - 1] == 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override byte[] Read(string address, ushort length,out bool res)
        {
            return FujiSPBHelper.Read(this, this.Station, address, length,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, byte[] value,out bool res)
        {
            return FujiSPBHelper.Write(this, this.Station, address, value,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override bool[] ReadBool(string address, ushort length,out bool res)
        {
            return FujiSPBHelper.ReadBool(this, this.Station, address, length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object Write(string address, bool value,out bool res)
        {
            return FujiSPBHelper.Write(this, this.Station, address, value,out res);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
