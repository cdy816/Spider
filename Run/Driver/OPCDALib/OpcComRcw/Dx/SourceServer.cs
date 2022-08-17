using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Dx
{
	// Token: 0x02000016 RID: 22
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct SourceServer
	{
		// Token: 0x040000F6 RID: 246
		[MarshalAs(UnmanagedType.U4)]
		public uint dwMask;

		// Token: 0x040000F7 RID: 247
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemPath;

		// Token: 0x040000F8 RID: 248
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemName;

		// Token: 0x040000F9 RID: 249
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szVersion;

		// Token: 0x040000FA RID: 250
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szName;

		// Token: 0x040000FB RID: 251
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x040000FC RID: 252
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szServerType;

		// Token: 0x040000FD RID: 253
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szServerURL;

		// Token: 0x040000FE RID: 254
		[MarshalAs(UnmanagedType.I4)]
		public int bDefaultSourceServerConnected;

		// Token: 0x040000FF RID: 255
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;
	}
}
