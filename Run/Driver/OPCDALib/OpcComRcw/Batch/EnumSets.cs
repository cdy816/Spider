using System;

namespace OpcRcw.Batch
{
	// Token: 0x02000010 RID: 16
	public static class EnumSets
	{
		// Token: 0x04000062 RID: 98
		private const int OPCB_ENUM_PHYS = 0;

		// Token: 0x04000063 RID: 99
		private const int OPCB_ENUM_PROC = 1;

		// Token: 0x04000064 RID: 100
		private const int OPCB_ENUM_STATE = 2;

		// Token: 0x04000065 RID: 101
		private const int OPCB_ENUM_MODE = 3;

		// Token: 0x04000066 RID: 102
		private const int OPCB_ENUM_PARAM = 4;

		// Token: 0x04000067 RID: 103
		private const int OPCB_ENUM_MR_PROC = 5;

		// Token: 0x04000068 RID: 104
		private const int OPCB_ENUM_RE_USE = 6;

		// Token: 0x04000069 RID: 105
		private const int OPCB_PHYS_ENTERPRISE = 0;

		// Token: 0x0400006A RID: 106
		private const int OPCB_PHYS_SITE = 1;

		// Token: 0x0400006B RID: 107
		private const int OPCB_PHYS_AREA = 2;

		// Token: 0x0400006C RID: 108
		private const int OPCB_PHYS_PROCESSCELL = 3;

		// Token: 0x0400006D RID: 109
		private const int OPCB_PHYS_UNIT = 4;

		// Token: 0x0400006E RID: 110
		private const int OPCB_PHYS_EQUIPMENTMODULE = 5;

		// Token: 0x0400006F RID: 111
		private const int OPCB_PHYS_CONTROLMODULE = 6;

		// Token: 0x04000070 RID: 112
		private const int OPCB_PHYS_EPE = 7;

		// Token: 0x04000071 RID: 113
		private const int OPCB_PROC_PROCEDURE = 0;

		// Token: 0x04000072 RID: 114
		private const int OPCB_PROC_UNITPROCEDURE = 1;

		// Token: 0x04000073 RID: 115
		private const int OPCB_PROC_OPERATION = 2;

		// Token: 0x04000074 RID: 116
		private const int OPCB_PROC_PHASE = 3;

		// Token: 0x04000075 RID: 117
		private const int OPCB_PROC_PARAMETER_COLLECTION = 4;

		// Token: 0x04000076 RID: 118
		private const int OPCB_PROC_PARAMETER = 5;

		// Token: 0x04000077 RID: 119
		private const int OPCB_PROC_RESULT_COLLECTION = 6;

		// Token: 0x04000078 RID: 120
		private const int OPCB_PROC_RESULT = 7;

		// Token: 0x04000079 RID: 121
		private const int OPCB_PROC_BATCH = 8;

		// Token: 0x0400007A RID: 122
		private const int OPCB_PROC_CAMPAIGN = 9;

		// Token: 0x0400007B RID: 123
		private const int OPCB_STATE_IDLE = 0;

		// Token: 0x0400007C RID: 124
		private const int OPCB_STATE_RUNNING = 1;

		// Token: 0x0400007D RID: 125
		private const int OPCB_STATE_COMPLETE = 2;

		// Token: 0x0400007E RID: 126
		private const int OPCB_STATE_PAUSING = 3;

		// Token: 0x0400007F RID: 127
		private const int OPCB_STATE_PAUSED = 4;

		// Token: 0x04000080 RID: 128
		private const int OPCB_STATE_HOLDING = 5;

		// Token: 0x04000081 RID: 129
		private const int OPCB_STATE_HELD = 6;

		// Token: 0x04000082 RID: 130
		private const int OPCB_STATE_RESTARTING = 7;

		// Token: 0x04000083 RID: 131
		private const int OPCB_STATE_STOPPING = 8;

		// Token: 0x04000084 RID: 132
		private const int OPCB_STATE_STOPPED = 9;

		// Token: 0x04000085 RID: 133
		private const int OPCB_STATE_ABORTING = 10;

		// Token: 0x04000086 RID: 134
		private const int OPCB_STATE_ABORTED = 11;

		// Token: 0x04000087 RID: 135
		private const int OPCB_STATE_UNKNOWN = 12;

		// Token: 0x04000088 RID: 136
		private const int OPCB_MODE_AUTOMATIC = 0;

		// Token: 0x04000089 RID: 137
		private const int OPCB_MODE_SEMIAUTOMATIC = 1;

		// Token: 0x0400008A RID: 138
		private const int OPCB_MODE_MANUAL = 2;

		// Token: 0x0400008B RID: 139
		private const int OPCB_MODE_UNKNOWN = 3;

		// Token: 0x0400008C RID: 140
		private const int OPCB_PARAM_PROCESSINPUT = 0;

		// Token: 0x0400008D RID: 141
		private const int OPCB_PARAM_PROCESSPARAMETER = 1;

		// Token: 0x0400008E RID: 142
		private const int OPCB_PARAM_PROCESSOUTPUT = 2;

		// Token: 0x0400008F RID: 143
		private const int OPCB_MR_PROC_PROCEDURE = 0;

		// Token: 0x04000090 RID: 144
		private const int OPCB_MR_PROC_UNITPROCEDURE = 1;

		// Token: 0x04000091 RID: 145
		private const int OPCB_MR_PROC_OPERATION = 2;

		// Token: 0x04000092 RID: 146
		private const int OPCB_MR_PROC_PHASE = 3;

		// Token: 0x04000093 RID: 147
		private const int OPCB_MR_PARAMETER_COLLECTION = 4;

		// Token: 0x04000094 RID: 148
		private const int OPCB_MR_PARAMETER = 5;

		// Token: 0x04000095 RID: 149
		private const int OPCB_MR_RESULT_COLLECTION = 6;

		// Token: 0x04000096 RID: 150
		private const int OPCB_MR_RESULT = 7;

		// Token: 0x04000097 RID: 151
		private const int OPCB_RE_USE_INVALID = 0;

		// Token: 0x04000098 RID: 152
		private const int OPCB_RE_USE_LINKED = 1;

		// Token: 0x04000099 RID: 153
		private const int OPCB_RE_USE_EMBEDDED = 2;

		// Token: 0x0400009A RID: 154
		private const int OPCB_RE_USE_COPIED = 3;
	}
}
