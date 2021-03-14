using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public unsafe class HisDataMemory : IDisposable
    {

        #region ... Variables  ...

        private IntPtr mHandle;

        public const int BufferItemSize = 32;

        public static byte[] zoreData = new byte[BufferItemSize];

        private long mAllocSize = 0;

        private long mSize = 0;

        private long mPosition = 0;

        private long mHandleValue;

        private int mValueMaxCount = 299;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public HisDataMemory() : this(BufferItemSize,300)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public HisDataMemory(int size,int count)
        {
            mValueMaxCount = count-1;
            Init(size*count);
        }


        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public IntPtr Buffers
        {
            get
            {
                return mHandle;
            }
            internal set
            {
                mHandle = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public long Length
        {
            get
            {
                return mSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ValueEnd { get; set; } = -1;

        /// <summary>
        /// 
        /// </summary>
        public int ValueStart { get; set; } = -1;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public void CheckAndResize(long size)
        {
            if (size > mPosition)
            {
                IntPtr moldptr = mHandle;
                long oldlen = mPosition;
                Init(size);
                Buffer.MemoryCopy((void*)moldptr, (void*)mHandle, size, oldlen);
                Marshal.FreeHGlobal(moldptr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        private void Init(long size)
        {
            mSize = size;
            mHandle = Marshal.AllocHGlobal(new IntPtr(size));
            mHandleValue = mHandle.ToInt64();
            mAllocSize = size;
        }

        #region ReadAndWrite



        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(DateTime value)
        {
            WriteDatetime(mPosition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(long value)
        {
            WriteLong(mPosition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>

        public void Write(ulong value)
        {
            WriteULong(mPosition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(int value)
        {
            WriteInt(mPosition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(uint value)
        {
            WriteUInt(mPosition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(short value)
        {
            WriteShort(mPosition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(ushort value)
        {
            WriteUShort(mPosition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(float value)
        {
            WriteFloat(mPosition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(double value)
        {
            WriteDouble(mPosition, value);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        public void Write(string value, Encoding encoding)
        {
            WriteString(mPosition, value, encoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(string value)
        {
            WriteString(mPosition, value, Encoding.Unicode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Write(byte value)
        {
            WriteByte(mPosition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public void Write(byte[] values)
        {
            WriteBytes(mPosition, values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        public void Write(byte[] values, int offset, int len)
        {
            WriteBytes(mPosition, values, offset, len);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public void Write(Memory<byte> values)
        {
            WriteMemory(mPosition, values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteLong(long offset, long value)
        {
            MemoryHelper.WriteInt64((void*)mHandle, offset, value);
            Position = offset + 8;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteLongDirect(long offset, long value)
        {
            MemoryHelper.WriteInt64((void*)mHandle, offset, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteULong(long offset, ulong value)
        {
            MemoryHelper.WriteUInt64((void*)mHandle, offset, value);
            Position = offset + 8;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteFloat(long offset, float value)
        {
            MemoryHelper.WriteFloat((void*)mHandle, offset, value);
            Position = offset + 4;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteDouble(long offset, double value)
        {
            MemoryHelper.WriteDouble((void*)mHandle, offset, value);
            Position = offset + 8;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="values"></param>
        public void WriteMemory(long offset, Memory<byte> values)
        {
            WriteMemory(offset, values, 0, values.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="values"></param>
        public void WriteBytes(long offset, byte[] values)
        {
            WriteBytes(offset, values, 0, values.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="values"></param>
        public void WriteBytesDirect(long offset, byte[] values)
        {
            WriteBytesDirect(offset, values, 0, values.Length);
        }

        /// <summary>
        /// 清空值
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        public void Clear(long offset, long len)
        {
            Clear(mHandle, offset, (int)len);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            Clear(mHandle, 0, (int)this.mAllocSize);
            ValueEnd = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        private void Clear(IntPtr target, long start, long len)
        {
            int i = 0;
            for (i = 0; i < len / zoreData.Length; i++)
            {
                Marshal.Copy(zoreData, 0, new IntPtr(target.ToInt64() + start + i * zoreData.Length), zoreData.Length);
            }
            long zz = len % zoreData.Length;
            if (zz > 0)
            {
                Marshal.Copy(zoreData, 0, new IntPtr(target.ToInt64() + start + i * zoreData.Length), (int)zz);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="values"></param>
        /// <param name="valueoffset"></param>
        /// <param name="len"></param>
        public void WriteMemory(long offset, Memory<byte> values, int valueoffset, int len)
        {
            Buffer.MemoryCopy((void*)((IntPtr)values.Pin().Pointer + valueoffset), (void*)(new IntPtr(mHandleValue + offset)), mSize - offset, len);
            Position = offset + len;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="values"></param>
        /// <param name="valueoffset"></param>
        /// <param name="len"></param>
        public void WriteBytes(long offset, byte[] values, int valueoffset, int len)
        {
            Marshal.Copy(values, valueoffset, (new IntPtr(mHandleValue + offset)), len);
            Position = offset + len;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="values"></param>
        /// <param name="valueOffset"></param>
        /// <param name="len"></param>
        public void WriteBytesDirect(long offset, IntPtr values, int valueOffset, int len)
        {
            Buffer.MemoryCopy((void*)(values + valueOffset), (void*)((mHandleValue + offset)), mSize - offset, len);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="values"></param>
        /// <param name="valueoffset"></param>
        /// <param name="len"></param>
        public void WriteBytesDirect(long offset, byte[] values, int valueoffset, int len)
        {
            Marshal.Copy(values, valueoffset, (new IntPtr(mHandleValue + offset)), len);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteByte(long offset, byte value)
        {
            MemoryHelper.WriteByte((void*)mHandle, offset, value);
            Position = offset + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteByteDirect(long offset, byte value)
        {
            MemoryHelper.WriteByte((void*)mHandle, offset, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
        {
            WriteByte(mPosition, value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteDatetime(long offset, DateTime value)
        {
            MemoryHelper.WriteDateTime((void*)mHandle, offset, value);
            Position = offset + 8;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteInt(long offset, int value)
        {
            MemoryHelper.WriteInt32((void*)mHandle, offset, value);
            Position = offset + 4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteIntDirect(long offset, int value)
        {
            MemoryHelper.WriteInt32((void*)mHandle, offset, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteUInt(long offset, uint value)
        {
            MemoryHelper.WriteUInt32((void*)mHandle, offset, value);
            Position = offset + 4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteUIntDirect(long offset, uint value)
        {
            MemoryHelper.WriteUInt32((void*)mHandle, offset, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteShort(long offset, short value)
        {
            MemoryHelper.WriteShort((void*)mHandle, offset, value);
            Position = offset + 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteShortDirect(long offset, short value)
        {
            MemoryHelper.WriteShort((void*)mHandle, offset, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteUShort(long offset, ushort value)
        {
            MemoryHelper.WriteUShort((void*)mHandle, offset, value);
            Position = offset + 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public void WriteUShortDirect(long offset, ushort value)
        {
            MemoryHelper.WriteUShort((void*)mHandle, offset, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="encode"></param>
        public void WriteString(long offset, string value, Encoding encode)
        {
            var sdata = encode.GetBytes(value);
            WriteByte(offset, (byte)sdata.Length);
            WriteBytes(offset + 1, sdata);
            Position = offset + sdata.Length + 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="encode"></param>
        public void WriteStringDirect(long offset, string value, Encoding encode)
        {
            var sdata = encode.GetBytes(value);
            WriteByte(offset, (byte)sdata.Length);
            WriteBytes(offset + 1, sdata);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public DateTime ReadDateTime(long offset)
        {
            mPosition = offset + sizeof(DateTime);
            return MemoryHelper.ReadDateTime((void*)mHandle, offset);
        }

        public List<DateTime> ReadDateTimes(long offset, int count)
        {
            List<DateTime> re = new List<DateTime>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadDateTime(offset + 8 * i));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public int ReadInt(long offset)
        {
            mPosition = offset + sizeof(int);
            return MemoryHelper.ReadInt32((void*)mHandle, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<int> ReadInts(long offset, int count)
        {
            List<int> re = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadInt(offset + 4 * i));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public uint ReadUInt(long offset)
        {
            mPosition = offset + 4;
            return MemoryHelper.ReadUInt32((void*)mHandle, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<uint> ReadUInts(long offset, int count)
        {
            List<uint> re = new List<uint>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadUInt(offset + 4 * i));
            }
            return re;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public short ReadShort(long offset)
        {
            mPosition = offset + 2;
            return MemoryHelper.ReadShort((void*)mHandle, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public ushort ReadUShort(long offset)
        {
            mPosition = offset + 2;
            return MemoryHelper.ReadUShort((void*)mHandle, offset);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public float ReadFloat(long offset)
        {
            mPosition = offset + 4;
            return MemoryHelper.ReadFloat((void*)mHandle, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<float> ReadFloats(long offset, int count)
        {
            List<float> re = new List<float>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadFloat(offset + 4 * i));
            }
            return re;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public double ReadDouble(long offset)
        {
            mPosition = offset + 8;
            return MemoryHelper.ReadDouble((void*)mHandle, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<double> ReadDoubles(long offset, int count)
        {
            List<double> re = new List<double>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadDouble(offset + 8 * i));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public long ReadLong(long offset)
        {
            mPosition = offset + 8;
            return MemoryHelper.ReadInt64((void*)mHandle, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public ulong ReadULong(long offset)
        {
            mPosition = offset + 8;
            return MemoryHelper.ReadUInt64((void*)mHandle, offset);
            //mPosition = offset + sizeof(long);
            //return MemoryHelper.ReadUInt64(handle, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte ReadByte(long offset)
        {
            mPosition = offset + 1;
            return MemoryHelper.ReadByte((void*)mHandle, offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public string ReadString(long offset, Encoding encoding)
        {
            var len = ReadByte(offset);
            mPosition = offset + len + 1;
            return encoding.GetString(ReadBytesInner(offset + 1, len));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public string ReadString(long offset)
        {
            return ReadString(offset, Encoding.Unicode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private byte[] ReadBytesInner(long offset, int len)
        {
            byte[] re = new byte[len];
            Marshal.Copy(new IntPtr(mHandleValue + offset), re, 0, len);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="target"></param>
        /// <param name="len"></param>
        public void ReadBytes(long offset, byte[] target, int len)
        {
            Marshal.Copy(new IntPtr(mHandleValue + offset), target, 0, len);
            mPosition += len;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte[] ReadBytes(long offset, int len)
        {
            byte[] re = ReadBytesInner(offset, len);
            mPosition += len;
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return ReadByte(mPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long ReadLong()
        {
            return ReadLong(mPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<long> ReadLongs(long offset, int count)
        {
            List<long> re = new List<long>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadLong(offset + 8 * i));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ulong ReadULong()
        {
            return ReadULong(mPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ulong> ReadULongs(long offset, int count)
        {
            List<ulong> re = new List<ulong>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadULong(offset + 8 * i));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            return ReadInt(mPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt()
        {
            return ReadUInt(mPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            return ReadFloat(mPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            return ReadDouble(mPosition);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            return ReadShort(mPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<short> ReadShorts(long offset, int count)
        {
            List<short> re = new List<short>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadShort(offset + 2 * i));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            return ReadUShort(mPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ushort> ReadUShorts(long offset, int count)
        {
            List<ushort> re = new List<ushort>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadUShort(offset + 2 * i));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime ReadDateTime()
        {
            return ReadDateTime(mPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ReadString(Encoding encoding)
        {
            return ReadString(mPosition, encoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            return ReadString(mPosition, Encoding.Unicode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<string> ReadStrings(long offset, int count)
        {
            mPosition = offset;
            List<string> re = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                re.Add(ReadString());
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] ReadBytes(int len)
        {
            return ReadBytes(mPosition, len);
        }




        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, double value)
        {
            CheckValueEnd();
            Position = ValueEnd * 16;
            this.WriteLong(Position, time.ToBinary());
            this.WriteDouble(Position, value);
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, float value)
        {
            CheckValueEnd();
            Position = ValueEnd * 12;
            this.WriteLong(Position, time.ToBinary());
            this.WriteFloat(Position, value);
           
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, int value)
        {
            CheckValueEnd();
            Position = ValueEnd * 12;
            this.WriteLong(Position, time.ToBinary());
            this.WriteInt(Position, value);
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, uint value)
        {
            CheckValueEnd();
            Position = ValueEnd * 12;
            this.WriteLong(Position, time.ToBinary());
            this.WriteUInt(Position, value);
            return this;
        }


        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, short value)
        {
            CheckValueEnd();
            Position = ValueEnd * 10;
            this.WriteLong(Position, time.ToBinary());
            this.WriteShort(Position, value);
            return this;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, ushort value)
        {
            CheckValueEnd();
            Position = ValueEnd * 10;
            this.WriteLong(Position, time.ToBinary());
            this.WriteUShort(Position, value);
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, byte value)
        {
            CheckValueEnd();

            Position = ValueEnd * 9;
            this.WriteLong(Position, time.ToBinary());
            this.WriteByte(Position, value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckValueEnd()
        {
            ValueEnd++;
            ValueEnd = ValueEnd > mValueMaxCount?-1:ValueEnd;
            if(ValueEnd==ValueStart && ValueEnd>0)
            {
                ValueStart++;
                if (ValueStart > mValueMaxCount) ValueStart = -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckValueStart()
        {
            ValueStart++;
            if (ValueStart == ValueEnd && ValueStart > 0)
            {
                ValueStart--;
                return false;
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, long value)
        {
            CheckValueEnd();
            Position = ValueEnd * 16;
            this.WriteLong(Position, time.ToBinary());
            this.WriteLong(Position, value);
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, ulong value)
        {
            CheckValueEnd();
            Position = ValueEnd * 16;
            this.WriteLong(Position, time.ToBinary());
            this.WriteULong(Position, value);
            return this;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, DateTime value)
        {
            CheckValueEnd();
            Position = ValueEnd * 16;
            this.WriteLong(Position, time.ToBinary());
            this.WriteLong(Position, value.ToBinary());
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, string value)
        {
            CheckValueEnd();
            Position = ValueEnd * (8+255);
            this.WriteLong(Position, time.ToBinary());
            this.WriteString(Position, value, Encoding.Unicode);
            return this;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, IntPoint value)
        {
            CheckValueEnd();
            Position = ValueEnd * 16;
            this.WriteLong(Position, time.ToBinary());
            this.WriteInt(Position, value.X);
            this.WriteInt(Position, value.Y);
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, UIntPoint value)
        {
            CheckValueEnd();
            Position = ValueEnd * 16;
            this.WriteLong(Position, time.ToBinary());
            this.WriteUInt(Position, value.X);
            this.WriteUInt(Position, value.Y);
            return this;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, IntPoint3 value)
        {
            CheckValueEnd();
            Position = ValueEnd * 20;
            this.WriteLong(Position, time.ToBinary());
            this.WriteInt(Position, value.X);
            this.WriteInt(Position, value.Y);
            this.WriteInt(Position, value.Z);
            return this;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, UIntPoint3 value)
        {
            CheckValueEnd();
            Position = ValueEnd * 20;
            this.WriteLong(Position, time.ToBinary());
            this.WriteUInt(Position, value.X);
            this.WriteUInt(Position, value.Y);
            this.WriteUInt(Position, value.Z);
            return this;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, LongPoint value)
        {
            CheckValueEnd();
            Position = ValueEnd * 24;
            this.WriteLong(Position, time.ToBinary());
            this.WriteLong(Position, value.X);
            this.WriteLong(Position, value.Y);
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, ULongPoint value)
        {
            CheckValueEnd();
            Position = ValueEnd * 24;
            this.WriteLong(Position, time.ToBinary());
            this.WriteULong(Position, value.X);
            this.WriteULong(Position, value.Y);
            return this;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, LongPoint3 value)
        {
            CheckValueEnd();
            Position = ValueEnd * 32;
            this.WriteLong(Position, time.ToBinary());
            this.WriteLong(Position, value.X);
            this.WriteLong(Position, value.Y);
            this.WriteLong(Position, value.Z);
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HisDataMemory AppendValue(DateTime time, ULongPoint3 value)
        {
            CheckValueEnd();
            Position = ValueEnd * 32;
            this.WriteLong(Position, time.ToBinary());
            this.WriteULong(Position, value.X);
            this.WriteULong(Position, value.Y);
            this.WriteULong(Position, value.Z);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReadValue(out DateTime time, out byte value)
        {
            if(CheckValueStart())
            {
                Position = ValueStart * 9;
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadByte();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReadValue(out DateTime time, out short value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 10;
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadShort();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReadValue(out DateTime time, out ushort value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 10;
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadUShort();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReadValue(out DateTime time, out int value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 12;
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadInt();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReadValue(out DateTime time, out uint value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 12;
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadUInt();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReadValue(out DateTime time, out float value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 12;
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadFloat();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = 0;
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out double value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 16;
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadDouble();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = 0;
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out long value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 16;
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadLong();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = 0;
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out ulong value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 16;
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadULong();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReadValue(out DateTime time, out DateTime value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 16;
                time = DateTime.FromBinary(this.ReadLong());
                value = DateTime.FromBinary(this.ReadLong());
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = DateTime.Now;
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out string value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * (8+255);
                time = DateTime.FromBinary(this.ReadLong());
                value = this.ReadString();
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = "";
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out IntPoint value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 16;
                time = DateTime.FromBinary(this.ReadLong());
                value = new IntPoint() { X = this.ReadInt(), Y = this.ReadInt() };
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = new IntPoint();
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out UIntPoint value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 16;
                time = DateTime.FromBinary(this.ReadLong());
                value = new UIntPoint() { X = this.ReadUInt(), Y = this.ReadUInt() };
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = new UIntPoint();
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out IntPoint3 value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 20;
                time = DateTime.FromBinary(this.ReadLong());
                value = new IntPoint3() { X = this.ReadInt(), Y = this.ReadInt(),Z=this.ReadInt() };
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = new IntPoint3();
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out UIntPoint3 value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 20;
                time = DateTime.FromBinary(this.ReadLong());
                value = new UIntPoint3() { X = this.ReadUInt(), Y = this.ReadUInt(),Z=this.ReadUInt() };
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = new UIntPoint3();
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out LongPoint value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 24;
                time = DateTime.FromBinary(this.ReadLong());
                value = new LongPoint() { X = this.ReadLong(), Y = this.ReadLong() };
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = new LongPoint();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReadValue(out DateTime time, out ULongPoint value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 24;
                time = DateTime.FromBinary(this.ReadLong());
                value = new ULongPoint() { X = this.ReadULong(), Y = this.ReadULong() };
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = new ULongPoint();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReadValue(out DateTime time, out LongPoint3 value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 32;
                time = DateTime.FromBinary(this.ReadLong());
                value = new LongPoint3() { X = this.ReadLong(), Y = this.ReadLong(),Z=this.ReadLong() };
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = new LongPoint3();
                return false;
            }
        }


        public bool ReadValue(out DateTime time, out ULongPoint3 value)
        {
            if (CheckValueStart())
            {
                Position = ValueStart * 32;
                time = DateTime.FromBinary(this.ReadLong());
                value = new ULongPoint3() { X = this.ReadULong(), Y = this.ReadULong(), Z = this.ReadULong() };
                return true;
            }
            else
            {
                time = DateTime.Now;
                value = new ULongPoint3();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (mHandle != IntPtr.Zero)
                    Marshal.FreeHGlobal(mHandle);
            }
            catch
            {

            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
