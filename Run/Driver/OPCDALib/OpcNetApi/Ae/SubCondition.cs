using System;

namespace Opc.Ae
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	public class SubCondition : ICloneable
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000605F File Offset: 0x0000505F
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00006067 File Offset: 0x00005067
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00006070 File Offset: 0x00005070
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00006078 File Offset: 0x00005078
		public string Definition
		{
			get
			{
				return this.m_definition;
			}
			set
			{
				this.m_definition = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00006081 File Offset: 0x00005081
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00006089 File Offset: 0x00005089
		public int Severity
		{
			get
			{
				return this.m_severity;
			}
			set
			{
				this.m_severity = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00006092 File Offset: 0x00005092
		// (set) Token: 0x06000172 RID: 370 RVA: 0x0000609A File Offset: 0x0000509A
		public string Description
		{
			get
			{
				return this.m_description;
			}
			set
			{
				this.m_description = value;
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000060A3 File Offset: 0x000050A3
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000060AB File Offset: 0x000050AB
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x040000DF RID: 223
		private string m_name;

		// Token: 0x040000E0 RID: 224
		private string m_definition;

		// Token: 0x040000E1 RID: 225
		private int m_severity = 1;

		// Token: 0x040000E2 RID: 226
		private string m_description;
	}
}
