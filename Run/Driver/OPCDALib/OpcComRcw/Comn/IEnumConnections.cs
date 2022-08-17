using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x02000026 RID: 38
	[Guid("B196B287-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumConnections
	{
		// Token: 0x06000029 RID: 41
		void RemoteNext([MarshalAs(UnmanagedType.I4)] int cConnections, [Out] IntPtr rgcd, [MarshalAs(UnmanagedType.I4)] out int pcFetched);

		// Token: 0x0600002A RID: 42
		void Skip([MarshalAs(UnmanagedType.I4)] int cConnections);

		// Token: 0x0600002B RID: 43
		void Reset();

		// Token: 0x0600002C RID: 44
		void Clone(out IEnumConnections ppEnum);
	}
}
