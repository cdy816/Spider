using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Batch
{
	// Token: 0x0200000B RID: 11
	[Guid("8BB4ED50-B314-11d3-B3EA-00C04F8ECEAA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCBatchServer
	{
		// Token: 0x06000012 RID: 18
		void GetDelimiter([MarshalAs(UnmanagedType.LPWStr)] [Out] string pszDelimiter);

		// Token: 0x06000013 RID: 19
		void CreateEnumerator(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 0)] out object ppUnk);
	}
}
