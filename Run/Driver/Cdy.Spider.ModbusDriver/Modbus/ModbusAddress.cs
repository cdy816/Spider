using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.ModbusDriver
{
    public class ModbusAddress : DeviceAddressDataBase
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ModbusAddress()
        {
            this.Station = -1;
            this.Function = -1;
            base.AddressStart = 0;
        }

        /// <summary>
        /// 实例化一个对象，使用指定的地址初始化<br />
        /// Instantiate an object, initialize with the specified address
        /// </summary>
        /// <param name="address">传入的地址信息，支持富地址，例如s=2;x=3;100</param>
        public ModbusAddress(string address)
        {
            this.Station = -1;
            this.Function = -1;
            base.AddressStart = 0;
            this.Parse(address);
        }

        /// <summary>
        /// 实例化一个对象，使用指定的地址及功能码初始化<br />
        /// Instantiate an object and initialize it with the specified address and function code
        /// </summary>
        /// <param name="address">传入的地址信息，支持富地址，例如s=2;x=3;100</param>
        /// <param name="function">默认的功能码信息</param>
        public ModbusAddress(string address, byte function)
        {
            this.Station = -1;
            this.Function = (int)function;
            base.AddressStart = 0;
            this.Parse(address);
        }

        /// <summary>
        /// 实例化一个对象，使用指定的地址，站号，功能码来初始化<br />
        /// Instantiate an object, use the specified address, station number, function code to initialize
        /// </summary>
        /// <param name="address">传入的地址信息，支持富地址，例如s=2;x=3;100</param>
        /// <param name="station">站号信息</param>
        /// <param name="function">默认的功能码信息</param>
        public ModbusAddress(string address, byte station, byte function)
        {
            this.Station = -1;
            this.Function = (int)function;
            this.Station = (int)station;
            base.AddressStart = 0;
            this.Parse(address);
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public int Station { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Function { get; set; }
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        public override void Parse(string address)
        {
            if (address.IndexOf(';') < 0)
            {
                base.AddressStart = ushort.Parse(address);
            }
            else
            {
                string[] array = address.Split(new char[]
                {
                    ';'
                });
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i][0] == 's' || array[i][0] == 'S')
                    {
                        this.Station = (int)byte.Parse(array[i].Substring(2));
                    }
                    else if (array[i][0] == 's' || array[i][0] == 'S')
                    {
                        this.Function = (int)byte.Parse(array[i].Substring(2));
                    }
                    else
                    {
                        base.AddressStart = ushort.Parse(array[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 地址偏移指定的位置，返回一个新的地址对象<br />
        /// The address is offset by the specified position and a new address object is returned
        /// </summary>
        /// <param name="value">数据值信息</param>
        /// <returns>新增后的地址信息</returns>
        public ModbusAddress AddressAdd(int value)
        {
            return new ModbusAddress
            {
                Station = this.Station,
                Function = this.Function,
                AddressStart = (ushort)((int)base.AddressStart + value)
            };
        }

        /// <summary>
        /// 地址偏移1，返回一个新的地址对象<br />
        /// The address is offset by 1 and a new address object is returned
        /// </summary>
        /// <returns>新增后的地址信息</returns>
        public ModbusAddress AddressAdd()
        {
            return this.AddressAdd(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (this.Station >= 0)
            {
                stringBuilder.Append("s=" + this.Station.ToString() + ";");
            }
            if (this.Function == 2 || this.Function == 4)
            {
                stringBuilder.Append("x=" + this.Function.ToString() + ";");
            }
            stringBuilder.Append(base.AddressStart.ToString());
            return stringBuilder.ToString();
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
