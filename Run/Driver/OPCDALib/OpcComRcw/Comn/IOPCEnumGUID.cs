using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x0200002D RID: 45
	[Guid("55C382C8-21C7-4e88-96C1-BECFB1E3F483")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCEnumGUID
	{
		// Token: 0x06000041 RID: 65
		void Next([MarshalAs(UnmanagedType.I4)] int celt, [Out] IntPtr rgelt, [MarshalAs(UnmanagedType.I4)] out int pceltFetched);

		// Token: 0x06000042 RID: 66
		void Skip([MarshalAs(UnmanagedType.I4)] int celt);

		// Token: 0x06000043 RID: 67
		void Reset();

		// Token: 0x06000044 RID: 68
		void Clone(out IOPCEnumGUID ppenum);
	}
}
