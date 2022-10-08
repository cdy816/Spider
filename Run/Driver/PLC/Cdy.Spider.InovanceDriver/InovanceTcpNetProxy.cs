using Cdy.Spider.ModbusDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.InovanceDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class InovanceTcpNetProxy : ModbusTcpNetProxy
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
        public InovanceTcpNetProxy(DriverRunnerBase driver) :base(driver)
        {
            this.Series = InovanceSeries.AM;
            base.DataFormat = Common.DataFormat.CDAB;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public InovanceSeries Series { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public byte ReadByte(string address,out bool res)
        {
            return InovanceHelper.ReadByte(this, address,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="modbusCode"></param>
        /// <returns></returns>
        public override string TranslateToModbusAddress(string address, byte modbusCode)
        {
            return InovanceHelper.PraseInovanceAddress(this.Series, address, modbusCode);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
