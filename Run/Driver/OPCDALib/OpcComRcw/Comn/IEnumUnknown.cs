using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x0200002F RID: 47
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000100-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IEnumUnknown
	{
		// Token: 0x06000049 RID: 73
		void RemoteNext([MarshalAs(UnmanagedType.I4)] int celt, [Out] IntPtr rgelt, [MarshalAs(UnmanagedType.I4)] out int pceltFetched);

		// Token: 0x0600004A RID: 74
		void Skip([MarshalAs(UnmanagedType.I4)] int celt);

		// Token: 0x0600004B RID: 75
		void Reset();

		// Token: 0x0600004C RID: 76
		void Clone(out IEnumUnknown ppenum);
	}
}
