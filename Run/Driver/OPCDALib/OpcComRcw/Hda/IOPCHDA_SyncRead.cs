using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x02000043 RID: 67
	[Guid("1F1217B2-DEE0-11d2-A5E5-000086339399")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCHDA_SyncRead
	{
		// Token: 0x0600005F RID: 95
		void ReadRaw(ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int dwNumValues, [MarshalAs(UnmanagedType.I4)] int bBounds, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);

		// Token: 0x06000060 RID: 96
		void ReadProcessed(ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, OPCHDA_FILETIME ftResampleInterval, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] haAggregate, out IntPtr ppItemValues, out IntPtr ppErrors);

		// Token: 0x06000061 RID: 97
		void ReadAtTime([MarshalAs(UnmanagedType.I4)] int dwNumTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCHDA_FILETIME[] ftTimeStamps, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);

		// Token: 0x06000062 RID: 98
		void ReadModified(ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int dwNumValues, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);

		// Token: 0x06000063 RID: 99
		void ReadAttribute(ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int hServer, [MarshalAs(UnmanagedType.I4)] int dwNumAttributes, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] pdwAttributeIDs, out IntPtr ppAttributeValues, out IntPtr ppErrors);
	}
}
