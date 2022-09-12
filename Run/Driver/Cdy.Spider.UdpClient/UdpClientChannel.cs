using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace Cdy.Spider.UdpClient
{
    public class UdpClientChannel : ChannelBase2
    {

        #region ... Variables  ...

        private System.Net.Sockets.UdpClient mClient;

        private UdpClientChannelData mData;

        private bool mIsClosed = false;

        private Queue<byte[]> mReceiveBuffers = new Queue<byte[]>();

        private int mReceiveDataLen = 0;

        private object mLockObj = new object();

        private bool mForSyncCall = false;

        private Thread mReceiveThread;

        System.IO.MemoryStream mReceiveBuffer = new System.IO.MemoryStream();

        private System.Net.IPEndPoint rp;

        private byte[] mRelayData;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        public override ChannelData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "UdpClient";


        public override string RemoteDescription => mData.ServerIp + ":" + mData.Port;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            mIsClosed = false;

            rp = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(mData.ServerIp), mData.Port);

            mClient = new System.Net.Sockets.UdpClient();
            mClient.Connect(System.Net.IPAddress.Parse(mData.ServerIp), mData.Port);

            mClient.Client.SendTimeout = mData.DataSendTimeout;
            mClient.Client.ReceiveTimeout = mData.Timeout;

            mIsConnected = true;

            mReceiveThread = new Thread(ThreadPro);
            mReceiveThread.IsBackground = true;
            mReceiveThread.Start();

            return base.InnerOpen();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerClose()
        {
            mIsClosed = true;
            mReceiveDataLen = 0;
            mReceiveBuffers.Clear();
            if (mClient != null)
            {
                mClient.Close();
                mClient = null;
            }
            mIsConnected = false;
            return base.InnerClose();
        }

       

        /// <summary>
        /// 
        /// </summary>
        private void ThreadPro()
        {
            
            while (!mIsClosed)
            {
                if (mClient != null && mClient.Client.Available > 0 && !mEnableSyncRead)
                {
                    var vdata = mClient.Receive(ref rp);
                    if (mForSyncCall)
                    {
                        lock (mLockObj)
                        {
                            mReceiveDataLen += vdata.Length;
                            mReceiveBuffers.Enqueue(vdata);
                        }
                    }
                    else
                    {
                        OnReceiveCallBack("", vdata);
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
            lock(mLockObj)
            {
                mReceiveBuffers.Clear();
                mReceiveDataLen = 0;
            }
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
            if (mClient != null)
            {
                byte[] bval;
                mForSyncCall = true;
                ClearBuffer();
                mClient.Client.Send(data);
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
            if (mClient != null)
            {
                List<byte> bval = new List<byte>(1024);

                byte[] btmp;

                mForSyncCall = true;
                ClearBuffer();

                mClient.Client.Send(data);
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
            if (mClient != null)
            {
                mForSyncCall = true;
                ClearBuffer();

                mClient.Client.Send(data);

                Thread.Sleep(timeout + 100);

                var bval = CopyReceiveBufferData(mReceiveDataLen);

                result = true;
                return bval;
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
            if (mClient != null)
            {
                return mClient.Client.Send(data) > 0;
            }
            return base.SendInner(data);
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="timeout"></param>
        /// <param name="receivecount"></param>
        /// <returns></returns>
        public override byte[] Read(int count, int timeout, out int receivecount)
        {
            byte[] bval = null;
            if (mClient != null)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                if (!mEnableSyncRead)
                {
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
                    int tmp = 0;
                    bval = new byte[count];
                    while (tmp < count)
                    {
                        var vd = mClient.Receive(ref rp);
                        if (vd != null && vd.Length>0)
                        {
                            if (vd.Length + tmp <= count)
                            {
                                Array.Copy(vd, 0, bval, tmp, vd.Length);
                            }
                            else
                            {
                                Array.Copy(vd, 0, bval, tmp, count - tmp);
                            }

                            tmp += vd.Length;
                        }

                        if (sw.ElapsedMilliseconds > timeout)
                        {
                            break;
                        }
                        Thread.Sleep(timeout / 10);
                    }
                    receivecount = tmp;
                    if (tmp < count)
                    {
                        bval = bval.AsSpan(tmp).ToArray();
                    }
                }
            }
            else
            {
                receivecount = 0;
            }
            return bval;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //public override byte[] Read(int count)
        //{
        //    byte[] bval = null;
        //    if (mClient != null)
        //    {
        //        if (!mEnableSyncRead)
        //        {
        //            bval = CopyReceiveBufferData(count);
        //        }
        //        else
        //        {
        //            int tmp = 0;
        //            bval = new byte[count];
        //            while (tmp < count)
        //            {
        //                var vd = mClient.Receive(ref rp);
        //                if (vd != null && vd.Length > 0)
        //                {
        //                    if (vd.Length + tmp <= count)
        //                    {
        //                        Array.Copy(vd, 0, bval, tmp, vd.Length);
        //                    }
        //                    else
        //                    {
        //                        Array.Copy(vd, 0, bval, tmp, count - tmp);
        //                    }

        //                    tmp += vd.Length;
        //                }
        //                Thread.Sleep(10);
        //            }
        //            if (tmp < count)
        //            {
        //                bval = bval.AsSpan(tmp).ToArray();
        //            }
        //        }
        //    }
        //    return bval;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="offset"></param>
        ///// <param name="len"></param>
        ///// <returns></returns>
        //public override int Read(byte[] buffer, int offset, int len)
        //{
        //    if ((mReceiveBuffer.Length - mReceiveBuffer.Position) < len)
        //    {
        //        var count = mClient.Receive(ref rp);
        //        mReceiveBuffer.Write(count, 0, count.Length);
        //        mReceiveBuffer.Position = 0;
        //    }

        //    if ((mReceiveBuffer.Length - mReceiveBuffer.Position) >= len)
        //    {
        //        return mReceiveBuffer.Read(buffer, offset, len);
        //    }
        //    else
        //    {
        //        return mReceiveBuffer.Read(buffer, offset, (int)mReceiveBuffer.Length);
        //    }

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
            sw.Start();
            if (!mEnableSyncRead)
            {
                while ((mReceiveBuffer.Length - mReceiveBuffer.Position) < len)
                {
                    var count = mClient.Receive(ref rp);
                    mReceiveBuffer.Write(count, 0, count.Length);
                    mReceiveBuffer.Position = 0;
                    if (sw.ElapsedMilliseconds > timeout)
                    {
                        break;
                    }
                }
                sw.Stop();

                if ((mReceiveBuffer.Length - mReceiveBuffer.Position) >= len)
                {
                    return mReceiveBuffer.Read(buffer, offset, len);
                }
                else
                {
                    return mReceiveBuffer.Read(buffer, offset, (int)mReceiveBuffer.Length);
                }
            }
            else
            {
                int tmp = 0;
                while (tmp < len)
                {
                    var vd = mClient.Receive(ref rp);
                    if (vd != null && vd.Length > 0)
                    {
                        if (vd.Length + tmp <= len)
                        {
                            Array.Copy(vd, 0, buffer, tmp, vd.Length);
                            tmp += vd.Length;
                        }
                        else
                        {
                            Array.Copy(vd, 0, buffer, tmp, len - tmp);
                            tmp =len;
                        }
                    }

                    if (sw.ElapsedMilliseconds > timeout)
                    {
                        break;
                    }
                    Thread.Sleep(timeout / 10);
                }
                sw.Stop();
                return tmp;
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel2 NewApi()
        {
            return new UdpClientChannel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new UdpClientChannelData();
            mData.LoadFromXML(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
