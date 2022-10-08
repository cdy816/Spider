using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.ModbusDriver
{

    public class ModbusAsciiNetProxy:ModbusRtuNetProxy
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        public ModbusAsciiNetProxy(DriverRunnerBase driver) :base(driver)
        {

        }
        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        protected override INetMessage GetNewNetMessage()
        {
            return new SpecifiedCharacterMessage(13, 10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override byte[] PackCommandWithHeader(byte[] command)
        {
            return ModbusInfo.TransModbusCoreToAsciiPackCommand(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public override byte[] UnpackResponseContent(byte[] send, byte[] response)
        {
            byte[] operateResult = ModbusInfo.TransAsciiPackCommandToCore(response);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                bool flag2 = send[1] + 128 == operateResult[1];
                if (flag2)
                {
                    return null;
                }
                else
                {
                    return ModbusInfo.ExtractActualData(operateResult);
                }
            }
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
