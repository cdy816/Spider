using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.TcpClient
{
    public class TcpClientChannel : ChannelBase
    {
        #region ... Variables  ...
        


        private System.Net.Sockets.Socket mClient;

        //System.Net.Sockets.NetworkStream mStream;

        private TcpClientChannelData mData;

        private Queue<byte[]> mReceiveBuffers = new Queue<byte[]>();

        private int mReceiveDataLen = 0;

        private object mLockObj = new object();

        private bool mForSyncCall = false;

        private Thread mReceiveThread;

        private bool mIsClosed = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "TcpClient";

        /// <summary>
        /// 
        /// </summary>
        public override string RemoteDescription => mData.ServerIp+":"+mData.Port;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            try
            {
                mIsClosed = false;
                mClient = new System.Net.Sockets.Socket( System.Net.Sockets.AddressFamily.InterNetwork,System.Net.Sockets.SocketType.Stream ,System.Net.Sockets.ProtocolType.Tcp);
                mClient.Connect(System.Net.IPAddress.Parse(mData.ServerIp), mData.Port);
                mIsConnected = true;
                mReceiveThread = new Thread(ThreadPro);
                mReceiveThread.IsBackground = true;
                mReceiveThread.Start();

            }
            catch
            {
                Task.Run(() => {
                    
                    TryConnect();
                });
            }
            return base.InnerOpen();
        }

        /// <summary>
        /// 
        /// </summary>
        private void TryConnect()
        {
            while(true)
            {
                Thread.Sleep(mData.ReTryDuration);
                try
                {
                    if (mClient.Connected) break;
                    mClient = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                    mClient.Connect(System.Net.IPAddress.Parse(mData.ServerIp), mData.Port);
                    if (mClient.Connected) break;
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ThreadPro()
        {
            while (!mIsClosed)
            {
                if (mClient != null && IsOnline(mClient) && mClient.Available > 0 && !mIsTransparentRead)
                {

                    var vdlen = mClient.Available;
                    byte[] btmp = new byte[vdlen];
                    mClient.Receive(btmp, 0, btmp.Length, System.Net.Sockets.SocketFlags.None);
                    if (mForSyncCall)
                    {
                        lock (mLockObj)
                        {
                            mReceiveDataLen += btmp.Length;
                            mReceiveBuffers.Enqueue(btmp);
                        }
                    }
                    else
                    {
                        OnReceiveCallBack("", btmp);
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private byte[] CopyReceiveBufferData(int count)
        {
            byte[] btmp = new byte[count];
            int cc = 0;
            int offset = 0;
            int removecount = 0;
            byte[] vdata;
            while (cc < count)
            {
                lock (mLockObj)
                    vdata = mReceiveBuffers.Dequeue();
                cc += vdata.Length;
                if (cc <= count)
                {
                    vdata.CopyTo(btmp, offset);
                }
                else
                {
                    Array.Copy(vdata, 0, btmp, offset, vdata.Length - (cc - count));
                }
                offset += vdata.Length;
                removecount += vdata.Length;
            }
            lock (mLockObj)
            {
                mReceiveDataLen -= removecount;
                mReceiveDataLen = mReceiveDataLen < 0 ? 0 : mReceiveDataLen;
            }
            return btmp;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearBuffer()
        {
            lock (mLockObj)
            {
                mReceiveBuffers.Clear();
                mReceiveDataLen = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsOnline(System.Net.Sockets.Socket c)
        {
            return !((c.Poll(1000, System.Net.Sockets.SelectMode.SelectRead) && (c.Available == 0)) || !c.Connected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeount"></param>
        /// <param name="waitResultCount"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendInner(Span<byte> data, int timeout, int waitResultCount, out bool result)
        {
            if(mClient!=null && IsOnline(mClient))
            {
                byte[] bval;
                mForSyncCall = true;
                ClearBuffer();
                mClient.Send(data);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Thread.Sleep(timeout / 10);
                while (mReceiveDataLen < waitResultCount)
                {
                    Thread.Sleep(timeout / 10);
                    if (sw.ElapsedMilliseconds > timeout)
                    {
                        break;
                    }
                }
                sw.Stop();

                if (mReceiveDataLen < waitResultCount)
                {
                    bval = CopyReceiveBufferData(mReceiveDataLen);
                    result = false;
                }
                else
                {
                    bval = CopyReceiveBufferData(waitResultCount);
                    result = true;
                }
                return bval;
            }
            return base.SendInner(data, timeout, waitResultCount, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeount"></param>
        /// <param name="waitPackageStartByte"></param>
        /// <param name="waitPackageEndByte"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendInner(Span<byte> data, int timeout, byte waitPackageStartByte, byte waitPackageEndByte, out bool result)
        {
            if (mClient != null && IsOnline(mClient))
            {
                List<byte> bval = new List<byte>(1024);

                byte[] btmp;

                mForSyncCall = true;
                ClearBuffer();

                mClient.Send(data);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                bool isstartfit = false;
                bool isendfit = false;

                Thread.Sleep(timeout / 10);

                while (true)
                {
                    var vdatalen = mReceiveDataLen;
                    if (vdatalen > 0)
                    {
                        btmp = CopyReceiveBufferData(vdatalen);
                        for (int i = 0; i < vdatalen; i++)
                        {
                            if (!isstartfit)
                            {
                                if (btmp[i] == waitPackageStartByte)
                                {
                                    isstartfit = true;
                                    bval.Add(btmp[i]);
                                }
                            }
                            else
                            {
                                bval.Add(btmp[i]);
                                if (btmp[i] == waitPackageEndByte)
                                {
                                    isendfit = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (isstartfit && isendfit) break;
                    Thread.Sleep(timeout / 10);
                    if (sw.ElapsedMilliseconds > timeout)
                    {
                        break;
                    }
                }
                sw.Stop();
                result = isstartfit && isstartfit;
                return bval.ToArray();
            }
            return base.SendInner(data, timeout, waitPackageStartByte, waitPackageEndByte, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendInner(Span<byte> data, int timeout, out bool result)
        {
            if (mClient != null && IsOnline(mClient))
            {
                mForSyncCall = true;
                ClearBuffer();

                mClient.Send(data);

                Thread.Sleep(timeout + 100);

                var bval = CopyReceiveBufferData(mReceiveDataLen);

                result = true;
                return bval;
            }
            return base.SendInner(data, timeout, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool SendInnerAsync(Span<byte> data)
        {
            if (mClient != null && IsOnline(mClient))
            {
                return mClient.Send(data)>0;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override byte[] Receive(int count, int timeout,out int receivecount)
        {
            
            byte[] bval = null;

            if (mClient != null && IsOnline(mClient))
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Thread.Sleep(timeout / 10);

                while (mReceiveDataLen != count)
                {
                    Thread.Sleep(timeout / 10);
                    if (sw.ElapsedMilliseconds > timeout)
                    {
                        break;
                    }
                }
                sw.Stop();

                if (mReceiveDataLen < count)
                {
                    receivecount = mReceiveDataLen;
                    bval = CopyReceiveBufferData(receivecount);
                }
                else
                {
                    receivecount = count;
                    bval = CopyReceiveBufferData(count);
                }
            }
            else
            {
                receivecount = 0;
            }

            return bval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public override byte[] Receive(int count)
        {
            if (mClient != null && IsOnline(mClient))
            {
                var bval = CopyReceiveBufferData(count);
                return bval;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int len)
        {
            return mClient.Receive(buffer, offset, len, SocketFlags.None);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Flush()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerClose()
        {
            try
            {
                mIsClosed = true;
                if (mClient != null)
                {
                    mClient.Close();
                    mClient = null;
                }
                mIsConnected = false;
            }
            catch
            {

            }
            return base.InnerClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel NewApi()
        {
            return new TcpClientChannel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new TcpClientChannelData();
            mData.LoadFromXML(xe);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
