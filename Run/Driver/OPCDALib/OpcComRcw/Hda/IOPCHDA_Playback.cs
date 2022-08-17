using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x02000049 RID: 73
	[Guid("1F1217B8-DEE0-11d2-A5E5-000086339399")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCHDA_Playback
	{
		// Token: 0x06000080 RID: 128
		void ReadRawWithUpdate([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int dwNumValues, OPCHDA_FILETIME ftUpdateDuration, OPCHDA_FILETIME ftUpdateInterval, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 6)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000081 RID: 129
		void ReadProcessedWithUpdate([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, OPCHDA_FILETIME ftResampleInterval, [MarshalAs(UnmanagedType.I4)] int dwNumIntervals, OPCHDA_FILETIME ftUpdateInterval, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 6)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 6)] int[] haAggregate, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000082 RID: 130
		void Cancel([MarshalAs(UnmanagedType.I4)] int dwCancelID);
	}
}
