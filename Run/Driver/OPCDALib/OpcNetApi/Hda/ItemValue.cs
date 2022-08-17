using System;

namespace Opc.Hda
{
    // Token: 0x020000A8 RID: 168
    [Serializable]
	public class ItemValue : ICloneable
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0000FBBD File Offset: 0x0000EBBD
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0000FBC5 File Offset: 0x0000EBC5
		public object Value
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0000FBCE File Offset: 0x0000EBCE
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x0000FBD6 File Offset: 0x0000EBD6
		public DateTime Timestamp
		{
			get
			{
				return this.m_timestamp;
			}
			set
			{
				this.m_timestamp = value;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0000FBDF File Offset: 0x0000EBDF
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x0000FBE7 File Offset: 0x0000EBE7
		public Opc.Da.Quality Quality
		{
			get
			{
				return this.m_quality;
			}
			set
			{
				this.m_quality = value;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0000FBF0 File Offset: 0x0000EBF0
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x0000FBF8 File Offset: 0x0000EBF8
		public Quality HistorianQuality
		{
			get
			{
				return this.m_historianQuality;
			}
			set
			{
				this.m_historianQuality = value;
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0000FC04 File Offset: 0x0000EC04
		public object Clone()
		{
			ItemValue itemValue = (ItemValue)base.MemberwiseClone();
			itemValue.Value = Convert.Clone(this.Value);
			return itemValue;
		}

		// Token: 0x0400028E RID: 654
		private object m_value;

		// Token: 0x0400028F RID: 655
		private DateTime m_timestamp = DateTime.MinValue;

		// Token: 0x04000290 RID: 656
		private Opc.Da.Quality m_quality = Opc.Da.Quality.Bad;

		// Token: 0x04000291 RID: 657
		private Quality m_historianQuality = Opc.Hda.Quality.NoData;
	}
}
