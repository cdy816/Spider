using System;

namespace OpcRcw.Dx
{
	// Token: 0x02000019 RID: 25
	public static class Names
	{
		// Token: 0x04000118 RID: 280
		public const string OPC_CATEGORY_DESCRIPTION_DX10 = "OPC Data Exchange Servers Version 1.0";

		// Token: 0x04000119 RID: 281
		public const string OPCDX_NAMESPACE_V10 = "http://opcfoundation.org/webservices/OPCDX/10";

		// Token: 0x0400011A RID: 282
		public const string OPCDX_DATABASE_ROOT = "DX";

		// Token: 0x0400011B RID: 283
		public const string OPCDX_SEPARATOR = "/";

		// Token: 0x0400011C RID: 284
		public const string OPCDX_ITEM_PATH = "ItemPath";

		// Token: 0x0400011D RID: 285
		public const string OPCDX_ITEM_NAME = "ItemName";

		// Token: 0x0400011E RID: 286
		public const string OPCDX_VERSION = "Version";

		// Token: 0x0400011F RID: 287
		public const string OPCDX_SERVER_STATUS_TYPE = "DXServerStatus";

		// Token: 0x04000120 RID: 288
		public const string OPCDX_SERVER_STATUS = "ServerStatus";

		// Token: 0x04000121 RID: 289
		public const string OPCDX_CONFIGURATION_VERSION = "ConfigurationVersion";

		// Token: 0x04000122 RID: 290
		public const string OPCDX_SERVER_STATE = "ServerState";

		// Token: 0x04000123 RID: 291
		public const string OPCDX_CONNECTION_COUNT = "DXConnectionCount";

		// Token: 0x04000124 RID: 292
		public const string OPCDX_MAX_CONNECTIONS = "MaxDXConnections";

		// Token: 0x04000125 RID: 293
		public const string OPCDX_SERVER_ERROR_ID = "ErrorID";

		// Token: 0x04000126 RID: 294
		public const string OPCDX_SERVER_ERROR_DIAGNOSTIC = "ErrorDiagnostic";

		// Token: 0x04000127 RID: 295
		public const string OPCDX_DIRTY_FLAG = "DirtyFlag";

		// Token: 0x04000128 RID: 296
		public const string OPCDX_SOURCE_SERVER_TYPES = "SourceServerTypes";

		// Token: 0x04000129 RID: 297
		public const string OPCDX_MAX_QUEUE_SIZE = "MaxQueueSize";

		// Token: 0x0400012A RID: 298
		public const string OPCDX_CONNECTIONS_ROOT = "DXConnectionsRoot";

		// Token: 0x0400012B RID: 299
		public const string OPCDX_CONNECTION_TYPE = "DXConnection";

		// Token: 0x0400012C RID: 300
		public const string OPCDX_CONNECTION_NAME = "Name";

		// Token: 0x0400012D RID: 301
		public const string OPCDX_CONNECTION_BROWSE_PATHS = "BrowsePath";

		// Token: 0x0400012E RID: 302
		public const string OPCDX_CONNECTION_VERSION = "Version";

		// Token: 0x0400012F RID: 303
		public const string OPCDX_CONNECTION_DESCRIPTION = "Description";

		// Token: 0x04000130 RID: 304
		public const string OPCDX_CONNECTION_KEYWORD = "Keyword";

		// Token: 0x04000131 RID: 305
		public const string OPCDX_DEFAULT_SOURCE_ITEM_CONNECTED = "DefaultSourceItemConnected";

		// Token: 0x04000132 RID: 306
		public const string OPCDX_DEFAULT_TARGET_ITEM_CONNECTED = "DefaultTargetItemConnected";

		// Token: 0x04000133 RID: 307
		public const string OPCDX_DEFAULT_OVERRIDDEN = "DefaultOverridden";

		// Token: 0x04000134 RID: 308
		public const string OPCDX_DEFAULT_OVERRIDE_VALUE = "DefaultOverrideValue";

