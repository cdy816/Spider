using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.AllenBradleyDriver
{
    /// <summary>
    /// 罗克韦尔PLC的地址信息
    /// </summary>
    public class AllenBradleySLCAddress : DeviceAddressDataBase
    {
        /// <summary>
        /// 获取或设置等待读取的数据的代码<br />
        /// Get or set the code of the data waiting to be read
        /// </summary>
        public byte DataCode { get; set; }

        /// <summary>
        /// 获取或设置PLC的DB块数据信息<br />
        /// Get or set PLC DB data information
        /// </summary>
        public ushort DbBlock { get; set; }

        /// <summary>
        /// 从指定的地址信息解析成真正的设备地址信息
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        public override void Parse(string address, ushort length)
        {
            AllenBradleySLCAddress operateResult = AllenBradleySLCAddress.ParseFrom(address, length,out bool res);
            if (operateResult!=null)
            {
                base.AddressStart = operateResult.AddressStart;
                base.Length = operateResult.Length;
                this.DataCode = operateResult.DataCode;
                this.DbBlock = operateResult.DbBlock;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch (this.DataCode)
            {
                case 130:
                    return string.Format("O{0}:{1}", this.DbBlock, base.AddressStart);
                case 131:
                    return string.Format("I{0}:{1}", this.DbBlock, base.AddressStart);
                case 132:
                    return string.Format("S{0}:{1}", this.DbBlock, base.AddressStart);
                case 133:
                    return string.Format("B{0}:{1}", this.DbBlock, base.AddressStart);
                case 134:
                    return string.Format("T{0}:{1}", this.DbBlock, base.AddressStart);
                case 135:
                    return string.Format("C{0}:{1}", this.DbBlock, base.AddressStart);
                case 136:
                    return string.Format("R{0}:{1}", this.DbBlock, base.AddressStart);
                case 137:
                    return string.Format("N{0}:{1}", this.DbBlock, base.AddressStart);
                case 138:
                    return string.Format("F{0}:{1}", this.DbBlock, base.AddressStart);
                case 141:
                    return string.Format("ST{0}:{1}", this.DbBlock, base.AddressStart);
                case 142:
                    return string.Format("A{0}:{1}", this.DbBlock, base.AddressStart);
                case 145:
                    return string.Format("L{0}:{1}", this.DbBlock, base.AddressStart);
            }
            return base.AddressStart.ToString();
        }

        /// <summary>
        /// 从实际的罗克韦尔的地址里面解析出地址对象，例如 A9:0<br />
        /// Parse the address object from the actual Rockwell address, such as A9:0
        /// </summary>
        /// <param name="address">实际的地址数据信息，例如 A9:0</param>
        /// <returns>是否成功的结果对象</returns>
        public static AllenBradleySLCAddress ParseFrom(string address,out bool res)
        {
            return AllenBradleySLCAddress.ParseFrom(address, 0,out res);
        }

        /// <summary>
        /// 从实际的罗克韦尔的地址里面解析出地址对象，例如 A9:0<br />
        /// Parse the address object from the actual Rockwell address, such as A9:0
        /// </summary>
        /// <param name="address">实际的地址数据信息，例如 A9:0</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        public static AllenBradleySLCAddress ParseFrom(string address, ushort length,out bool res)
        {
            AllenBradleySLCAddress result;
            if (!address.Contains(":"))
            {
                res = false;
                result=null;
            }
            else
            {
                string[] array = address.Split(new char[]
                {
                    ':'
                });
                try
                {
                    AllenBradleySLCAddress allenBradleySLCAddress = new AllenBradleySLCAddress();
                    switch (array[0][0])
                    {
                        case 'A':
                            allenBradleySLCAddress.DataCode = 142;
                            break;
                        case 'B':
                            allenBradleySLCAddress.DataCode = 133;
                            break;
                        case 'C':
                            allenBradleySLCAddress.DataCode = 135;
                            break;
                        case 'F':
                            allenBradleySLCAddress.DataCode = 138;
                            break;
                        case 'I':
                            allenBradleySLCAddress.DataCode = 131;
                            break;
                        case 'L':
                            allenBradleySLCAddress.DataCode = 145;
                            break;
                        case 'N':
                            allenBradleySLCAddress.DataCode = 137;
                            break;
                        case 'O':
                            allenBradleySLCAddress.DataCode = 130;
                            break;
                        case 'R':
                            allenBradleySLCAddress.DataCode = 136;
                            break;
                        case 'S':
                            {
                                bool flag2 = array[0].Length > 1 && array[0][1] == 'T';
                                if (flag2)
                                {
                                    allenBradleySLCAddress.DataCode = 141;
                                }
                                else
                                {
                                    allenBradleySLCAddress.DataCode = 132;
                                }
                                break;
                            }
                        case 'T':
                            allenBradleySLCAddress.DataCode = 134;
                            break;
                        default:
                            res = false;
                            return null;
                    }
                    byte dataCode = allenBradleySLCAddress.DataCode;
                    byte b = dataCode;
                    switch (b)
                    {
                        case 130:
                            allenBradleySLCAddress.DbBlock = (ushort)((array[0].Length == 1) ? 0 : ushort.Parse(array[0].Substring(1)));
                            break;
                        case 131:
                            allenBradleySLCAddress.DbBlock = (ushort)((array[0].Length == 1) ? 1 : ushort.Parse(array[0].Substring(1)));
                            break;
                        case 132:
                            allenBradleySLCAddress.DbBlock = (ushort)((array[0].Length == 1) ? 2 : ushort.Parse(array[0].Substring(1)));
                            break;
                        default:
                            if (b != 141)
                            {
                                allenBradleySLCAddress.DbBlock = ushort.Parse(array[0].Substring(1));
                            }
                            else
                            {
                                allenBradleySLCAddress.DbBlock = (ushort)((array[0].Length == 2) ? 1 : ushort.Parse(array[0].Substring(2)));
                            }
                            break;
                    }
                    allenBradleySLCAddress.AddressStart = (int)ushort.Parse(array[1]);
                    result = allenBradleySLCAddress;
                }
                catch (Exception ex)
                {
                    res = false;
                    result = null;
                }
            }
            res = true;
            return result;
        }
    }
}
