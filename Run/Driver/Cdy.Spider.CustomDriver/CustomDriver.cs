using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Linq;
using System.Text;

namespace Cdy.Spider.CustomDriver
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomDriverImp
    {
        void Init();
        
        object OnReceiveData(string key, object data);
        
        void ProcessTimerElapsed();

        void WriteValue(string deviceInfo, object value, byte valueType);
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomDriver : TimerDriverRunner
    {

        #region ... Variables  ...
        private CustomDriverData mData;

        /// <summary>
        /// 
        /// </summary>
        public Script<object> mInitScript { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Script<object> mOnReceiveDataScript { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Script<object> mOnSetTagValueToDevicescript { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Script<object> mOnTimerFunScript { get; set; }

        private string mTemplate = @"using System;
                                    using System.Collections.Generic;
                                    using System.Text;
                                    using System.Linq;
                                    using System.Xml.Linq;
                                    using System.Buffers;
                                    using System.Threading;
                                    using Cdy.Spider.CustomDriver;

                                    namespace Cdy.Spider
                                    {
	                                    /// <summary>
                                        /// 
                                        /// </summary>
	                                    public class $ClassName$Driver:ICustomDriverImp
	                                    {

		                                    public void Init()
		                                    {
			                                    $InitBody$
		                                    }
		
		
		                                    public object OnReceiveData(string key, object data)
		                                    {
			                                    $OnReceiveDataBody$
		                                    }

		                                    public void ProcessTimerElapsed()
		                                    {
			                                    $ProcessTimerElapsed$
		                                    }

		                                    public void WriteValue(string deviceInfo, object value, byte valueType)
		                                    {
			                                    $WriteValue$
		                                    }
                                         }
                                    }";


        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "CustomDriver";


        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data => mData;

        

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            ScriptOptions sop = ScriptOptions.Default;
            //if (CalculateExtend.extend.ExtendDlls.Count > 0)
            //{
            //    sop = sop.AddReferences(CalculateExtend.extend.ExtendDlls.Select(e => Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(e)));
            //}
            //sop = sop.AddReferences(typeof(System.Collections.Generic.ReferenceEqualityComparer).Assembly).AddReferences(this.GetType().Assembly).WithImports("Cdy.Spider", "System", "System.Collections.Generic", "System.Linq", "System.Text");

            StringBuilder sb = new StringBuilder(mTemplate);
            sb.Replace("$ClassName$", "C"+Guid.NewGuid().ToString().Replace("-",""));
            sb.Replace("$InitBody$", mData.OnInitFunExpress);
            sb.Replace("$OnReceiveDataBody$", mData.OnReceiveDataFunExpress);
            sb.Replace("$WriteValue$", mData.OnSetTagValueToDeviceFunExpress);
            sb.Replace("$ProcessTimerElapsed$", mData.OnTimerFunExpress);

           

            base.Init();

           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="sop"></param>
        /// <returns></returns>
        private Script<object> OnCompile(string exp, ScriptOptions sop)
        {
            return null;
        }

        /// <summary>
        /// 被动接受数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        protected override object OnReceiveData(string key, object data, out bool handled)
        {
            if (mOnReceiveDataScript != null)
            {
                mOnReceiveDataScript.RunAsync(this);
            }
            return base.OnReceiveData(key, data, out handled);
        }


        /// <summary>
        /// 定时执行
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            if(this.mOnTimerFunScript!=null)
            {
                this.mOnTimerFunScript.RunAsync(this);
            }
            base.ProcessTimerElapsed();
        }

        /// <summary>
        /// 写入值到设备上
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public override void WriteValue(string deviceInfo, object value, byte valueType)
        {
            base.WriteValue(deviceInfo, value, valueType);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new CustomDriver();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


    }
}
