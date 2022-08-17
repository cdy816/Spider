using System;

namespace Opc.Dx
{
	// Token: 0x0200009D RID: 157
	[Serializable]
	public class IdentifiedResult : ItemIdentifier
	{
		// Token: 0x060004B2 RID: 1202 RVA: 0x0000DFB6 File Offset: 0x0000CFB6
		public IdentifiedResult()
		{
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000DFC9 File Offset: 0x0000CFC9
		public IdentifiedResult(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000DFDD File Offset: 0x0000CFDD
		public IdentifiedResult(IdentifiedResult item) : base(item)
		{
			if (item != null)
			{
				this.ResultID = item.ResultID;
				this.DiagnosticInfo = item.DiagnosticInfo;
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000E00C File Offset: 0x0000D00C
		public IdentifiedResult(string itemName, ResultID resultID) : base(itemName)
		{
			this.ResultID = resultID;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000E027 File Offset: 0x0000D027
		public IdentifiedResult(string itemName, ResultID resultID, string diagnosticInfo) : base(itemName)
		{
			this.ResultID = resultID;
			this.DiagnosticInfo = diagnosticInfo;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000E049 File Offset: 0x0000D049
		public IdentifiedResult(ItemIdentifier item, ResultID resultID) : base(item)
		{
			this.ResultID = resultID;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000E064 File Offset: 0x0000D064
		public IdentifiedResult(ItemIdentifier item, ResultID resultID, string diagnosticInfo) : base(item)
		{
			this.ResultID = resultID;
			this.DiagnosticInfo = diagnosticInfo;
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0000E086 File Offset: 0x0000D086
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x0000E08E File Offset: 0x0000D08E
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

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0000E097 File Offset: 0x0000D097
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x0000E09F File Offset: 0x0000D09F
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

		// Token: 0x04000257 RID: 599
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x04000258 RID: 600
		private string m_diagnosticInfo;
	}
}
