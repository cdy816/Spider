using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 基于MC协议的ASCII格式的辅助类
    /// </summary>
    // Token: 0x020000B7 RID: 183
    public class McAsciiHelper
    {
        /// <summary>
        /// 将MC协议的核心报文打包成一个可以直接对PLC进行发送的原始报文
        /// </summary>
        /// <param name="mcCore">MC协议的核心报文</param>
        /// <param name="networkNumber">网络号</param>
        /// <param name="networkStationNumber">网络站号</param>
        /// <returns>原始报文信息</returns>
        public static byte[] PackMcCommand(byte[] mcCore, byte networkNumber = 0, byte networkStationNumber = 0)
        {
            byte[] array = new byte[22 + mcCore.Length];
            array[0] = 53;
            array[1] = 48;
            array[2] = 48;
            array[3] = 48;
            array[4] = DataExtend.BuildAsciiBytesFrom(networkNumber)[0];
            array[5] = DataExtend.BuildAsciiBytesFrom(networkNumber)[1];
            array[6] = 70;
            array[7] = 70;
            array[8] = 48;
            array[9] = 51;
            array[10] = 70;
            array[11] = 70;
            array[12] = DataExtend.BuildAsciiBytesFrom(networkStationNumber)[0];
            array[13] = DataExtend.BuildAsciiBytesFrom(networkStationNumber)[1];
            array[14] = DataExtend.BuildAsciiBytesFrom((ushort)(array.Length - 18))[0];
            array[15] = DataExtend.BuildAsciiBytesFrom((ushort)(array.Length - 18))[1];
            array[16] = DataExtend.BuildAsciiBytesFrom((ushort)(array.Length - 18))[2];
            array[17] = DataExtend.BuildAsciiBytesFrom((ushort)(array.Length - 18))[3];
            array[18] = 48;
            array[19] = 48;
            array[20] = 49;
            array[21] = 48;
            mcCore.CopyTo(array, 22);
            return array;
        }

        /// <summary>
        /// 从PLC反馈的数据中提取出实际的数据内容，需要传入反馈数据，是否位读取
        /// </summary>
        /// <param name="response">反馈的数据内容</param>
        /// <param name="isBit">是否位读取</param>
        /// <returns>解析后的结果对象</returns>
        // Token: 0x06000DFC RID: 3580 RVA: 0x00058404 File Offset: 0x00056604
        public static byte[] ExtractActualDataHelper(byte[] response, bool isBit)
        {
            byte[] result;
            if (isBit)
            {
                result = (from m in response select m == 48 ? (byte)0 : (byte)1).ToArray();
            }
            else
            {
                result = MelsecHelper.TransAsciiByteArrayToByteArray(response);
            }
            return result;
        }

        /// <summary>
        /// 检查反馈的内容是否正确的
        /// </summary>
        /// <param name="content">MC的反馈的内容</param>
        /// <returns>是否正确</returns>
        public static bool CheckResponseContent(byte[] content)
        {
            ushort num = Convert.ToUInt16(Encoding.ASCII.GetString(content, 18, 4), 16);
            if (num > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 从三菱地址，是否位读取进行创建读取Ascii格式的MC的核心报文
        /// </summary>
        /// <param name="addressData">三菱Mc协议的数据地址</param>
        /// <param name="isBit">是否进行了位读取操作</param>
        /// <returns>带有成功标识的报文对象</returns>
        public static byte[] BuildAsciiReadMcCoreCommand(McAddressData addressData, bool isBit)
        {
            return new byte[]
            {
                48,
                52,
                48,
                49,
                48,
                48,
                48,
                 isBit ? (byte)49 : (byte)48,
                Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[0],
                Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[1],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[0],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[1],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[2],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[3],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[4],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[5],
                DataExtend.BuildAsciiBytesFrom(addressData.Length)[0],
                DataExtend.BuildAsciiBytesFrom(addressData.Length)[1],
                DataExtend.BuildAsciiBytesFrom(addressData.Length)[2],
                DataExtend.BuildAsciiBytesFrom(addressData.Length)[3]
            };
        }

        /// <summary>
        /// 从三菱扩展地址，是否位读取进行创建读取的MC的核心报文
        /// </summary>
        /// <param name="isBit">是否进行了位读取操作</param>
        /// <param name="extend">扩展指定</param>
        /// <param name="addressData">三菱Mc协议的数据地址</param>
        /// <returns>带有成功标识的报文对象</returns>
        public static byte[] BuildAsciiReadMcCoreExtendCommand(McAddressData addressData, ushort extend, bool isBit)
        {
            return new byte[]
            {
                48,
                52,
                48,
                49,
                48,
                48,
                56,
                isBit ? (byte)49 : (byte)48,
                48,
                48,
                74,
                DataExtend.BuildAsciiBytesFrom(extend)[1],
                DataExtend.BuildAsciiBytesFrom(extend)[2],
                DataExtend.BuildAsciiBytesFrom(extend)[3],
                48,
                48,
                48,
                Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[0],
                Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[1],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[0],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[1],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[2],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[3],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[4],
                MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[5],
                48,
                48,
                48,
                DataExtend.BuildAsciiBytesFrom(addressData.Length)[0],
                DataExtend.BuildAsciiBytesFrom(addressData.Length)[1],
                DataExtend.BuildAsciiBytesFrom(addressData.Length)[2],
                DataExtend.BuildAsciiBytesFrom(addressData.Length)[3]
            };
        }

        /// <summary>
        /// 以字为单位，创建ASCII数据写入的核心报文
        /// </summary>
        /// <param name="addressData">三菱Mc协议的数据地址</param>
        /// <param name="value">实际的原始数据信息</param>
        /// <returns>带有成功标识的报文对象</returns>
        public static byte[] BuildAsciiWriteWordCoreCommand(McAddressData addressData, byte[] value)
        {
            value = MelsecHelper.TransByteArrayToAsciiByteArray(value);
            byte[] array = new byte[20 + value.Length];
            array[0] = 49;
            array[1] = 52;
            array[2] = 48;
            array[3] = 49;
            array[4] = 48;
            array[5] = 48;
            array[6] = 48;
            array[7] = 48;
            array[8] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[0];
            array[9] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[1];
            array[10] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[0];
            array[11] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[1];
            array[12] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[2];
            array[13] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[3];
            array[14] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[4];
            array[15] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[5];
            array[16] = DataExtend.BuildAsciiBytesFrom((ushort)(value.Length / 4))[0];
            array[17] = DataExtend.BuildAsciiBytesFrom((ushort)(value.Length / 4))[1];
            array[18] = DataExtend.BuildAsciiBytesFrom((ushort)(value.Length / 4))[2];
            array[19] = DataExtend.BuildAsciiBytesFrom((ushort)(value.Length / 4))[3];
            value.CopyTo(array, 20);
            return array;
        }

        /// <summary>
        /// 以位为单位，创建ASCII数据写入的核心报文
        /// </summary>
        /// <param name="addressData">三菱Mc协议的数据地址</param>
        /// <param name="value">原始的bool数组数据</param>
        /// <returns>带有成功标识的报文对象</returns>
        public static byte[] BuildAsciiWriteBitCoreCommand(McAddressData addressData, bool[] value)
        {
            bool flag = value == null;
            if (flag)
            {
                value = new bool[0];
            }
            byte[] array = (from m in value select m ? (byte)49 : (byte)48).ToArray();
            byte[] array2 = new byte[20 + array.Length];
            array2[0] = 49;
            array2[1] = 52;
            array2[2] = 48;
            array2[3] = 49;
            array2[4] = 48;
            array2[5] = 48;
            array2[6] = 48;
            array2[7] = 49;
            array2[8] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[0];
            array2[9] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[1];
            array2[10] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[0];
            array2[11] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[1];
            array2[12] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[2];
            array2[13] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[3];
            array2[14] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[4];
            array2[15] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[5];
            array2[16] = DataExtend.BuildAsciiBytesFrom((ushort)value.Length)[0];
            array2[17] = DataExtend.BuildAsciiBytesFrom((ushort)value.Length)[1];
            array2[18] = DataExtend.BuildAsciiBytesFrom((ushort)value.Length)[2];
            array2[19] = DataExtend.BuildAsciiBytesFrom((ushort)value.Length)[3];
            array.CopyTo(array2, 20);
            return array2;
        }

        /// <summary>
        /// 按字为单位随机读取的指令创建
        /// </summary>
        /// <param name="address">地址数组</param>
        /// <returns>指令</returns>
        public static byte[] BuildAsciiReadRandomWordCommand(McAddressData[] address)
        {
            byte[] array = new byte[12 + address.Length * 8];
            array[0] = 48;
            array[1] = 52;
            array[2] = 48;
            array[3] = 51;
            array[4] = 48;
            array[5] = 48;
            array[6] = 48;
            array[7] = 48;
            array[8] = DataExtend.BuildAsciiBytesFrom((byte)address.Length)[0];
            array[9] = DataExtend.BuildAsciiBytesFrom((byte)address.Length)[1];
            array[10] = 48;
            array[11] = 48;
            for (int i = 0; i < address.Length; i++)
            {
                array[i * 8 + 12] = Encoding.ASCII.GetBytes(address[i].McDataType.AsciiCode)[0];
                array[i * 8 + 13] = Encoding.ASCII.GetBytes(address[i].McDataType.AsciiCode)[1];
                array[i * 8 + 14] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[0];
                array[i * 8 + 15] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[1];
                array[i * 8 + 16] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[2];
                array[i * 8 + 17] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[3];
                array[i * 8 + 18] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[4];
                array[i * 8 + 19] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[5];
            }
            return array;
        }

        /// <summary>
        /// 随机读取的指令创建
        /// </summary>
        /// <param name="address">地址数组</param>
        /// <returns>指令</returns>
        public static byte[] BuildAsciiReadRandomCommand(McAddressData[] address)
        {
            byte[] array = new byte[12 + address.Length * 12];
            array[0] = 48;
            array[1] = 52;
            array[2] = 48;
            array[3] = 54;
            array[4] = 48;
            array[5] = 48;
            array[6] = 48;
            array[7] = 48;
            array[8] = DataExtend.BuildAsciiBytesFrom((byte)address.Length)[0];
            array[9] = DataExtend.BuildAsciiBytesFrom((byte)address.Length)[1];
            array[10] = 48;
            array[11] = 48;
            for (int i = 0; i < address.Length; i++)
            {
                array[i * 12 + 12] = Encoding.ASCII.GetBytes(address[i].McDataType.AsciiCode)[0];
                array[i * 12 + 13] = Encoding.ASCII.GetBytes(address[i].McDataType.AsciiCode)[1];
                array[i * 12 + 14] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[0];
                array[i * 12 + 15] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[1];
                array[i * 12 + 16] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[2];
                array[i * 12 + 17] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[3];
                array[i * 12 + 18] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[4];
                array[i * 12 + 19] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[5];
                array[i * 12 + 20] = DataExtend.BuildAsciiBytesFrom(address[i].Length)[0];
                array[i * 12 + 21] = DataExtend.BuildAsciiBytesFrom(address[i].Length)[1];
                array[i * 12 + 22] = DataExtend.BuildAsciiBytesFrom(address[i].Length)[2];
                array[i * 12 + 23] = DataExtend.BuildAsciiBytesFrom(address[i].Length)[3];
            }
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] BuildAsciiReadMemoryCommand(string address, ushort length)
        {
            byte[] result = null;
            try
            {
                uint value = uint.Parse(address);
                byte[] array = new byte[20];
                array[0] = 48;
                array[1] = 54;
                array[2] = 49;
                array[3] = 51;
                array[4] = 48;
                array[5] = 48;
                array[6] = 48;
                array[7] = 48;
                DataExtend.BuildAsciiBytesFrom(value).CopyTo(array, 8);
                DataExtend.BuildAsciiBytesFrom(length).CopyTo(array, 16);
                result = array;
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] BuildAsciiReadSmartModule(ushort module, string address, ushort length)
        {
            byte[] result = null;
            try
            {
                uint value = uint.Parse(address);
                byte[] array = new byte[24];
                array[0] = 48;
                array[1] = 54;
                array[2] = 48;
                array[3] = 49;
                array[4] = 48;
                array[5] = 48;
                array[6] = 48;
                array[7] = 48;
                DataExtend.BuildAsciiBytesFrom(value).CopyTo(array, 8);
                DataExtend.BuildAsciiBytesFrom(length).CopyTo(array, 16);
                DataExtend.BuildAsciiBytesFrom(module).CopyTo(array, 20);
                result = array;
            }
            catch (Exception)
            {

            }
            return result;
        }
    }
}
