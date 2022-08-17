using System;

namespace OpcRcw.Da
{
	// Token: 0x020000AC RID: 172
	public static class Qualities
	{
		// Token: 0x04000424 RID: 1060
		public const short OPC_QUALITY_MASK = 192;

		// Token: 0x04000425 RID: 1061
		public const short OPC_STATUS_MASK = 252;

		// Token: 0x04000426 RID: 1062
		public const short OPC_LIMIT_MASK = 3;

		// Token: 0x04000427 RID: 1063
		public const short OPC_QUALITY_BAD = 0;

		// Token: 0x04000428 RID: 1064
		public const short OPC_QUALITY_UNCERTAIN = 64;

		// Token: 0x04000429 RID: 1065
		public const short OPC_QUALITY_GOOD = 192;

		// Token: 0x0400042A RID: 1066
		public const short OPC_QUALITY_CONFIG_ERROR = 4;

		// Token: 0x0400042B RID: 1067
		public const short OPC_QUALITY_NOT_CONNECTED = 8;

		// Token: 0x0400042C RID: 1068
		public const short OPC_QUALITY_DEVICE_FAILURE = 12;

		// Token: 0x0400042D RID: 1069
		public const short OPC_QUALITY_SENSOR_FAILURE = 16;

		// Token: 0x0400042E RID: 1070
		public const short OPC_QUALITY_LAST_KNOWN = 20;

		// Token: 0x0400042F RID: 1071
		public const short OPC_QUALITY_COMM_FAILURE = 24;

		// Token: 0x04000430 RID: 1072
		public const short OPC_QUALITY_OUT_OF_SERVICE = 28;

		// Token: 0x04000431 RID: 1073
		public const short OPC_QUALITY_WAITING_FOR_INITIAL_DATA = 32;

		// Token: 0x04000432 RID: 1074
		public const short OPC_QUALITY_LAST_USABLE = 68;

		// Token: 0x04000433 RID: 1075
		public const short OPC_QUALITY_SENSOR_CAL = 80;

		// Token: 0x04000434 RID: 1076
		public const short OPC_QUALITY_EGU_EXCEEDED = 84;

		// Token: 0x04000435 RID: 1077
		public const short OPC_QUALITY_SUB_NORMAL = 88;

		// Token: 0x04000436 RID: 1078
		public const short OPC_QUALITY_LOCAL_OVERRIDE = 216;

		// Token: 0x04000437 RID: 1079
		public const short OPC_LIMIT_OK = 0;

		// Token: 0x04000438 RID: 1080
		public const short OPC_LIMIT_LOW = 1;

		// Token: 0x04000439 RID: 1081
		public const short OPC_LIMIT_HIGH = 2;

		// Token: 0x0400043A RID: 1082
		public const short OPC_LIMIT_CONST = 3;
	}
}
