using System;
using System.Text;

namespace Opc.Hda
{
	// Token: 0x020000D4 RID: 212
	[Serializable]
	public class Time
	{
		// Token: 0x06000719 RID: 1817 RVA: 0x000118D4 File Offset: 0x000108D4
		public Time()
		{
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x000118F2 File Offset: 0x000108F2
		public Time(DateTime time)
		{
			this.AbsoluteTime = time;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00011918 File Offset: 0x00010918
		public Time(string time)
		{
			Time time2 = Time.Parse(time);
			this.m_absoluteTime = DateTime.MinValue;
			this.m_baseTime = time2.m_baseTime;
			this.m_offsets = time2.m_offsets;
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0001196B File Offset: 0x0001096B
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0001197D File Offset: 0x0001097D
		public bool IsRelative
		{
			get
			{
				return this.m_absoluteTime == DateTime.MinValue;
			}
			set
			{
				this.m_absoluteTime = DateTime.MinValue;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001198A File Offset: 0x0001098A
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x00011992 File Offset: 0x00010992
		public DateTime AbsoluteTime
		{
			get
			{
				return this.m_absoluteTime;
			}
			set
			{
				this.m_absoluteTime = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001199B File Offset: 0x0001099B
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x000119A3 File Offset: 0x000109A3
		public RelativeTime BaseTime
		{
			get
			{
				return this.m_baseTime;
			}
			set
			{
				this.m_baseTime = value;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x000119AC File Offset: 0x000109AC
		public TimeOffsetCollection Offsets
		{
			get
			{
				return this.m_offsets;
			}
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x000119B4 File Offset: 0x000109B4
		public DateTime ResolveTime()
		{
			if (!this.IsRelative)
			{
				return this.m_absoluteTime;
			}
			DateTime result = DateTime.UtcNow;
			int year = result.Year;
			int month = result.Month;
			int day = result.Day;
			int hour = result.Hour;
			int minute = result.Minute;
			int second = result.Second;
			int millisecond = result.Millisecond;
			switch (this.BaseTime)
			{
			case RelativeTime.Second:
				millisecond = 0;
				break;
			case RelativeTime.Minute:
				second = 0;
				millisecond = 0;
				break;
			case RelativeTime.Hour:
				minute = 0;
				second = 0;
				millisecond = 0;
				break;
			case RelativeTime.Day:
			case RelativeTime.Week:
				hour = 0;
				minute = 0;
				second = 0;
				millisecond = 0;
				break;
			case RelativeTime.Month:
				day = 0;
				hour = 0;
				minute = 0;
				second = 0;
				millisecond = 0;
				break;
			case RelativeTime.Year:
				month = 0;
				day = 0;
				hour = 0;
				minute = 0;
				second = 0;
				millisecond = 0;
				break;
			}
			result = new DateTime(year, month, day, hour, minute, second, millisecond);
			if (this.BaseTime == RelativeTime.Week && result.DayOfWeek != DayOfWeek.Sunday)
			{
				result = result.AddDays((double)(-(double)result.DayOfWeek));
			}
			foreach (object obj in this.Offsets)
			{
				TimeOffset timeOffset = (TimeOffset)obj;
				switch (timeOffset.Type)
				{
				case RelativeTime.Second:
					result = result.AddSeconds((double)timeOffset.Value);
					break;
				case RelativeTime.Minute:
					result = result.AddMinutes((double)timeOffset.Value);
					break;
				case RelativeTime.Hour:
					result = result.AddHours((double)timeOffset.Value);
					break;
				case RelativeTime.Day:
					result = result.AddDays((double)timeOffset.Value);
					break;
				case RelativeTime.Week:
					result = result.AddDays((double)(timeOffset.Value * 7));
					break;
				case RelativeTime.Month:
					result = result.AddMonths(timeOffset.Value);
					break;
				case RelativeTime.Year:
					result = result.AddYears(timeOffset.Value);
					break;
				}
			}
			return result;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00011BBC File Offset: 0x00010BBC
		public override string ToString()
		{
			if (!this.IsRelative)
			{
				return Convert.ToString(this.m_absoluteTime);
			}
			StringBuilder stringBuilder = new StringBuilder(256);
			stringBuilder.Append(Time.BaseTypeToString(this.BaseTime));
			stringBuilder.Append(this.Offsets.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00011C18 File Offset: 0x00010C18
		public static Time Parse(string buffer)
		{
			buffer = buffer.Trim();
			Time time = new Time();
			bool flag = false;
			foreach (object obj in Enum.GetValues(typeof(RelativeTime)))
			{
				RelativeTime baseTime = (RelativeTime)obj;
				string text = Time.BaseTypeToString(baseTime);
				if (buffer.StartsWith(text))
				{
					buffer = buffer.Substring(text.Length).Trim();
					time.BaseTime = baseTime;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				time.AbsoluteTime = System.Convert.ToDateTime(buffer).ToUniversalTime();
				return time;
			}
			if (buffer.Length > 0)
			{
				time.Offsets.Parse(buffer);
			}
			return time;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00011CE8 File Offset: 0x00010CE8
		private static string BaseTypeToString(RelativeTime baseTime)
		{
			switch (baseTime)
			{
			case RelativeTime.Now:
				return "NOW";
			case RelativeTime.Second:
				return "SECOND";
			case RelativeTime.Minute:
				return "MINUTE";
			case RelativeTime.Hour:
				return "HOUR";
			case RelativeTime.Day:
				return "DAY";
			case RelativeTime.Week:
				return "WEEK";
			case RelativeTime.Month:
				return "MONTH";
			case RelativeTime.Year:
				return "YEAR";
			default:
				throw new ArgumentOutOfRangeException("baseTime", baseTime.ToString(), "Invalid value for relative base time.");
			}
		}

		// Token: 0x04000338 RID: 824
		private DateTime m_absoluteTime = DateTime.MinValue;

		// Token: 0x04000339 RID: 825
		private RelativeTime m_baseTime;

		// Token: 0x0400033A RID: 826
		private TimeOffsetCollection m_offsets = new TimeOffsetCollection();
	}
}
