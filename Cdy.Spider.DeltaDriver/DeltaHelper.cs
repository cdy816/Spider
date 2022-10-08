using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.DeltaDriver
{
    /// <summary>
    /// 台达的系列信息
    /// </summary>
    public enum DeltaSeries
    {
        /// <summary>
        /// 台达的Dvp系列，适用DVP-ES/EX/EC/SS型号，DVP-SA/SC/SX/EH型号
        /// </summary>
        Dvp,
        /// <summary>
        /// 适用于AS300系列
        /// </summary>
        AS
    }

    /// <summary>
    /// 台达PLC的相关的接口信息
    /// </summary>
    // Token: 0x02000100 RID: 256
    public interface IDelta
    {
        /// <summary>
        /// 获取或设置当前的台达PLC的系列信息，默认为 DVP 系列<br />
        /// Get or set the current series information of Delta PLC, the default is DVP series
        /// </summary>
        DeltaSeries Series { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DeltaHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="address"></param>
        /// <param name="modbusCode"></param>
        /// <returns></returns>
        public static string TranslateToModbusAddress(IDelta delta, string address, byte modbusCode)
        {
            DeltaSeries series = delta.Series;
            DeltaSeries deltaSeries = series;
            string result;
            if (deltaSeries != DeltaSeries.Dvp)
            {
                if (deltaSeries != DeltaSeries.AS)
                {
                    result =null;
                }
                else
                {
                    result = DeltaASHelper.ParseDeltaASAddress(address, modbusCode);
                }
            }
            else
            {
                result = DeltaDvpHelper.ParseDeltaDvpAddress(address, modbusCode);
            }
            return result;
        }


        public static bool[] ReadBool(IDelta delta, Func<string, ushort, bool[]> readBoolFunc, string address, ushort length)
        {
            DeltaSeries series = delta.Series;
            DeltaSeries deltaSeries = series;
            bool[] result;
            if (deltaSeries != DeltaSeries.Dvp)
            {
                if (deltaSeries != DeltaSeries.AS)
                {
                    result = null;
                }
                else
                {
                    result = readBoolFunc(address, length);
                }
            }
            else
            {
                result = DeltaDvpHelper.ReadBool(readBoolFunc, address, length);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="writeBoolFunc"></param>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        internal static object Write(IDelta delta, Func<string, bool[], object> writeBoolFunc, string address, bool[] values)
        {
            DeltaSeries series = delta.Series;
            DeltaSeries deltaSeries = series;
            object result;
            if (deltaSeries != DeltaSeries.Dvp)
            {
                if (deltaSeries != DeltaSeries.AS)
                {
                    result = null;
                }
                else
                {
                    result = writeBoolFunc(address, values);
                }
            }
            else
            {
                result = DeltaDvpHelper.Write(writeBoolFunc, address, values);
            }
            return result;
        }

       
        internal static byte[] Read(IDelta delta, Func<string, ushort, byte[]> readFunc, string address, ushort length)
        {
            DeltaSeries series = delta.Series;
            DeltaSeries deltaSeries = series;
            byte[] result;
            if (deltaSeries != DeltaSeries.Dvp)
            {
                if (deltaSeries != DeltaSeries.AS)
                {
                    result = null;
                }
                else
                {
                    result = readFunc(address, length);
                }
            }
            else
            {
                result = DeltaDvpHelper.Read(readFunc, address, length);
            }
            return result;
        }

        
        internal static object Write(IDelta delta, Func<string, byte[], object> writeFunc, string address, byte[] value)
        {
            DeltaSeries series = delta.Series;
            DeltaSeries deltaSeries = series;
            object result;
            if (deltaSeries != DeltaSeries.Dvp)
            {
                if (deltaSeries != DeltaSeries.AS)
                {
                    result = null;
                }
                else
                {
                    result = writeFunc(address, value);
                }
            }
            else
            {
                result = DeltaDvpHelper.Write(writeFunc, address, value);
            }
            return result;
        }
    }
}
