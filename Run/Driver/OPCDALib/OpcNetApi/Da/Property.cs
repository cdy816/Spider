using System;

namespace Opc.Da
{
	// Token: 0x0200008E RID: 142
	public class Property
	{
		// Token: 0x040001D2 RID: 466
		public static readonly PropertyID DATATYPE = new PropertyID("dataType", 1, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001D3 RID: 467
		public static readonly PropertyID VALUE = new PropertyID("value", 2, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001D4 RID: 468
		public static readonly PropertyID QUALITY = new PropertyID("quality", 3, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001D5 RID: 469
		public static readonly PropertyID TIMESTAMP = new PropertyID("timestamp", 4, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001D6 RID: 470
		public static readonly PropertyID ACCESSRIGHTS = new PropertyID("accessRights", 5, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001D7 RID: 471
		public static readonly PropertyID SCANRATE = new PropertyID("scanRate", 6, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001D8 RID: 472
		public static readonly PropertyID EUTYPE = new PropertyID("euType", 7, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001D9 RID: 473
		public static readonly PropertyID EUINFO = new PropertyID("euInfo", 8, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001DA RID: 474
		public static readonly PropertyID ENGINEERINGUINTS = new PropertyID("engineeringUnits", 100, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001DB RID: 475
		public static readonly PropertyID DESCRIPTION = new PropertyID("description", 101, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001DC RID: 476
		public static readonly PropertyID HIGHEU = new PropertyID("highEU", 102, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001DD RID: 477
		public static readonly PropertyID LOWEU = new PropertyID("lowEU", 103, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001DE RID: 478
		public static readonly PropertyID HIGHIR = new PropertyID("highIR", 104, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001DF RID: 479
		public static readonly PropertyID LOWIR = new PropertyID("lowIR", 105, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E0 RID: 480
		public static readonly PropertyID CLOSELABEL = new PropertyID("closeLabel", 106, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E1 RID: 481
		public static readonly PropertyID OPENLABEL = new PropertyID("openLabel", 107, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E2 RID: 482
		public static readonly PropertyID TIMEZONE = new PropertyID("timeZone", 108, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E3 RID: 483
		public static readonly PropertyID CONDITION_STATUS = new PropertyID("conditionStatus", 300, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E4 RID: 484
		public static readonly PropertyID ALARM_QUICK_HELP = new PropertyID("alarmQuickHelp", 301, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E5 RID: 485
		public static readonly PropertyID ALARM_AREA_LIST = new PropertyID("alarmAreaList", 302, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E6 RID: 486
		public static readonly PropertyID PRIMARY_ALARM_AREA = new PropertyID("primaryAlarmArea", 303, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E7 RID: 487
		public static readonly PropertyID CONDITION_LOGIC = new PropertyID("conditionLogic", 304, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E8 RID: 488
		public static readonly PropertyID LIMIT_EXCEEDED = new PropertyID("limitExceeded", 305, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001E9 RID: 489
		public static readonly PropertyID DEADBAND = new PropertyID("deadband", 306, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001EA RID: 490
		public static readonly PropertyID HIHI_LIMIT = new PropertyID("hihiLimit", 307, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001EB RID: 491
		public static readonly PropertyID HI_LIMIT = new PropertyID("hiLimit", 308, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001EC RID: 492
		public static readonly PropertyID LO_LIMIT = new PropertyID("loLimit", 309, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001ED RID: 493
		public static readonly PropertyID LOLO_LIMIT = new PropertyID("loloLimit", 310, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001EE RID: 494
		public static readonly PropertyID RATE_CHANGE_LIMIT = new PropertyID("rangeOfChangeLimit", 311, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001EF RID: 495
		public static readonly PropertyID DEVIATION_LIMIT = new PropertyID("deviationLimit", 312, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F0 RID: 496
		public static readonly PropertyID SOUNDFILE = new PropertyID("soundFile", 313, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F1 RID: 497
		public static readonly PropertyID TYPE_SYSTEM_ID = new PropertyID("typeSystemID", 600, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F2 RID: 498
		public static readonly PropertyID DICTIONARY_ID = new PropertyID("dictionaryID", 601, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F3 RID: 499
		public static readonly PropertyID TYPE_ID = new PropertyID("typeID", 602, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F4 RID: 500
		public static readonly PropertyID DICTIONARY = new PropertyID("dictionary", 603, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F5 RID: 501
		public static readonly PropertyID TYPE_DESCRIPTION = new PropertyID("typeDescription", 604, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F6 RID: 502
		public static readonly PropertyID CONSISTENCY_WINDOW = new PropertyID("consistencyWindow", 605, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F7 RID: 503
		public static readonly PropertyID WRITE_BEHAVIOR = new PropertyID("writeBehavior", 606, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F8 RID: 504
		public static readonly PropertyID UNCONVERTED_ITEM_ID = new PropertyID("unconvertedItemID", 607, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001F9 RID: 505
		public static readonly PropertyID UNFILTERED_ITEM_ID = new PropertyID("unfilteredItemID", 608, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001FA RID: 506
		public static readonly PropertyID DATA_FILTER_VALUE = new PropertyID("dataFilterValue", 609, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001FB RID: 507
		public static readonly PropertyID MINIMUM_VALUE = new PropertyID("minimumValue", 109, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001FC RID: 508
		public static readonly PropertyID MAXIMUM_VALUE = new PropertyID("maximumValue", 110, "http://opcfoundation.org/DataAccess/");

		// Token: 0x040001FD RID: 509
		public static readonly PropertyID VALUE_PRECISION = new PropertyID("valuePrecision", 111, "http://opcfoundation.org/DataAccess/");
	}
}
