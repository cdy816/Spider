using System;

namespace OpcRcw.Hda
{
	// Token: 0x0200004B RID: 75
	public static class Constants
	{
		// Token: 0x0400022B RID: 555
		public const string OPC_CATEGORY_DESCRIPTION_HDA10 = "OPC History Data Access Servers Version 1.0";

		// Token: 0x0400022C RID: 556
		public const int OPCHDA_DATA_TYPE = 1;

		// Token: 0x0400022D RID: 557
		public const int OPCHDA_DESCRIPTION = 2;

		// Token: 0x0400022E RID: 558
		public const int OPCHDA_ENG_UNITS = 3;

		// Token: 0x0400022F RID: 559
		public const int OPCHDA_STEPPED = 4;

		// Token: 0x04000230 RID: 560
		public const int OPCHDA_ARCHIVING = 5;

		// Token: 0x04000231 RID: 561
		public const int OPCHDA_DERIVE_EQUATION = 6;

		// Token: 0x04000232 RID: 562
		public const int OPCHDA_NODE_NAME = 7;

		// Token: 0x04000233 RID: 563
		public const int OPCHDA_PROCESS_NAME = 8;

		// Token: 0x04000234 RID: 564
		public const int OPCHDA_SOURCE_NAME = 9;

		// Token: 0x04000235 RID: 565
		public const int OPCHDA_SOURCE_TYPE = 10;

		// Token: 0x04000236 RID: 566
		public const int OPCHDA_NORMAL_MAXIMUM = 11;

		// Token: 0x04000237 RID: 567
		public const int OPCHDA_NORMAL_MINIMUM = 12;

		// Token: 0x04000238 RID: 568
		public const int OPCHDA_ITEMID = 13;

		// Token: 0x04000239 RID: 569
		public const int OPCHDA_MAX_TIME_INT = 14;

		// Token: 0x0400023A RID: 570
		public const int OPCHDA_MIN_TIME_INT = 15;

		// Token: 0x0400023B RID: 571
		public const int OPCHDA_EXCEPTION_DEV = 16;

		// Token: 0x0400023C RID: 572
		public const int OPCHDA_EXCEPTION_DEV_TYPE = 17;

		// Token: 0x0400023D RID: 573
		public const int OPCHDA_HIGH_ENTRY_LIMIT = 18;

		// Token: 0x0400023E RID: 574
		public const int OPCHDA_LOW_ENTRY_LIMIT = 19;

		// Token: 0x0400023F RID: 575
		public const string OPCHDA_ATTRNAME_DATA_TYPE = "Data Type";

		// Token: 0x04000240 RID: 576
		public const string OPCHDA_ATTRNAME_DESCRIPTION = "Description";

		// Token: 0x04000241 RID: 577
		public const string OPCHDA_ATTRNAME_ENG_UNITS = "Eng Units";

		// Token: 0x04000242 RID: 578
		public const string OPCHDA_ATTRNAME_STEPPED = "Stepped";

		// Token: 0x04000243 RID: 579
		public const string OPCHDA_ATTRNAME_ARCHIVING = "Archiving";

		// Token: 0x04000244 RID: 580
		public const string OPCHDA_ATTRNAME_DERIVE_EQUATION = "Derive Equation";

		// Token: 0x04000245 RID: 581
		public const string OPCHDA_ATTRNAME_NODE_NAME = "Node Name";

		// Token: 0x04000246 RID: 582
		public const string OPCHDA_ATTRNAME_PROCESS_NAME = "Process Name";

		// Token: 0x04000247 RID: 583
		public const string OPCHDA_ATTRNAME_SOURCE_NAME = "Source Name";

		// Token: 0x04000248 RID: 584
		public const string OPCHDA_ATTRNAME_SOURCE_TYPE = "Source Type";

		// Token: 0x04000249 RID: 585
		public const string OPCHDA_ATTRNAME_NORMAL_MAXIMUM = "Normal Maximum";

		// Token: 0x0400024A RID: 586
		public const string OPCHDA_ATTRNAME_NORMAL_MINIMUM = "Normal Minimum";

		// Token: 0x0400024B RID: 587
		public const string OPCHDA_ATTRNAME_ITEMID = "ItemID";

		// Token: 0x0400024C RID: 588
		public const string OPCHDA_ATTRNAME_MAX_TIME_INT = "Max Time Interval";

		// Token: 0x0400024D RID: 589
		public const string OPCHDA_ATTRNAME_MIN_TIME_INT = "Min Time Interval";

		// Token: 0x0400024E RID: 590
		public const string OPCHDA_ATTRNAME_EXCEPTION_DEV = "Exception Deviation";

