using System;

namespace Opc.Da
{
	// Token: 0x020000BD RID: 189
	[Serializable]
	public class ItemValueResult : ItemValue, IResult
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x00010E77 File Offset: 0x0000FE77
		public ItemValueResult()
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00010E8A File Offset: 0x0000FE8A
		public ItemValueResult(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00010E9E File Offset: 0x0000FE9E
		public ItemValueResult(ItemValue item) : base(item)
		{
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00010EB2 File Offset: 0x0000FEB2
		public ItemValueResult(ItemValueResult item) : base(item)
		{
			if (item != null)
			{
				this.ResultID = item.ResultID;
				this.DiagnosticInfo = item.DiagnosticInfo;
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00010EE1 File Offset: 0x0000FEE1
		public ItemValueResult(string itemName, ResultID resultID) : base(itemName)
		{
			this.ResultID = resultID;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00010EFC File Offset: 0x0000FEFC
		public ItemValueResult(string itemName, ResultID resultID, string diagnosticInfo) : base(itemName)
		{
			this.ResultID = resultID;
			this.DiagnosticInfo = diagnosticInfo;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00010F1E File Offset: 0x0000FF1E
		public ItemValueResult(ItemIdentifier item, ResultID resultID) : base(item)
		{
			this.ResultID = resultID;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00010F39 File Offset: 0x0000FF39
		public ItemValueResult(ItemIdentifier item, ResultID resultID, string diagnosticInfo) : base(item)
		{
			this.ResultID = resultID;
			this.DiagnosticInfo = diagnosticInfo;
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x00010F5B File Offset: 0x0000FF5B
		// (set) Token: 0x06000683 RID: 1667 RVA: 0x00010F63 File Offset: 0x0000FF63
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

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x00010F6C File Offset: 0x0000FF6C
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x00010F74 File Offset: 0x0000FF74
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

		// Token: 0x040002C1 RID: 705
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x040002C2 RID: 706
		private string m_diagnosticInfo;
	}
}
