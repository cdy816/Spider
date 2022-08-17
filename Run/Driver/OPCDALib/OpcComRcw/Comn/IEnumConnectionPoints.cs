using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x02000028 RID: 40
	[Guid("B196B285-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumConnectionPoints
	{
		// Token: 0x06000032 RID: 50
		void RemoteNext([MarshalAs(UnmanagedType.I4)] int cConnections, [Out] IntPtr ppCP, [MarshalAs(UnmanagedType.I4)] out int pcFetched);

		// Token: 0x06000033 RID: 51
		void Skip([MarshalAs(UnmanagedType.I4)] int cConnections);

		// Token: 0x06000034 RID: 52
		void Reset();

		// Token: 0x06000035 RID: 53
		void Clone(out IEnumConnectionPoints ppEnum);
	}
}
