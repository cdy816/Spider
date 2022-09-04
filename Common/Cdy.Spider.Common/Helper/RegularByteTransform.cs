using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdy.Spider.Common
{

    /// <summary>
    /// 常规的字节转换类
    /// </summary>
    public class RegularByteTransform : ByteTransformBase
    {

        #region Constructor

        /// <summary>
        /// 实例化一个默认的对象
        /// </summary>
        public RegularByteTransform( )
        {

        }

        /// <summary>
        /// 使用指定的解析规则来初始化对象
        /// </summary>
        /// <param name="dataFormat">解析规则</param>
        public RegularByteTransform(DataFormat dataFormat) : base( dataFormat )
        {

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFormat"></param>
        /// <returns></returns>
        public override IByteTransform CreateByDateFormat(DataFormat dataFormat)
        {
            return new RegularByteTransform(dataFormat)
            {
                IsStringReverseByteWord = base.IsStringReverseByteWord
            };
        }

    }
}
