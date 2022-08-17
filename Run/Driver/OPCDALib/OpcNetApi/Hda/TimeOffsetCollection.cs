using System;
using System.Collections;
using System.Text;

namespace Opc.Hda
{
	// Token: 0x020000D7 RID: 215
	[Serializable]
	public class TimeOffsetCollection : ArrayList
	{
		// Token: 0x170001B1 RID: 433
		public TimeOffset this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = value;
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00011E18 File Offset: 0x00010E18
		public int Add(int value, RelativeTime type)
		{
			return base.Add(new TimeOffset
			{
				Value = value,
				Type = type
			});
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00011E4C File Offset: 0x00010E4C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			foreach (object obj in ((IEnumerable)this))
			{
				TimeOffset timeOffset = (TimeOffset)obj;
				if (timeOffset.Value >= 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.AppendFormat("{0}", timeOffset.Value);
				stringBuilder.Append(TimeOffset.OffsetTypeToString(timeOffset.Type));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00011EEC File Offset: 0x00010EEC
		public void Parse(string buffer)
		{
			this.Clear();
			bool positive = true;
			int num = 0;
			string text = "";
			int num2 = 0;
			for (int i = 0; i < buffer.Length; i++)
			{
				if (buffer[i] == '+' || buffer[i] == '-')
				{
					if (num2 == 3)
					{
						this.Add(TimeOffsetCollection.CreateOffset(positive, num, text));
						num = 0;
						text = "";
						num2 = 0;
					}
					if (num2 != 0)
					{
						throw new FormatException("Unexpected token encountered while parsing relative time string.");
					}
					positive = (buffer[i] == '+');
					num2 = 1;
				}
				else if (char.IsDigit(buffer, i))
				{
					if (num2 == 3)
					{
						this.Add(TimeOffsetCollection.CreateOffset(positive, num, text));
						num = 0;
						text = "";
						num2 = 0;
					}
					if (num2 != 0 && num2 != 1 && num2 != 2)
					{
						throw new FormatException("Unexpected token encountered while parsing relative time string.");
					}
					num *= 10;
					num += System.Convert.ToInt32((int)(buffer[i] - '0'));
					num2 = 2;
				}
				else if (!char.IsWhiteSpace(buffer, i))
				{
					if (num2 != 2 && num2 != 3)
					{
						throw new FormatException("Unexpected token encountered while parsing relative time string.");
					}
					text += buffer[i];
					num2 = 3;
				}
			}
			if (num2 == 3)
			{
				this.Add(TimeOffsetCollection.CreateOffset(positive, num, text));
				num2 = 0;
			}
			if (num2 != 0)
			{
				throw new FormatException("Unexpected end of string encountered while parsing relative time string.");
			}
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001202C File Offset: 0x0001102C
		public void CopyTo(TimeOffset[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00012036 File Offset: 0x00011036
		public void Insert(int index, TimeOffset value)
		{
			this.Insert(index, value);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00012045 File Offset: 0x00011045
		public void Remove(TimeOffset value)
		{
			this.Remove(value);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00012053 File Offset: 0x00011053
		public bool Contains(TimeOffset value)
		{
			return this.Contains(value);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00012061 File Offset: 0x00011061
		public int IndexOf(TimeOffset value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001206F File Offset: 0x0001106F
		public int Add(TimeOffset value)
		{
			return this.Add(value);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00012080 File Offset: 0x00011080
		private static TimeOffset CreateOffset(bool positive, int magnitude, string units)
		{
			foreach (object obj in Enum.GetValues(typeof(RelativeTime)))
			{
				RelativeTime relativeTime = (RelativeTime)obj;
				if (relativeTime != RelativeTime.Now && units == TimeOffset.OffsetTypeToString(relativeTime))
				{
					return new TimeOffset
					{
						Value = (positive ? magnitude : (-magnitude)),
						Type = relativeTime
					};
				}
			}
			throw new ArgumentOutOfRangeException("units", units, "String is not a valid offset time type.");
		}
	}
}
