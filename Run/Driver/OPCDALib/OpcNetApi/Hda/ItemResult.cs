using System;

namespace Opc.Hda
{
	// Token: 0x020000A5 RID: 165
	[Serializable]
	public class ItemResult : Item, IResult
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x0000F851 File Offset: 0x0000E851
		public ItemResult()
		{
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0000F864 File Offset: 0x0000E864
		public ItemResult(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0000F878 File Offset: 0x0000E878
		public ItemResult(Item item) : base(item)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0000F88C File Offset: 0x0000E88C
		public ItemResult(ItemResult item) : base(item)
		{
			if (item != null)
			{
				this.ResultID = item.ResultID;
				this.DiagnosticInfo = item.DiagnosticInfo;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0000F8BB File Offset: 0x0000E8BB
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x0000F8C3 File Offset: 0x0000E8C3
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

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0000F8CC File Offset: 0x0000E8CC
		// (set) Token: 0x06000566 RID: 1382 RVA: 0x0000F8D4 File Offset: 0x0000E8D4
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

		// Token: 0x04000281 RID: 641
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x04000282 RID: 642
		private string m_diagnosticInfo;
	}
}
