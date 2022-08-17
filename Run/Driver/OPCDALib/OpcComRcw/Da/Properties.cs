using System;

namespace OpcRcw.Da
{
	// Token: 0x020000AD RID: 173
	public static class Properties
	{
		// Token: 0x0400043B RID: 1083
		public const int OPC_PROPERTY_DATATYPE = 1;

		// Token: 0x0400043C RID: 1084
		public const int OPC_PROPERTY_VALUE = 2;

		// Token: 0x0400043D RID: 1085
		public const int OPC_PROPERTY_QUALITY = 3;

		// Token: 0x0400043E RID: 1086
		public const int OPC_PROPERTY_TIMESTAMP = 4;

		// Token: 0x0400043F RID: 1087
		public const int OPC_PROPERTY_ACCESS_RIGHTS = 5;

		// Token: 0x04000440 RID: 1088
		public const int OPC_PROPERTY_SCAN_RATE = 6;

		// Token: 0x04000441 RID: 1089
		public const int OPC_PROPERTY_EU_TYPE = 7;

		// Token: 0x04000442 RID: 1090
		public const int OPC_PROPERTY_EU_INFO = 8;

		// Token: 0x04000443 RID: 1091
		public const int OPC_PROPERTY_EU_UNITS = 100;

		// Token: 0x04000444 RID: 1092
		public const int OPC_PROPERTY_DESCRIPTION = 101;

		// Token: 0x04000445 RID: 1093
		public const int OPC_PROPERTY_HIGH_EU = 102;

		// Token: 0x04000446 RID: 1094
		public const int OPC_PROPERTY_LOW_EU = 103;

		// Token: 0x04000447 RID: 1095
		public const int OPC_PROPERTY_HIGH_IR = 104;

		// Token: 0x04000448 RID: 1096
		public const int OPC_PROPERTY_LOW_IR = 105;

		// Token: 0x04000449 RID: 1097
		public const int OPC_PROPERTY_CLOSE_LABEL = 106;

		// Token: 0x0400044A RID: 1098
		public const int OPC_PROPERTY_OPEN_LABEL = 107;

		// Token: 0x0400044B RID: 1099
		public const int OPC_PROPERTY_TIMEZONE = 108;

		// Token: 0x0400044C RID: 1100
		public const int OPC_PROPERTY_CONDITION_STATUS = 300;

		// Token: 0x0400044D RID: 1101
		public const int OPC_PROPERTY_ALARM_QUICK_HELP = 301;

		// Token: 0x0400044E RID: 1102
		public const int OPC_PROPERTY_ALARM_AREA_LIST = 302;

		// Token: 0x0400044F RID: 1103
		public const int OPC_PROPERTY_PRIMARY_ALARM_AREA = 303;

		// Token: 0x04000450 RID: 1104
		public const int OPC_PROPERTY_CONDITION_LOGIC = 304;

		// Token: 0x04000451 RID: 1105
		public const int OPC_PROPERTY_LIMIT_EXCEEDED = 305;

		// Token: 0x04000452 RID: 1106
		public const int OPC_PROPERTY_DEADBAND = 306;

		// Token: 0x04000453 RID: 1107
		public const int OPC_PROPERTY_HIHI_LIMIT = 307;

		// Token: 0x04000454 RID: 1108
		public const int OPC_PROPERTY_HI_LIMIT = 308;

		// Token: 0x04000455 RID: 1109
		public const int OPC_PROPERTY_LO_LIMIT = 309;

		// Token: 0x04000456 RID: 1110
		public const int OPC_PROPERTY_LOLO_LIMIT = 310;

		// Token: 0x04000457 RID: 1111
		public const int OPC_PROPERTY_CHANGE_RATE_LIMIT = 311;

		// Token: 0x04000458 RID: 1112
		public const int OPC_PROPERTY_DEVIATION_LIMIT = 312;

		// Token: 0x04000459 RID: 1113
		public const int OPC_PROPERTY_SOUND_FILE = 313;

		// Token: 0x0400045A RID: 1114
		public const int OPC_PROPERTY_TYPE_SYSTEM_ID = 600;

		// Token: 0x0400045B RID: 1115
		public const int OPC_PROPERTY_DICTIONARY_ID = 601;

		// Token: 0x0400045C RID: 1116
		public const int OPC_PROPERTY_TYPE_ID = 602;

		// Token: 0x0400045D RID: 1117
		public const int OPC_PROPERTY_DICTIONARY = 603;

		// Token: 0x0400045E RID: 1118
		public const int OPC_PROPERTY_TYPE_DESCRIPTION = 604;

		// Token: 0x0400045F RID: 1119
		public const int OPC_PROPERTY_CONSISTENCY_WINDOW = 605;

		// Token: 0x04000460 RID: 1120
		public const int OPC_PROPERTY_WRITE_BEHAVIOR = 606;

		// Token: 0x04000461 RID: 1121
		public const int OPC_PROPERTY_UNCONVERTED_ITEM_ID = 607;

		// Token: 0x04000462 RID: 1122
		public const int OPC_PROPERTY_UNFILTERED_ITEM_ID = 608;

		// Token: 0x04000463 RID: 1123
		public const int OPC_PROPERTY_DATA_FILTER_VALUE = 609;

		// Token: 0x04000464 RID: 1124
		public const string OPC_PROPERTY_DESC_DATATYPE = "Item Canonical Data Type";

		// Token: 0x04000465 RID: 1125
		public const string OPC_PROPERTY_DESC_VALUE = "Item Value";

		// Token: 0x04000466 RID: 1126
		public const string OPC_PROPERTY_DESC_QUALITY = "Item Quality";

