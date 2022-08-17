using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x02000046 RID: 70
	[Guid("1F1217B5-DEE0-11d2-A5E5-000086339399")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCHDA_AsyncRead
	{
		// Token: 0x0600006D RID: 109
		void ReadRaw([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int dwNumValues, [MarshalAs(UnmanagedType.I4)] int bBounds, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 5)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x0600006E RID: 110
		void AdviseRaw([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, OPCHDA_FILETIME ftUpdateInterval, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x0600006F RID: 111
		void ReadProcessed([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, OPCHDA_FILETIME ftResampleInterval, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] haAggregate, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000070 RID: 112
		void AdviseProcessed([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, OPCHDA_FILETIME ftResampleInterval, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] haAggregate, [MarshalAs(UnmanagedType.I4)] int dwNumIntervals, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000071 RID: 113
		void ReadAtTime([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int dwNumTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] OPCHDA_FILETIME[] ftTimeStamps, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000072 RID: 114
		void ReadModified([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int dwNumValues, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000073 RID: 115
		void ReadAttribute([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int hServer, [MarshalAs(UnmanagedType.I4)] int dwNumAttributes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] dwAttributeIDs, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000074 RID: 116
		void Cancel([MarshalAs(UnmanagedType.I4)] int dwCancelID);
	}
}
