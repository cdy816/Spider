using System;

namespace Opc.Ae
{
	// Token: 0x0200006B RID: 107
	[Serializable]
	public class AttributeValue : ICloneable, IResult
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00007348 File Offset: 0x00006348
		// (set) Token: 0x06000296 RID: 662 RVA: 0x00007350 File Offset: 0x00006350
		public int ID
		{
			get
			{
				return this.m_id;
			}
			set
			{
				this.m_id = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00007359 File Offset: 0x00006359
		// (set) Token: 0x06000298 RID: 664 RVA: 0x00007361 File Offset: 0x00006361
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

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000736A File Offset: 0x0000636A
		// (set) Token: 0x0600029A RID: 666 RVA: 0x00007372 File Offset: 0x00006372
		public ResultID ResultID
		{
			get
			{
				return this.m_resultID;
			}
			set
			{
				this.m_resultID = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000737B File Offset: 0x0000637B
		// (set) Token: 0x0600029C RID: 668 RVA: 0x00007383 File Offset: 0x00006383
		public string DiagnosticInfo
		{
			get
			{
				return this.m_diagnosticInfo;
			}
			set
			{
				this.m_diagnosticInfo = value;
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000738C File Offset: 0x0000638C
		public virtual object Clone()
		{
			AttributeValue attributeValue = (AttributeValue)base.MemberwiseClone();
			attributeValue.Value = Convert.Clone(this.Value);
			return attributeValue;
		}

		// Token: 0x0400013C RID: 316
		private int m_id;

		// Token: 0x0400013D RID: 317
		private object m_value;

		// Token: 0x0400013E RID: 318
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x0400013F RID: 319
		private string m_diagnosticInfo;
	}
}
