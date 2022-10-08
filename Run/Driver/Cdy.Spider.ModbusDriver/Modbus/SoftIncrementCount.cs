using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.ModbusDriver
{
    public sealed class SoftIncrementCount : IDisposable
    {
        /// <summary>
        /// 实例化一个自增信息的对象，包括最大值，初始值，增量值<br />
        /// Instantiate an object with incremental information, including the maximum value and initial value, IncreaseTick
        /// </summary>
        /// <param name="max">数据的最大值，必须指定</param>
        /// <param name="start">数据的起始值，默认为0</param>
        /// <param name="tick">每次的增量值</param>
        public SoftIncrementCount(long max, long start = 0L, int tick = 1)
        {
            this.start = start;
            this.max = max;
            this.current = start;
            this.IncreaseTick = tick;
        }

        /// <summary>
        /// 获取自增信息，获得数据之后，下一次获取将会自增，如果自增后大于最大值，则会重置为最小值，如果小于最小值，则会重置为最大值。<br />
        /// Get the auto-increment information. After getting the data, the next acquisition will auto-increase. 
        /// If the auto-increment is greater than the maximum value, it will reset to the minimum value.
        /// If the auto-increment is smaller than the minimum value, it will reset to the maximum value.
        /// </summary>
        /// <returns>计数自增后的值</returns>
        public long GetCurrentValue()
        {
            lock (this)
            {
                long result = this.current;
                this.current += (long)this.IncreaseTick;
                if (this.current > this.max)
                {
                    this.current = this.start;
                }
                else
                {
                    if (this.current < this.start)
                    {
                        this.current = this.max;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 重置当前序号的最大值，最大值应该大于初始值，如果当前值大于最大值，则当前值被重置为最大值<br />
        /// Reset the maximum value of the current serial number. The maximum value should be greater than the initial value. 
        /// If the current value is greater than the maximum value, the current value is reset to the maximum value.
        /// </summary>
        /// <param name="max">最大值</param>
        // Token: 0x06002C22 RID: 11298 RVA: 0x000E5248 File Offset: 0x000E3448
        public void ResetMaxValue(long max)
        {
            lock (this)
            {
                if (max > this.start)
                {
                    if (max < this.current)
                    {
                        this.current = this.start;
                    }
                    this.max = max;
                }
            }
        }

        /// <summary>
        /// 重置当前序号的初始值，需要小于最大值，如果当前值小于初始值，则当前值被重置为初始值。<br />
        /// To reset the initial value of the current serial number, it must be less than the maximum value. 
        /// If the current value is less than the initial value, the current value is reset to the initial value.
        /// </summary>
        /// <param name="start">初始值</param>
        public void ResetStartValue(long start)
        {
            lock (this)
            {
                if (start < this.max)
                {
                    if (this.current < start)
                    {
                        this.current = start;
                    }
                    this.start = start;
                }
            }
        }

        /// <summary>
        /// 将当前的值重置为初始值。<br />
        /// Reset the current value to the initial value.
        /// </summary>
        // Token: 0x06002C24 RID: 11300 RVA: 0x000E52F0 File Offset: 0x000E34F0
        public void ResetCurrentValue()
        {
            lock (this)
            {
                this.current = this.start;
            }
        }

        /// <summary>
        /// 将当前的值重置为指定值，该值不能大于max，如果大于max值，就会自动设置为max<br />
        /// Reset the current value to the specified value. The value cannot be greater than max. If it is greater than max, it will be automatically set to max.
        /// </summary>
        /// <param name="value">指定的数据值</param>
        // Token: 0x06002C25 RID: 11301 RVA: 0x000E5318 File Offset: 0x000E3518
        public void ResetCurrentValue(long value)
        {
            lock (this)
            {
                if (value > this.max)
                {
                    this.current = this.max;
                }
                else
                {
                    if (value < this.start)
                    {
                        this.current = this.start;
                    }
                    else
                    {
                        this.current = value;
                    }
                }
            }
        }

        /// <summary>
        /// 增加的单元，如果设置为0，就是不增加。如果为小于0，那就是减少，会变成负数的可能。<br />
        /// Increased units, if set to 0, do not increase. If it is less than 0, it is a decrease and it may become a negative number.
        /// </summary>
        public int IncreaseTick { get; set; } = 1;

        /// <summary>
        /// 获取当前的计数器的最大的设置值。<br />
        /// Get the maximum setting value of the current counter.
        /// </summary>
        public long MaxValue
        {
            get
            {
                return this.max;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("SoftIncrementCount[{0}]", this.current);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            bool flag = !this.disposedValue;
            if (flag)
            {
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        private long start = 0L;

        private long current = 0L;

        private long max = long.MaxValue;

        private bool disposedValue = false;
    }
}
