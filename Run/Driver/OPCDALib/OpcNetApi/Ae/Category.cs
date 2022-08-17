using System;

namespace Opc.Ae
{
	// Token: 0x020000A1 RID: 161
	[Serializable]
	public class Category : ICloneable
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0000E72B File Offset: 0x0000D72B
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x0000E733 File Offset: 0x0000D733
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

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0000E73C File Offset: 0x0000D73C
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x0000E744 File Offset: 0x0000D744
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

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000E74D File Offset: 0x0000D74D
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000E755 File Offset: 0x0000D755
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x04000262 RID: 610
		private int m_id;

		// Token: 0x04000263 RID: 611
		private string m_name;
	}
}
