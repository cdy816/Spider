//==============================================================
//  Copyright (C) 2019  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2019/12/27 18:45:02.
//  Version 1.0
//  种道洋
//==============================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public static class MemoryHelper
    {
        #region Helper fun

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteByte(void* ptr, long ofs, byte val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                addr[0] = val;
            }
            catch (NullReferenceException)
            {
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteShort(void* ptr, long ofs, short val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x1) == 0)
                {
                    // aligned write
                    *((short*)addr) = val;
                }
                else
                {
                    // unaligned write
                    byte* valPtr = (byte*)&val;
                    addr[0] = valPtr[0];
                    addr[1] = valPtr[1];
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteShortReverse(void* ptr, long ofs, short val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                // unaligned write
                byte* valPtr = (byte*)&val;
                addr[1] = valPtr[0];
                addr[0] = valPtr[1];
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteUShort(void* ptr, long ofs, ushort val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x1) == 0)
                {
                    // aligned write
                    *((ushort*)addr) = val;
                }
                else
                {
                    // unaligned write
                    byte* valPtr = (byte*)&val;
                    addr[0] = valPtr[0];
                    addr[1] = valPtr[1];
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteUShortReverse(void* ptr, long ofs, ushort val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                // unaligned write
                byte* valPtr = (byte*)&val;
                addr[1] = valPtr[0];
                addr[0] = valPtr[1];
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteInt32(void* ptr, long ofs, int val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x3) == 0)
                {
                    // aligned write
                    *((int*)addr) = val;
                }
                else
                {
                    // unaligned write
                    byte* valPtr = (byte*)&val;
                    addr[0] = valPtr[0];
                    addr[1] = valPtr[1];
                    addr[2] = valPtr[2];
                    addr[3] = valPtr[3];
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteInt32Reverse(void* ptr, long ofs, int val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                // unaligned write
                byte* valPtr = (byte*)&val;
                addr[3] = valPtr[0];
                addr[2] = valPtr[1];
                addr[1] = valPtr[2];
                addr[0] = valPtr[3];
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteUInt32(void* ptr, long ofs, uint val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x3) == 0)
                {
                    // aligned write
                    *((uint*)addr) = val;
                }
                else
                {
                    // unaligned write
                    byte* valPtr = (byte*)&val;
                    addr[0] = valPtr[0];
                    addr[1] = valPtr[1];
                    addr[2] = valPtr[2];
                    addr[3] = valPtr[3];
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteUInt32Reverse(void* ptr, long ofs, uint val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                // unaligned write
                byte* valPtr = (byte*)&val;
                addr[3] = valPtr[0];
                addr[2] = valPtr[1];
                addr[1] = valPtr[2];
                addr[0] = valPtr[3];
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteFloat(void* ptr, long ofs, float val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x3) == 0)
                {
                    // aligned write
                    *((int*)addr) = *(int*)(&val);
                }
                else
                {
                    // unaligned write
                    byte* valPtr = (byte*)&val;
                    addr[0] = valPtr[0];
                    addr[1] = valPtr[1];
                    addr[2] = valPtr[2];
                    addr[3] = valPtr[3];
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteFloatReverse(void* ptr, long ofs, float val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                // unaligned write
                byte* valPtr = (byte*)&val;
                addr[3] = valPtr[0];
                addr[2] = valPtr[1];
                addr[1] = valPtr[2];
                addr[0] = valPtr[3];
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteInt64(void* ptr, long ofs, Int64 val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x7) == 0)
                {
                    // aligned write
                    *((Int64*)addr) = val;
                }
                else
                {
                    // unaligned write
                    byte* valPtr = (byte*)&val;
                    addr[0] = valPtr[0];
                    addr[1] = valPtr[1];
                    addr[2] = valPtr[2];
                    addr[3] = valPtr[3];
                    addr[4] = valPtr[4];
                    addr[5] = valPtr[5];
                    addr[6] = valPtr[6];
                    addr[7] = valPtr[7];
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteInt64Reverse(void* ptr, long ofs, Int64 val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                // unaligned write
                byte* valPtr = (byte*)&val;
                addr[7] = valPtr[0];
                addr[6] = valPtr[1];
                addr[5] = valPtr[2];
                addr[4] = valPtr[3];
                addr[3] = valPtr[4];
                addr[2] = valPtr[5];
                addr[1] = valPtr[6];
                addr[0] = valPtr[7];
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteUInt64(void* ptr, long ofs, UInt64 val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x7) == 0)
                {
                    // aligned write
                    *((UInt64*)addr) = val;
                }
                else
                {
                    // unaligned write
                    byte* valPtr = (byte*)&val;
                    addr[0] = valPtr[0];
                    addr[1] = valPtr[1];
                    addr[2] = valPtr[2];
                    addr[3] = valPtr[3];
                    addr[4] = valPtr[4];
                    addr[5] = valPtr[5];
                    addr[6] = valPtr[6];
                    addr[7] = valPtr[7];
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteUInt64Reverse(void* ptr, long ofs, UInt64 val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                // unaligned write
                byte* valPtr = (byte*)&val;
                addr[7] = valPtr[0];
                addr[6] = valPtr[1];
                addr[5] = valPtr[2];
                addr[4] = valPtr[3];
                addr[3] = valPtr[4];
                addr[2] = valPtr[5];
                addr[1] = valPtr[6];
                addr[0] = valPtr[7];
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteDouble(void* ptr, long ofs, double val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x7) == 0)
                {
                    // aligned write
                    *((Int64*)addr) = *(Int64*)(&val);
                }
                else
                {
                    // unaligned write
                    byte* valPtr = (byte*)&val;
                    addr[0] = valPtr[0];
                    addr[1] = valPtr[1];
                    addr[2] = valPtr[2];
                    addr[3] = valPtr[3];
                    addr[4] = valPtr[4];
                    addr[5] = valPtr[5];
                    addr[6] = valPtr[6];
                    addr[7] = valPtr[7];
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteDoubleReverse(void* ptr, long ofs, double val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                // unaligned write
                byte* valPtr = (byte*)&val;
                addr[7] = valPtr[0];
                addr[6] = valPtr[1];
                addr[5] = valPtr[2];
                addr[4] = valPtr[3];
                addr[3] = valPtr[4];
                addr[2] = valPtr[5];
                addr[1] = valPtr[6];
                addr[0] = valPtr[7];
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static unsafe byte[] GetBytes(DateTime date)
        {
            byte[] bval = new byte[8];
            var ptr = (IntPtr)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(bval, 0);
            WriteDateTime(bval.AsMemory(0, 8).Pin().Pointer, 0, date);
            return bval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static unsafe DateTime ReadDateTime(byte[] value, int offset = 0)
        {
            
           // var ptr = (IntPtr)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(value, offset);
            return ReadDateTime(value.AsMemory(offset,8).Pin().Pointer, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteDateTime(void* ptr, long ofs, DateTime val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x7) == 0)
                {
                    // aligned write
                    *((Int64*)addr) = *(Int64*)(&val);
                }
                else
                {
                    // unaligned write
                    byte* valPtr = (byte*)&val;
                    addr[0] = valPtr[0];
                    addr[1] = valPtr[1];
                    addr[2] = valPtr[2];
                    addr[3] = valPtr[3];
                    addr[4] = valPtr[4];
                    addr[5] = valPtr[5];
                    addr[6] = valPtr[6];
                    addr[7] = valPtr[7];
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <param name="val"></param>
        public static unsafe void WriteDateTimeReverse(void* ptr, long ofs, DateTime val)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                // unaligned write
                byte* valPtr = (byte*)&val;
                addr[7] = valPtr[0];
                addr[6] = valPtr[1];
                addr[5] = valPtr[2];
                addr[4] = valPtr[3];
                addr[3] = valPtr[4];
                addr[2] = valPtr[5];
                addr[1] = valPtr[6];
                addr[0] = valPtr[7];
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe int ReadInt32(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x3) == 0)
                {
                    //aligned read
                    return *((int*)addr);
                }
                else
                {
                    // unaligned read
                    int val;
                    byte* valPtr = (byte*)&val;
                    valPtr[0] = addr[0];
                    valPtr[1] = addr[1];
                    valPtr[2] = addr[2];
                    valPtr[3] = addr[3];
                    return val;
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="sourceoffset"></param>
        /// <param name="targetoffset"></param>
        /// <param name="size"></param>
        public static unsafe void MemoryCopy(IntPtr source,int sourceoffset, IntPtr target, int targetoffset,int size)
        {
            Buffer.MemoryCopy((void*)(source + sourceoffset), (void*)(target+ targetoffset), size, size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe int ReadInt32(IntPtr ptr, long ofs)
        {
            return ReadInt32((void*)ptr, ofs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe uint ReadUInt32(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x3) == 0)
                {
                    //aligned read
                    return *((uint*)addr);
                }
                else
                {
                    // unaligned read
                    uint val;
                    byte* valPtr = (byte*)&val;
                    valPtr[0] = addr[0];
                    valPtr[1] = addr[1];
                    valPtr[2] = addr[2];
                    valPtr[3] = addr[3];
                    return val;
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe uint ReadUInt32(IntPtr ptr, long ofs)
        {
            return ReadUInt32((void*)ptr, ofs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static unsafe float ReadFloat(byte[] value)
        {

            return ReadFloat(value.AsMemory(0, 4).Pin().Pointer, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe float ReadFloat(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x3) == 0)
                {
                    //aligned read
                    return *((float*)addr);
                }
                else
                {
                    // unaligned read
                    float val;
                    byte* valPtr = (byte*)&val;
                    valPtr[0] = addr[0];
                    valPtr[1] = addr[1];
                    valPtr[2] = addr[2];
                    valPtr[3] = addr[3];
                    return val;
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe short ReadShort(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x1) == 0)
                {
                    //aligned read
                    return *((short*)addr);
                }
                else
                {
                    // unaligned read
                    short val;
                    byte* valPtr = (byte*)&val;
                    valPtr[0] = addr[0];
                    valPtr[1] = addr[1];
                    return val;
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe ushort ReadUShort(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x1) == 0)
                {
                    //aligned read
                    return *((ushort*)addr);
                }
                else
                {
                    // unaligned read
                    ushort val;
                    byte* valPtr = (byte*)&val;
                    valPtr[0] = addr[0];
                    valPtr[1] = addr[1];
                    return val;
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe byte ReadByte(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                return *(addr);
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe long ReadInt64(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x7) == 0)
                {
                    //aligned read
                    return *((long*)addr);
                }
                else
                {
                    // unaligned read
                    long val;
                    byte* valPtr = (byte*)&val;
                    valPtr[0] = addr[0];
                    valPtr[1] = addr[1];
                    valPtr[2] = addr[2];
                    valPtr[3] = addr[3];

                    valPtr[4] = addr[4];
                    valPtr[5] = addr[5];
                    valPtr[6] = addr[6];
                    valPtr[7] = addr[7];

                    return val;
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }


        public static unsafe ulong ReadUInt64(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x7) == 0)
                {
                    //aligned read
                    return *((ulong*)addr);
                }
                else
                {
                    // unaligned read
                    ulong val;
                    byte* valPtr = (byte*)&val;
                    valPtr[0] = addr[0];
                    valPtr[1] = addr[1];
                    valPtr[2] = addr[2];
                    valPtr[3] = addr[3];

                    valPtr[4] = addr[4];
                    valPtr[5] = addr[5];
                    valPtr[6] = addr[6];
                    valPtr[7] = addr[7];

                    return val;
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static unsafe double ReadDouble(byte[] value)
        {

            return ReadDouble(value.AsMemory(0, 8).Pin().Pointer, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe double ReadDouble(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x7) == 0)
                {
                    //aligned read
                    return *((double*)addr);
                }
                else
                {
                    // unaligned read
                    double val;
                    byte* valPtr = (byte*)&val;
                    valPtr[0] = addr[0];
                    valPtr[1] = addr[1];
                    valPtr[2] = addr[2];
                    valPtr[3] = addr[3];

                    valPtr[4] = addr[4];
                    valPtr[5] = addr[5];
                    valPtr[6] = addr[6];
                    valPtr[7] = addr[7];

                    return val;
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="ofs"></param>
        /// <returns></returns>
        public static unsafe DateTime ReadDateTime(void* ptr, long ofs)
        {
            try
            {
                byte* addr = (byte*)ptr + ofs;
                if ((unchecked((int)addr) & 0x7) == 0)
                {
                    //aligned read
                    return *((DateTime*)addr);
                }
                else
                {
                    // unaligned read
                    DateTime val;
                    byte* valPtr = (byte*)&val;
                    valPtr[0] = addr[0];
                    valPtr[1] = addr[1];
                    valPtr[2] = addr[2];
                    valPtr[3] = addr[3];

                    valPtr[4] = addr[4];
                    valPtr[5] = addr[5];
                    valPtr[6] = addr[6];
                    valPtr[7] = addr[7];

                    return val;
                }
            }
            catch (NullReferenceException)
            {
                // this method is documented to throw AccessViolationException on any AV
                throw new AccessViolationException();
            }
        }
        #endregion
    }
}
