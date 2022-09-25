using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cdy.Spider.Common.Helper;

namespace Cdy.Spider.AllenBradleyDriver
{
    /// <summary>
    /// 自定义的消息路由类，可以实现CIP协议自定义的路由消息<br />
    /// A custom message routing class that can implement custom routing messages of the CIP protocol
    /// </summary>
    public class MessageRouter
    {
        /// <summary>
        /// 实例化一个默认的实例对象<br />
        /// instantiate a default instance object
        /// </summary>
        public MessageRouter()
        {
            this._router[0] = 1;
            new byte[]
            {
                15,
                2,
                18,
                1
            }.CopyTo(this._router, 1);
            this._router[5] = 12;
        }

        /// <summary>
        /// 指定路由来实例化一个对象，使用字符串的表示方式<br />
        /// Specify the route to instantiate an object, using the string representation
        /// </summary>
        /// <remarks>
        /// 路有消息支持两种格式，格式1：1.15.2.18.1.12   格式2： 1.1.2.130.133.139.61.1.0<br />
        /// There are two formats for the channel message, format 1: 1.15.2.18.1.12 format 2: 1.1.2.130.133.139.61.1.0
        /// </remarks>
        /// <param name="router">路由信息</param>
        public MessageRouter(string router)
        {
            string[] array = router.Split(new char[]
            {
                '.'
            }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length <= 6)
            {
                if (array.Length != 0)
                {
                    this._router[0] = byte.Parse(array[0]);
                }
                if (array.Length > 1)
                {
                    this._router[1] = byte.Parse(array[1]);
                }
                if (array.Length > 2)
                {
                    this._router[2] = byte.Parse(array[2]);
                }
                if (array.Length > 3)
                {
                    this._router[3] = byte.Parse(array[3]);
                }
                if (array.Length > 4)
                {
                    this._router[4] = byte.Parse(array[4]);
                }
                if (array.Length > 5)
                {
                    this._router[5] = byte.Parse(array[5]);
                }
            }
            else
            {
                if (array.Length == 9)
                {
                    string text = string.Concat(new string[]
                    {
                        array[3],
                        ".",
                        array[4],
                        ".",
                        array[5],
                        ".",
                        array[6]
                    });
                    this._router = new byte[6 + text.Length];
                    this._router[0] = byte.Parse(array[0]);
                    this._router[1] = byte.Parse(array[1]);
                    this._router[2] = (byte)(16 + byte.Parse(array[2]));
                    this._router[3] = (byte)text.Length;
                    Encoding.ASCII.GetBytes(text).CopyTo(this._router, 4);
                    this._router[this._router.Length - 2] = byte.Parse(array[7]);
                    this._router[this._router.Length - 1] = byte.Parse(array[8]);
                }
            }
        }

        /// <summary>
        /// 使用完全自定义的消息路由来初始化数据<br />
        /// Use fully custom message routing to initialize data
        /// </summary>
        /// <param name="router">完全自定义的路由消息</param>
        public MessageRouter(byte[] router)
        {
            this._router = router;
        }

        /// <summary>
        /// 获取路由信息
        /// </summary>
        /// <returns>路由消息的字节信息</returns>
        public byte[] GetRouter()
        {
            return this._router;
        }

        /// <summary>
        /// 获取用于发送的CIP路由报文信息<br />
        /// Get information about CIP routing packets for sending
        /// </summary>
        /// <returns>路由信息</returns>
        public byte[] GetRouterCIP()
        {
            byte[] array = this.GetRouter();
            bool flag = array.Length % 2 == 1;
            if (flag)
            {
                array = DataExtend.SpliceArray<byte>(new byte[][]
                {
                    array,
                    new byte[1]
                });
            }
            byte[] array2 = new byte[46 + array.Length];
            "54022006240105f70200 00800100fe8002001b05 28a7fd03020000008084 1e00f44380841e00f443 a305".ToHexBytes().CopyTo(array2, 0);
            array.CopyTo(array2, 42);
            "20022401".ToHexBytes().CopyTo(array2, 42 + array.Length);
            array2[41] = (byte)(array.Length / 2);
            return array2;
        }

        /// <summary>
        /// 背板信息
        /// </summary>
        public byte Backplane
        {
            get
            {
                return this._router[0];
            }
            set
            {
                this._router[0] = value;
            }
        }

        /// <summary>
        /// 槽号信息
        /// </summary>
        public byte Slot
        {
            get
            {
                return this._router[5];
            }
            set
            {
                this._router[5] = value;
            }
        }

        private byte[] _router = new byte[6];
    }
}
