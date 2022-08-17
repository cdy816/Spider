using System;
using System.Collections;
using System.Reflection;

namespace Opc.Da
{
	// Token: 0x0200008F RID: 143
	[Serializable]
	public class PropertyDescription
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0000C545 File Offset: 0x0000B545
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x0000C54D File Offset: 0x0000B54D
		public PropertyID ID
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0000C556 File Offset: 0x0000B556
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x0000C55E File Offset: 0x0000B55E
		public System.Type Type
		{
			get
			{
				return this.m_type;
			}
			set
			{
				this.m_type = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000C567 File Offset: 0x0000B567
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x0000C56F File Offset: 0x0000B56F
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

		// Token: 0x06000416 RID: 1046 RVA: 0x0000C578 File Offset: 0x0000B578
		public PropertyDescription(PropertyID id, System.Type type, string name)
		{
			this.ID = id;
			this.Type = type;
			this.Name = name;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000C595 File Offset: 0x0000B595
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000C5A0 File Offset: 0x0000B5A0
		public static PropertyDescription Find(PropertyID id)
		{
			FieldInfo[] fields = typeof(PropertyDescription).GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				PropertyDescription propertyDescription = (PropertyDescription)fieldInfo.GetValue(typeof(PropertyDescription));
				if (propertyDescription.ID == id)
				{
					return propertyDescription;
				}
			}
			return null;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000C608 File Offset: 0x0000B608
		public static PropertyDescription[] Enumerate()
		{
			ArrayList arrayList = new ArrayList();
			FieldInfo[] fields = typeof(PropertyDescription).GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				arrayList.Add(fieldInfo.GetValue(typeof(PropertyDescription)));
			}
			return (PropertyDescription[])arrayList.ToArray(typeof(PropertyDescription));
		}

		// Token: 0x040001FE RID: 510
		private PropertyID m_id;

		// Token: 0x040001FF RID: 511
		private System.Type m_type;

		// Token: 0x04000200 RID: 512
		private string m_name;

		// Token: 0x04000201 RID: 513
		public static readonly PropertyDescription DATATYPE = new PropertyDescription(Property.DATATYPE, typeof(Type), "Item Canonical DataType");

		// Token: 0x04000202 RID: 514
		public static readonly PropertyDescription VALUE = new PropertyDescription(Property.VALUE, typeof(object), "Item Value");

		// Token: 0x04000203 RID: 515
		public static readonly PropertyDescription QUALITY = new PropertyDescription(Property.QUALITY, typeof(Quality), "Item Quality");

		// Token: 0x04000204 RID: 516
		public static readonly PropertyDescription TIMESTAMP = new PropertyDescription(Property.TIMESTAMP, typeof(DateTime), "Item Timestamp");

		// Token: 0x04000205 RID: 517
		public static readonly PropertyDescription ACCESSRIGHTS = new PropertyDescription(Property.ACCESSRIGHTS, typeof(accessRights), "Item Access Rights");

		// Token: 0x04000206 RID: 518
		public static readonly PropertyDescription SCANRATE = new PropertyDescription(Property.SCANRATE, typeof(float), "Server Scan Rate");

		// Token: 0x04000207 RID: 519
		public static readonly PropertyDescription EUTYPE = new PropertyDescription(Property.EUTYPE, typeof(euType), "Item EU Type");

		// Token: 0x04000208 RID: 520
		public static readonly PropertyDescription EUINFO = new PropertyDescription(Property.EUINFO, typeof(string[]), "Item EU Info");

		// Token: 0x04000209 RID: 521
		public static readonly PropertyDescription ENGINEERINGUINTS = new PropertyDescription(Property.ENGINEERINGUINTS, typeof(string), "EU Units");

		// Token: 0x0400020A RID: 522
		public static readonly PropertyDescription DESCRIPTION = new PropertyDescription(Property.DESCRIPTION, typeof(string), "Item Description");

		// Token: 0x0400020B RID: 523
		public static readonly PropertyDescription HIGHEU = new PropertyDescription(Property.HIGHEU, typeof(double), "High EU");

		// Token: 0x0400020C RID: 524
		public static readonly PropertyDescription LOWEU = new PropertyDescription(Property.LOWEU, typeof(double), "Low EU");

		// Token: 0x0400020D RID: 525
		public static readonly PropertyDescription HIGHIR = new PropertyDescription(Property.HIGHIR, typeof(double), "High Instrument Range");

		// Token: 0x0400020E RID: 526
		public static readonly PropertyDescription LOWIR = new PropertyDescription(Property.LOWIR, typeof(double), "Low Instrument Range");

		// Token: 0x0400020F RID: 527
		public static readonly PropertyDescription CLOSELABEL = new PropertyDescription(Property.CLOSELABEL, typeof(string), "Contact Close Label");

		// Token: 0x04000210 RID: 528
		public static readonly PropertyDescription OPENLABEL = new PropertyDescription(Property.OPENLABEL, typeof(string), "Contact Open Label");

		// Token: 0x04000211 RID: 529
		public static readonly PropertyDescription TIMEZONE = new PropertyDescription(Property.TIMEZONE, typeof(int), "Timezone");

		// Token: 0x04000212 RID: 530
		public static readonly PropertyDescription CONDITION_STATUS = new PropertyDescription(Property.CONDITION_STATUS, typeof(string), "Condition Status");

		// Token: 0x04000213 RID: 531
		public static readonly PropertyDescription ALARM_QUICK_HELP = new PropertyDescription(Property.ALARM_QUICK_HELP, typeof(string), "Alarm Quick Help");

		// Token: 0x04000214 RID: 532
		public static readonly PropertyDescription ALARM_AREA_LIST = new PropertyDescription(Property.ALARM_AREA_LIST, typeof(string), "Alarm Area List");

		// Token: 0x04000215 RID: 533
		public static readonly PropertyDescription PRIMARY_ALARM_AREA = new PropertyDescription(Property.PRIMARY_ALARM_AREA, typeof(string), "Primary Alarm Area");

		// Token: 0x04000216 RID: 534
		public static readonly PropertyDescription CONDITION_LOGIC = new PropertyDescription(Property.CONDITION_LOGIC, typeof(string), "Condition Logic");

		// Token: 0x04000217 RID: 535
		public static readonly PropertyDescription LIMIT_EXCEEDED = new PropertyDescription(Property.LIMIT_EXCEEDED, typeof(string), "Limit Exceeded");

		// Token: 0x04000218 RID: 536
		public static readonly PropertyDescription DEADBAND = new PropertyDescription(Property.DEADBAND, typeof(double), "Deadband");

		// Token: 0x04000219 RID: 537
		public static readonly PropertyDescription HIHI_LIMIT = new PropertyDescription(Property.HIHI_LIMIT, typeof(double), "HiHi Limit");

		// Token: 0x0400021A RID: 538
		public static readonly PropertyDescription HI_LIMIT = new PropertyDescription(Property.HI_LIMIT, typeof(double), "Hi Limit");

		// Token: 0x0400021B RID: 539
		public static readonly PropertyDescription LO_LIMIT = new PropertyDescription(Property.LO_LIMIT, typeof(double), "Lo Limit");

		// Token: 0x0400021C RID: 540
		public static readonly PropertyDescription LOLO_LIMIT = new PropertyDescription(Property.LOLO_LIMIT, typeof(double), "LoLo Limit");

		// Token: 0x0400021D RID: 541
		public static readonly PropertyDescription RATE_CHANGE_LIMIT = new PropertyDescription(Property.RATE_CHANGE_LIMIT, typeof(double), "Rate of Change Limit");

		// Token: 0x0400021E RID: 542
		public static readonly PropertyDescription DEVIATION_LIMIT = new PropertyDescription(Property.DEVIATION_LIMIT, typeof(double), "Deviation Limit");

		// Token: 0x0400021F RID: 543
		public static readonly PropertyDescription SOUNDFILE = new PropertyDescription(Property.SOUNDFILE, typeof(string), "Sound File");

		// Token: 0x04000220 RID: 544
		public static readonly PropertyDescription TYPE_SYSTEM_ID = new PropertyDescription(Property.TYPE_SYSTEM_ID, typeof(string), "Type System ID");

		// Token: 0x04000221 RID: 545
		public static readonly PropertyDescription DICTIONARY_ID = new PropertyDescription(Property.DICTIONARY_ID, typeof(string), "Dictionary ID");

		// Token: 0x04000222 RID: 546
		public static readonly PropertyDescription TYPE_ID = new PropertyDescription(Property.TYPE_ID, typeof(string), "Type ID");

		// Token: 0x04000223 RID: 547
		public static readonly PropertyDescription DICTIONARY = new PropertyDescription(Property.DICTIONARY, typeof(object), "Dictionary");

		// Token: 0x04000224 RID: 548
		public static readonly PropertyDescription TYPE_DESCRIPTION = new PropertyDescription(Property.TYPE_DESCRIPTION, typeof(string), "Type Description");

		// Token: 0x04000225 RID: 549
		public static readonly PropertyDescription CONSISTENCY_WINDOW = new PropertyDescription(Property.CONSISTENCY_WINDOW, typeof(string), "Consistency Window");

		// Token: 0x04000226 RID: 550
		public static readonly PropertyDescription WRITE_BEHAVIOR = new PropertyDescription(Property.WRITE_BEHAVIOR, typeof(string), "Write Behavior");

		// Token: 0x04000227 RID: 551
		public static readonly PropertyDescription UNCONVERTED_ITEM_ID = new PropertyDescription(Property.UNCONVERTED_ITEM_ID, typeof(string), "Unconverted Item ID");

		// Token: 0x04000228 RID: 552
		public static readonly PropertyDescription UNFILTERED_ITEM_ID = new PropertyDescription(Property.UNFILTERED_ITEM_ID, typeof(string), "Unfiltered Item ID");

		// Token: 0x04000229 RID: 553
		public static readonly PropertyDescription DATA_FILTER_VALUE = new PropertyDescription(Property.DATA_FILTER_VALUE, typeof(string), "Data Filter Value");

		// Token: 0x0400022A RID: 554
		public static readonly PropertyDescription MINIMUM_VALUE = new PropertyDescription(Property.MINIMUM_VALUE, typeof(object), "Minimum Value");

		// Token: 0x0400022B RID: 555
		public static readonly PropertyDescription MAXIMUM_VALUE = new PropertyDescription(Property.MAXIMUM_VALUE, typeof(object), "Maximum Value");

		// Token: 0x0400022C RID: 556
		public static readonly PropertyDescription VALUE_PRECISION = new PropertyDescription(Property.VALUE_PRECISION, typeof(object), "Value Precision");
	}
}
