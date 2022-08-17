using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Dx
{
	// Token: 0x02000017 RID: 23
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DXConnection
	{
		// Token: 0x04000100 RID: 256
		[MarshalAs(UnmanagedType.U4)]
		public uint dwMask;

		// Token: 0x04000101 RID: 257
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemPath;

		// Token: 0x04000102 RID: 258
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemName;

		// Token: 0x04000103 RID: 259
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szVersion;

		// Token: 0x04000104 RID: 260
		[MarshalAs(UnmanagedType.I4)]
		public int dwBrowsePathCount;

		// Token: 0x04000105 RID: 261
		public IntPtr pszBrowsePaths;

		// Token: 0x04000106 RID: 262
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szName;

		// Token: 0x04000107 RID: 263
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x04000108 RID: 264
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szKeyword;

		// Token: 0x04000109 RID: 265
		[MarshalAs(UnmanagedType.I4)]
		public int bDefaultSourceItemConnected;

		// Token: 0x0400010A RID: 266
		[MarshalAs(UnmanagedType.I4)]
		public int bDefaultTargetItemConnected;

		// Token: 0x0400010B RID: 267
		[MarshalAs(UnmanagedType.I4)]
		public int bDefaultOverridden;

		// Token: 0x0400010C RID: 268
		[MarshalAs(UnmanagedType.Struct)]
		public object vDefaultOverrideValue;

		// Token: 0x0400010D RID: 269
		[MarshalAs(UnmanagedType.Struct)]
		public object vSubstituteValue;

		// Token: 0x0400010E RID: 270
		[MarshalAs(UnmanagedType.I4)]
		public int bEnableSubstituteValue;

		// Token: 0x0400010F RID: 271
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szTargetItemPath;

		// Token: 0x04000110 RID: 272
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szTargetItemName;

		// Token: 0x04000111 RID: 273
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szSourceServerName;

		// Token: 0x04000112 RID: 274
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szSourceItemPath;

		// Token: 0x04000113 RID: 275
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szSourceItemName;

		// Token: 0x04000114 RID: 276
		[MarshalAs(UnmanagedType.I4)]
		public int dwSourceItemQueueSize;

		// Token: 0x04000115 RID: 277
		[MarshalAs(UnmanagedType.I4)]
		public int dwUpdateRate;

		// Token: 0x04000116 RID: 278
		[MarshalAs(UnmanagedType.R4)]
		public float fltDeadBand;

		// Token: 0x04000117 RID: 279
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szVendorData;
	}
}
