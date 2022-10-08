using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    public class MelsecHelper
    {
        /// <summary>
        /// 解析A1E协议数据地址<br />
        /// Parse A1E protocol data address
        /// </summary>
        /// <param name="address">数据地址</param>
        /// <returns>结果对象</returns>
        public static Tuple<MelsecA1EDataType, int> McA1EAnalysisAddress(string address)
        {
            try
            {
                //char c = address[0];
                char c2 = address[0];
                if (c2 <= 'Y')
                {
                    switch (c2)
                    {
                        case 'B':
                            return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.B, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.B.FromBase));
                        case 'C':
                            if (address[1] == 'S' || address[1] == 's')
                            {
                                return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.CS, Convert.ToInt32(address.Substring(2), MelsecA1EDataType.CS.FromBase));
                            }
                            else if (address[1] == 'C' || address[1] == 'c')
                            {
                                return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.CC, Convert.ToInt32(address.Substring(2), MelsecA1EDataType.CC.FromBase));
                            }
                            else if (address[1] == 'N' || address[1] == 'n')
                            {
                                return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.CN, Convert.ToInt32(address.Substring(2), MelsecA1EDataType.CN.FromBase));
                            }
                            else
                            {
                                throw new Exception("NotSupportedDataType");
                            }
                        case 'D':
                            return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.D, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.D.FromBase));
                        case 'E':
                            throw new Exception("NotSupportedDataType");
                        case 'F':
                            return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.F, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.F.FromBase));
                        default:
                            switch (c2)
                            {
                                case 'M':
                                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.M, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.M.FromBase));
                                case 'N':
                                case 'O':
                                case 'P':
                                case 'Q':
                                case 'U':
                                case 'V':
                                    throw new Exception("NotSupportedDataType");
                                case 'R':
                                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.R, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.R.FromBase));
                                case 'S':
                                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.S, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.S.FromBase));
                                case 'T':
                                    break;
                                case 'W':
                                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.W, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.W.FromBase));
                                case 'X':
                                    address = address.Substring(1);
                                    if (address.StartsWith("0"))
                                    {
                                        return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.X, Convert.ToInt32(address, 8));
                                    }
                                    else
                                    {
                                        return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.X, MelsecA1EDataType.X.FromBase);
                                    }
                                case 'Y':
                                    address = address.Substring(1);
                                    if (address.StartsWith("0"))
                                    {
                                        return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.Y, Convert.ToInt32(address, 8));
                                    }
                                    else
                                    {
                                        return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.Y, MelsecA1EDataType.Y.FromBase);
                                    }
                                default:
                                    throw new Exception("NotSupportedDataType");
                            }
                            break;
                    }
                }
                else
                {
                    switch (c2)
                    {
                        case 'b':
                            return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.B, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.B.FromBase));
                        case 'c':
                            if (address[1] == 'S' || address[1] == 's')
                            {
                                return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.CS, Convert.ToInt32(address.Substring(2), MelsecA1EDataType.CS.FromBase));
                            }
                            else if (address[1] == 'C' || address[1] == 'c')
                            {
                                return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.CC, Convert.ToInt32(address.Substring(2), MelsecA1EDataType.CC.FromBase));
                            }
                            else if (address[1] == 'N' || address[1] == 'n')
                            {
                                return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.CN, Convert.ToInt32(address.Substring(2), MelsecA1EDataType.CN.FromBase));
                            }
                            else
                            {
                                throw new Exception("NotSupportedDataType");
                            }
                        case 'd':
                            return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.D, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.D.FromBase));
                        case 'e':
                            throw new Exception("NotSupportedDataType");
                        case 'f':
                            return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.F, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.F.FromBase));
                        default:
                            switch (c2)
                            {
                                case 'm':
                                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.M, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.M.FromBase));
                                case 'n':
                                case 'o':
                                case 'p':
                                case 'q':
                                case 'u':
                                case 'v':
                                    throw new Exception("NotSupportedDataType");
                                case 'r':
                                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.R, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.R.FromBase));
                                case 's':
                                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.S, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.S.FromBase));
                                case 't':
                                    break;
                                case 'w':
                                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.W, Convert.ToInt32(address.Substring(1), MelsecA1EDataType.W.FromBase));
                                case 'x':
                                    address = address.Substring(1);
                                    if (address.StartsWith("0"))
                                    {
                                        return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.X, Convert.ToInt32(address, 8));
                                    }
                                    else
                                    {
                                        return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.X, MelsecA1EDataType.X.FromBase);
                                    }
                                case 'y':
                                    address = address.Substring(1);
                                    if (address.StartsWith("0"))
                                    {
                                        return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.Y, Convert.ToInt32(address, 8));
                                    }
                                    else
                                    {
                                        return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.Y, MelsecA1EDataType.Y.FromBase);
                                    }
                                default:
                                    throw new Exception("NotSupportedDataType");
                            }
                            break;
                    }
                }
                if (address[1] == 'S' || address[1] == 's')
                {
                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.TS, Convert.ToInt32(address.Substring(2), MelsecA1EDataType.TS.FromBase));
                }
                else if (address[1] == 'C' || address[1] == 'c')
                {
                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.TC, Convert.ToInt32(address.Substring(2), MelsecA1EDataType.TC.FromBase));
                }
                else if(address[1] == 'N' || address[1] == 'n')
                {
                    return new Tuple<MelsecA1EDataType, int>(MelsecA1EDataType.TN, Convert.ToInt32(address.Substring(2), MelsecA1EDataType.TN.FromBase));
                }
                else{

                    throw new Exception("NotSupportedDataType");
                }
            
            }
            catch (Exception ex)
            {
               
            }
            return null;
        }

        /// <summary>
        /// 从三菱的地址中构建MC协议的6字节的ASCII格式的地址
        /// </summary>
        /// <param name="address">三菱地址</param>
        /// <param name="type">三菱的数据类型</param>
        /// <returns>6字节的ASCII格式的地址</returns>
        public static byte[] BuildBytesFromAddress(int address, MelsecMcDataType type)
        {
            return Encoding.ASCII.GetBytes(address.ToString((type.FromBase == 10) ? "D6" : "X6"));
        }

        /// <summary>
        /// 将bool的组压缩成三菱格式的字节数组来表示开关量的
        /// </summary>
        /// <param name="value">原始的数据字节</param>
        /// <returns>压缩过后的数据字节</returns>
        public static byte[] TransBoolArrayToByteData(bool[] value)
        {
            int num = (value.Length + 1) / 2;
            byte[] array = new byte[num];
            for (int i = 0; i < num; i++)
            {
                bool flag = value[i * 2];
                if (flag)
                {
                    byte[] array2 = array;
                    int num2 = i;
                    array2[num2] += 16;
                }
                bool flag2 = i * 2 + 1 < value.Length;
                if (flag2)
                {
                    bool flag3 = value[i * 2 + 1];
                    if (flag3)
                    {
                        byte[] array3 = array;
                        int num3 = i;
                        array3[num3] += 1;
                    }
                }
            }
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] TransByteArrayToAsciiByteArray(byte[] value)
        {
            bool flag = value == null;
            byte[] result;
            if (flag)
            {
                result = new byte[0];
            }
            else
            {
                byte[] array = new byte[value.Length * 2];
                for (int i = 0; i < value.Length / 2; i++)
                {
                    DataExtend.BuildAsciiBytesFrom((byte)BitConverter.ToUInt16(value, i * 2)).CopyTo(array, 4 * i);
                }
                result = array;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] TransAsciiByteArrayToByteArray(byte[] value)
        {
            byte[] array = new byte[value.Length / 2];
            for (int i = 0; i < array.Length / 2; i++)
            {
                ushort value2 = Convert.ToUInt16(Encoding.ASCII.GetString(value, i * 4, 4), 16);
                BitConverter.GetBytes(value2).CopyTo(array, i * 2);
            }
            return array;
        }

        /// <summary>
        /// 计算Fx协议指令的和校验信息
        /// </summary>
        /// <param name="data">字节数据</param>
        /// <param name="start">起始的索引信息</param>
        /// <param name="tail">结束的长度信息</param>
        /// <returns>校验之后的数据</returns>
        public static byte[] FxCalculateCRC(byte[] data, int start = 1, int tail = 2)
        {
            int num = 0;
            for (int i = start; i < data.Length - tail; i++)
            {
                num += (int)data[i];
            }
            return DataExtend.BuildAsciiBytesFrom((byte)num);
        }

        /// <summary>
        /// 检查指定的和校验是否是正确的
        /// </summary>
        /// <param name="data">字节数据</param>
        /// <returns>是否成功</returns>
        public static bool CheckCRC(byte[] data)
        {
            byte[] array = MelsecHelper.FxCalculateCRC(data, 1, 2);
            bool flag = array[0] != data[data.Length - 2];
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = array[1] != data[data.Length - 1];
                result = !flag2;
            }
            return result;
        }
    }
}
