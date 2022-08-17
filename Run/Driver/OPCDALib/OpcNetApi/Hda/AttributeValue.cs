using System;

namespace Opc.Hda
{
	// Token: 0x020000DA RID: 218
	[Serializable]
	public class AttributeValue : ICloneable
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x000122FA File Offset: 0x000112FA
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x00012302 File Offset: 0x00011302
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

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0001230B File Offset: 0x0001130B
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x00012313 File Offset: 0x00011313
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

		// Token: 0x06000756 RID: 1878 RVA: 0x0001231C File Offset: 0x0001131C
		public virtual object Clone()
		{
			AttributeValue attributeValue = (AttributeValue)base.MemberwiseClone();
			attributeValue.m_value = Convert.Clone(this.m_value);
			return attributeValue;
		}

		// Token: 0x0400034B RID: 843
		private object m_value;

		// Token: 0x0400034C RID: 844
		private DateTime m_timestamp = DateTime.MinValue;
	}
}
