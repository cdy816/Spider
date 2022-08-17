using System;
using Opc.Da;

namespace Opc.Ae
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public class Condition : ICloneable
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00005A79 File Offset: 0x00004A79
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00005A81 File Offset: 0x00004A81
		public int State
		{
			get
			{
				return this.m_state;
			}
			set
			{
				this.m_state = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00005A8A File Offset: 0x00004A8A
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00005A92 File Offset: 0x00004A92
		public SubCondition ActiveSubCondition
		{
			get
			{
				return this.m_activeSubcondition;
			}
			set
			{
				this.m_activeSubcondition = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00005A9B File Offset: 0x00004A9B
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00005AA3 File Offset: 0x00004AA3
		public Quality Quality
		{
			get
			{
				return this.m_quality;
			}
			set
			{
				this.m_quality = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00005AAC File Offset: 0x00004AAC
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00005AB4 File Offset: 0x00004AB4
		public DateTime LastAckTime
		{
			get
			{
				return this.m_lastAckTime;
			}
			set
			{
				this.m_lastAckTime = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00005ABD File Offset: 0x00004ABD
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00005AC5 File Offset: 0x00004AC5
		public DateTime SubCondLastActive
		{
			get
			{
				return this.m_subCondLastActive;
			}
			set
			{
				this.m_subCondLastActive = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00005ACE File Offset: 0x00004ACE
		// (set) Token: 0x0600013C RID: 316 RVA: 0x00005AD6 File Offset: 0x00004AD6
		public DateTime CondLastActive
		{
			get
			{
				return this.m_condLastActive;
			}
			set
			{
				this.m_condLastActive = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00005ADF File Offset: 0x00004ADF
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00005AE7 File Offset: 0x00004AE7
		public DateTime CondLastInactive
		{
			get
			{
				return this.m_condLastInactive;
			}
			set
			{
				this.m_condLastInactive = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00005AF0 File Offset: 0x00004AF0
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00005AF8 File Offset: 0x00004AF8
		public string AcknowledgerID
		{
			get
			{
				return this.m_acknowledgerID;
			}
			set
			{
				this.m_acknowledgerID = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00005B01 File Offset: 0x00004B01
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00005B09 File Offset: 0x00004B09
		public string Comment
		{
			get
			{
				return this.m_comment;
			}
			set
			{
				this.m_comment = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00005B12 File Offset: 0x00004B12
		public Condition.SubConditionCollection SubConditions
		{
			get
			{
				return this.m_subconditions;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00005B1A File Offset: 0x00004B1A
		public Condition.AttributeValueCollection Attributes
		{
			get
			{
				return this.m_attributes;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005B24 File Offset: 0x00004B24
		public virtual object Clone()
		{
			Condition condition = (Condition)base.MemberwiseClone();
			condition.m_activeSubcondition = (SubCondition)this.m_activeSubcondition.Clone();
			condition.m_subconditions = (Condition.SubConditionCollection)this.m_subconditions.Clone();
			condition.m_attributes = (Condition.AttributeValueCollection)this.m_attributes.Clone();
			return condition;
		}

		// Token: 0x040000C9 RID: 201
		private int m_state;

		// Token: 0x040000CA RID: 202
		private SubCondition m_activeSubcondition = new SubCondition();

		// Token: 0x040000CB RID: 203
		private Quality m_quality = Quality.Bad;

		// Token: 0x040000CC RID: 204
		private DateTime m_lastAckTime = DateTime.MinValue;

		// Token: 0x040000CD RID: 205
		private DateTime m_subCondLastActive = DateTime.MinValue;

		// Token: 0x040000CE RID: 206
		private DateTime m_condLastActive = DateTime.MinValue;

		// Token: 0x040000CF RID: 207
		private DateTime m_condLastInactive = DateTime.MinValue;

		// Token: 0x040000D0 RID: 208
		private string m_acknowledgerID;

		// Token: 0x040000D1 RID: 209
		private string m_comment;

		// Token: 0x040000D2 RID: 210
		private Condition.SubConditionCollection m_subconditions = new Condition.SubConditionCollection();

		// Token: 0x040000D3 RID: 211
		private Condition.AttributeValueCollection m_attributes = new Condition.AttributeValueCollection();

		// Token: 0x02000037 RID: 55
		public class AttributeValueCollection : WriteableCollection
		{
			// Token: 0x1700003D RID: 61
			public AttributeValue this[int index]
			{
				get
				{
					return (AttributeValue)this.Array[index];
				}
			}

			// Token: 0x06000166 RID: 358 RVA: 0x00006002 File Offset: 0x00005002
			public new AttributeValue[] ToArray()
			{
				return (AttributeValue[])this.Array.ToArray();
			}

			// Token: 0x06000167 RID: 359 RVA: 0x00006014 File Offset: 0x00005014
			internal AttributeValueCollection() : base(null, typeof(AttributeValue))
			{
			}
		}

		// Token: 0x02000038 RID: 56
		public class SubConditionCollection : WriteableCollection
		{
			// Token: 0x1700003E RID: 62
			public SubCondition this[int index]
			{
				get
				{
					return (SubCondition)this.Array[index];
				}
			}

			// Token: 0x06000169 RID: 361 RVA: 0x0000603A File Offset: 0x0000503A
			public new SubCondition[] ToArray()
			{
				return (SubCondition[])this.Array.ToArray();
			}

			// Token: 0x0600016A RID: 362 RVA: 0x0000604C File Offset: 0x0000504C
			internal SubConditionCollection() : base(null, typeof(SubCondition))
			{
			}
		}
	}
}
