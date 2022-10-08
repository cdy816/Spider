using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 基于Qna 兼容3C帧的格式一的通讯，具体的地址需要参照三菱的基本地址
    /// </summary>
    public class MelsecA3CSeriseProxy : SerialDeviceProxyBase, IReadWriteA3C
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="runnerBase"></param>
        public MelsecA3CSeriseProxy(DriverRunnerBase runnerBase) : base(runnerBase)
        {
            ByteTransform = new RegularByteTransform();
            WordLength = 1;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public byte Station
        {
            get;
            set;
        }

        public bool SumCheck { get; set; } = true;

        public int Format { get; set; } = 1;
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override byte[] Read(string address, ushort length, out bool res)
        {
            return MelsecA3CNetHelper.Read(this, address, length, out res);
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
            return MelsecA3CNetHelper.Write(this, address, value, out result);
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
            return MelsecA3CNetHelper.ReadBool(this, address, length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public string ReadPlcType(out bool res)
        {
            return MelsecA3CNetHelper.ReadPlcType(this, out res);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
