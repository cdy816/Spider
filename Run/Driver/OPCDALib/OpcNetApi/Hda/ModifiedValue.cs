using System;

namespace Opc.Hda
{
	// Token: 0x020000AA RID: 170
	[Serializable]
	public class ModifiedValue : ItemValue
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00010051 File Offset: 0x0000F051
		// (set) Token: 0x060005B4 RID: 1460 RVA: 0x00010059 File Offset: 0x0000F059
		public DateTime ModificationTime
		{
			get
			{
				return this.m_modificationTime;
			}
			set
			{
				this.m_modificationTime = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00010062 File Offset: 0x0000F062
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x0001006A File Offset: 0x0000F06A
		public EditType EditType
		{
			get
			{
				return this.m_editType;
			}
			set
			{
				this.m_editType = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00010073 File Offset: 0x0000F073
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x0001007B File Offset: 0x0000F07B
		public string User
		{
			get
			{
				return this.m_user;
			}
			set
			{
				this.m_user = value;
			}
		}

		// Token: 0x04000297 RID: 663
		private DateTime m_modificationTime = DateTime.MinValue;

		// Token: 0x04000298 RID: 664
		private EditType m_editType = EditType.Insert;

		// Token: 0x04000299 RID: 665
		private string m_user;
	}
}
