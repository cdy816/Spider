using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x02000047 RID: 71
	[Guid("1F1217B6-DEE0-11d2-A5E5-000086339399")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCHDA_AsyncUpdate
	{
		// Token: 0x06000075 RID: 117
		void QueryCapabilities(out OPCHDA_UPDATECAPABILITIES pCapabilities);

		// Token: 0x06000076 RID: 118
		void Insert([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] OPCHDA_FILETIME[] ftTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 1)] object[] vDataValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] pdwQualities, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000077 RID: 119
		void Replace([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] OPCHDA_FILETIME[] ftTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 1)] object[] vDataValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] pdwQualities, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000078 RID: 120
		void InsertReplace([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] OPCHDA_FILETIME[] ftTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 1)] object[] vDataValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] pdwQualities, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000079 RID: 121
		void DeleteRaw([MarshalAs(UnmanagedType.I4)] int dwTransactionID, ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x0600007A RID: 122
		void DeleteAtTime([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] OPCHDA_FILETIME[] ftTimeStamps, out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x0600007B RID: 123
		void Cancel([MarshalAs(UnmanagedType.I4)] int dwCancelID);
	}
}
