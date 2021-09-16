using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvanceConvert : IValueConvert
    {

        #region ... Variables  ...

        private string mExpress;

        private string mConvertBackExpress;

        private string mConvertCompileId;

        private string mConvertBackCompileId;

        private ConvertExecuteContext mContext = new ConvertExecuteContext();

        private ConvertExecuteContext mBackContext = new ConvertExecuteContext();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public string Name => "Advance";

        /// <summary>
        /// 
        /// </summary>
        public string Express
        {
            get
            {
                return mExpress;
            }
            set
            {
                if (mExpress != value)
                {
                    mExpress = value;
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string ConvertBackExpress
        {
            get
            {
                return mConvertBackExpress;
            }
            set
            {
                if (mConvertBackExpress != value)
                {
                    mConvertBackExpress = value;
                }
            }
        }



        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueConvert Clone()
        {
            return new AdvanceConvert() { Express = this.Express };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValueConvert LoadFromString(string value)
        {
            string[] ss = value.Split("{}");
            return new AdvanceConvert() { Express = ss[0],ConvertBackExpress=ss[1] };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SaveToString()
        {
            return this.Express+"{}"+this.ConvertBackExpress;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool SupportTag(Tagbase tag)
        {
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertBackTo(object value)
        {
            mBackContext.Value = value;
            if (string.IsNullOrEmpty(mConvertBackExpress))
            {
                return value;
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(mConvertBackCompileId))
                    {
                        mConvertCompileId = ServiceLocator.Locator.Resolve<IScriptService>().Compile(mExpress, mContext);
                    }
                    return ServiceLocator.Locator.Resolve<IScriptService>().RunById(mConvertBackCompileId, mContext);
                }
                catch
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertTo(object value)
        {
            mContext.Value = value;
            if (string.IsNullOrEmpty(mExpress))
            {
                return value;
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(mConvertCompileId))
                    {
                        mConvertCompileId = ServiceLocator.Locator.Resolve<IScriptService>().Compile(mExpress, mContext);
                    }
                    return ServiceLocator.Locator.Resolve<IScriptService>().RunById(mConvertCompileId, mContext);
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }

    /// <summary>
    /// 
    /// </summary>
    public class ConvertExecuteContext
    {
        public object Value { get; set; }
    }


}