		// Token: 0x04000135 RID: 309
		public const string OPCDX_ENABLE_SUBSTITUTE_VALUE = "EnableSubstituteValue";

		// Token: 0x04000136 RID: 310
		public const string OPCDX_SUBSTITUTE_VALUE = "SubstituteValue";

		// Token: 0x04000137 RID: 311
		public const string OPCDX_TARGET_ITEM_PATH = "TargetItemPath";

		// Token: 0x04000138 RID: 312
		public const string OPCDX_TARGET_ITEM_NAME = "TargetItemName";

		// Token: 0x04000139 RID: 313
		public const string OPCDX_CONNECTION_SOURCE_SERVER_NAME = "SourceServerName";

		// Token: 0x0400013A RID: 314
		public const string OPCDX_SOURCE_ITEM_PATH = "SourceItemPath";

		// Token: 0x0400013B RID: 315
		public const string OPCDX_SOURCE_ITEM_NAME = "SourceItemName";

		// Token: 0x0400013C RID: 316
		public const string OPCDX_SOURCE_ITEM_QUEUE_SIZE = "QueueSize";

		// Token: 0x0400013D RID: 317
		public const string OPCDX_UPDATE_RATE = "UpdateRate";

		// Token: 0x0400013E RID: 318
		public const string OPCDX_DEADBAND = "Deadband";

		// Token: 0x0400013F RID: 319
		public const string OPCDX_VENDOR_DATA = "VendorData";

		// Token: 0x04000140 RID: 320
		public const string OPCDX_CONNECTION_STATUS = "Status";

		// Token: 0x04000141 RID: 321
		public const string OPCDX_CONNECTION_STATUS_TYPE = "DXConnectionStatus";

		// Token: 0x04000142 RID: 322
		public const string OPCDX_CONNECTION_STATE = "DXConnectionState";

		// Token: 0x04000143 RID: 323
		public const string OPCDX_WRITE_VALUE = "WriteValue";

		// Token: 0x04000144 RID: 324
		public const string OPCDX_WRITE_TIMESTAMP = "WriteTimestamp";

		// Token: 0x04000145 RID: 325
		public const string OPCDX_WRITE_QUALITY = "WriteQuality";

		// Token: 0x04000146 RID: 326
		public const string OPCDX_WRITE_ERROR_ID = "WriteErrorID";

		// Token: 0x04000147 RID: 327
		public const string OPCDX_WRITE_ERROR_DIAGNOSTIC = "WriteErrorDiagnostic";

		// Token: 0x04000148 RID: 328
		public const string OPCDX_SOURCE_VALUE = "SourceValue";

		// Token: 0x04000149 RID: 329
		public const string OPCDX_SOURCE_TIMESTAMP = "SourceTimestamp";

		// Token: 0x0400014A RID: 330
		public const string OPCDX_SOURCE_QUALITY = "SourceQuality";

		// Token: 0x0400014B RID: 331
		public const string OPCDX_SOURCE_ERROR_ID = "SourceErrorID";

		// Token: 0x0400014C RID: 332
		public const string OPCDX_SOURCE_ERROR_DIAGNOSTIC = "SourceErrorDiagnostic";

		// Token: 0x0400014D RID: 333
		public const string OPCDX_ACTUAL_UPDATE_RATE = "ActualUpdateRate";

		// Token: 0x0400014E RID: 334
		public const string OPCDX_QUEUE_HIGH_WATER_MARK = "QueueHighWaterMark";

		// Token: 0x0400014F RID: 335
		public const string OPCDX_QUEUE_FLUSH_COUNT = "QueueFlushCount";

		// Token: 0x04000150 RID: 336
		public const string OPCDX_SOURCE_ITEM_CONNECTED = "SourceItemConnected";

		// Token: 0x04000151 RID: 337
		public const string OPCDX_TARGET_ITEM_CONNECTED = "TargetItemConnected";

