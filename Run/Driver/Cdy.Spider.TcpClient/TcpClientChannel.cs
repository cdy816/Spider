using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.TcpClient
{
    public class TcpClientChannel : ChannelBase2
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

        private bool mIsConnecting = false;

        private byte[] mRelayData;

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
        public override ChannelData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string RemoteDescription => mData.ServerIp+":"+mData.Port;

        /// <summary>
        /// 
        /// </summary>
        public EndPoint LocalEndPoint
        {
            get
            {
                return mClient.LocalEndPoint;
            }
        }

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
                mClient.SendTimeout = mData.DataSendTimeout;
                mClient.ReceiveTimeout = mData.Timeout;
                mClient.NoDelay = true;
                
                //mIsConnected = true;
                mReceiveThread = new Thread(ThreadPro);
                mReceiveThread.IsBackground = true;
                mReceiveThread.Start();

                ConnectedChanged(true);
            }
            catch
            {
                StartConnect();
            }
            return base.InnerOpen();
        }

        private void StartConnect()
        {
            if (!mIsConnecting)
            {
                Task.Run(() =>
                {
                    TryConnect();
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void TryConnect()
        {
            while(true)
            {
                try
                {
                    mIsConnecting = true;
                    Thread.Sleep(mData.ReTryDuration);
                    try
                    {
                        try
                        {
                            if (mClient != null)
                            {
                                mClient.Close();
                                mClient = null;
                            }
                        }
                        catch
                        {

                        }
                        //if (mClient.Connected)
                        //{
                        //    break;
                        //}
                        mClient = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                        mClient.Connect(System.Net.IPAddress.Parse(mData.ServerIp), mData.Port);

                        mIsConnected = mClient.Connected;
                        if (mClient.Connected)
                        {
                            ConnectedChanged(mIsConnected);
                            LoggerService.Service.Info("TcpClient", $"Connect {mData.ServerIp}:{mData.Port} successful.");
                            break;
                        }
                        //else
                        //{
                        //    LoggerService.Service.Info("TcpClient", $"Connect {mData.ServerIp}:{mData.Port} failed.");
                        //}
                       
                    }
                    catch
                    {

                    }
                }
                catch
                {

                }
            }
            mIsConnecting = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ThreadPro()
        {
            while (!mIsClosed)
            {
                if (mClient != null && IsOnline(mClient) && mClient.Available > 0 && !mEnableSyncRead)
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
                else
                {
                    //if (mClient != null && !IsOnline(mClient))
                    if(mClient!=null && !mClient.Connected)
                    {
                        StartConnect();
                        Thread.Sleep(mData.ReTryDuration);
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
                {
                    if (mRelayData != null)
                    {
                        vdata = mRelayData;
                    }
                    else
                    {
                        vdata = mReceiveBuffers.Dequeue();
                    }
                }
                cc += vdata.Length;
                if (cc <= count)
                {
                    vdata.CopyTo(btmp, offset);
                    mRelayData = null;
                }
                else
                {
                    Array.Copy(vdata, 0, btmp, offset, vdata.Length - (cc - count));
                    int relaydatasize = (cc - count);
                    byte[] rd = new byte[relaydatasize];
                    Array.Copy(vdata, vdata.Length - relaydatasize, rd, rd.Length, relaydatasize);
                    mRelayData = rd;
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
        public override void ClearBuffer()
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
            return c.Connected;
            //return !((c.Poll(1000, System.Net.Sockets.SelectMode.SelectRead) && (c.Available == 0)) || !c.Connected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeount"></param>
        /// <param name="waitResultCount"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendAndWaitInner(Span<byte> data, int timeout, int waitResultCount, out bool result)
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
            else
            {
                StartConnect();
            }
            return base.SendAndWaitInner(data, timeout, waitResultCount, out result);
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
        protected override byte[] SendAndWaitInner(Span<byte> data, int timeout, byte waitPackageStartByte, byte waitPackageEndByte, out bool result)
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
            else
            {
                StartConnect();
            }
            return base.SendAndWaitInner(data, timeout, waitPackageStartByte, waitPackageEndByte, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendAndWaitInner(Span<byte> data, int timeout, out bool result)
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
            else
            {
                StartConnect();
            }
            return base.SendAndWaitInner(data, timeout, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool SendInner(Span<byte> data)
        {
            if (mClient != null && IsOnline(mClient))
            {
                if (data.Length > 0)
                {
                    return mClient.Send(data) > 0;
                }
                else
                {
                    mClient.Send(data);
                    return true;
                }
            }
            else
            {
                StartConnect();
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override byte[] Read(int count, int timeout,out int receivecount)
        {
            
            byte[] bval = null;

            if (mClient != null && IsOnline(mClient))
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                if (!mEnableSyncRead)
                {
                    Thread.Sleep(10);

                    while (mReceiveDataLen < count)
                    {
                        Thread.Sleep(10);
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
                  int tmp=0;
                    bval = new byte[count];
                    while (tmp < count)
                    {
                        if (mClient.Available > 0)
                        {
                            var rec = mClient.Receive(bval, tmp, Math.Min( count - tmp,mClient.Available), SocketFlags.None);
                            tmp += rec;
                        }
                        //else
                        //{
                        //    Thread.Sleep(10);
                        //}
                        if (sw.ElapsedMilliseconds>timeout)
                        {
                            break;
                        }
                        //Thread.Sleep(timeout / 10);
                    }
                    receivecount = tmp;
                    if(tmp < count)
                    {
                        bval = bval.AsSpan(tmp).ToArray();
                    }
                    sw.Stop();
                }
                //Console.WriteLine(sw.ElapsedMilliseconds);
            }
            else
            {
                StartConnect();
                receivecount = 0;
            }

            return bval;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="offset"></param>
        ///// <param name="len"></param>
        ///// <returns></returns>
        //public override int Read(byte[] buffer, int offset, int len)
        //{
        //    if (mClient != null && IsOnline(mClient))
        //    {
        //        return mClient.Receive(buffer, offset, len, SocketFlags.None);
        //    }
        //    else
        //    {
        //        StartConnect();
        //    }
        //    return 0;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int len, int timeout)
        {
            Stopwatch sw = new Stopwatch();
            int count = 0;
            //mClient.ReceiveTimeout = timeout;
            if (mClient != null && IsOnline(mClient))
            {
                sw.Start();
                if (!mEnableSyncRead)
                {
                    while (mReceiveDataLen < len)
                    {
                        Thread.Sleep( 10);
                        if (sw.ElapsedMilliseconds > timeout)
                        {
                            break;
                        }
                    }
                    byte[] bval;
                    if (mReceiveDataLen < count)
                    {
                        count = mReceiveDataLen;
                        bval = CopyReceiveBufferData(count);
                    }
                    else
                    {
                        count = len;
                        bval = CopyReceiveBufferData(count);
                    }

                    Array.Copy(bval,0,buffer,offset,count);

                }
                else
                {
                  
                    while (count < len)
                    {
                        if (mClient == null) break;
                        if (mClient.Available > 0)
                        {
                            count += mClient.Receive(buffer, offset + count, len - count, SocketFlags.None);
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                        if (sw.ElapsedMilliseconds > timeout)
                        {
                            break;
                        }

                    }
                    sw.Stop();
                }
                return count;
            }
            else
            {
                StartConnect();
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int AvaiableLenght()
        {
            if (!mEnableSyncRead)
            {
                int icount = 0;
                foreach (var vv in mReceiveBuffers)
                {
                    icount += vv.Length;
                }
                return icount;
            }
            else
            {
                return mClient.Available;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //public override byte[] Read(int count)
        //{
        //    if (mClient != null && IsOnline(mClient))
        //    {
        //        if (!mEnableSyncRead)
        //        {
        //            var bval = CopyReceiveBufferData(count);
        //            return bval;
        //        }
        //        else
        //        {
        //            int tmp = 0;
        //            var bval = new byte[count];
        //            while (tmp < count)
        //            {
        //                count += mClient.Receive(bval, count, count - tmp, SocketFlags.None);
        //                Thread.Sleep(1);
        //            }
        //            return bval;
        //        }
        //    }
        //    else
        //    {
        //        StartConnect();
        //        return null;
        //    }
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="offset"></param>
        ///// <param name="len"></param>
        ///// <returns></returns>
        //public override bool Write(byte[] buffer, int offset, int len)
        //{
        //    if (mClient != null && IsOnline(mClient))
        //    {
        //        var re = mClient.Send(buffer, offset, len, SocketFlags.None) > 0;

        //        return re;
        //    }
        //    else
        //    {
        //        StartConnect();
        //    }
        //    return false;
        //}

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
        public override ICommChannel2 NewApi()
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
