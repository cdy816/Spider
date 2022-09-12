using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OmronDriver.HostLink
{
    /// <summary>
    /// 
    /// </summary>
    public class OmronHostLinkSeriseProxy : SerialDeviceProxyBase, IHostLink
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driverRunner"></param>
        public OmronHostLinkSeriseProxy(DriverRunnerBase driverRunner) : base(driverRunner)
        {
            ByteTransform = new ReverseWordTransform();
            WordLength = 1;
            ByteTransform.DataFormat = DataFormat.CDAB;
            ByteTransform.IsStringReverseByteWord = true;
        }

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public byte ICF { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public byte DA2 { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public byte SA2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte SID { get; set; } = 0;

        /// <summary>
        /// 站点
        /// </summary>
        public byte UnitNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ReadSplits { get; set; } = 260;

        /// <summary>
        /// 
        /// </summary>
        public byte ResponseWaitTime { get; set; } = 48;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public override bool CheckDataReceiveCompletely(MemoryStream ms)
        {
            var buf = ms.GetBuffer();
            if (ms.Position >= 0 && buf[ms.Position-1] ==13)
            {
                return true;
            }
            return base.CheckDataReceiveCompletely(ms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public override byte[] UnpackResponseContent(byte[] send, byte[] response)
        {
            return OmronHostLinkHelper.ResponseValidAnalysis(send, response, out string err);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override byte[] Read(string address, ushort length, out bool res)
        {
            return OmronHostLinkHelper.Read(this, address, length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override object Write(string address, byte[] value, out bool result)
        {
            return OmronHostLinkHelper.Write(this, address, value, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override bool[] ReadBool(string address, ushort length, out bool res)
        {
            return OmronHostLinkHelper.ReadBool(this, address, length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool[] value, out bool res)
        {
            return OmronHostLinkHelper.Write(this, address, value, out res);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
