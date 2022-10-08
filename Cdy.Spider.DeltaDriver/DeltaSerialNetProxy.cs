using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cdy.Spider.DeltaDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class DeltaNetProxy:ModbusDriver.ModbusRtuNetProxy,IDelta
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
        public DeltaNetProxy(DriverRunnerBase driver) :base(driver)
        {
            base.ByteTransform.DataFormat = Common.DataFormat.CDAB;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public DeltaSeries Series { get; set; } = DeltaSeries.Dvp;
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="modbusCode"></param>
        /// <returns></returns>
        public override string TranslateToModbusAddress(string address, byte modbusCode)
        {
            return DeltaHelper.TranslateToModbusAddress(this, address, modbusCode);
        }

        private bool[] ReadBoolBase(string address, ushort length)
        {
            var re = base.ReadBool(address, length, out bool res);
            if(!res)
            {
                return null;
            }
            else
            {
                return re;
            }
        }

        public override bool[] ReadBool(string address, ushort length,out bool res)
        {
           var re = DeltaHelper.ReadBool(this, new Func<string, ushort, bool[]>(ReadBoolBase), address, length);
            res = re != null;
            return re;
        }

        private object WriteBase(string address, bool[] values)
        {
            var re = base.Write(address, values, out bool res);
            if (!res)
            {
                return null;
            }
            else
            {
                return re;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool[] values,out bool res)
        {
           var re = DeltaHelper.Write(this, new Func<string, bool[], object>(WriteBase), address, values);
            res = re != null;
            return re;
        }

        private byte[] ReadByteBase(string address, ushort length)
        {
            var re = base.Read(address, length, out bool res);
            if (!res)
            {
                return null;
            }
            else
            {
                return re;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override byte[] Read(string address, ushort length,out bool res)
        {
            var re = DeltaHelper.Read(this, new Func<string, ushort, byte[]>(ReadByteBase), address, length);
            res = re != null;
            return re;
        }

        private object WriteByteBase(string address, byte[] values)
        {
            var re = base.Write(address, values, out bool res);
            if (!res)
            {
                return null;
            }
            else
            {
                return re;
            }
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
            var re = DeltaHelper.Write(this, new Func<string, byte[], object>(WriteByteBase), address, value);
            res=re != null;
            return re;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
