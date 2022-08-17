using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x0200002A RID: 42
	[Guid("F31DFDE1-07B6-11d2-B2D8-0060083BA1FB")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCShutdown
	{
		// Token: 0x06000038 RID: 56
		void ShutdownRequest([MarshalAs(UnmanagedType.LPWStr)] string szReason);
	}
}
