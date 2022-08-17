using System;

namespace Opc.Da
{
	// Token: 0x020000BC RID: 188
	[Serializable]
	public class ItemValue : ItemIdentifier
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00010CEC File Offset: 0x0000FCEC
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x00010CF4 File Offset: 0x0000FCF4
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

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x00010CFD File Offset: 0x0000FCFD
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x00010D05 File Offset: 0x0000FD05
		public Quality Quality
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

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00010D0E File Offset: 0x0000FD0E
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x00010D16 File Offset: 0x0000FD16
		public bool QualitySpecified
		{
			get
			{
				return this.m_qualitySpecified;
			}
			set
			{
				this.m_qualitySpecified = value;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00010D1F File Offset: 0x0000FD1F
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x00010D27 File Offset: 0x0000FD27
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

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x00010D30 File Offset: 0x0000FD30
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x00010D38 File Offset: 0x0000FD38
		public bool TimestampSpecified
		{
			get
			{
				return this.m_timestampSpecified;
			}
			set
			{
				this.m_timestampSpecified = value;
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00010D41 File Offset: 0x0000FD41
		public ItemValue()
		{
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00010D60 File Offset: 0x0000FD60
		public ItemValue(ItemIdentifier item)
		{
			if (item != null)
			{
				base.ItemName = item.ItemName;
				base.ItemPath = item.ItemPath;
				base.ClientHandle = item.ClientHandle;
				base.ServerHandle = item.ServerHandle;
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00010DBC File Offset: 0x0000FDBC
		public ItemValue(string itemName) : base(itemName)
		{
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00010DDC File Offset: 0x0000FDDC
		public ItemValue(ItemValue item) : base(item)
		{
			if (item != null)
			{
				this.Value = Convert.Clone(item.Value);
				this.Quality = item.Quality;
				this.QualitySpecified = item.QualitySpecified;
				this.Timestamp = item.Timestamp;
				this.TimestampSpecified = item.TimestampSpecified;
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00010E4C File Offset: 0x0000FE4C
		public override object Clone()
		{
			ItemValue itemValue = (ItemValue)base.MemberwiseClone();
			itemValue.Value = Convert.Clone(this.Value);
			return itemValue;
		}

		// Token: 0x040002BC RID: 700
		private object m_value;

		// Token: 0x040002BD RID: 701
		private Quality m_quality = Quality.Bad;

		// Token: 0x040002BE RID: 702
		private bool m_qualitySpecified;

		// Token: 0x040002BF RID: 703
		private DateTime m_timestamp = DateTime.MinValue;

		// Token: 0x040002C0 RID: 704
		private bool m_timestampSpecified;
	}
}
