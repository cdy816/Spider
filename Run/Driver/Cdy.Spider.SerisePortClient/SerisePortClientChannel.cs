using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.SerisePortClient
{
    public class SerisePortClientChannel : ChannelBase2
    {


        #region ... Variables  ...

        private SerisePortClientChannelData mData;

        private System.IO.Ports.SerialPort mClient;

        private Thread mReceiveThread;

        private bool mIsClosed = false;

        private Queue<byte[]> mReceiveBuffers = new Queue<byte[]>();

        private int mReceiveDataLen = 0;

        private object mLockObj = new object();

        private bool mForSyncCall = false;

        private byte[] mRelayData;

        Stopwatch sw = new Stopwatch();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "SerisePort";

        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string RemoteDescription => mData.PortName;

        #endregion ...Properties...

        #region ... Methods    ...

        //public void TestOpen()
        //{
        //    mClient = new System.IO.Ports.SerialPort("Com2", 9600);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            mClient = new System.IO.Ports.SerialPort(mData.PortName, mData.BandRate);
            mClient.DataBits = mData.DataSize;
            mClient.StopBits = (System.IO.Ports.StopBits) mData.StopBits;
            mClient.Parity = (System.IO.Ports.Parity)mData.Check;
            mClient.DtrEnable = mData.EnableDTR;
            mClient.RtsEnable = mData.EnableRTS;

            mReceiveThread = new Thread(ThreadPro);
            mReceiveThread.IsBackground = true;
            mReceiveThread.Start();

            try
            {
                mClient.Open();
                mIsConnected = true;
                LoggerService.Service.Info("SerisePort", $"Open {mData.PortName} successful.");
            }
            catch
            {
                LoggerService.Service.Warn("OpcUA", $"connect to {this.mData.PortName} failed.");
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
            while (true)
            {
                Thread.Sleep(mData.ReTryDuration);
                try
                {
                    if(!mClient.IsOpen)
                    mClient.Open();
                    mIsConnected = true;
                    LoggerService.Service.Info("SerisePort", $"Open {mData.PortName} successful.");
                    break;
                }
                catch
                {
                    //LoggerService.Service.Warn("SerisePort", $"Open {mData.PortName} failed.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerClose()
        {
            try
            {
                mIsConnected = false;
                if (mClient != null)
                {
                    mClient.Close();
                    mClient = null;
                }
            }
            catch
            {

            }
            return base.InnerClose();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ThreadPro()
        {
            while (!mIsClosed)
            {
                if (mClient != null && mClient.IsOpen &&  mClient.BytesToRead > 0 && !mEnableSyncRead)
                {

                    var vdlen = mClient.BytesToRead;
                    byte[] btmp = new byte[vdlen];
                    mClient.Read(btmp, 0, btmp.Length);
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
                    if(mClient!=null && !mClient.IsOpen)
                    {
                        TryConnect();
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


        private int CopyReceiveBufferData(byte[] btmp,int offset,int count,int timeount=-1)
        {
            //byte[] btmp = new byte[count];
            int cc = 0;
            //int offset = 0;
            int removecount = 0;
            byte[] vdata;

            Stopwatch sw = Stopwatch.StartNew();
            if(timeount>0)
            {
                sw.Start();
            }
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


                if (timeount > 0 && sw.ElapsedMilliseconds>timeount)
                {
                    break;
                }
            }
            if (timeount > 0)
            {
                sw.Stop();
            }
            lock (mLockObj)
            {
                mReceiveDataLen -= removecount;
                mReceiveDataLen = mReceiveDataLen < 0 ? 0 : mReceiveDataLen;
            }
            return cc;
        }


        

        /// <summary>
        /// 
        /// </summary>
        public override void ClearBuffer()
        {
            lock (mLockObj)
            {
                mClient.DiscardInBuffer();
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
            if (mClient != null && mClient.IsOpen)
            {
                byte[] bval;
                mForSyncCall = true;
                ClearBuffer();
                mClient.Write(data.ToArray(),0,data.Length);
                //Stopwatch sw = new Stopwatch();
                sw.Restart();
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
            if (mClient != null && mClient.IsOpen)
            {
                List<byte> bval = new List<byte>(1024);

                byte[] btmp;

                mForSyncCall = true;
                ClearBuffer();

                mClient.Write(data.ToArray(),0,data.Length);
                //Stopwatch sw = new Stopwatch();
                sw.Restart();
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
            if (mClient != null && mClient.IsOpen)
            {
                mForSyncCall = true;
                ClearBuffer();

                mClient.Write(data.ToArray(),0,data.Length);

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
            if (mClient != null && mClient.IsOpen)
            {
                mClient.Write(data.ToArray(), 0, data.Length);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override byte[] Read(int count, int timeout, out int receivecount)
        {

            byte[] bval = null;

            if (mClient != null && mClient.IsOpen)
            {
                sw.Restart();
                Thread.Sleep(timeout / 10);
                if (!mEnableSyncRead)
                {
                    while (mReceiveDataLen < count)
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
                    while (mClient.BytesToRead < count)
                    {
                        Thread.Sleep(timeout / 10);
                        if (sw.ElapsedMilliseconds > timeout)
                        {
                            break;
                        }
                    }
                    sw.Stop();
                    if (mClient.BytesToRead > 0)
                    {
                        bval = new byte[Math.Min(mClient.BytesToRead, count)];
                        receivecount = mClient.Read(bval, 0, bval.Length);
                    }
                    else
                    {
                        receivecount = 0;
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
        //    if (mClient != null && mClient.IsOpen)
        //    {
        //        if(!mEnableSyncRead)
        //        {
        //            var bval = CopyReceiveBufferData(count);
        //            return bval;
        //        }
        //        else
        //        {
        //            byte[] bval = new byte[count];
        //            mClient.Read(bval, 0, count);
        //            return bval;
        //        }
        //    }
        //    else
        //    {
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
        //public override int Read(byte[] buffer, int offset, int len)
        //{
        //    if (mClient != null && mClient.IsOpen)
        //    {
        //        if(!mEnableSyncRead)
        //        {
        //           return  CopyReceiveBufferData(buffer, offset, len);
        //        }
        //        else
        //        {
        //            return mClient.Read(buffer, offset, len);
        //        }
        //    }
        //    else
        //    {
        //        return 0;
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
            if (mClient != null && mClient.IsOpen)
            {
                if (!mEnableSyncRead)
                {
                    return CopyReceiveBufferData(buffer, offset, len,timeout);
                }
                else
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int count = 0;
                    while (count < len)
                    {
                        count += mClient.Read(buffer, offset + count, len - count);
                        if (sw.ElapsedMilliseconds > timeout)
                        {
                            break;
                        }
                        Thread.Sleep(timeout / 10);
                    }
                    sw.Stop();
                    return count;
                }
            }
            else
            {
                return 0;
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="offset"></param>
        ///// <param name="len"></param>
        ///// <returns></returns>
        //public override bool Write(byte[] buffer, int offset, int len)
        //{
        //    try
        //    {
        //        mClient.Write(buffer, offset, len);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel2 NewApi()
        {
           return new SerisePortClientChannel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new SerisePortClientChannelData();
            mData.LoadFromXML(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
