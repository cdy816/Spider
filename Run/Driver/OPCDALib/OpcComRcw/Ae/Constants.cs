using System;

namespace OpcRcw.Ae
{
	// Token: 0x0200007C RID: 124
	public static class Constants
	{
		// Token: 0x04000385 RID: 901
		public const string OPC_CATEGORY_DESCRIPTION_AE10 = "OPC Alarm & Event Server Version 1.0";

		// Token: 0x04000386 RID: 902
		public const int CONDITION_ENABLED = 1;

		// Token: 0x04000387 RID: 903
		public const int CONDITION_ACTIVE = 2;

		// Token: 0x04000388 RID: 904
		public const int CONDITION_ACKED = 4;

		// Token: 0x04000389 RID: 905
		public const int CHANGE_ACTIVE_STATE = 1;

		// Token: 0x0400038A RID: 906
		public const int CHANGE_ACK_STATE = 2;

		// Token: 0x0400038B RID: 907
		public const int CHANGE_ENABLE_STATE = 4;

		// Token: 0x0400038C RID: 908
		public const int CHANGE_QUALITY = 8;

		// Token: 0x0400038D RID: 909
		public const int CHANGE_SEVERITY = 16;

		// Token: 0x0400038E RID: 910
		public const int CHANGE_SUBCONDITION = 32;

		// Token: 0x0400038F RID: 911
		public const int CHANGE_MESSAGE = 64;

		// Token: 0x04000390 RID: 912
		public const int CHANGE_ATTRIBUTE = 128;

		// Token: 0x04000391 RID: 913
		public const int SIMPLE_EVENT = 1;

		// Token: 0x04000392 RID: 914
		public const int TRACKING_EVENT = 2;

		// Token: 0x04000393 RID: 915
		public const int CONDITION_EVENT = 4;

		// Token: 0x04000394 RID: 916
		public const int ALL_EVENTS = 7;

		// Token: 0x04000395 RID: 917
		public const int FILTER_BY_EVENT = 1;

		// Token: 0x04000396 RID: 918
		public const int FILTER_BY_CATEGORY = 2;

		// Token: 0x04000397 RID: 919
		public const int FILTER_BY_SEVERITY = 4;

		// Token: 0x04000398 RID: 920
		public const int FILTER_BY_AREA = 8;

		// Token: 0x04000399 RID: 921
		public const int FILTER_BY_SOURCE = 16;
	}
}
