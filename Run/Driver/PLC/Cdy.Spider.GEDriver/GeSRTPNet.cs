using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.GEDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class GeSRTPNet : NetworkDeviceProxyBase
    {

        #region ... Variables  ...
        private SoftIncrementCount incrementCount = new SoftIncrementCount(65535L, 0L, 1);
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        public GeSRTPNet(DriverRunnerBase driver):base(driver)
        {
            base.ByteTransform = new RegularByteTransform();
            base.WordLength = 2;
        }

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override INetMessage GetNewNetMessage()
        {
            return new GeSRTPMessage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool InitializationOnConnect()
        {
            var operateResult = this.ReadFromCoreServer(new byte[56], true, true);
            return operateResult != null;
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
           var operateResult = GeHelper.BuildReadCommand(this.incrementCount.GetCurrentValue(), address, length, false);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = this.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                   return GeHelper.ExtraResponseContent(operateResult2);
                }
            }
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
            var operateResult = GeHelper.BuildWriteCommand(this.incrementCount.GetCurrentValue(), address, value);
            if (operateResult==null)
            {
                result = false;
                return null;
            }
            else
            {
                var operateResult2 = this.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    result = false;
                    return null;
                }
                else
                {
                    result = GeHelper.ExtraResponseContent(operateResult2) != null;
                    return result;
                }
            }
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
            var operateResult = GeSRTPAddress.ParseFrom(address, length, true);
            if (operateResult==null)
            {
                res=false;
                return null;
            }
            else
            {
                var operateResult2 = GeHelper.BuildReadCommand(this.incrementCount.GetCurrentValue(), operateResult);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    var operateResult3 = this.ReadFromCoreServer(operateResult2);
                    if (operateResult3==null)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        var operateResult4 = GeHelper.ExtraResponseContent(operateResult3);
                        if (operateResult4==null)
                        {
                            res = false;
                            return null;
                        }
                        else
                        {
                            res = true;
                           return (operateResult4.ToBoolArray().SelectMiddle(operateResult.AddressStart % 8, (int)length));
                        }
                    }
                }
            }
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
            var operateResult = GeHelper.BuildWriteCommand(this.incrementCount.GetCurrentValue(), address, value);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = this.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = GeHelper.ExtraResponseContent(operateResult2)!=null;
                    return res;
                }
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
