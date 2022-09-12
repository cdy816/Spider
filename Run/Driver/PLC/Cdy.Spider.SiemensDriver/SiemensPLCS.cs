using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.SiemensDriver
{
    /// <summary>
    /// 西门子的PLC类型，目前支持的访问类型
    /// </summary>
    public enum SiemensPLCS
    {
        /// <summary>
        /// 1200系列
        /// </summary>
        S1200 = 1,
        /// <summary>
        /// 300系列
        /// </summary>
        S300,
        /// <summary>
        /// 400系列
        /// </summary>
        S400,
        /// <summary>
        /// 1500系列PLC
        /// </summary>
        S1500,
        /// <summary>
        /// 200的smart系列
        /// </summary>
        S200Smart,
        /// <summary>
        /// 200系统，需要额外配置以太网模块
        /// </summary>
        S200
    }


}
