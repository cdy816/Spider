using System;

namespace Opc.Ae
{
	// Token: 0x020000D0 RID: 208
	public class EnabledStateResult : IResult
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x000116EA File Offset: 0x000106EA
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x000116F2 File Offset: 0x000106F2
		public bool Enabled
		{
			get
			{
				return this.m_enabled;
			}
			set
			{
				this.m_enabled = value;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x000116FB File Offset: 0x000106FB
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x00011703 File Offset: 0x00010703
		public bool EffectivelyEnabled
		{
			get
			{
				return this.m_effectivelyEnabled;
			}
			set
			{
				this.m_effectivelyEnabled = value;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001170C File Offset: 0x0001070C
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x00011714 File Offset: 0x00010714
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

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0001171D File Offset: 0x0001071D
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x00011725 File Offset: 0x00010725
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

		// Token: 0x06000707 RID: 1799 RVA: 0x0001172E File Offset: 0x0001072E
		public EnabledStateResult()
		{
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00011741 File Offset: 0x00010741
		public EnabledStateResult(string qualifiedName)
		{
			this.m_qualifiedName = qualifiedName;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001175B File Offset: 0x0001075B
		public EnabledStateResult(string qualifiedName, ResultID resultID)
		{
			this.m_qualifiedName = qualifiedName;
			this.m_resultID = this.ResultID;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00011781 File Offset: 0x00010781
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x0400032E RID: 814
		private string m_qualifiedName;

		// Token: 0x0400032F RID: 815
		private bool m_enabled;

		// Token: 0x04000330 RID: 816
		private bool m_effectivelyEnabled;

		// Token: 0x04000331 RID: 817
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x04000332 RID: 818
		private string m_diagnosticInfo;
	}
}
