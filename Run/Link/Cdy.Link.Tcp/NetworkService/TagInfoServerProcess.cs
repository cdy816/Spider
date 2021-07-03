using Cheetah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Link.Tcp
{
    public class TagInfoServerProcess : ServerProcessBase
    {

        #region ... Variables  ...


        public const byte Login = 1;


        public const byte Hart = 2;



        private List<string> mClients = new List<string>();

       

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        public override byte FunId => APIConst.TagInfoRequestFun;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        protected unsafe override void ProcessSingleData(string client, ByteBuffer data)
        {
           
            byte sfun = data.ReadByte();
            switch (sfun)
            {
                case Login:
                    try
                    {
                        string user = data.ReadString();
                        string pass = data.ReadString();
                        long result = SecurityService.Service.Login(user,pass);

                        if (result > 0)
                        {
                            lock (mClients)
                                mClients.Add(client);
                        }
                        Console.WriteLine("客户端 "+client+" 登录成功!");
                        Parent.AsyncCallback(client, ToByteBuffer(APIConst.TagInfoRequestFun, Login, result));
                    }
                    catch (Exception eex)
                    {
                        //LoggerService.Service.Erro("SpiderDriver", eex.Message);
                    }

                    break;
                case Hart:
                    long id = data.ReadLong();
                    SecurityService.Service.RefreshId(id);
                    break;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public override void OnClientDisconnected(string id)
        {
            lock (mClients)
            {
                if (mClients.Contains(id))
                {
                    mClients.Remove(id);
                }
            }
            base.OnClientDisconnected(id);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
