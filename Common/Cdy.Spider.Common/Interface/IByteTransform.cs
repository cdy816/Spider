using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.Common
{
    /// <summary>
    /// 支持转换器的基础接口，规定了实际的数据类型和字节数组进行相互转换的方法。主要为<see cref="T:System.Boolean" />,<see cref="T:System.Byte" />,<see cref="T:System.Int16" />,<see cref="T:System.UInt16" />,<see cref="T:System.Int32" />,<see cref="T:System.UInt32" />,
    /// <see cref="T:System.Int64" />,<see cref="T:System.UInt64" />,<see cref="T:System.Single" />,<see cref="T:System.Double" />,<see cref="T:System.String" />之间的变换关系<br />
    /// Support the basic interface of the converter, and stipulate the method of mutual conversion between actual data types and byte arrays. Mainly <see cref="T:System.Boolean" />,<see cref="T:System.Byte" />,
    /// <see cref="T:System.Int16" />,<see cref="T:System.UInt16" />,<see cref="T:System.Int32" />,<see cref="T:System.UInt32" />,<see cref="T:System.Int64" />,<see cref="T:System.UInt64" />,<see cref="T:System.Single" />,<see cref="T:System.Double" />,
    /// <see cref="T:System.String" />The transformation relationship between
    /// </summary>
    /// <remarks>
    /// 所有的设备通讯类都内置了该转换的模型，并且已经配置好数据的高地位模式，可以方便的转换信息，当然您也可以手动修改<see cref="P:HslCommunication.Core.IByteTransform.DataFormat" />属性来满足更加特殊的场景<br />
    /// All device communication classes have a built-in conversion model, and the high-status mode of the data has been configured, 
    /// which can easily convert the information. Of course, you can also manually modify the <see cref="P:HslCommunication.Core.IByteTransform.DataFormat" /> attribute to meet more special requirements.
    /// </remarks>
    /// <example>
    /// 事实上来说，在不同的PLC设备里，对数据类型的描述通常有所区别。比如说一般PLC所说的字数据，int数据，对于C#来说则是short，ushort。
    /// </example>
    // Token: 0x0200019B RID: 411
    public interface IByteTransform
    {
        /// <summary>
        /// 从缓存中提取出bool结果，需要传入想要提取的位索引，注意：是从0开始的位索引，10则表示 buffer[1] 的第二位。<br />
        /// To extract the bool result from the cache, you need to pass in the bit index you want to extract. Note: the bit index starts from 0, and 10 represents the second bit of buffer[1].
        /// </summary>
        /// <param name="buffer">等待提取的缓存数据</param>
        /// <param name="index">位的索引，注意：是从0开始的位索引，10则表示 buffer[1] 的第二位。</param>
        /// <returns>表示不是 <c>true</c> 就是 <c>false</c> 的 bool 数据</returns>
        bool TransBool(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取出bool数组结果，需要传入想要提取的位索引，注意：是从0开始的位索引，10则表示 buffer[1] 的第二位。长度为 bool 数量的长度，传入 10 则获取 10 个长度的 bool[] 数组。<br />
        /// To extract the result of the bool array from the cache, you need to pass in the bit index you want to extract. Note: the bit index starts from 0, 
        /// and 10 represents the second bit of buffer[1]. The length is the length of the number of bools. If you pass in 10, you will get a bool[] array of 10 lengths.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">位的起始索引，需要传入想要提取的位索引，注意：是从0开始的位索引，10则表示 buffer[1] 的第二位</param>
        /// <param name="length">读取的 bool 长度，按照位为单位，传入 10 则表示获取 10 个长度的 bool[] </param>
        /// <returns>bool数组</returns>
        bool[] TransBool(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取byte结果，需要指定起始的字节索引<br />
        /// To extract the byte result from the cache, you need to specify the starting byte index
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>byte对象</returns>
        // Token: 0x060022ED RID: 8941
        byte TransByte(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取byte数组结果，需要指定起始的字节索引，以及指定读取的字节长度<br />
        /// To extract the byte array result from the cache, you need to specify the starting byte index and the byte length to be read
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>byte数组对象</returns>
        // Token: 0x060022EE RID: 8942
        byte[] TransByte(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取short结果，需要指定起始的字节索引，按照字节为单位，一个short占用两个字节<br />
        /// To extract short results from the cache, you need to specify the starting byte index, in bytes, A short occupies two bytes
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>short对象</returns>
        // Token: 0x060022EF RID: 8943
        short TransInt16(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取short数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 short 数组的长度，如果传入 10 ，则表示提取 10 个连续的 short 数据，该数据共占用 20 字节。<br />
        /// To extract the result of the short array from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the extracted short array. If 10 is passed in, it means to extract 10 consecutive short data. Occupies 20 bytes.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>short数组对象</returns>
        // Token: 0x060022F0 RID: 8944
        short[] TransInt16(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取short二维数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 short 数组的行和列的长度，按照 short 为单位的个数。<br />
        /// To extract the result of a short two-dimensional array from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the rows and columns of the extracted short array, in terms of the number of shorts.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="row">二维数组行</param>
        /// <param name="col">二维数组列</param>
        /// <returns>二维short数组</returns>
        // Token: 0x060022F1 RID: 8945
        short[,] TransInt16(byte[] buffer, int index, int row, int col);

        /// <summary>
        /// 从缓存中提取ushort结果，需要指定起始的字节索引，按照字节为单位，一个ushort占用两个字节<br />
        /// To extract ushort results from the cache, you need to specify the starting byte index, in bytes, A ushort occupies two bytes
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ushort对象</returns>
        // Token: 0x060022F2 RID: 8946
        ushort TransUInt16(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取ushort数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 ushort 数组的长度，如果传入 10 ，则表示提取 10 个连续的 ushort 数据，该数据共占用 20 字节。<br />
        /// To extract the ushort array result from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the extracted ushort array. If 10 is passed in, it means to extract 10 consecutive ushort data. Occupies 20 bytes.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>ushort数组对象</returns>
        // Token: 0x060022F3 RID: 8947
        ushort[] TransUInt16(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取ushort二维数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 ushort 数组的行和列的长度，按照 ushort 为单位的个数。<br />
        /// To extract the result of the ushort two-dimensional array from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the rows and columns of the extracted ushort array, in terms of the number of ushorts.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="row">二维数组行</param>
        /// <param name="col">二维数组列</param>
        /// <returns>二维ushort数组</returns>
        // Token: 0x060022F4 RID: 8948
        ushort[,] TransUInt16(byte[] buffer, int index, int row, int col);

        /// <summary>
        /// 从缓存中提取int结果，需要指定起始的字节索引，按照字节为单位，一个int占用四个字节<br />
        /// To extract the int result from the cache, you need to specify the starting byte index, in bytes, A int occupies four bytes
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>int对象</returns>
        // Token: 0x060022F5 RID: 8949
        int TransInt32(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取int数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 int 数组的长度，如果传入 10 ，则表示提取 10 个连续的 int 数据，该数据共占用 40 字节。<br />
        /// To extract the int array result from the cache, you need to specify the starting byte index, in bytes,
        /// and then specify the length of the extracted int array. If 10 is passed in, it means to extract 10 consecutive int data. Occupies 40 bytes.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>int数组对象</returns>
        // Token: 0x060022F6 RID: 8950
        int[] TransInt32(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取int二维数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 int 数组的行和列的长度，按照 int 为单位的个数。<br />
        /// To extract the result of an int two-dimensional array from the cache, you need to specify the starting byte index, 
        /// in bytes, and then specify the length of the rows and columns of the extracted int array, in the number of int units.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="row">二维数组行</param>
        /// <param name="col">二维数组列</param>
        /// <returns>二维int数组</returns>
        // Token: 0x060022F7 RID: 8951
        int[,] TransInt32(byte[] buffer, int index, int row, int col);

        /// <summary>
        /// 从缓存中提取uint结果，需要指定起始的字节索引，按照字节为单位，一个uint占用四个字节<br />
        /// To extract uint results from the cache, you need to specify the starting byte index, in bytes, A uint occupies four bytes
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>uint对象</returns>
        // Token: 0x060022F8 RID: 8952
        uint TransUInt32(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取uint数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 uint 数组的长度，如果传入 10 ，则表示提取 10 个连续的 uint 数据，该数据共占用 40 字节。<br />
        /// To extract the uint array result from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the extracted uint array. If 10 is passed in, it means to extract 10 consecutive uint data. Occupies 40 bytes.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>uint数组对象</returns>
        // Token: 0x060022F9 RID: 8953
        uint[] TransUInt32(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取uint二维数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 uint 数组的行和列的长度，按照 uint 为单位的个数。<br />
        /// To extract the result of a uint two-dimensional array from the cache, you need to specify the starting byte index, 
        /// in bytes, and then specify the length of the rows and columns of the extracted uint array, in terms of uint as the unit.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="row">二维数组行</param>
        /// <param name="col">二维数组列</param>
        /// <returns>uint二维数组对象</returns>
        // Token: 0x060022FA RID: 8954
        uint[,] TransUInt32(byte[] buffer, int index, int row, int col);

        /// <summary>
        /// 从缓存中提取long结果，需要指定起始的字节索引，按照字节为单位，一个long占用八个字节<br />
        /// To extract the long result from the cache, you need to specify the starting byte index, in bytes, A long occupies eight bytes
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>long对象</returns>
        // Token: 0x060022FB RID: 8955
        long TransInt64(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取long数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 long 数组的长度，如果传入 10 ，则表示提取 10 个连续的 long 数据，该数据共占用 80 字节。<br />
        /// To extract the long array result from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the long array to be extracted. If 10 is passed in, it means to extract 10 consecutive long data. Occupies 80 bytes.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>long数组对象</returns>
        // Token: 0x060022FC RID: 8956
        long[] TransInt64(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取long二维数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 long 数组的行和列的长度，按照 long 为单位的个数。<br />
        /// To extract the result of a long two-dimensional array from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the rows and columns of the extracted long array, in long as the number of units.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="row">二维数组行</param>
        /// <param name="col">二维数组列</param>
        /// <returns>long二维数组对象</returns>
        // Token: 0x060022FD RID: 8957
        long[,] TransInt64(byte[] buffer, int index, int row, int col);

        /// <summary>
        /// 从缓存中提取ulong结果，需要指定起始的字节索引，按照字节为单位，一个ulong占用八个字节<b />
        /// To extract the ulong result from the cache, you need to specify the starting byte index, in bytes, A ulong occupies eight bytes
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ulong对象</returns>
        // Token: 0x060022FE RID: 8958
        ulong TransUInt64(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取ulong数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 ulong 数组的长度，如果传入 10 ，则表示提取 10 个连续的 ulong 数据，该数据共占用 80 字节。<br />
        /// To extract the ulong array result from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the extracted ulong array. If 10 is passed in, it means to extract 10 consecutive ulong data. Occupies 80 bytes.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>ulong数组对象</returns>
        // Token: 0x060022FF RID: 8959
        ulong[] TransUInt64(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取ulong二维数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 ulong 数组的行和列的长度，按照 ulong 为单位的个数。<br />
        /// To extract the result of the ulong two-dimensional array from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the rows and columns of the extracted ulong array, in the number of ulong units.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="row">二维数组行</param>
        /// <param name="col">二维数组列</param>
        /// <returns>ulong二维数组对象</returns>
        // Token: 0x06002300 RID: 8960
        ulong[,] TransUInt64(byte[] buffer, int index, int row, int col);

        /// <summary>
        /// 从缓存中提取float结果，需要指定起始的字节索引，按照字节为单位，一个float占用四个字节<b />
        /// To extract the float result from the cache, you need to specify the starting byte index, in units of bytes, A float occupies four bytes
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>float对象</returns>
        // Token: 0x06002301 RID: 8961
        float TransSingle(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取float数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 float 数组的长度，如果传入 10 ，则表示提取 10 个连续的 float 数据，该数据共占用 40 字节。<br />
        /// To extract the result of the float array from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the extracted float array. If 10 is passed in, it means that 10 consecutive float data are extracted. Occupies 40 bytes.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>float数组</returns>
        // Token: 0x06002302 RID: 8962
        float[] TransSingle(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取float二维数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 float 数组的行和列的长度，按照 float 为单位的个数。<br />
        /// To extract the result of a float two-dimensional array from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the rows and columns of the extracted float array, in terms of the number of floats.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="row">二维数组行</param>
        /// <param name="col">二维数组列</param>
        /// <returns>float二维数组对象</returns>
        // Token: 0x06002303 RID: 8963
        float[,] TransSingle(byte[] buffer, int index, int row, int col);

        /// <summary>
        /// 从缓存中提取double结果，需要指定起始的字节索引，按照字节为单位，一个double占用八个字节<br />
        /// To extract the double result from the cache, you need to specify the starting byte index, in bytes, A double occupies eight bytes
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>double对象</returns>
        // Token: 0x06002304 RID: 8964
        double TransDouble(byte[] buffer, int index);

        /// <summary>
        /// 从缓存中提取double数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 double 数组的长度，如果传入 10 ，则表示提取 10 个连续的 double 数据，该数据共占用 80 字节。<br />
        /// To extract the double array result from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the extracted double array. If 10 is passed in, it means to extract 10 consecutive double data. Occupies 80 bytes.
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>double数组</returns>
        // Token: 0x06002305 RID: 8965
        double[] TransDouble(byte[] buffer, int index, int length);

        /// <summary>
        /// 从缓存中提取double二维数组结果，需要指定起始的字节索引，按照字节为单位，然后指定提取的 double 数组的行和列的长度，按照 double 为单位的个数。<br />
        /// To extract the result of a double two-dimensional array from the cache, you need to specify the starting byte index, in bytes, 
        /// and then specify the length of the rows and columns of the extracted double array, in terms of the number of doubles.
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="row">二维数组行</param>
        /// <param name="col">二维数组列</param>
        /// <returns>double二维数组对象</returns>
        // Token: 0x06002306 RID: 8966
        double[,] TransDouble(byte[] buffer, int index, int row, int col);

        /// <summary>
        /// 从缓存中提取string结果，使用指定的编码将全部的缓存转为字符串<br />
        /// Extract the string result from the cache, use the specified encoding to convert all the cache into a string
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="encoding">字符串的编码</param>
        /// <returns>string对象</returns>
        // Token: 0x06002307 RID: 8967
        string TransString(byte[] buffer, Encoding encoding);

        /// <summary>
        /// 从缓存中的部分字节数组转化为string结果，使用指定的编码，指定起始的字节索引，字节长度信息。<br />
        /// Convert a part of the byte array in the buffer into a string result, use the specified encoding, specify the starting byte index, and byte length information.
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">byte数组长度</param>
        /// <param name="encoding">字符串的编码</param>
        /// <returns>string对象</returns>
        // Token: 0x06002308 RID: 8968
        string TransString(byte[] buffer, int index, int length, Encoding encoding);

        /// <summary>
        /// bool变量转化缓存数据，一般来说单bool只能转化为0x01 或是 0x00<br />
        /// The bool variable is converted to cache data, a single bool can only be converted to 0x01 or 0x00
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002309 RID: 8969
        byte[] TransByte(bool value);

        /// <summary>
        /// 将bool数组变量转化缓存数据，如果数组长度不满足8的倍数，则自动补0操作。<br />
        /// Convert the bool array variable to the cache data. If the length of the array does not meet a multiple of 8, it will automatically add 0.
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600230A RID: 8970
        byte[] TransByte(bool[] values);

        /// <summary>
        /// 将byte变量转化缓存数据<br />
        /// Convert byte variables into cached data
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600230B RID: 8971
        byte[] TransByte(byte value);

        /// <summary>
        /// short变量转化缓存数据，一个short数据可以转为2个字节的byte数组<br />
        /// Short variable is converted to cache data, a short data can be converted into a 2-byte byte array
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600230C RID: 8972
        byte[] TransByte(short value);

        /// <summary>
        /// short数组变量转化缓存数据，n个长度的short数组，可以转为2*n个长度的byte数组<br />
        /// The short array variable transforms the buffered data, a short array of n lengths can be converted into a byte array of 2*n lengths
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600230D RID: 8973
        byte[] TransByte(short[] values);

        /// <summary>
        /// ushort变量转化缓存数据，一个ushort数据可以转为2个字节的Byte数组<br />
        /// ushort variable converts buffer data, a ushort data can be converted into a 2-byte Byte array
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600230E RID: 8974
        byte[] TransByte(ushort value);

        /// <summary>
        /// ushort数组变量转化缓存数据，n个长度的ushort数组，可以转为2*n个长度的byte数组<br />
        /// The ushort array variable transforms the buffer data, the ushort array of n length can be converted into a byte array of 2*n length
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600230F RID: 8975
        byte[] TransByte(ushort[] values);

        /// <summary>
        /// int变量转化缓存数据，一个int数据可以转为4个字节的byte数组<br />
        /// Int variable converts cache data, an int data can be converted into a 4-byte byte array
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002310 RID: 8976
        byte[] TransByte(int value);

        /// <summary>
        /// int数组变量转化缓存数据，n个长度的int数组，可以转为4*n个长度的byte数组<br />
        /// The int array variable transforms the cache data, the int array of n length can be converted to the byte array of 4*n length
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002311 RID: 8977
        byte[] TransByte(int[] values);

        /// <summary>
        /// uint变量转化缓存数据，一个uint数据可以转为4个字节的byte数组<br />
        /// uint variable converts buffer data, a uint data can be converted into a 4-byte byte array
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002312 RID: 8978
        byte[] TransByte(uint value);

        /// <summary>
        /// uint数组变量转化缓存数据，n个长度的uint数组，可以转为4*n个长度的byte数组<br />
        /// uint array variable converts buffer data, uint array of n length can be converted to byte array of 4*n length
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002313 RID: 8979
        byte[] TransByte(uint[] values);

        /// <summary>
        /// long变量转化缓存数据，一个long数据可以转为8个字节的byte数组<br />
        /// Long variable is converted into cache data, a long data can be converted into 8-byte byte array
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002314 RID: 8980
        byte[] TransByte(long value);

        /// <summary>
        /// long数组变量转化缓存数据，n个长度的long数组，可以转为8*n个长度的byte数组<br />
        /// The long array variable transforms the buffer data, the long array of n length can be converted into the byte array of 8*n length
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002315 RID: 8981
        byte[] TransByte(long[] values);

        /// <summary>
        /// ulong变量转化缓存数据，一个ulong数据可以转为8个字节的byte数组<br />
        /// Ulong variable converts cache data, a ulong data can be converted into 8-byte byte array
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002316 RID: 8982
        byte[] TransByte(ulong value);

        /// <summary>
        /// ulong数组变量转化缓存数据，n个长度的ulong数组，可以转为8*n个长度的byte数组<br />
        /// The ulong array variable transforms the buffer data, the ulong array of n length can be converted to the byte array of 8*n length
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002317 RID: 8983
        byte[] TransByte(ulong[] values);

        /// <summary>
        /// float变量转化缓存数据，一个float数据可以转为4个字节的byte数组<br />
        /// Float variable is converted into cache data, a float data can be converted into a 4-byte byte array
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002318 RID: 8984
        byte[] TransByte(float value);

        /// <summary>
        /// float数组变量转化缓存数据，n个长度的float数组，可以转为4*n个长度的byte数组<br />
        /// Float array variable converts buffer data, n-length float array can be converted into 4*n-length byte array
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        // Token: 0x06002319 RID: 8985
        byte[] TransByte(float[] values);

        /// <summary>
        /// double变量转化缓存数据，一个double数据可以转为8个字节的byte数组<br />
        /// The double variable is converted to cache data, a double data can be converted into an 8-byte byte array
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600231A RID: 8986
        byte[] TransByte(double value);

        /// <summary>
        /// double数组变量转化缓存数据，n个长度的double数组，可以转为8*n个长度的byte数组<br />
        /// The double array variable transforms the buffer data, the double array of n length can be converted to the byte array of 8*n length
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600231B RID: 8987
        byte[] TransByte(double[] values);

        /// <summary>
        /// 使用指定的编码字符串转化缓存数据<br />
        /// Use the specified encoding string to convert the cached data
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <param name="encoding">字符串的编码方式</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600231C RID: 8988
        byte[] TransByte(string value, Encoding encoding);

        /// <summary>
        /// 使用指定的编码字符串转化缓存数据，指定转换之后的字节长度信息<br />
        /// Use the specified encoding string to convert the cached data, specify the byte length information after conversion
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <param name="length">转换之后的数据长度</param>
        /// <param name="encoding">字符串的编码方式</param>
        /// <returns>buffer数据</returns>
        // Token: 0x0600231D RID: 8989
        byte[] TransByte(string value, int length, Encoding encoding);

        /// <summary>
        /// 获取或设置数据解析的格式，可选ABCD, BADC，CDAB，DCBA格式，对int,uint,float,double,long,ulong类型有作用<br />
        /// Get or set the format of the data analysis, optional ABCD, BADC, CDAB, DCBA format, effective for int, uint, float, double, long, ulong type
        /// </summary>
        // Token: 0x17000793 RID: 1939
        // (get) Token: 0x0600231E RID: 8990
        // (set) Token: 0x0600231F RID: 8991
        DataFormat DataFormat { get; set; }

        /// <summary>
        /// 获取或设置在解析字符串的时候是否将字节按照字单位反转<br />
        /// Gets or sets whether to reverse the bytes in word units when parsing strings
        /// </summary>
        // Token: 0x17000794 RID: 1940
        // (get) Token: 0x06002320 RID: 8992
        // (set) Token: 0x06002321 RID: 8993
        bool IsStringReverseByteWord { get; set; }

        /// <summary>
        /// 根据指定的<see cref="P:HslCommunication.Core.IByteTransform.DataFormat" />格式，来实例化一个新的对象，除了<see cref="P:HslCommunication.Core.IByteTransform.DataFormat" />不同，其他都相同<br />
        /// According to the specified <see cref="P:HslCommunication.Core.IByteTransform.DataFormat" /> format, to instantiate a new object, except that <see cref="P:HslCommunication.Core.IByteTransform.DataFormat" /> is different, everything else is the same
        /// </summary>
        /// <param name="dataFormat">数据格式</param>
        /// <returns>新的<see cref="T:HslCommunication.Core.IByteTransform" />对象</returns>
        // Token: 0x06002322 RID: 8994
        IByteTransform CreateByDateFormat(DataFormat dataFormat);
    }
}
