using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.SerisePortClient
{
    public class SerisePortClientChannel : ChannelBase
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
            while (true)
            {
                Thread.Sleep(mData.ReTryDuration);
                try
                {
                    mClient.Open();
                    mIsConnected = true;
                    break;
                }
                catch
                {

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
                if (mClient != null && mClient.BytesToRead > 0 && !mIsTransparentRead)
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
        /// <param name="data"></param>
        /// <param name="timeount"></param>
        /// <param name="waitResultCount"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendInner(Span<byte> data, int timeout, int waitResultCount, out bool result)
        {
            if (mClient != null && mClient.IsOpen)
            {
                byte[] bval;
                mForSyncCall = true;
                ClearBuffer();
                mClient.Write(data.ToArray(),0,data.Length);
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
            if (mClient != null && mClient.IsOpen)
            {
                List<byte> bval = new List<byte>(1024);

                byte[] btmp;

                mForSyncCall = true;
                ClearBuffer();

                mClient.Write(data.ToArray(),0,data.Length);
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
            return base.SendInner(data, timeout, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool SendInnerAsync(Span<byte> data)
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
        public override byte[] Receive(int count, int timeout, out int receivecount)
        {

            byte[] bval = null;

            if (mClient != null && mClient.IsOpen)
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
            if (mClient != null && mClient.IsOpen)
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
            return mClient.Read(buffer,offset,len);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel NewApi()
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
