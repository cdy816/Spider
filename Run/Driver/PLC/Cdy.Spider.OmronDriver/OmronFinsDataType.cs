using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OmronDriver
{
    public class OmronFinsDataType
    {

        #region ... Variables  ...
        /// <summary>
        ///  DM Area
        /// </summary>
        public static readonly OmronFinsDataType DM = new OmronFinsDataType(2, 130);
        /// <summary>
        /// CIO Area
        /// </summary>
        public static readonly OmronFinsDataType CIO = new OmronFinsDataType(48, 176);
        /// <summary>
        ///  Work Area
        /// </summary>
        public static readonly OmronFinsDataType WR = new OmronFinsDataType(49, 177);
        /// <summary>
        /// Holding Bit Area
        /// </summary>
        public static readonly OmronFinsDataType HR = new OmronFinsDataType(50, 178);
        /// <summary>
        ///  Auxiliary Bit Area
        /// </summary>
        public static readonly OmronFinsDataType AR = new OmronFinsDataType(51, 179);
        /// <summary>
        ///  TIM Area
        /// </summary>
        public static readonly OmronFinsDataType TIM = new OmronFinsDataType(9, 137);

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitCode"></param>
        /// <param name="wordCode"></param>
        public OmronFinsDataType(byte bitCode, byte wordCode)
        {
            this.BitCode = bitCode;
            this.WordCode = wordCode;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 进行位操作的指令
        /// </summary>
        public byte BitCode { get; private set; }

        /// <summary>
        /// 进行字操作的指令
        /// </summary>
        public byte WordCode { get; private set; }
        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
