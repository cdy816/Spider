using System;

namespace Opc.Hda
{
	// Token: 0x02000050 RID: 80
	[Serializable]
	public class AnnotationValue : ICloneable
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000066EA File Offset: 0x000056EA
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x000066F2 File Offset: 0x000056F2
		public DateTime Timestamp
		{
			get
			{
				return this.m_timestamp;
			}
			set
			{
				this.m_timestamp = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x000066FB File Offset: 0x000056FB
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00006703 File Offset: 0x00005703
		public string Annotation
		{
			get
			{
				return this.m_annotation;
			}
			set
			{
				this.m_annotation = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000670C File Offset: 0x0000570C
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00006714 File Offset: 0x00005714
		public DateTime CreationTime
		{
			get
			{
				return this.m_creationTime;
			}
			set
			{
				this.m_creationTime = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000671D File Offset: 0x0000571D
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00006725 File Offset: 0x00005725
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

		// Token: 0x060001EF RID: 495 RVA: 0x0000672E File Offset: 0x0000572E
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x040000FE RID: 254
		private DateTime m_timestamp = DateTime.MinValue;

		// Token: 0x040000FF RID: 255
		private string m_annotation;

		// Token: 0x04000100 RID: 256
		private DateTime m_creationTime = DateTime.MinValue;

		// Token: 0x04000101 RID: 257
		private string m_user;
	}
}
