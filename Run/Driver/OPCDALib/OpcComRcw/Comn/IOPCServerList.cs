using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x0200002C RID: 44
	[Guid("13486D50-4821-11D2-A494-3CB306C10000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCServerList
	{
		// Token: 0x0600003E RID: 62
		void EnumClassesOfCategories([MarshalAs(UnmanagedType.I4)] int cImplemented, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] Guid[] rgcatidImpl, [MarshalAs(UnmanagedType.I4)] int cRequired, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 2)] Guid[] rgcatidReq, [MarshalAs(UnmanagedType.IUnknown)] out object ppenumClsid);

		// Token: 0x0600003F RID: 63
		void GetClassDetails(ref Guid clsid, [MarshalAs(UnmanagedType.LPWStr)] out string ppszProgID, [MarshalAs(UnmanagedType.LPWStr)] out string ppszUserType);

		// Token: 0x06000040 RID: 64
		void CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] string szProgId, out Guid clsid);
	}
}
