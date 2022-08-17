using System;

namespace Opc.Da
{
	// Token: 0x02000060 RID: 96
	public enum qualityBits
	{
		// Token: 0x0400010A RID: 266
		good = 192,
		// Token: 0x0400010B RID: 267
		goodLocalOverride = 216,
		// Token: 0x0400010C RID: 268
		bad = 0,
		// Token: 0x0400010D RID: 269
		badConfigurationError = 4,
		// Token: 0x0400010E RID: 270
		badNotConnected = 8,
		// Token: 0x0400010F RID: 271
		badDeviceFailure = 12,
		// Token: 0x04000110 RID: 272
		badSensorFailure = 16,
		// Token: 0x04000111 RID: 273
		badLastKnownValue = 20,
		// Token: 0x04000112 RID: 274
		badCommFailure = 24,
		// Token: 0x04000113 RID: 275
		badOutOfService = 28,
		// Token: 0x04000114 RID: 276
		badWaitingForInitialData = 32,
		// Token: 0x04000115 RID: 277
		uncertain = 64,
		// Token: 0x04000116 RID: 278
		uncertainLastUsableValue = 68,
		// Token: 0x04000117 RID: 279
		uncertainSensorNotAccurate = 80,
		// Token: 0x04000118 RID: 280
		uncertainEUExceeded = 84,
		// Token: 0x04000119 RID: 281
		uncertainSubNormal = 88
	}
}
