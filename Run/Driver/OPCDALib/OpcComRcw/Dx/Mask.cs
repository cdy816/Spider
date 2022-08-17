using System;

namespace OpcRcw.Dx
{
	// Token: 0x02000024 RID: 36
	public enum Mask
	{
		// Token: 0x040001B4 RID: 436
		None,
		// Token: 0x040001B5 RID: 437
		ItemPath,
		// Token: 0x040001B6 RID: 438
		ItemName,
		// Token: 0x040001B7 RID: 439
		Version = 4,
		// Token: 0x040001B8 RID: 440
		BrowsePaths = 8,
		// Token: 0x040001B9 RID: 441
		Name = 16,
		// Token: 0x040001BA RID: 442
		Description = 32,
		// Token: 0x040001BB RID: 443
		Keyword = 64,
		// Token: 0x040001BC RID: 444
		DefaultSourceItemConnected = 128,
		// Token: 0x040001BD RID: 445
		DefaultTargetItemConnected = 256,
		// Token: 0x040001BE RID: 446
		DefaultOverridden = 512,
		// Token: 0x040001BF RID: 447
		DefaultOverrideValue = 1024,
		// Token: 0x040001C0 RID: 448
		SubstituteValue = 2048,
		// Token: 0x040001C1 RID: 449
		EnableSubstituteValue = 4096,
		// Token: 0x040001C2 RID: 450
		TargetItemPath = 8192,
		// Token: 0x040001C3 RID: 451
		TargetItemName = 16384,
		// Token: 0x040001C4 RID: 452
		SourceServerName = 32768,
		// Token: 0x040001C5 RID: 453
		SourceItemPath = 65536,
		// Token: 0x040001C6 RID: 454
		SourceItemName = 131072,
		// Token: 0x040001C7 RID: 455
		SourceItemQueueSize = 262144,
		// Token: 0x040001C8 RID: 456
		UpdateRate = 524288,
		// Token: 0x040001C9 RID: 457
		DeadBand = 1048576,
		// Token: 0x040001CA RID: 458
		VendorData = 2097152,
		// Token: 0x040001CB RID: 459
		ServerType = 4194304,
		// Token: 0x040001CC RID: 460
		ServerURL = 8388608,
		// Token: 0x040001CD RID: 461
		DefaultSourceServerConnected = 16777216,
		// Token: 0x040001CE RID: 462
		All = 2147483647
	}
}
