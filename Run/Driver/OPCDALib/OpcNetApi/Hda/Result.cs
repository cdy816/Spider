using System;

namespace Opc.Hda
{
	// Token: 0x020000AD RID: 173
	[Serializable]
	public class Result : ICloneable, IResult
	{
		// Token: 0x060005C0 RID: 1472 RVA: 0x000100D4 File Offset: 0x0000F0D4
		public Result()
		{
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000100E7 File Offset: 0x0000F0E7
		public Result(ResultID resultID)
		{
			this.ResultID = resultID;
			this.DiagnosticInfo = null;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00010108 File Offset: 0x0000F108
		public Result(IResult result)
		{
			this.ResultID = result.ResultID;
			this.DiagnosticInfo = result.DiagnosticInfo;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00010133 File Offset: 0x0000F133
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x0001013B File Offset: 0x0000F13B
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

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00010144 File Offset: 0x0000F144
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0001014C File Offset: 0x0000F14C
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

		// Token: 0x060005C7 RID: 1479 RVA: 0x00010155 File Offset: 0x0000F155
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x0400029F RID: 671
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x040002A0 RID: 672
		private string m_diagnosticInfo;
	}
}
