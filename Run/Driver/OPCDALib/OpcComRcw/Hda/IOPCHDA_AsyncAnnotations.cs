using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x02000048 RID: 72
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1F1217B7-DEE0-11d2-A5E5-000086339399")]
	[ComImport]
	public interface IOPCHDA_AsyncAnnotations
	{
		// Token: 0x0600007C RID: 124
		void QueryCapabilities(out OPCHDA_ANNOTATIONCAPABILITIES pCapabilities);

		// Token: 0x0600007D RID: 125
		void Read([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x0600007E RID: 126
		void Insert([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] OPCHDA_FILETIME[] ftTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] OPCHDA_ANNOTATION[] pAnnotationValues, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x0600007F RID: 127
		void Cancel([MarshalAs(UnmanagedType.I4)] int dwCancelID);
	}
}