		// Token: 0x0400024F RID: 591
		public const string OPCHDA_ATTRNAME_EXCEPTION_DEV_TYPE = "Exception Dev Type";

		// Token: 0x04000250 RID: 592
		public const string OPCHDA_ATTRNAME_HIGH_ENTRY_LIMIT = "High Entry Limit";

		// Token: 0x04000251 RID: 593
		public const string OPCHDA_ATTRNAME_LOW_ENTRY_LIMIT = "Low Entry Limit";

		// Token: 0x04000252 RID: 594
		public const string OPCHDA_AGGRNAME_INTERPOLATIVE = "Interpolative";

		// Token: 0x04000253 RID: 595
		public const string OPCHDA_AGGRNAME_TOTAL = "Total";

		// Token: 0x04000254 RID: 596
		public const string OPCHDA_AGGRNAME_AVERAGE = "Average";

		// Token: 0x04000255 RID: 597
		public const string OPCHDA_AGGRNAME_TIMEAVERAGE = "Time Average";

		// Token: 0x04000256 RID: 598
		public const string OPCHDA_AGGRNAME_COUNT = "Count";

		// Token: 0x04000257 RID: 599
		public const string OPCHDA_AGGRNAME_STDEV = "Standard Deviation";

		// Token: 0x04000258 RID: 600
		public const string OPCHDA_AGGRNAME_MINIMUMACTUALTIME = "Minimum Actual Time";

		// Token: 0x04000259 RID: 601
		public const string OPCHDA_AGGRNAME_MINIMUM = "Minimum";

		// Token: 0x0400025A RID: 602
		public const string OPCHDA_AGGRNAME_MAXIMUMACTUALTIME = "Maximum Actual Time";

		// Token: 0x0400025B RID: 603
		public const string OPCHDA_AGGRNAME_MAXIMUM = "Maximum";

		// Token: 0x0400025C RID: 604
		public const string OPCHDA_AGGRNAME_START = "Start";

		// Token: 0x0400025D RID: 605
		public const string OPCHDA_AGGRNAME_END = "End";

		// Token: 0x0400025E RID: 606
		public const string OPCHDA_AGGRNAME_DELTA = "Delta";

		// Token: 0x0400025F RID: 607
		public const string OPCHDA_AGGRNAME_REGSLOPE = "Regression Line Slope";

		// Token: 0x04000260 RID: 608
		public const string OPCHDA_AGGRNAME_REGCONST = "Regression Line Constant";

		// Token: 0x04000261 RID: 609
		public const string OPCHDA_AGGRNAME_REGDEV = "Regression Line Error";

		// Token: 0x04000262 RID: 610
		public const string OPCHDA_AGGRNAME_VARIANCE = "Variance";

		// Token: 0x04000263 RID: 611
		public const string OPCHDA_AGGRNAME_RANGE = "Range";

		// Token: 0x04000264 RID: 612
		public const string OPCHDA_AGGRNAME_DURATIONGOOD = "Duration Good";

		// Token: 0x04000265 RID: 613
		public const string OPCHDA_AGGRNAME_DURATIONBAD = "Duration Bad";

		// Token: 0x04000266 RID: 614
		public const string OPCHDA_AGGRNAME_PERCENTGOOD = "Percent Good";

		// Token: 0x04000267 RID: 615
		public const string OPCHDA_AGGRNAME_PERCENTBAD = "Percent Bad";

		// Token: 0x04000268 RID: 616
		public const string OPCHDA_AGGRNAME_WORSTQUALITY = "Worst Quality";

		// Token: 0x04000269 RID: 617
		public const string OPCHDA_AGGRNAME_ANNOTATIONS = "Annotations";

		// Token: 0x0400026A RID: 618
		public const int OPCHDA_EXTRADATA = 65536;

		// Token: 0x0400026B RID: 619
		public const int OPCHDA_INTERPOLATED = 131072;

		// Token: 0x0400026C RID: 620
		public const int OPCHDA_RAW = 262144;

		// Token: 0x0400026D RID: 621
		public const int OPCHDA_CALCULATED = 524288;

		// Token: 0x0400026E RID: 622
		public const int OPCHDA_NOBOUND = 1048576;

		// Token: 0x0400026F RID: 623
		public const int OPCHDA_NODATA = 2097152;

		// Token: 0x04000270 RID: 624
		public const int OPCHDA_DATALOST = 4194304;

		// Token: 0x04000271 RID: 625
		public const int OPCHDA_CONVERSION = 8388608;

		// Token: 0x04000272 RID: 626
		public const int OPCHDA_PARTIAL = 16777216;
	}
}
