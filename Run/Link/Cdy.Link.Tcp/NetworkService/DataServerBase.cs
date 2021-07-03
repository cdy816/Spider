using Cheetah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Link.Tcp
{
    public class DataServerBase:TcpServer
    {
        #region ... Variables  ...
        private TagInfoServerProcess mInfoProcess;
        private RealDataServerProcess mRealProcess;

        private bool mIsStarted = false;

        //private IByteBuffer mAsyncCalldata;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public DataServerBase()
        {
            RegistorInit();

        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public TcpRuntime Owner { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        private void RegistorInit()
        {
            this.RegistorFunCallBack(APIConst.TagInfoRequestFun, TagInfoRequest);
            this.RegistorFunCallBack(APIConst.RealValueFun, RealDataRequest);
        }


        public override void OnClientConnected(string id, bool isConnected)
        {
            if (isConnected)
            {
                mRealProcess?.OnClientConnected(id);
                mInfoProcess?.OnClientConnected(id);
            }
            else
            {
                mRealProcess?.OnClientDisconnected(id);
                mInfoProcess?.OnClientDisconnected(id);
            }
            base.OnClientConnected(id, isConnected);
        }



        /// <summary>
        /// 
        /// </summary>
        public void PushRealDatatoClient(string clientId, byte[] value)
        {
            this.SendData(clientId, APIConst.PushDataChangedFun, value, value.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="value"></param>
        public void PushRealDatatoClient(string clientId, ByteBuffer value)
        {
            this.SendDataToClient(clientId, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="fun"></param>
        /// <param name="value"></param>
        public void AsyncCallback(string clientId, byte fun, byte[] value, int len)
        {
            this.SendData(clientId, fun, value, len);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="data"></param>
        public void AsyncCallback(string clientId, ByteBuffer data)
        {
            this.SendDataToClient(clientId, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="fun"></param>
        /// <param name="value"></param>
        /// <param name="len"></param>
        public void AsyncCallback(string clientId, byte fun, IntPtr value, int len)
        {
            this.SendData(clientId, fun, value, len);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private IByteBuffer GetAsyncData()
        //{
        //    mAsyncCalldata = BufferManager.Manager.Allocate(APIConst.AysncReturn, 4);
        //    mAsyncCalldata.WriteInt(0);
        //    return mAsyncCalldata;
        //}

        /// <summary>
        /// 
        /// </summary>
        private ByteBuffer TagInfoRequest(string clientId, ByteBuffer memory)
        {
            if (mIsStarted && mInfoProcess != null)
            {
                mInfoProcess.ProcessData(clientId, memory);
            }
            else
            {
               // LoggerService.Service.Info("Spider Driver", "Spider driver is not started!");
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="memory"></param>
        /// <returns></returns>
        private ByteBuffer RealDataRequest(string clientId, ByteBuffer memory)
        {
            if (mIsStarted && mRealProcess != null)
            {
                this.mRealProcess.ProcessData(clientId, memory);
            }
            else
            {
               // LoggerService.Service.Info("Spider Driver", "Spider driver is not started!");
            }
            return null;
        }

        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        public override void Start(int port)
        {
            try
            {
                mRealProcess = new RealDataServerProcess() { Parent = this,Service = Owner };
                mRealProcess.Init();
                mInfoProcess = new TagInfoServerProcess() { Parent = this };
                mRealProcess.Start();
                mInfoProcess.Start();
                mIsStarted = true;
            }
            catch (Exception ex)
            {
               // LoggerService.Service.Erro("Spider Driver", ex.Message);
            }
            base.Start(port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        public void WriteDeviceValue(string device, string tagid, byte tagtype, object value)
        {
            mRealProcess.WriteDeviceValue(device, tagid, tagtype, value);
        }
        

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            if (mRealProcess != null)
            {
                mRealProcess.Stop();
                mRealProcess.Dispose();
                mRealProcess = null;
            }
            if (mInfoProcess != null)
            {
                mInfoProcess.Stop();
                mInfoProcess.Dispose();
                mInfoProcess = null;
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
