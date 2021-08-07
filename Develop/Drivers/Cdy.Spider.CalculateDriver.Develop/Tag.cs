using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// 获取变量的值
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static object GetTagValue(string tag)
        {
            return null;
        }

        /// <summary>
        /// 设置变量的值
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetTagValue(string tag,object value)
        {
            return true;
        }

        /// <summary>
        /// 变量的质量戳是否为有效值
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static bool IsTagQualityGood(string tag)
        {
            return true;
        }


        /// <summary>
        /// 对变量的值求和
        /// </summary>
        /// <param name="tags">变量名</param>
        /// <returns></returns>
        public static double TagValueSum(params string[] tags)
        {
            return 0;
        }

        /// <summary>
        /// 对变量的值求平均值
        /// </summary>
        /// <param name="tags">变量名</param>
        /// <returns></returns>
        public static double TagValueAvg(params string[] tags)
        {
            return 0;
        }

        /// <summary>
        /// 对变量的值取最大
        /// </summary>
        /// <param name="tags">变量名</param>
        /// <returns></returns>
        public static double TagValueMax(params string[] tags)
        {
            return 0;
        }

        /// <summary>
        /// 对变量的值取最小
        /// </summary>
        /// <param name="tags">变量名</param>
        /// <returns></returns>
        public static double TagValueMin(params string[] tags)
        {
            return 0;
        }

        /// <summary>
        /// 对数值进行请平均值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Avg(params object[] values)
        {
            return 0;
        }

        /// <summary>
        /// 对数值进行取最大值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Max(params object[] values)
        {
            return 0;
        }

        /// <summary>
        /// 对数值进行取最小值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double Min(params object[] values)
        {
            return 0;
        }

        /// <summary>
        /// 对值进行取位
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="index">要取位的序号，从0开始</param>
        /// <returns></returns>
        public static byte Bit(object value,byte index)
        {
            return 0;
        }

    }
}