		// Token: 0x04000152 RID: 338
		public const string OPCDX_OVERRIDDEN = "Overridden";

		// Token: 0x04000153 RID: 339
		public const string OPCDX_OVERRIDE_VALUE = "OverrideValue";

		// Token: 0x04000154 RID: 340
		public const string OPCDX_SOURCE_SERVERS_ROOT = "SourceServers";

		// Token: 0x04000155 RID: 341
		public const string OPCDX_SOURCE_SERVER_TYPE = "SourceServer";

		// Token: 0x04000156 RID: 342
		public const string OPCDX_SOURCE_SERVER_NAME = "Name";

		// Token: 0x04000157 RID: 343
		public const string OPCDX_SOURCE_SERVER_VERSION = "Version";

		// Token: 0x04000158 RID: 344
		public const string OPCDX_SOURCE_SERVER_DESCRIPTION = "Description";

		// Token: 0x04000159 RID: 345
		public const string OPCDX_SERVER_URL = "ServerURL";

		// Token: 0x0400015A RID: 346
		public const string OPCDX_SERVER_TYPE = "ServerType";

		// Token: 0x0400015B RID: 347
		public const string OPCDX_DEFAULT_SOURCE_SERVER_CONNECTED = "DefaultSourceServerConnected";

		// Token: 0x0400015C RID: 348
		public const string OPCDX_SOURCE_SERVER_STATUS_TYPE = "DXSourceServerStatus";

		// Token: 0x0400015D RID: 349
		public const string OPCDX_SOURCE_SERVER_STATUS = "Status";

		// Token: 0x0400015E RID: 350
		public const string OPCDX_SERVER_CONNECT_STATUS = "ConnectStatus";

		// Token: 0x0400015F RID: 351
		public const string OPCDX_SOURCE_SERVER_ERROR_ID = "ErrorID";

		// Token: 0x04000160 RID: 352
		public const string OPCDX_SOURCE_SERVER_ERROR_DIAGNOSTIC = "ErrorDiagnostic";

		// Token: 0x04000161 RID: 353
		public const string OPCDX_LAST_CONNECT_TIMESTAMP = "LastConnectTimestamp";

		// Token: 0x04000162 RID: 354
		public const string OPCDX_LAST_CONNECT_FAIL_TIMESTAMP = "LastConnectFailTimestamp";

		// Token: 0x04000163 RID: 355
		public const string OPCDX_CONNECT_FAIL_COUNT = "ConnectFailCount";

		// Token: 0x04000164 RID: 356
		public const string OPCDX_PING_TIME = "PingTime";

		// Token: 0x04000165 RID: 357
		public const string OPCDX_LAST_DATA_CHANGE_TIMESTAMP = "LastDataChangeTimestamp";

		// Token: 0x04000166 RID: 358
		public const string OPCDX_SOURCE_SERVER_CONNECTED = "SourceServerConnected";

		// Token: 0x04000167 RID: 359
		public const string OPCDX_QUALITY = "DXQuality";

		// Token: 0x04000168 RID: 360
		public const string OPCDX_QUALITY_STATUS = "Quality";

		// Token: 0x04000169 RID: 361
		public const string OPCDX_LIMIT_BITS = "LimitBits";

		// Token: 0x0400016A RID: 362
		public const string OPCDX_VENDOR_BITS = "VendorBits";

		// Token: 0x0400016B RID: 363
		public const string OPCDX_ERROR = "OPCError";

		// Token: 0x0400016C RID: 364
		public const string OPCDX_ERROR_ID = "ID";

		// Token: 0x0400016D RID: 365
		public const string OPCDX_ERROR_TEXT = "Text";

		// Token: 0x0400016E RID: 366
		public const string OPCDX_SOURCE_SERVER_URL_SCHEME_OPCDA = "opcda";

		// Token: 0x0400016F RID: 367
		public const string OPCDX_SOURCE_SERVER_URL_SCHEME_XMLDA = "http";
	}
}
