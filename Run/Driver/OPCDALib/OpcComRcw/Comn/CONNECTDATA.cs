using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x02000025 RID: 37
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct CONNECTDATA
	{
		// Token: 0x040001CF RID: 463
		[MarshalAs(UnmanagedType.IUnknown)]
		private object pUnk;

		// Token: 0x040001D0 RID: 464
		[MarshalAs(UnmanagedType.I4)]
		private int dwCookie;
	}
}
