using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Batch
{
	// Token: 0x0200000C RID: 12
	[Guid("895A78CF-B0C5-11d4-A0B7-000102A980B1")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCBatchServer2
	{
		// Token: 0x06000014 RID: 20
		void CreateFilteredEnumerator(Guid riid, OPCBATCHSUMMARYFILTER pFilter, [MarshalAs(UnmanagedType.LPWStr)] string szModel, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 0)] out object ppUnk);
	}
}
