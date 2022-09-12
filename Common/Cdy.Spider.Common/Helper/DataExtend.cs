using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Cdy.Spider.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataExtend
    {
        public static Random HslRandom { get; private set; } = new Random();

        public static bool[] ToBoolArray(this byte[] InBytes)
        {
            return ByteToBoolArray(InBytes);
        }

        /// <summary>
        /// 根据当前的位偏移地址及读取位长度信息，计算出实际的字节索引，字节数，字节位偏移
        /// </summary>
        /// <param name="addressStart">起始地址</param>
        /// <param name="length">读取的长度</param>
        /// <param name="newStart">返回的新的字节的索引，仍然按照位单位</param>
        /// <param name="byteLength">字节长度</param>
        /// <param name="offset">当前偏移的信息</param>
        public static void CalculateStartBitIndexAndLength(int addressStart, ushort length, out int newStart, out ushort byteLength, out int offset)
        {
            byteLength = (ushort)((addressStart + (int)length - 1) / 8 - addressStart / 8 + 1);
            offset = addressStart % 8;
            newStart = addressStart - offset;
        }

        /// <summary>
        /// 将指定的数据按照指定长度进行分割，例如int[10]，指定长度4，就分割成int[4],int[4],int[2]，然后拼接list<br />
        /// Divide the specified data according to the specified length, such as int [10], and specify the length of 4 to divide into int [4], int [4], int [2], and then concatenate the list
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="array">等待分割的数组</param>
        /// <param name="length">指定的长度信息</param>
        /// <returns>分割后结果内容</returns>
        /// <example>
        /// </example>
        public static List<T[]> ArraySplitByLength<T>(T[] array, int length)
        {
            List<T[]> result;
            if (array == null)
            {
                result = new List<T[]>();
            }
            else
            {
                List<T[]> list = new List<T[]>();
                int i = 0;
                while (i < array.Length)
                {
                    bool flag2 = i + length < array.Length;
                    if (flag2)
                    {
                        T[] array2 = new T[length];
                        Array.Copy(array, i, array2, 0, length);
                        i += length;
                        list.Add(array2);
                    }
                    else
                    {
                        T[] array3 = new T[array.Length - i];
                        Array.Copy(array, i, array3, 0, array3.Length);
                        i += length;
                        list.Add(array3);
                    }
                }
                result = list;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="paraName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ExtractParameter(ref string address, string paraName, int defaultValue)
        {
            return ExtractParameter(ref address, paraName);
        }

        /// <summary>
        /// 解析地址的附加参数方法，比如你的地址是s=100;D100，可以提取出"s"的值的同时，修改地址本身，如果"s"不存在的话，返回错误的消息内容<br />
        /// The method of parsing additional parameters of the address, for example, if your address is s=100;D100, you can extract the value of "s" and modify the address itself. 
        /// If "s" does not exist, return the wrong message content
        /// </summary>
        /// <param name="address">复杂的地址格式，比如：s=100;D100</param>
        /// <param name="paraName">等待提取的参数名称</param>
        /// <returns>解析后的参数结果内容</returns>
        // Token: 0x06002340 RID: 9024 RVA: 0x000BA058 File Offset: 0x000B8258
        public static int ExtractParameter(ref string address, string paraName)
        {
            int result =default(int);
            try
            {
                Match match = Regex.Match(address, paraName + "=[0-9A-Fa-fxX]+;");
                if(match.Success)
                {
                    string text = match.Value.Substring(paraName.Length + 1, match.Value.Length - paraName.Length - 2);
                    int value = (text.StartsWith("0x") || text.StartsWith("0X")) ? Convert.ToInt32(text.Substring(2), 16) : (text.StartsWith("0") ? Convert.ToInt32(text, 8) : Convert.ToInt32(text));
                    address = address.Replace(match.Value, "");
                    result = value;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 从字节构建一个ASCII格式的数据内容<br />
        /// Build an ASCII-formatted data content from bytes
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>ASCII格式的字节数组</returns>
        public static byte[] BuildAsciiBytesFrom(byte value)
        {
            return Encoding.ASCII.GetBytes(value.ToString("X2"));
        }

        /// <summary>
        /// 将原始的byte数组转换成ascii格式的byte数组<br />
        /// Converts the original byte array to an ASCII-formatted byte array
        /// </summary>
        /// <param name="inBytes">等待转换的byte数组</param>
        /// <returns>转换后的数组</returns>
        public static byte[] BytesToAsciiBytes(byte[] inBytes)
        {
            return Encoding.ASCII.GetBytes(ByteToHexString(inBytes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InBytes"></param>
        /// <returns></returns>
        public static string ByteToHexString(byte[] InBytes)
        {
            return ByteToHexString(InBytes, '\0');
        }

        /// <summary>
        /// 将16进制的字符串转化成Byte数据，将检测每2个字符转化，也就是说，中间可以是任意字符<br />
        /// Converts a 16-character string into byte data, which will detect every 2 characters converted, that is, the middle can be any character
        /// </summary>
        /// <param name="hex">十六进制的字符串，中间可以是任意的分隔符</param>
        /// <returns>转换后的字节数组</returns>
        /// <remarks>参数举例：AA 01 34 A8</remarks>
        /// <example>
        /// </example>
        public static byte[] HexStringToBytes(string hex)
        {
            MemoryStream memoryStream = new MemoryStream();
            for (int i = 0; i < hex.Length; i++)
            {
                bool flag = i + 1 < hex.Length;
                if (flag)
                {
                    bool flag2 = GetHexCharIndex(hex[i]) >= 0 && GetHexCharIndex(hex[i + 1]) >= 0;
                    if (flag2)
                    {
                        memoryStream.WriteByte((byte)(GetHexCharIndex(hex[i]) * 16 + GetHexCharIndex(hex[i + 1])));
                        i++;
                    }
                }
            }
            byte[] result = memoryStream.ToArray();
            memoryStream.Dispose();
            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static int GetHexCharIndex(char ch)
        {
            switch (ch)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case ':':
                case ';':
                case '<':
                case '=':
                case '>':
                case '?':
                case '@':
                    goto IL_D6;
                case 'A':
                    break;
                case 'B':
                    goto IL_BD;
                case 'C':
                    goto IL_C2;
                case 'D':
                    goto IL_C7;
                case 'E':
                    goto IL_CC;
                case 'F':
                    goto IL_D1;
                default:
                    switch (ch)
                    {
                        case 'a':
                            break;
                        case 'b':
                            goto IL_BD;
                        case 'c':
                            goto IL_C2;
                        case 'd':
                            goto IL_C7;
                        case 'e':
                            goto IL_CC;
                        case 'f':
                            goto IL_D1;
                        default:
                            goto IL_D6;
                    }
                    break;
            }
            return 10;
        IL_BD:
            return 11;
        IL_C2:
            return 12;
        IL_C7:
            return 13;
        IL_CC:
            return 14;
        IL_D1:
            return 15;
        IL_D6:
            return -1;
        }


        public static string ToHexString(this byte[] InBytes, char segment)
        {
            return ByteToHexString(InBytes, segment);
        }

        public static string ByteToHexString(byte[] InBytes, char segment)
        {
            return ByteToHexString(InBytes, segment, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InBytes"></param>
        /// <param name="segment"></param>
        /// <param name="newLineCount"></param>
        /// <returns></returns>
        public static string ByteToHexString(byte[] InBytes, char segment, int newLineCount)
        {
            string result;
            if (InBytes == null)
            {
                result = string.Empty;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                long num = 0L;
                foreach (byte b in InBytes)
                {
                    if (segment == '\0')
                    {
                        stringBuilder.Append(string.Format("{0:X2}", b));
                    }
                    else
                    {
                        stringBuilder.Append(string.Format("{0:X2}{1}", b, segment));
                    }
                    num += 1L;
                    if (newLineCount > 0 && num >= (long)newLineCount)
                    {
                        stringBuilder.Append(Environment.NewLine);
                        num = 0L;
                    }
                }
                bool flag4 = segment != '\0' && stringBuilder.Length > 1 && stringBuilder[stringBuilder.Length - 1] == segment;
                if (flag4)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }
                result = stringBuilder.ToString();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueStringByGuidAndRandom()
        {
            return Guid.NewGuid().ToString("N") + HslRandom.Next(1000, 10000).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="leftLength"></param>
        /// <param name="rightLength"></param>
        /// <returns></returns>
        public static T[] RemoveDouble<T>(this T[] value, int leftLength, int rightLength)
        {
            return ArrayRemoveDouble<T>(value, leftLength, rightLength);
        }

        /// <summary>
        /// 将一个数组的前后移除指定位数，返回新的一个数组<br />
        /// Removes a array before and after the specified number of bits, returning a new array
        /// </summary>
        /// <param name="value">数组</param>
        /// <param name="leftLength">前面的位数</param>
        /// <param name="rightLength">后面的位数</param>
        /// <returns>新的数组</returns>
        /// <example>
        /// </example> 
        public static T[] ArrayRemoveDouble<T>(T[] value, int leftLength, int rightLength)
        {
            T[] result;
            if (value == null)
            {
                result = null;
            }
            else
            {
                if (value.Length <= leftLength + rightLength)
                {
                    result = new T[0];
                }
                else
                {
                    T[] array = new T[value.Length - leftLength - rightLength];
                    Array.Copy(value, leftLength, array, 0, array.Length);
                    result = array;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] RemoveBegin<T>(this T[] value, int length)
        {
            return ArrayRemoveBegin<T>(value, length);
        }

        /// <summary>
        /// 将一个数组的前面指定位数移除，返回新的一个数组<br />
        /// Removes the preceding specified number of bits in a array, returning a new array
        /// </summary>
        /// <param name="value">数组</param>
        /// <param name="length">等待移除的长度</param>
        /// <returns>新的数组</returns>
        /// <exception cref="T:System.RankException"></exception>
        /// <example>
        /// </example> 
        public static T[] ArrayRemoveBegin<T>(T[] value, int length)
        {
            return ArrayRemoveDouble<T>(value, length, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] RemoveLast<T>(this T[] value, int length)
        {
            return ArrayRemoveLast<T>(value, length);
        }

        /// <summary>
        /// 将一个数组的后面指定位数移除，返回新的一个数组<br />
        /// Removes the specified number of digits after a array, returning a new array
        /// </summary>
        /// <param name="value">数组</param>
        /// <param name="length">等待移除的长度</param>
        /// <returns>新的数组</returns>
        /// <exception cref="T:System.RankException"></exception>
        /// <example>
        /// </example> 
        public static T[] ArrayRemoveLast<T>(T[] value, int length)
        {
            return ArrayRemoveDouble<T>(value, 0, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] SelectMiddle<T>(this T[] value, int index, int length)
        {
            return ArraySelectMiddle<T>(value, index, length);
        }

        /// <summary>
        /// 获取到数组里面的中间指定长度的数组<br />
        /// Get an array of the specified length in the array
        /// </summary>
        /// <param name="value">数组</param>
        /// <param name="index">起始索引</param>
        /// <param name="length">数据的长度</param>
        /// <returns>新的数组值</returns>
        /// <example>
        /// </example> 
        public static T[] ArraySelectMiddle<T>(T[] value, int index, int length)
        {
            T[] result;
            if (value == null)
            {
                result = null;
            }
            else
            {
                T[] array = new T[Math.Min(value.Length, length)];
                Array.Copy(value, index, array, 0, array.Length);
                result = array;
            }
            return result;
        }

        /// <inheritdoc cref="M:HslCommunication.BasicFramework.SoftBasic.ArraySelectBegin``1(``0[],System.Int32)" />
        // Token: 0x060000CE RID: 206 RVA: 0x00013E7F File Offset: 0x0001207F
        public static T[] SelectBegin<T>(this T[] value, int length)
        {
            return ArraySelectBegin<T>(value, length);
        }

        /// <summary>
        /// 选择一个数组的前面的几个数据信息<br />
        /// Select the begin few items of data information of a array
        /// </summary>
        /// <param name="value">数组</param>
        /// <param name="length">数据的长度</param>
        /// <returns>新的数组</returns>
        /// <example>
        /// </example> 
        public static T[] ArraySelectBegin<T>(T[] value, int length)
        {
            T[] array = new T[Math.Min(value.Length, length)];
            bool flag = array.Length != 0;
            if (flag)
            {
                Array.Copy(value, 0, array, 0, array.Length);
            }
            return array;
        }

        /// <inheritdoc cref="M:HslCommunication.BasicFramework.SoftBasic.ArraySelectLast``1(``0[],System.Int32)" />
        // Token: 0x060000CF RID: 207 RVA: 0x00013E88 File Offset: 0x00012088
        public static T[] SelectLast<T>(this T[] value, int length)
        {
            return ArraySelectLast<T>(value, length);
        }

        /// <summary>
        /// 选择一个数组的后面的几个数据信息<br />
        /// Select the last few items of data information of a array
        /// </summary>
        /// <param name="value">数组</param>
        /// <param name="length">数据的长度</param>
        /// <returns>新的数组信息</returns>
        /// <example>
        /// </example> 
        public static T[] ArraySelectLast<T>(T[] value, int length)
        {
            T[] array = new T[Math.Min(value.Length, length)];
            Array.Copy(value, value.Length - length, array, 0, array.Length);
            return array;
        }

        /// <summary>
        /// 拼接任意个泛型数组为一个总的泛型数组对象，采用深度拷贝实现。<br />
        /// Splicing any number of generic arrays into a total generic array object is implemented using deep copy.
        /// </summary>
        /// <typeparam name="T">数组的类型信息</typeparam>
        /// <param name="arrays">任意个长度的数组</param>
        /// <returns>拼接之后的最终的结果对象</returns>
        public static T[] SpliceArray<T>(params T[][] arrays)
        {
            int num = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                T[] array = arrays[i];
                if (array != null && array.Length != 0)
                {
                    num += arrays[i].Length;
                }
            }
            int num2 = 0;
            T[] array2 = new T[num];
            for (int j = 0; j < arrays.Length; j++)
            {
                T[] array3 = arrays[j];
                if (array3 != null && array3.Length != 0)
                {
                    arrays[j].CopyTo(array2, num2);
                    num2 += arrays[j].Length;
                }
            }
            return array2;
        }

        /// <summary>
        /// 将一个一维数组中的所有数据按照行列信息拷贝到二维数组里，返回当前的二维数组
        /// </summary>
        /// <typeparam name="T">数组的类型对象</typeparam>
        /// <param name="array">一维数组信息</param>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        public static T[,] CreateTwoArrayFromOneArray<T>(T[] array, int row, int col)
        {
            T[,] array2 = new T[row, col];
            int num = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    array2[i, j] = array[num];
                    num++;
                }
            }
            return array2;
        }

        /// <summary>
        /// 从Byte数组中提取所有的位数组<br />
        /// Extracts a bit array from a byte array, length represents the number of digits
        /// </summary>
        /// <param name="InBytes">原先的字节数组</param>
        /// <returns>转换后的bool数组</returns>
        /// <example>
        /// </example> 
        // Token: 0x06002B97 RID: 11159 RVA: 0x000E37C4 File Offset: 0x000E19C4
        public static bool[] ByteToBoolArray(byte[] InBytes)
        {
            return (InBytes == null) ? null : ByteToBoolArray(InBytes, InBytes.Length * 8);
        }

        /// <summary>
        /// 从Byte数组中提取位数组，length代表位数<br />
        /// Extracts a bit array from a byte array, length represents the number of digits
        /// </summary>
        /// <param name="InBytes">原先的字节数组</param>
        /// <param name="length">想要转换的长度，如果超出自动会缩小到数组最大长度</param>
        /// <returns>转换后的bool数组</returns>
        /// <example>
        /// </example> 
        public static bool[] ByteToBoolArray(byte[] InBytes, int length)
        {
            bool[] result;
            if (InBytes == null)
            {
                result = null;
            }
            else
            {
                bool flag2 = length > InBytes.Length * 8;
                if (flag2)
                {
                    length = InBytes.Length * 8;
                }
                bool[] array = new bool[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = BoolOnByteIndex(InBytes[i / 8], i % 8);
                }
                result = array;
            }
            return result;
        }

        /// <summary>
        /// 获取byte数据类型的第offset位，是否为True<br />
        /// Gets the index bit of the byte data type, whether it is True
        /// </summary>
        /// <param name="value">byte数值</param>
        /// <param name="offset">索引位置</param>
        /// <returns>结果</returns>
        /// <example>
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\BasicFramework\SoftBasicExample.cs" region="BoolOnByteIndex" title="BoolOnByteIndex示例" />
        /// </example>
        // Token: 0x06002B92 RID: 11154 RVA: 0x000E3640 File Offset: 0x000E1840
        public static bool BoolOnByteIndex(byte value, int offset)
        {
            byte dataByBitIndex = GetDataByBitIndex(offset);
            return (value & dataByBitIndex) == dataByBitIndex;
        }

        /// <summary>
        /// 将bool数组转换到byte数组<br />
        /// Converting a bool array to a byte array
        /// </summary>
        /// <param name="array">bool数组</param>
        /// <returns>转换后的字节数组</returns>
        /// <example>
        /// </example>
        public static byte[] BoolArrayToByte(bool[] array)
        {
            byte[] result;
            if (array == null)
            {
                result = null;
            }
            else
            {
                int num = (array.Length % 8 == 0) ? (array.Length / 8) : (array.Length / 8 + 1);
                byte[] array2 = new byte[num];
                for (int i = 0; i < array.Length; i++)
                {
                    bool flag2 = array[i];
                    if (flag2)
                    {
                        byte[] array3 = array2;
                        int num2 = i / 8;
                        array3[num2] += GetDataByBitIndex(i % 8);
                    }
                }
                result = array2;
            }
            return result;
        }

        /// <summary>
        /// 将byte数组按照双字节进行反转，如果为单数的情况，则自动补齐<br />
        /// Reverses the byte array by double byte, or if the singular is the case, automatically
        /// </summary>
        /// <param name="inBytes">输入的字节信息</param>
        /// <returns>反转后的数据</returns>
        /// <example>
        /// </example>
        public static byte[] BytesReverseByWord(byte[] inBytes)
        {
            byte[] result;
            if (inBytes == null)
            {
                result = null;
            }
            else
            {
                bool flag2 = inBytes.Length == 0;
                if (flag2)
                {
                    result = new byte[0];
                }
                else
                {
                    byte[] array = ArrayExpandToLengthEven<byte>(inBytes.CopyArray<byte>());
                    for (int i = 0; i < array.Length / 2; i++)
                    {
                        byte b = array[i * 2];
                        array[i * 2] = array[i * 2 + 1];
                        array[i * 2 + 1] = b;
                    }
                    result = array;
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static byte GetDataByBitIndex(int offset)
        {
            byte result;
            switch (offset)
            {
                case 0:
                    result = 1;
                    break;
                case 1:
                    result = 2;
                    break;
                case 2:
                    result = 4;
                    break;
                case 3:
                    result = 8;
                    break;
                case 4:
                    result = 16;
                    break;
                case 5:
                    result = 32;
                    break;
                case 6:
                    result = 64;
                    break;
                case 7:
                    result = 128;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
        /// <summary>
        /// 拷贝当前的实例数组，是基于引用层的浅拷贝，如果类型为值类型，那就是深度拷贝，如果类型为引用类型，就是浅拷贝
        /// </summary>
        /// <typeparam name="T">类型对象</typeparam>
        /// <param name="value">数组对象</param>
        /// <returns>拷贝的结果内容</returns>
        public static T[] CopyArray<T>(this T[] value)
        {
            T[] result;
            if (value == null)
            {
                result = null;
            }
            else
            {
                T[] array = new T[value.Length];
                Array.Copy(value, array, value.Length);
                result = array;
            }
            return result;
        }

        /// <summary>
        /// 根据英文小数点进行切割字符串，去除空白的字符<br />
        /// Cut the string according to the English decimal point and remove the blank characters
        /// </summary>
        /// <param name="str">字符串本身</param>
        /// <returns>切割好的字符串数组，例如输入 "100.5"，返回 "100", "5"</returns>
        public static string[] SplitDot(this string str)
        {
            return str.Split(".",StringSplitOptions.RemoveEmptyEntries);
        }


        /// <summary>
        /// 根据字符串内容，获取当前的位索引地址，例如输入 6,返回6，输入15，返回15，输入B，返回11
        /// </summary>
        /// <param name="bit">位字符串</param>
        /// <returns>结束数据</returns>
        public static int CalculateBitStartIndex(this string bit)
        {
            bool flag = bit.Contains("A") || bit.Contains("B") || bit.Contains("C") || bit.Contains("D") || bit.Contains("E") || bit.Contains("F");
            if (flag)
            {
                return Convert.ToInt32(bit, 16);
            }
            else
            {
                return Convert.ToInt32(bit);
            }
        }

        /// <summary>
        /// 将整数进行有效的拆分成数组，指定每个元素的最大值<br />
        /// Effectively split integers into arrays, specifying the maximum value for each element
        /// </summary>
        /// <param name="integer">整数信息</param>
        /// <param name="everyLength">单个的数组长度</param>
        /// <returns>拆分后的数组长度</returns>
        
        public static int[] SplitIntegerToArray(this int integer, int everyLength)
        {
            int[] array = new int[integer / everyLength + ((integer % everyLength == 0) ? 0 : 1)];
            for (int i = 0; i < array.Length; i++)
            {
                bool flag = i == array.Length - 1;
                if (flag)
                {
                    array[i] = ((integer % everyLength == 0) ? everyLength : (integer % everyLength));
                }
                else
                {
                    array[i] = everyLength;
                }
            }
            return array;
        }

        /// <summary>
        /// 将一个数组进行扩充到偶数长度<br />
        /// Extend an array to even lengths
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="data">原先数据的数据</param>
        /// <returns>新数组长度信息</returns>
        /// <example>
        /// </example>
        public static T[] ArrayExpandToLengthEven<T>(T[] data)
        {
            T[] result;
            if (data == null)
            {
                result = new T[0];
            }
            else
            {
                if (data.Length % 2 == 1)
                {
                    result = ArrayExpandToLength<T>(data, data.Length + 1);
                }
                else
                {
                    result = data;
                }
            }
            return result;
        }

        /// <summary>
        /// 将一个数组进行扩充到指定长度，或是缩短到指定长度<br />
        /// Extend an array to a specified length, or shorten to a specified length or fill
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="data">原先数据的数据</param>
        /// <param name="length">新数组的长度</param>
        /// <returns>新数组长度信息</returns>
        /// <example>
        /// </example>
        public static T[] ArrayExpandToLength<T>(T[] data, int length)
        {
            T[] result;
            if (data == null)
            {
                result = new T[length];
            }
            else
            {
                if (data.Length == length)
                {
                    result = data;
                }
                else
                {
                    T[] array = new T[length];
                    Array.Copy(data, array, Math.Min(data.Length, array.Length));
                    result = array;
                }
            }
            return result;
        }
    }
}
