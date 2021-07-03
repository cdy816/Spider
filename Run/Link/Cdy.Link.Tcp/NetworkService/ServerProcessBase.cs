using Cheetah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cdy.Link.Tcp
{
    public abstract class ServerProcessBase : IDisposable
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, Queue<ByteBuffer>> mDatasCach = new Dictionary<string, Queue<ByteBuffer>>();

        private Thread mProcessThread;

        private ManualResetEvent resetEvent;

        private bool mIsClosed = false;

        private List<string> mClients = new List<string>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public abstract byte FunId { get; }

        /// <summary>
        /// 
        /// </summary>
        public DataServerBase Parent { get; set; }



        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected ByteBuffer ToByteBuffer(byte id, string value)
        {
            var re = Parent.Allocate(id, value.Length * 2 + 4);
            re.Write(value);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected ByteBuffer ToByteBuffer(byte id, byte value)
        {
            var re = Parent.Allocate(id, 1);
            re.Write(value);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected ByteBuffer ToByteBuffer(byte id, long value)
        {
            var re = Parent.Allocate(id, 8);
            re.Write(value);
            return re;
        }


        protected ByteBuffer ToByteBuffer(byte id,byte sid, long value)
        {
            var re = Parent.Allocate(id, 9);
            re.Write(sid);
            re.Write(value);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        protected ByteBuffer ToByteBuffer(byte id, byte value, byte value2)
        {
            var re = Parent.Allocate(id, 2);
            re.Write(value);
            re.Write(value2);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public virtual void ProcessData(string client, ByteBuffer data)
        {
            data.IncRef();
            if (mDatasCach.ContainsKey(client))
            {
                mDatasCach[client].Enqueue(data);
            }
            else
            {
                var vq = new Queue<ByteBuffer>();
                vq.Enqueue(data);

                lock (mDatasCach)
                    mDatasCach.Add(client, vq);
                lock (mClients)
                    mClients.Add(client);
            }
            CheckDataBusy(client);
            resetEvent.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void CheckDataBusy(string client)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(string client)
        {
            lock (mClients)
            {
                if (mClients.Contains(client))
                {
                    mClients.Remove(client);
                }
            }

            lock (mDatasCach)
            {
                if (mDatasCach.ContainsKey(client))
                {
                    mDatasCach.Remove(client);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DataProcess()
        {
            string sname = "";
            Queue<ByteBuffer> datas = null;
            while (!mIsClosed)
            {
                resetEvent.WaitOne();
                if (mIsClosed) return;
                resetEvent.Reset();

                for (int i = 0; i < mClients.Count; i++)
                {
                    sname = "";
                    datas = null;
                    lock (mClients)
                    {
                        if (i < mClients.Count)
                        {
                            sname = mClients[i];
                        }
                    }

                    if (!string.IsNullOrEmpty(sname))
                    {
                        lock (mDatasCach)
                        {
                            if (mDatasCach.ContainsKey(sname))
                            {
                                datas = mDatasCach[sname];
                            }
                        }
                        if (datas != null)
                        {
                            //Stopwatch sw = new Stopwatch();
                            //sw.Start();
                            //Debug.Print("开始实时数据请求:" + FunId +"  " + datas.Count);
                            while (datas.Count > 0)
                            {
                                ProcessSingleData(sname, datas.Dequeue());
                            }
                            //sw.Stop();
                            //Debug.Print("结束实时数据请求:" + sw.ElapsedMilliseconds);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        protected virtual void ProcessSingleData(string client, ByteBuffer data)
        {
            data.UnlockAndReturn();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {
            resetEvent = new ManualResetEvent(false);
            mProcessThread = new Thread(DataProcess);
            mProcessThread.IsBackground = true;
            mProcessThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stop()
        {
            mIsClosed = true;
            resetEvent.Set();
            resetEvent.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            Parent = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public virtual void OnClientConnected(string id)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public virtual void OnClientDisconnected(string id)
        {
            RemoveClient(id);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
