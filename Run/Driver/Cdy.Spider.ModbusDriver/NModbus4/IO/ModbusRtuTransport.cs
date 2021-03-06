﻿namespace Modbus.IO
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Cdy.Spider;
    using Message;
    using Utility;

    /// <summary>
    ///     Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
    /// </summary>
    internal class ModbusRtuTransport : ModbusSerialTransport
    {
        public const int RequestFrameStartLength = 7;

        public const int ResponseFrameStartLength = 4;

        internal ModbusRtuTransport(ICommChannel2 streamResource)
            : base(streamResource)
        {
            Debug.Assert(streamResource != null, "Argument streamResource cannot be null.");
        }

        public static int RequestBytesToRead(byte[] frameStart)
        {
            byte functionCode = frameStart[1];
            int numBytes;

            switch (functionCode)
            {
                case Modbus.ReadCoils:
                case Modbus.ReadInputs:
                case Modbus.ReadHoldingRegisters:
                case Modbus.ReadInputRegisters:
                case Modbus.WriteSingleCoil:
                case Modbus.WriteSingleRegister:
                case Modbus.Diagnostics:
                    numBytes = 1;
                    break;
                case Modbus.WriteMultipleCoils:
                case Modbus.WriteMultipleRegisters:
                    byte byteCount = frameStart[6];
                    numBytes = byteCount + 2;
                    break;
                default:
                    string msg = $"Function code {functionCode} not supported.";
                    Debug.WriteLine(msg);
                    throw new NotImplementedException(msg);
            }

            return numBytes;
        }

        public static int ResponseBytesToRead(byte[] frameStart)
        {
            byte functionCode = frameStart[1];

            // exception response
            if (functionCode > Modbus.ExceptionOffset)
            {
                return 1;
            }

            int numBytes;
            switch (functionCode)
            {
                case Modbus.ReadCoils:
                case Modbus.ReadInputs:
                case Modbus.ReadHoldingRegisters:
                case Modbus.ReadInputRegisters:
                    numBytes = frameStart[2] + 1;
                    break;
                case Modbus.WriteSingleCoil:
                case Modbus.WriteSingleRegister:
                case Modbus.WriteMultipleCoils:
                case Modbus.WriteMultipleRegisters:
                case Modbus.Diagnostics:
                    numBytes = 4;
                    break;
                default:
                    string msg = $"Function code {functionCode} not supported.";
                    Debug.WriteLine(msg);
                    throw new NotImplementedException(msg);
            }

            return numBytes;
        }

        public virtual byte[] Read(int count)
        {
            byte[] frameBytes = new byte[count];
            int numBytesRead = 0;

            while (numBytesRead != count)
            {
                int rcount = 0;
                var data = StreamResource.Read(count - numBytesRead, StreamResource.Data.Timeout, out rcount);

                if (rcount != 0)
                {
                    Array.Copy(data, 0, frameBytes, numBytesRead, rcount);
                    numBytesRead += rcount;
                }
                else
                {
                    LoggerService.Service.Warn("Modbusrtutransport", "Data send and receive is timeout: "+StreamResource.Data.DataSendTimeout);
                    return null;
                }
                //numBytesRead += StreamResource.Read(frameBytes, numBytesRead, count - numBytesRead);
            }

            return frameBytes;
        }

        internal override byte[] BuildMessageFrame(IModbusMessage message)
        {
            var messageFrame = message.MessageFrame;
            var crc = ModbusUtility.CalculateCrc(messageFrame);
            var messageBody = new MemoryStream(messageFrame.Length + crc.Length);

            messageBody.Write(messageFrame, 0, messageFrame.Length);
            messageBody.Write(crc, 0, crc.Length);

            return messageBody.ToArray();
        }

        internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
        {
            return BitConverter.ToUInt16(messageFrame, messageFrame.Length - 2) ==
                BitConverter.ToUInt16(ModbusUtility.CalculateCrc(message.MessageFrame), 0);
        }

        internal override IModbusMessage ReadResponse<T>()
        {
            try
            {
                byte[] frameStart = Read(ResponseFrameStartLength);
                if (frameStart == null)
                {
                    return null;
                }
                byte[] frameEnd = Read(ResponseBytesToRead(frameStart));
                if (frameEnd == null)
                {
                    return null;
                }
                byte[] frame = Enumerable.Concat(frameStart, frameEnd).ToArray();
#if DEBUG
                LoggerService.Service.Debug("ModbusRTU",$"RX: {string.Join(", ", frame)}");
#endif
                return CreateResponse<T>(frame);
            }
            catch(Exception ex)
            {
                LoggerService.Service.Warn("Modbusrtutransport",ex.Message);
                StreamResource.ClearBuffer();
                return null;
            }
        }

        internal override byte[] ReadRequest()
        {
            try
            {
                byte[] frameStart = Read(RequestFrameStartLength);
                if (frameStart == null)
                {
                    return null;
                }
                byte[] frameEnd = Read(RequestBytesToRead(frameStart));
                if (frameEnd == null)
                {
                    return null;
                }
                byte[] frame = Enumerable.Concat(frameStart, frameEnd).ToArray();
#if DEBUG
                LoggerService.Service.Debug("ModbusRTU", $"RX: {string.Join(", ", frame)}");
#endif

                return frame;
            }
            catch(Exception ex)
            {
                LoggerService.Service.Warn("Modbusrtutransport", ex.Message);
                StreamResource.ClearBuffer();
                return null;
                //throw ex;
            }
        }
    }
}
