using System;

namespace Opc
{
	// Token: 0x02000093 RID: 147
	[Serializable]
	public struct Specification
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000CBA9 File Offset: 0x0000BBA9
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x0000CBB1 File Offset: 0x0000BBB1
		public string ID
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

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000CBBA File Offset: 0x0000BBBA
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x0000CBC2 File Offset: 0x0000BBC2
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

		// Token: 0x06000422 RID: 1058 RVA: 0x0000CBCB File Offset: 0x0000BBCB
		public static bool operator ==(Specification a, Specification b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000CBE0 File Offset: 0x0000BBE0
		public static bool operator !=(Specification a, Specification b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000CBF8 File Offset: 0x0000BBF8
		public Specification(string id, string description)
		{
			this.m_id = id;
			this.m_description = description;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000CC08 File Offset: 0x0000BC08
		public override bool Equals(object target)
		{
			return target != null && target.GetType() == typeof(Specification) && this.ID == ((Specification)target).ID;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000CC45 File Offset: 0x0000BC45
		public override string ToString()
		{
			return this.Description;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000CC4D File Offset: 0x0000BC4D
		public override int GetHashCode()
		{
			if (this.ID == null)
			{
				return base.GetHashCode();
			}
			return this.ID.GetHashCode();
		}

		// Token: 0x04000235 RID: 565
		private string m_id;

		// Token: 0x04000236 RID: 566
		private string m_description;

		// Token: 0x04000237 RID: 567
		public static readonly Specification COM_AE_10 = new Specification("58E13251-AC87-11d1-84D5-00608CB8A7E9", "Alarms and Event 1.XX");

		// Token: 0x04000238 RID: 568
		public static readonly Specification COM_BATCH_10 = new Specification("A8080DA0-E23E-11D2-AFA7-00C04F539421", "Batch 1.00");

		// Token: 0x04000239 RID: 569
		public static readonly Specification COM_BATCH_20 = new Specification("843DE67B-B0C9-11d4-A0B7-000102A980B1", "Batch 2.00");

		// Token: 0x0400023A RID: 570
		public static readonly Specification COM_DA_10 = new Specification("63D5F430-CFE4-11d1-B2C8-0060083BA1FB", "Data Access 1.0a");

		// Token: 0x0400023B RID: 571
		public static readonly Specification COM_DA_20 = new Specification("63D5F432-CFE4-11d1-B2C8-0060083BA1FB", "Data Access 2.XX");

		// Token: 0x0400023C RID: 572
		public static readonly Specification COM_DA_30 = new Specification("CC603642-66D7-48f1-B69A-B625E73652D7", "Data Access 3.00");

		// Token: 0x0400023D RID: 573
		public static readonly Specification COM_DX_10 = new Specification("A0C85BB8-4161-4fd6-8655-BB584601C9E0", "Data eXchange 1.00");

		// Token: 0x0400023E RID: 574
		public static readonly Specification COM_HDA_10 = new Specification("7DE5B060-E089-11d2-A5E6-000086339399", "Historical Data Access 1.XX");

		// Token: 0x0400023F RID: 575
		public static readonly Specification XML_DA_10 = new Specification("3098EDA4-A006-48b2-A27F-247453959408", "XML Data Access 1.00");

		// Token: 0x04000240 RID: 576
		public static readonly Specification UA10 = new Specification("EC10AFD8-9BC0-4828-B47E-B3D907F929B1", "Unified Architecture 1.00");
	}
}
