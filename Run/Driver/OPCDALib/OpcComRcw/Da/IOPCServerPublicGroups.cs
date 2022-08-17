using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000099 RID: 153
	[Guid("39c13a4e-011e-11d0-9675-0020afd8adb3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCServerPublicGroups
	{
		// Token: 0x06000116 RID: 278
		void GetPublicGroupByName([MarshalAs(UnmanagedType.LPWStr)] string szName, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 1)] out object ppUnk);

		// Token: 0x06000117 RID: 279
		void RemovePublicGroup([MarshalAs(UnmanagedType.I4)] int hServerGroup, [MarshalAs(UnmanagedType.I4)] int bForce);
	}
}
