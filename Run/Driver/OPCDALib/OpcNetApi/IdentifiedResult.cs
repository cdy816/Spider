using System;

namespace Opc
{
	// Token: 0x0200009A RID: 154
	[Serializable]
	public class IdentifiedResult : ItemIdentifier, IResult
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x0000DC46 File Offset: 0x0000CC46
		public IdentifiedResult()
		{
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000DC59 File Offset: 0x0000CC59
		public IdentifiedResult(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000DC6D File Offset: 0x0000CC6D
		public IdentifiedResult(IdentifiedResult item) : base(item)
		{
			if (item != null)
			{
				this.ResultID = item.ResultID;
				this.DiagnosticInfo = item.DiagnosticInfo;
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000DC9C File Offset: 0x0000CC9C
		public IdentifiedResult(string itemName, ResultID resultID) : base(itemName)
		{
			this.ResultID = resultID;
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000DCB7 File Offset: 0x0000CCB7
		public IdentifiedResult(string itemName, ResultID resultID, string diagnosticInfo) : base(itemName)
		{
			this.ResultID = resultID;
			this.DiagnosticInfo = diagnosticInfo;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000DCD9 File Offset: 0x0000CCD9
		public IdentifiedResult(ItemIdentifier item, ResultID resultID) : base(item)
		{
			this.ResultID = resultID;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000DCF4 File Offset: 0x0000CCF4
		public IdentifiedResult(ItemIdentifier item, ResultID resultID, string diagnosticInfo) : base(item)
		{
			this.ResultID = resultID;
			this.DiagnosticInfo = diagnosticInfo;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x0000DD16 File Offset: 0x0000CD16
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x0000DD1E File Offset: 0x0000CD1E
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

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x0000DD27 File Offset: 0x0000CD27
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x0000DD2F File Offset: 0x0000CD2F
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

		// Token: 0x04000253 RID: 595
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x04000254 RID: 596
		private string m_diagnosticInfo;
	}
}
