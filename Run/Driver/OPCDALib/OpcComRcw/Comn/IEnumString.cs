using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x02000030 RID: 48
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000101-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IEnumString
	{
		// Token: 0x0600004D RID: 77
		void RemoteNext([MarshalAs(UnmanagedType.I4)] int celt, IntPtr rgelt, [MarshalAs(UnmanagedType.I4)] out int pceltFetched);

		// Token: 0x0600004E RID: 78
		void Skip([MarshalAs(UnmanagedType.I4)] int celt);

		// Token: 0x0600004F RID: 79
		void Reset();

		// Token: 0x06000050 RID: 80
		void Clone(out IEnumString ppenum);
	}
}
