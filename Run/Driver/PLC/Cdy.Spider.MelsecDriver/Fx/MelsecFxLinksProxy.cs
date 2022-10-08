using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 三菱计算机链接协议，适用FX3U系列，FX3G，FX3S等等系列，通常在PLC侧连接的是485的接线口
    /// </summary>
    public class MelsecFxLinksProxy : SerialDeviceProxyBase, IReadWriteFxLinks
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
        public MelsecFxLinksProxy(DriverRunnerBase driver) : base(driver)
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
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public override bool CheckDataReceiveCompletely(MemoryStream ms)
        {
            byte[] array = ms.ToArray();
            bool result = false;
            if (array.Length < 5)
            {
                result = false;
            }
            else
            {
                if (Format == 1)
                {
                    if (array[0] == 21)
                    {
                        result = array.Length == 7;
                    }
                    else if (array[0] == 6)
                    {
                        result = array.Length == 5;
                    }
                    else if (array[0] == 2)
                    {
                        if (SumCheck)
                        {
                            result = array[array.Length - 3] == 3;
                        }
                        else
                        {
                            result = array[array.Length - 1] == 3;
                        }
                    }
                }
                else
                {
                    result = Format == 4 && array[array.Length - 1] == 10 && array[array.Length - 2] == 13;
                }
            }
            return result;
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
            return MelsecFxLinksHelper.Read(this, address, length, out res);
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
            return MelsecFxLinksHelper.Write(this, address, value, out result);
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
            return MelsecFxLinksHelper.ReadBool(this, address, length, out res);
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
