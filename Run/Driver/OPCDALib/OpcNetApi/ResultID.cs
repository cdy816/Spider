using System;
using System.Runtime.Serialization;
using System.Xml;

namespace Opc
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public struct ResultID : ISerializable
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x000047B4 File Offset: 0x000037B4
		private ResultID(SerializationInfo info, StreamingContext context)
		{
			string name = (string)info.GetValue("NA", typeof(string));
			string ns = (string)info.GetValue("NS", typeof(string));
			this.m_name = new XmlQualifiedName(name, ns);
			this.m_code = (int)info.GetValue("CO", typeof(int));
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004824 File Offset: 0x00003824
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (this.m_name != null)
			{
				info.AddValue("NA", this.m_name.Name);
				info.AddValue("NS", this.m_name.Namespace);
			}
			info.AddValue("CO", this.m_code);
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000487C File Offset: 0x0000387C
		public XmlQualifiedName Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004884 File Offset: 0x00003884
		public int Code
		{
			get
			{
				return this.m_code;
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000488C File Offset: 0x0000388C
		public static bool operator ==(ResultID a, ResultID b)
		{
			return a.Equals(b);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000048A1 File Offset: 0x000038A1
		public static bool operator !=(ResultID a, ResultID b)
		{
			return !a.Equals(b);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000048B9 File Offset: 0x000038B9
		public bool Succeeded()
		{
			if (this.Code != -1)
			{
				return this.Code >= 0;
			}
			return this.Name != null && this.Name.Name.StartsWith("S_");
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000048F6 File Offset: 0x000038F6
		public bool Failed()
		{
			if (this.Code != -1)
			{
				return this.Code < 0;
			}
			return this.Name != null && this.Name.Name.StartsWith("E_");
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004930 File Offset: 0x00003930
		public ResultID(XmlQualifiedName name)
		{
			this.m_name = name;
			this.m_code = -1;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004940 File Offset: 0x00003940
		public ResultID(long code)
		{
			this.m_name = null;
			if (code > 2147483647L)
			{
				code = -(4294967296L - code);
			}
			this.m_code = (int)code;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004968 File Offset: 0x00003968
		public ResultID(string name, string ns)
		{
			this.m_name = new XmlQualifiedName(name, ns);
			this.m_code = -1;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000497E File Offset: 0x0000397E
		public ResultID(ResultID resultID, long code)
		{
			this.m_name = resultID.Name;
			if (code > 2147483647L)
			{
				code = -(4294967296L - code);
			}
			this.m_code = (int)code;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000049AC File Offset: 0x000039AC
		public override bool Equals(object target)
		{
			if (target != null && target.GetType() == typeof(ResultID))
			{
				ResultID resultID = (ResultID)target;
				if (resultID.Code != -1 && this.Code != -1)
				{
					return resultID.Code == this.Code && resultID.Name == this.Name;
				}
				if (resultID.Name != null && this.Name != null)
				{
					return resultID.Name == this.Name;
				}
			}
			return false;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004A40 File Offset: 0x00003A40
		public override string ToString()
		{
			if (this.Name != null)
			{
				return this.Name.Name;
			}
			return string.Format("0x{0,0:X}", this.Code);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004A71 File Offset: 0x00003A71
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000039 RID: 57
		private XmlQualifiedName m_name;

		// Token: 0x0400003A RID: 58
		private int m_code;

		// Token: 0x0400003B RID: 59
		public static readonly ResultID S_OK = new ResultID("S_OK", "http://opcfoundation.org/DataAccess/");

		// Token: 0x0400003C RID: 60
		public static readonly ResultID S_FALSE = new ResultID("S_FALSE", "http://opcfoundation.org/DataAccess/");

		// Token: 0x0400003D RID: 61
		public static readonly ResultID E_FAIL = new ResultID("E_FAIL", "http://opcfoundation.org/DataAccess/");

		// Token: 0x0400003E RID: 62
		public static readonly ResultID E_INVALIDARG = new ResultID("E_INVALIDARG", "http://opcfoundation.org/DataAccess/");

		// Token: 0x0400003F RID: 63
		public static readonly ResultID E_TIMEDOUT = new ResultID("E_TIMEDOUT", "http://opcfoundation.org/DataAccess/");

		// Token: 0x04000040 RID: 64
		public static readonly ResultID E_OUTOFMEMORY = new ResultID("E_OUTOFMEMORY", "http://opcfoundation.org/DataAccess/");

		// Token: 0x04000041 RID: 65
		public static readonly ResultID E_NETWORK_ERROR = new ResultID("E_NETWORK_ERROR", "http://opcfoundation.org/DataAccess/");

		// Token: 0x04000042 RID: 66
		public static readonly ResultID E_ACCESS_DENIED = new ResultID("E_ACCESS_DENIED", "http://opcfoundation.org/DataAccess/");

		// Token: 0x04000043 RID: 67
		public static readonly ResultID E_NOTSUPPORTED = new ResultID("E_NOTSUPPORTED", "http://opcfoundation.org/DataAccess/");

		// Token: 0x02000028 RID: 40
		private class Names
		{
			// Token: 0x04000044 RID: 68
			internal const string NAME = "NA";

			// Token: 0x04000045 RID: 69
			internal const string NAMESPACE = "NS";

			// Token: 0x04000046 RID: 70
			internal const string CODE = "CO";
		}

		// Token: 0x02000029 RID: 41
		public class Da
		{
			// Token: 0x04000047 RID: 71
			public static readonly ResultID S_DATAQUEUEOVERFLOW = new ResultID("S_DATAQUEUEOVERFLOW", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000048 RID: 72
			public static readonly ResultID S_UNSUPPORTEDRATE = new ResultID("S_UNSUPPORTEDRATE", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000049 RID: 73
			public static readonly ResultID S_CLAMP = new ResultID("S_CLAMP", "http://opcfoundation.org/DataAccess/");

			// Token: 0x0400004A RID: 74
			public static readonly ResultID E_INVALIDHANDLE = new ResultID("E_INVALIDHANDLE", "http://opcfoundation.org/DataAccess/");

			// Token: 0x0400004B RID: 75
			public static readonly ResultID E_UNKNOWN_ITEM_NAME = new ResultID("E_UNKNOWN_ITEM_NAME", "http://opcfoundation.org/DataAccess/");

			// Token: 0x0400004C RID: 76
			public static readonly ResultID E_INVALID_ITEM_NAME = new ResultID("E_INVALID_ITEM_NAME", "http://opcfoundation.org/DataAccess/");

			// Token: 0x0400004D RID: 77
			public static readonly ResultID E_UNKNOWN_ITEM_PATH = new ResultID("E_UNKNOWN_ITEM_PATH", "http://opcfoundation.org/DataAccess/");

			// Token: 0x0400004E RID: 78
			public static readonly ResultID E_INVALID_ITEM_PATH = new ResultID("E_INVALID_ITEM_PATH", "http://opcfoundation.org/DataAccess/");

			// Token: 0x0400004F RID: 79
			public static readonly ResultID E_INVALID_PID = new ResultID("E_INVALID_PID", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000050 RID: 80
			public static readonly ResultID E_READONLY = new ResultID("E_READONLY", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000051 RID: 81
			public static readonly ResultID E_WRITEONLY = new ResultID("E_WRITEONLY", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000052 RID: 82
			public static readonly ResultID E_BADTYPE = new ResultID("E_BADTYPE", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000053 RID: 83
			public static readonly ResultID E_RANGE = new ResultID("E_RANGE", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000054 RID: 84
			public static readonly ResultID E_INVALID_FILTER = new ResultID("E_INVALID_FILTER", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000055 RID: 85
			public static readonly ResultID E_INVALIDCONTINUATIONPOINT = new ResultID("E_INVALIDCONTINUATIONPOINT", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000056 RID: 86
			public static readonly ResultID E_NO_WRITEQT = new ResultID("E_NO_WRITEQT", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000057 RID: 87
			public static readonly ResultID E_NO_ITEM_DEADBAND = new ResultID("E_NO_ITEM_DEADBAND", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000058 RID: 88
			public static readonly ResultID E_NO_ITEM_SAMPLING = new ResultID("E_NO_ITEM_SAMPLING", "http://opcfoundation.org/DataAccess/");

			// Token: 0x04000059 RID: 89
			public static readonly ResultID E_NO_ITEM_BUFFERING = new ResultID("E_NO_ITEM_BUFFERING", "http://opcfoundation.org/DataAccess/");
		}

		// Token: 0x0200002A RID: 42
		public class Cpx
		{
			// Token: 0x0400005A RID: 90
			public static readonly ResultID E_TYPE_CHANGED = new ResultID("E_TYPE_CHANGED", "http://opcfoundation.org/ComplexData/");

			// Token: 0x0400005B RID: 91
			public static readonly ResultID E_FILTER_DUPLICATE = new ResultID("E_FILTER_DUPLICATE", "http://opcfoundation.org/ComplexData/");

			// Token: 0x0400005C RID: 92
			public static readonly ResultID E_FILTER_INVALID = new ResultID("E_FILTER_INVALID", "http://opcfoundation.org/ComplexData/");

			// Token: 0x0400005D RID: 93
			public static readonly ResultID E_FILTER_ERROR = new ResultID("E_FILTER_ERROR", "http://opcfoundation.org/ComplexData/");

			// Token: 0x0400005E RID: 94
			public static readonly ResultID S_FILTER_NO_DATA = new ResultID("S_FILTER_NO_DATA", "http://opcfoundation.org/ComplexData/");
		}

		// Token: 0x0200002B RID: 43
		public class Hda
		{
			// Token: 0x0400005F RID: 95
			public static readonly ResultID E_MAXEXCEEDED = new ResultID("E_MAXEXCEEDED", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000060 RID: 96
			public static readonly ResultID S_NODATA = new ResultID("S_NODATA", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000061 RID: 97
			public static readonly ResultID S_MOREDATA = new ResultID("S_MOREDATA", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000062 RID: 98
			public static readonly ResultID E_INVALIDAGGREGATE = new ResultID("E_INVALIDAGGREGATE", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000063 RID: 99
			public static readonly ResultID S_CURRENTVALUE = new ResultID("S_CURRENTVALUE", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000064 RID: 100
			public static readonly ResultID S_EXTRADATA = new ResultID("S_EXTRADATA", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000065 RID: 101
			public static readonly ResultID W_NOFILTER = new ResultID("W_NOFILTER", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000066 RID: 102
			public static readonly ResultID E_UNKNOWNATTRID = new ResultID("E_UNKNOWNATTRID", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000067 RID: 103
			public static readonly ResultID E_NOT_AVAIL = new ResultID("E_NOT_AVAIL", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000068 RID: 104
			public static readonly ResultID E_INVALIDDATATYPE = new ResultID("E_INVALIDDATATYPE", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x04000069 RID: 105
			public static readonly ResultID E_DATAEXISTS = new ResultID("E_DATAEXISTS", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x0400006A RID: 106
			public static readonly ResultID E_INVALIDATTRID = new ResultID("E_INVALIDATTRID", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x0400006B RID: 107
			public static readonly ResultID E_NODATAEXISTS = new ResultID("E_NODATAEXISTS", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x0400006C RID: 108
			public static readonly ResultID S_INSERTED = new ResultID("S_INSERTED", "http://opcfoundation.org/HistoricalDataAccess/");

			// Token: 0x0400006D RID: 109
			public static readonly ResultID S_REPLACED = new ResultID("S_REPLACED", "http://opcfoundation.org/HistoricalDataAccess/");
		}

		// Token: 0x0200002C RID: 44
		public class Dx
		{
			// Token: 0x0400006E RID: 110
			public static readonly ResultID E_PERSISTING = new ResultID("E_PERSISTING", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400006F RID: 111
			public static readonly ResultID E_NOITEMLIST = new ResultID("E_NOITEMLIST", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000070 RID: 112
			public static readonly ResultID E_SERVER_STATE = new ResultID("E_SERVER_STATE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000071 RID: 113
			public static readonly ResultID E_VERSION_MISMATCH = new ResultID("E_VERSION_MISMATCH", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000072 RID: 114
			public static readonly ResultID E_UNKNOWN_ITEM_PATH = new ResultID("E_UNKNOWN_ITEM_PATH", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000073 RID: 115
			public static readonly ResultID E_UNKNOWN_ITEM_NAME = new ResultID("E_UNKNOWN_ITEM_NAME", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000074 RID: 116
			public static readonly ResultID E_INVALID_ITEM_PATH = new ResultID("E_INVALID_ITEM_PATH", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000075 RID: 117
			public static readonly ResultID E_INVALID_ITEM_NAME = new ResultID("E_INVALID_ITEM_NAME", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000076 RID: 118
			public static readonly ResultID E_INVALID_NAME = new ResultID("E_INVALID_NAME", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000077 RID: 119
			public static readonly ResultID E_DUPLICATE_NAME = new ResultID("E_DUPLICATE_NAME", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000078 RID: 120
			public static readonly ResultID E_INVALID_BROWSE_PATH = new ResultID("E_INVALID_BROWSE_PATH", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000079 RID: 121
			public static readonly ResultID E_INVALID_SERVER_URL = new ResultID("E_INVALID_SERVER_URL", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400007A RID: 122
			public static readonly ResultID E_INVALID_SERVER_TYPE = new ResultID("E_INVALID_SERVER_TYPE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400007B RID: 123
			public static readonly ResultID E_UNSUPPORTED_SERVER_TYPE = new ResultID("E_UNSUPPORTED_SERVER_TYPE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400007C RID: 124
			public static readonly ResultID E_CONNECTIONS_EXIST = new ResultID("E_CONNECTIONS_EXIST", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400007D RID: 125
			public static readonly ResultID E_TOO_MANY_CONNECTIONS = new ResultID("E_TOO_MANY_CONNECTIONS", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400007E RID: 126
			public static readonly ResultID E_OVERRIDE_BADTYPE = new ResultID("E_OVERRIDE_BADTYPE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400007F RID: 127
			public static readonly ResultID E_OVERRIDE_RANGE = new ResultID("E_OVERRIDE_RANGE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000080 RID: 128
			public static readonly ResultID E_SUBSTITUTE_BADTYPE = new ResultID("E_SUBSTITUTE_BADTYPE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000081 RID: 129
			public static readonly ResultID E_SUBSTITUTE_RANGE = new ResultID("E_SUBSTITUTE_RANGE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000082 RID: 130
			public static readonly ResultID E_INVALID_TARGET_ITEM = new ResultID("E_INVALID_TARGET_ITEM", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000083 RID: 131
			public static readonly ResultID E_UNKNOWN_TARGET_ITEM = new ResultID("E_UNKNOWN_TARGET_ITEM", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000084 RID: 132
			public static readonly ResultID E_TARGET_ALREADY_CONNECTED = new ResultID("E_TARGET_ALREADY_CONNECTED", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000085 RID: 133
			public static readonly ResultID E_UNKNOWN_SERVER_NAME = new ResultID("E_UNKNOWN_SERVER_NAME", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000086 RID: 134
			public static readonly ResultID E_UNKNOWN_SOURCE_ITEM = new ResultID("E_UNKNOWN_SOURCE_ITEM", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000087 RID: 135
			public static readonly ResultID E_INVALID_SOURCE_ITEM = new ResultID("E_INVALID_SOURCE_ITEM", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000088 RID: 136
			public static readonly ResultID E_INVALID_QUEUE_SIZE = new ResultID("E_INVALID_QUEUE_SIZE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000089 RID: 137
			public static readonly ResultID E_INVALID_DEADBAND = new ResultID("E_INVALID_DEADBAND", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400008A RID: 138
			public static readonly ResultID E_INVALID_CONFIG_FILE = new ResultID("E_INVALID_CONFIG_FILE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400008B RID: 139
			public static readonly ResultID E_PERSIST_FAILED = new ResultID("E_PERSIST_FAILED", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400008C RID: 140
			public static readonly ResultID E_TARGET_FAULT = new ResultID("E_TARGET_FAULT", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400008D RID: 141
			public static readonly ResultID E_TARGET_NO_ACCESSS = new ResultID("E_TARGET_NO_ACCESSS", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400008E RID: 142
			public static readonly ResultID E_SOURCE_SERVER_FAULT = new ResultID("E_SOURCE_SERVER_FAULT", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400008F RID: 143
			public static readonly ResultID E_SOURCE_SERVER_NO_ACCESSS = new ResultID("E_SOURCE_SERVER_NO_ACCESSS", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000090 RID: 144
			public static readonly ResultID E_SUBSCRIPTION_FAULT = new ResultID("E_SUBSCRIPTION_FAULT", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000091 RID: 145
			public static readonly ResultID E_SOURCE_ITEM_BADRIGHTS = new ResultID("E_SOURCE_ITEM_BADRIGHTS", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000092 RID: 146
			public static readonly ResultID E_SOURCE_ITEM_BAD_QUALITY = new ResultID("E_SOURCE_ITEM_BAD_QUALITY", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000093 RID: 147
			public static readonly ResultID E_SOURCE_ITEM_BADTYPE = new ResultID("E_SOURCE_ITEM_BADTYPE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000094 RID: 148
			public static readonly ResultID E_SOURCE_ITEM_RANGE = new ResultID("E_SOURCE_ITEM_RANGE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000095 RID: 149
			public static readonly ResultID E_SOURCE_SERVER_NOT_CONNECTED = new ResultID("E_SOURCE_SERVER_NOT_CONNECTED", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000096 RID: 150
			public static readonly ResultID E_SOURCE_SERVER_TIMEOUT = new ResultID("E_SOURCE_SERVER_TIMEOUT", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000097 RID: 151
			public static readonly ResultID E_TARGET_ITEM_DISCONNECTED = new ResultID("E_TARGET_ITEM_DISCONNECTED", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000098 RID: 152
			public static readonly ResultID E_TARGET_NO_WRITES_ATTEMPTED = new ResultID("E_TARGET_NO_WRITES_ATTEMPTED", "http://opcfoundation.org/DataExchange/");

			// Token: 0x04000099 RID: 153
			public static readonly ResultID E_TARGET_ITEM_BADTYPE = new ResultID("E_TARGET_ITEM_BADTYPE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400009A RID: 154
			public static readonly ResultID E_TARGET_ITEM_RANGE = new ResultID("E_TARGET_ITEM_RANGE", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400009B RID: 155
			public static readonly ResultID S_TARGET_SUBSTITUTED = new ResultID("S_TARGET_SUBSTITUTED", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400009C RID: 156
			public static readonly ResultID S_TARGET_OVERRIDEN = new ResultID("S_TARGET_OVERRIDEN", "http://opcfoundation.org/DataExchange/");

			// Token: 0x0400009D RID: 157
			public static readonly ResultID S_CLAMP = new ResultID("S_CLAMP", "http://opcfoundation.org/DataExchange/");
		}

		// Token: 0x0200002D RID: 45
		public class Ae
		{
			// Token: 0x0400009E RID: 158
			public static readonly ResultID S_ALREADYACKED = new ResultID("S_ALREADYACKED", "http://opcfoundation.org/AlarmAndEvents/");

			// Token: 0x0400009F RID: 159
			public static readonly ResultID S_INVALIDBUFFERTIME = new ResultID("S_INVALIDBUFFERTIME", "http://opcfoundation.org/AlarmAndEvents/");

			// Token: 0x040000A0 RID: 160
			public static readonly ResultID S_INVALIDMAXSIZE = new ResultID("S_INVALIDMAXSIZE", "http://opcfoundation.org/AlarmAndEvents/");

			// Token: 0x040000A1 RID: 161
			public static readonly ResultID S_INVALIDKEEPALIVETIME = new ResultID("S_INVALIDKEEPALIVETIME", "http://opcfoundation.org/AlarmAndEvents/");

			// Token: 0x040000A2 RID: 162
			public static readonly ResultID E_INVALIDBRANCHNAME = new ResultID("E_INVALIDBRANCHNAME", "http://opcfoundation.org/AlarmAndEvents/");

			// Token: 0x040000A3 RID: 163
			public static readonly ResultID E_INVALIDTIME = new ResultID("E_INVALIDTIME", "http://opcfoundation.org/AlarmAndEvents/");

			// Token: 0x040000A4 RID: 164
			public static readonly ResultID E_BUSY = new ResultID("E_BUSY", "http://opcfoundation.org/AlarmAndEvents/");

			// Token: 0x040000A5 RID: 165
			public static readonly ResultID E_NOINFO = new ResultID("E_NOINFO", "http://opcfoundation.org/AlarmAndEvents/");
		}
	}
}
