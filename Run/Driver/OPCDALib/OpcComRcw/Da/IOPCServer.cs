using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000098 RID: 152
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("39c13a4d-011e-11d0-9675-0020afd8adb3")]
	[ComImport]
	public interface IOPCServer
	{
		// Token: 0x06000110 RID: 272
		void AddGroup([MarshalAs(UnmanagedType.LPWStr)] string szName, [MarshalAs(UnmanagedType.I4)] int bActive, [MarshalAs(UnmanagedType.I4)] int dwRequestedUpdateRate, [MarshalAs(UnmanagedType.I4)] int hClientGroup, IntPtr pTimeBias, IntPtr pPercentDeadband, [MarshalAs(UnmanagedType.I4)] int dwLCID, [MarshalAs(UnmanagedType.I4)] out int phServerGroup, [MarshalAs(UnmanagedType.I4)] out int pRevisedUpdateRate, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 9)] out object ppUnk);

		// Token: 0x06000111 RID: 273
		void GetErrorString([MarshalAs(UnmanagedType.I4)] int dwError, [MarshalAs(UnmanagedType.I4)] int dwLocale, [MarshalAs(UnmanagedType.LPWStr)] out string ppString);

		// Token: 0x06000112 RID: 274
		void GetGroupByName([MarshalAs(UnmanagedType.LPWStr)] string szName, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 1)] out object ppUnk);

		// Token: 0x06000113 RID: 275
		void GetStatus(out IntPtr ppServerStatus);

		// Token: 0x06000114 RID: 276
		void RemoveGroup([MarshalAs(UnmanagedType.I4)] int hServerGroup, [MarshalAs(UnmanagedType.I4)] int bForce);

		// Token: 0x06000115 RID: 277
		void CreateGroupEnumerator(OPCENUMSCOPE dwScope, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 1)] out object ppUnk);
	}
}
