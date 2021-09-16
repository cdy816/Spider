//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/5/8 9:52:24.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class ValueConvertManager
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        public static ValueConvertManager manager = new ValueConvertManager();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, IValueConvert> mValueConverts = new Dictionary<string, IValueConvert>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public List<IValueConvert> Converts
        {
            get
            {
                return mValueConverts.Values.ToList();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueConvert"></param>
        public ValueConvertManager Registor(IValueConvert valueConvert)
        {
            if(!mValueConverts.ContainsKey(valueConvert.Name))
            {
                mValueConverts.Add(valueConvert.Name, valueConvert);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IValueConvert GetConvert(string name)
        {
            if(mValueConverts.ContainsKey(name))
            {
                return mValueConverts[name].Clone();
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            //注册线性转换器
            ValueConvertManager.manager.Registor(new LinerConvert()).Registor(new NumberToBitConvert()).Registor(new LinerConvert()).Registor(new StringFormateConvert()).Registor(new AdvanceConvert());
        }

       

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
