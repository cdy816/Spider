using System;

namespace Opc.Da
{
	// Token: 0x020000BB RID: 187
	[Serializable]
	public class ItemResult : Item, IResult
	{
		// Token: 0x06000661 RID: 1633 RVA: 0x00010C25 File Offset: 0x0000FC25
		public ItemResult()
		{
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00010C38 File Offset: 0x0000FC38
		public ItemResult(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00010C4C File Offset: 0x0000FC4C
		public ItemResult(ItemIdentifier item, ResultID resultID) : base(item)
		{
			this.ResultID = this.ResultID;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00010C6C File Offset: 0x0000FC6C
		public ItemResult(Item item) : base(item)
		{
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00010C80 File Offset: 0x0000FC80
		public ItemResult(Item item, ResultID resultID) : base(item)
		{
			this.ResultID = resultID;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00010C9B File Offset: 0x0000FC9B
		public ItemResult(ItemResult item) : base(item)
		{
			if (item != null)
			{
				this.ResultID = item.ResultID;
				this.DiagnosticInfo = item.DiagnosticInfo;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00010CCA File Offset: 0x0000FCCA
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x00010CD2 File Offset: 0x0000FCD2
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

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x00010CDB File Offset: 0x0000FCDB
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x00010CE3 File Offset: 0x0000FCE3
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

		// Token: 0x040002BA RID: 698
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x040002BB RID: 699
		private string m_diagnosticInfo;
	}
}
