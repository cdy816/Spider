using System;

namespace Opc.Hda
{
	// Token: 0x020000D6 RID: 214
	[Serializable]
	public struct TimeOffset
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00011D69 File Offset: 0x00010D69
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x00011D71 File Offset: 0x00010D71
		public int Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x00011D7A File Offset: 0x00010D7A
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x00011D82 File Offset: 0x00010D82
		public RelativeTime Type
		{
			get
			{
				return this.m_type;
			}
			set
			{
				this.m_type = value;
			}
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00011D8C File Offset: 0x00010D8C
		internal static string OffsetTypeToString(RelativeTime offsetType)
		{
			switch (offsetType)
			{
			case RelativeTime.Second:
				return "S";
			case RelativeTime.Minute:
				return "M";
			case RelativeTime.Hour:
				return "H";
			case RelativeTime.Day:
				return "D";
			case RelativeTime.Week:
				return "W";
			case RelativeTime.Month:
				return "MO";
			case RelativeTime.Year:
				return "Y";
			default:
				throw new ArgumentOutOfRangeException("offsetType", offsetType.ToString(), "Invalid value for relative time offset type.");
			}
		}

		// Token: 0x04000344 RID: 836
		private int m_value;

		// Token: 0x04000345 RID: 837
		private RelativeTime m_type;
	}
}