		// Token: 0x04000467 RID: 1127
		public const string OPC_PROPERTY_DESC_TIMESTAMP = "Item Timestamp";

		// Token: 0x04000468 RID: 1128
		public const string OPC_PROPERTY_DESC_ACCESS_RIGHTS = "Item Access Rights";

		// Token: 0x04000469 RID: 1129
		public const string OPC_PROPERTY_DESC_SCAN_RATE = "Server Scan Rate";

		// Token: 0x0400046A RID: 1130
		public const string OPC_PROPERTY_DESC_EU_TYPE = "Item EU Type";

		// Token: 0x0400046B RID: 1131
		public const string OPC_PROPERTY_DESC_EU_INFO = "Item EU Info";

		// Token: 0x0400046C RID: 1132
		public const string OPC_PROPERTY_DESC_EU_UNITS = "EU Units";

		// Token: 0x0400046D RID: 1133
		public const string OPC_PROPERTY_DESC_DESCRIPTION = "Item Description";

		// Token: 0x0400046E RID: 1134
		public const string OPC_PROPERTY_DESC_HIGH_EU = "High EU";

		// Token: 0x0400046F RID: 1135
		public const string OPC_PROPERTY_DESC_LOW_EU = "Low EU";

		// Token: 0x04000470 RID: 1136
		public const string OPC_PROPERTY_DESC_HIGH_IR = "High Instrument Range";

		// Token: 0x04000471 RID: 1137
		public const string OPC_PROPERTY_DESC_LOW_IR = "Low Instrument Range";

		// Token: 0x04000472 RID: 1138
		public const string OPC_PROPERTY_DESC_CLOSE_LABEL = "Contact Close Label";

		// Token: 0x04000473 RID: 1139
		public const string OPC_PROPERTY_DESC_OPEN_LABEL = "Contact Open Label";

		// Token: 0x04000474 RID: 1140
		public const string OPC_PROPERTY_DESC_TIMEZONE = "Item Timezone";

		// Token: 0x04000475 RID: 1141
		public const string OPC_PROPERTY_DESC_CONDITION_STATUS = "Condition Status";

		// Token: 0x04000476 RID: 1142
		public const string OPC_PROPERTY_DESC_ALARM_QUICK_HELP = "Alarm Quick Help";

		// Token: 0x04000477 RID: 1143
		public const string OPC_PROPERTY_DESC_ALARM_AREA_LIST = "Alarm Area List";

		// Token: 0x04000478 RID: 1144
		public const string OPC_PROPERTY_DESC_PRIMARY_ALARM_AREA = "Primary Alarm Area";

		// Token: 0x04000479 RID: 1145
		public const string OPC_PROPERTY_DESC_CONDITION_LOGIC = "Condition Logic";

		// Token: 0x0400047A RID: 1146
		public const string OPC_PROPERTY_DESC_LIMIT_EXCEEDED = "Limit Exceeded";

		// Token: 0x0400047B RID: 1147
		public const string OPC_PROPERTY_DESC_DEADBAND = "Deadband";

		// Token: 0x0400047C RID: 1148
		public const string OPC_PROPERTY_DESC_HIHI_LIMIT = "HiHi Limit";

		// Token: 0x0400047D RID: 1149
		public const string OPC_PROPERTY_DESC_HI_LIMIT = "Hi Limit";

		// Token: 0x0400047E RID: 1150
		public const string OPC_PROPERTY_DESC_LO_LIMIT = "Lo Limit";

		// Token: 0x0400047F RID: 1151
		public const string OPC_PROPERTY_DESC_LOLO_LIMIT = "LoLo Limit";

		// Token: 0x04000480 RID: 1152
		public const string OPC_PROPERTY_DESC_CHANGE_RATE_LIMIT = "Rate of Change Limit";

		// Token: 0x04000481 RID: 1153
		public const string OPC_PROPERTY_DESC_DEVIATION_LIMIT = "Deviation Limit";

		// Token: 0x04000482 RID: 1154
		public const string OPC_PROPERTY_DESC_SOUND_FILE = "Sound File";

		// Token: 0x04000483 RID: 1155
		public const string OPC_PROPERTY_DESC_TYPE_SYSTEM_ID = "Type System ID";

		// Token: 0x04000484 RID: 1156
		public const string OPC_PROPERTY_DESC_DICTIONARY_ID = "Dictionary ID";

		// Token: 0x04000485 RID: 1157
		public const string OPC_PROPERTY_DESC_TYPE_ID = "Type ID";

		// Token: 0x04000486 RID: 1158
		public const string OPC_PROPERTY_DESC_DICTIONARY = "Dictionary";

		// Token: 0x04000487 RID: 1159
		public const string OPC_PROPERTY_DESC_TYPE_DESCRIPTION = "Type Description";

		// Token: 0x04000488 RID: 1160
		public const string OPC_PROPERTY_DESC_CONSISTENCY_WINDOW = "Consistency Window";

		// Token: 0x04000489 RID: 1161
		public const string OPC_PROPERTY_DESC_WRITE_BEHAVIOR = "Write Behavior";

		// Token: 0x0400048A RID: 1162
		public const string OPC_PROPERTY_DESC_UNCONVERTED_ITEM_ID = "Unconverted Item ID";

		// Token: 0x0400048B RID: 1163
		public const string OPC_PROPERTY_DESC_UNFILTERED_ITEM_ID = "Unfiltered Item ID";

		// Token: 0x0400048C RID: 1164
		public const string OPC_PROPERTY_DESC_DATA_FILTER_VALUE = "Data Filter Value";
	}
}
