using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 三菱计算机链接协议的网口版本，适用FX3U系列，FX3G，FX3S等等系列，通常在PLC侧连接的是485的接线口
    /// </summary>
    public class MelsecFxLinksNetProxy : NetworkDeviceProxyBase, IReadWriteFxLinks
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
        public MelsecFxLinksNetProxy(DriverRunnerBase driver) : base(driver)
        {
            ByteTransform = new RegularByteTransform();
            WordLength = 1;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        public byte Station
        {
            get;
            set;
        }

        public byte WaittingTime
        {
            get;
            set;
        }

        public bool SumCheck
        {
            get;
            set;
        }

        public int Format { get; set; } = 1;
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override byte[] PackCommandWithHeader(byte[] command)
        {
            return MelsecFxLinksHelper.PackCommandWithHeader(this, command);
        }


        /// <summary>
        /// Read PLC data in batches, in units of words, supports reading X, Y, M, S, D, T, C.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override byte[] Read(string address, ushort length, out bool res)
        {
            return MelsecFxLinksHelper.Read(this, address, length, out res);
        }

        /// <summary>
        /// The data written to the PLC in batches is in units of words, that is, at least 2 bytes of information. It supports X, Y, M, S, D, T, and C.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override object Write(string address, byte[] value, out bool result)
        {
            return MelsecFxLinksHelper.Write(this, address, value, out result);
        }

        /// <summary>
        /// Read bool data in batches. The supported types are X, Y, S, T, C.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override bool[] ReadBool(string address, ushort length, out bool res)
        {
            return MelsecFxLinksHelper.ReadBool(this, address, length, out res);
        }

        /// <summary>
        /// Write arrays of type bool in batches. The supported types are X, Y, S, T, C.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool[] value, out bool res)
        {
            return MelsecFxLinksHelper.Write(this, address, value, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public string ReadPlcType(string parameter, out bool res)
        {
            return MelsecFxLinksHelper.ReadPlcType(this, parameter, out res);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
