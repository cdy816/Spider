using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Security
{
	// Token: 0x02000003 RID: 3
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7AA83A01-6C77-11d3-84F9-00008630A38B")]
	[ComImport]
	public interface IOPCSecurityNT
	{
		// Token: 0x06000001 RID: 1
		void IsAvailableNT([MarshalAs(UnmanagedType.I4)] out int pbAvailable);

		// Token: 0x06000002 RID: 2
		void QueryMinImpersonationLevel([MarshalAs(UnmanagedType.I4)] out int pdwMinImpLevel);

		// Token: 0x06000003 RID: 3
		void ChangeUser();
	}
}
