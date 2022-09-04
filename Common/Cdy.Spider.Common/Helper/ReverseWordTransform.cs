using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdy.Spider.Common
{
    /// <summary>
    /// 按照字节错位的数据转换类
    /// </summary>
    public class ReverseWordTransform : ByteTransformBase
    {
        public bool IsInteger16Reverse { get; set; }

        #region Constructor

        /// <summary>
        /// 实例化一个默认的对象
        /// </summary>
        public ReverseWordTransform( )
        {
            base.DataFormat = DataFormat.ABCD;
            this.IsInteger16Reverse = true;
        }

        /// <summary>
        /// 使用指定的数据解析来实例化对象
        /// </summary>
        /// <param name="dataFormat">数据规则</param>
        public ReverseWordTransform( DataFormat dataFormat ) : base( dataFormat )
        {
            this.IsInteger16Reverse = true;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// 按照字节错位的方法
        /// </summary>
        /// <param name="buffer">实际的字节数据</param>
        /// <param name="index">起始字节位置</param>
        /// <param name="length">数据长度</param>
        /// <returns>处理过的数据信息</returns>
        private byte[] ReverseBytesByWord( byte[] buffer, int index, int length )
        {
            if (buffer == null) return null;

            // copy data
            byte[] tmp = new byte[length];
            for (int i = 0; i < length; i++)
            {
                tmp[i] = buffer[index + i];
            }

            // change
            for (int i = 0; i < length / 2; i++)
            {
                byte b = tmp[i * 2 + 0];
                tmp[i * 2 + 0] = tmp[i * 2 + 1];
                tmp[i * 2 + 1] = b;
            }

            return tmp;
        }

        private byte[] ReverseBytesByWord( byte[] buffer )
        {
            return ReverseBytesByWord( buffer, 0, buffer.Length );
        }
        


        #endregion

        #region Public Properties
        



        #endregion

        #region Get Value From Bytes


        /// <summary>
        /// 从缓存中提取short结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>short对象</returns>
        public override short TransInt16( byte[] buffer, int index )
        {
            bool isInteger16Reverse = this.IsInteger16Reverse;
            short result;
            if (isInteger16Reverse)
            {
                result = BitConverter.ToInt16(new byte[]
                {
                    buffer[index + 1],
                    buffer[index]
                }, 0);
            }
            else
            {
                result = base.TransInt16(buffer, index);
            }
            return result;
        }



        /// <summary>
        /// 从缓存中提取ushort结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ushort对象</returns>
        public override ushort TransUInt16( byte[] buffer, int index )
        {
            bool isInteger16Reverse = this.IsInteger16Reverse;
            ushort result;
            if (isInteger16Reverse)
            {
                result = BitConverter.ToUInt16(new byte[]
                {
                    buffer[index + 1],
                    buffer[index]
                }, 0);
            }
            else
            {
                result = base.TransUInt16(buffer, index);
            }
            return result;
        }

        #endregion

        #region Get Bytes From Value
        

        /// <summary>
        /// short数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( short[] values )
        {
            byte[] buffer = base.TransByte( values );
            return this.IsInteger16Reverse ? DataExtend.BytesReverseByWord(buffer) : buffer;
        }


        /// <summary>
        /// ushort数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( ushort[] values )
        {
            byte[] array = base.TransByte(values);
            return this.IsInteger16Reverse ? DataExtend.BytesReverseByWord(array) : array;
        }
        



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataFormat"></param>
        /// <returns></returns>
        public override IByteTransform CreateByDateFormat(DataFormat dataFormat)
        {
            return new ReverseWordTransform(dataFormat)
            {
                IsStringReverseByteWord = base.IsStringReverseByteWord,
                IsInteger16Reverse = this.IsInteger16Reverse
            };
        }

        #endregion

    }
}
