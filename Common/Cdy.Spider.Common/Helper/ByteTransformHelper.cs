using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.Common
{
    public class ByteTransformHelper
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 结果转换操作的基础方法，需要支持类型，及转换的委托，并捕获转换时的异常方法
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="result">源</param>
        /// <param name="translator">实际转换的委托</param>
        /// <returns>转换结果</returns>
        public static TResult GetResultFromBytes<TResult>(byte[] result, Func<byte[], TResult> translator,out bool res)
        {
            if(translator!=null && result!=null)
            {
                res = true;
                return translator(result);
            }
            else
            {
                res = false;
                return default(TResult);
            }
        }


        /// <summary>
        /// 结果转换操作的基础方法，需要支持类型，及转换的委托
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="result">源结果</param>
        /// <returns>转换结果</returns>
        public static TResult GetResultFromArray<TResult>(TResult[] result,out bool res)
        {
            return GetSuccessResultFromOther<TResult, TResult[]>(result, (TResult[] m) => m[0],out res );
        }

        /// <summary>
        /// 使用指定的转换方法，来获取到实际的结果对象内容，所转换的规则
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <typeparam name="TIn">输入类型</typeparam>
        /// <param name="result">原始的结果对象</param>
        /// <param name="trans">转换方法，从类型TIn转换拿到TResult的泛型委托</param>
        /// <returns>类型为TResult的对象</returns>
        public static TResult GetSuccessResultFromOther<TResult, TIn>(TIn result, Func<TIn, TResult> trans,out bool res)
        {
            if (result==null)
            {
                res=false;
                return default(TResult);
            }
            else
            {
                res=true;
               return trans(result);
            }
        }

        /// <summary>
        /// 使用指定的转换方法，来获取到实际的结果对象内容
        /// </summary>
        /// <typeparam name="TIn">输入类型</typeparam>
        /// <param name="result">原始的结果对象</param>
        /// <param name="trans">转换方法，从类型TIn转换拿到OperateResult的TResult的泛型委托</param>
        /// <returns>类型为TResult的对象</returns>
        public static bool GetResultFromOther<TIn>(TIn result, Func<TIn, bool> trans)
        {
            if (result==null)
            {
                return false;
            }
            else
            {
               return trans(result);
            }
        }

        /// <summary>
        /// 使用指定的转换方法，来获取到实际的结果对象内容
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <typeparam name="TIn">输入类型</typeparam>
        /// <param name="result">原始的结果对象</param>
        /// <param name="trans">转换方法，从类型TIn转换拿到OperateResult的TResult的泛型委托</param>
        /// <returns>类型为TResult的对象</returns>
        // Token: 0x060022E1 RID: 8929 RVA: 0x000B9378 File Offset: 0x000B7578
        public static TResult GetResultFromOther<TResult, TIn>(TIn result, Func<TIn, TResult> trans,out bool res)
        {
            if (result==null)
            {
                res = false;
                return default(TResult);
            }
            else
            {
                res = true;
               return trans(result);
            }
        }

        
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
