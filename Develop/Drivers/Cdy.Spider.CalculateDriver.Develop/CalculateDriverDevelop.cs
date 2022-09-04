using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.CalculateDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculateDriverDevelop : DriverDevelop
    {

        #region ... Variables  ...
        private CalculateDriverData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        
        public override DriverData Data { get => mData; set { mData = value as CalculateDriverData; } }
        
        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "CalculateDriver";

        /// <summary>
        /// 
        /// </summary>
        public override string Desc => Res.Get("Desc");

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<string> ListSupportChannels()
        {
            return new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverDevelop NewDriver()
        {
            return new CalculateDriverDevelop() { Data = new CalculateDriverData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DriverData CreatNewData()
        {
            return new CalculateDriverData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return new CalculateDriverDevelopViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IRegistorConfigModel RegistorConfig()
        {
            return new ScriptExpressConfigModel();
            //throw new NotImplementedException();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...







    }
}
