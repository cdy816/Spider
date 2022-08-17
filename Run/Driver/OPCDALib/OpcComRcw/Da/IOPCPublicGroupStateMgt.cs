using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200009C RID: 156
	[Guid("39c13a51-011e-11d0-9675-0020afd8adb3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCPublicGroupStateMgt
	{
		// Token: 0x06000121 RID: 289
		void GetState([MarshalAs(UnmanagedType.I4)] out int pPublic);

		// Token: 0x06000122 RID: 290
		void MoveToPublic();
	}
}
