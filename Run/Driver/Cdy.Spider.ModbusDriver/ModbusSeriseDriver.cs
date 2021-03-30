using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusSeriseDriver:TimerDriverRunner
    {

        #region ... Variables  ...
        private ModbusDriverSeriseData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "ModbusSeriseDriver";

        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data => base.Data;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new ModbusDriverSeriseData();
            mData.LoadFromXML(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new ModbusSeriseDriver();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
