using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.Common.Tags
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TagHisValueBuffer<T>
    {

        #region ... Variables  ...
        
        private T[] mColections;
        
        private DateTime[] mTimes;

        private int mCount = 0;
        
        private object mReadLockObj = new object();

        private uint mWriteCount = 0;

        private uint mReadCount = 0;


        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public TagHisValueBuffer(int count)
        {
            mColections = new T[count];

            mTimes = new DateTime[count];
            mCount = count;
        }

        #endregion ...Constructor...

        #region ... Properties ...



        /// <summary>
        /// 
        /// </summary>
        public int WriteIndex { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public int ReadIndex { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Length { get { return mCount; } }

        /// <summary>
        /// 是否覆盖比较旧的数据
        /// </summary>
        public bool Overwrite { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public void CheckAndResize(int size)
        {
            if (size > mCount)
            {
                var vt = new T[size];
                var dt = new DateTime[size];

                if (WriteIndex >= ReadIndex)
                {
                    mColections.CopyTo(vt, 0);
                    mTimes.CopyTo(dt, 0);
                }
                else
                {
                    //说明写数据超出一圈,则需要重新将写起始重置到0位

                    mColections.AsSpan(WriteIndex).CopyTo(vt);
                    mColections.AsSpan(0, WriteIndex).CopyTo(vt.AsSpan(mColections.Length - WriteIndex));

                    mTimes.AsSpan(WriteIndex).CopyTo(dt);
                    mTimes.AsSpan(0, WriteIndex).CopyTo(dt.AsSpan(mColections.Length - WriteIndex));

                    ReadIndex -= WriteIndex;
                    WriteIndex = 0;
                    
                }

                mColections = vt;
                mTimes = dt;

                mCount = size;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void IncReadIndex()
        {
            ReadIndex++;
            if (ReadIndex >= mColections.Length)
            {
                ReadIndex = 0;
                mReadCount = mReadCount > int.MaxValue - 1 ? 0 : mReadCount + 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void IncWriteIndex()
        {
            WriteIndex++;
            if (WriteIndex >= mColections.Length)
            {
                WriteIndex = 0;
                mWriteCount = mWriteCount > int.MaxValue - 1 ? 0 : mWriteCount + 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Insert(T value,DateTime time)
        {
            lock (mReadLockObj)
            {
                if(mWriteCount>mReadCount && WriteIndex>=ReadIndex)
                {
                    //说明写数据超过了读数据
                    if (Overwrite)
                    {
                        //将进行数据覆盖
                        IncReadIndex();
                    }
                    else
                    {
                        CheckAndResize(Length * 2);
                    }
                }

                mColections[WriteIndex] = value;
                mTimes[WriteIndex] = time;
                IncWriteIndex();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        public void Get(int index,out T value,out DateTime time)
        {
            value = mColections[index];
            time = mTimes[index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        public bool Pop(out T value,out DateTime time)
        {
            lock (mReadLockObj)
            {
                if (mReadCount > mWriteCount || (mReadCount == mWriteCount && ReadIndex >= WriteIndex))
                {
                    value = default(T);
                    time = DateTime.MinValue;
                    return false;
                }

                Get(ReadIndex, out value, out time);
                IncReadIndex();
                return true;
            }
        }

        /// <summary>
        /// 获取可读数据的个数
        /// </summary>
        /// <returns></returns>
        public int GetAvaiableCount()
        {
            if (WriteIndex >= ReadIndex && mReadCount == mWriteCount)
            {
                return WriteIndex - ReadIndex;
            }
            else
            {
                return mColections.Length - ReadIndex + WriteIndex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            WriteIndex = 0;
            ReadIndex = 0;
            mReadCount = 0;
            mWriteCount = 0;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
