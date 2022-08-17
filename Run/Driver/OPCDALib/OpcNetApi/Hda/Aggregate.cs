using System;

namespace Opc.Hda
{
	// Token: 0x0200002F RID: 47
	[Serializable]
	public class Aggregate : ICloneable
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000053DA File Offset: 0x000043DA
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000053E2 File Offset: 0x000043E2
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000053EB File Offset: 0x000043EB
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x000053F3 File Offset: 0x000043F3
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

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x000053FC File Offset: 0x000043FC
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00005404 File Offset: 0x00004404
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

		// Token: 0x060000F6 RID: 246 RVA: 0x0000540D File Offset: 0x0000440D
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005415 File Offset: 0x00004415
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x040000A7 RID: 167
		private int m_id;

		// Token: 0x040000A8 RID: 168
		private string m_name;

		// Token: 0x040000A9 RID: 169
		private string m_description;
	}
}
