using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Security
{
	// Token: 0x02000004 RID: 4
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7AA83A02-6C77-11d3-84F9-00008630A38B")]
	[ComImport]
	public interface IOPCSecurityPrivate
	{
		// Token: 0x06000004 RID: 4
		void IsAvailablePriv([MarshalAs(UnmanagedType.I4)] out int pbAvailable);

		// Token: 0x06000005 RID: 5
		void Logon([MarshalAs(UnmanagedType.LPWStr)] string szUserID, [MarshalAs(UnmanagedType.LPWStr)] string szPassword);

		// Token: 0x06000006 RID: 6
		void Logoff();
	}
}
