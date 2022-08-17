using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x0200002E RID: 46
	[Guid("0002E000-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumGUID
	{
		// Token: 0x06000045 RID: 69
		void Next([MarshalAs(UnmanagedType.I4)] int celt, [Out] IntPtr rgelt, [MarshalAs(UnmanagedType.I4)] out int pceltFetched);

		// Token: 0x06000046 RID: 70
		void Skip([MarshalAs(UnmanagedType.I4)] int celt);

		// Token: 0x06000047 RID: 71
		void Reset();

		// Token: 0x06000048 RID: 72
		void Clone(out IEnumGUID ppenum);
	}
}
