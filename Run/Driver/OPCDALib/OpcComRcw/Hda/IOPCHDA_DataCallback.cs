using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x0200004A RID: 74
	[Guid("1F1217B9-DEE0-11d2-A5E5-000086339399")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCHDA_DataCallback
	{
		// Token: 0x06000083 RID: 131
		void OnDataChange([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int hrStatus, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 2)] OPCHDA_ITEM[] pItemValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phrErrors);

		// Token: 0x06000084 RID: 132
		void OnReadComplete([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int hrStatus, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 2)] OPCHDA_ITEM[] pItemValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phrErrors);

		// Token: 0x06000085 RID: 133
		void OnReadModifiedComplete([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int hrStatus, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 2)] OPCHDA_MODIFIEDITEM[] pItemValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phrErrors);

		// Token: 0x06000086 RID: 134
		void OnReadAttributeComplete([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int hrStatus, [MarshalAs(UnmanagedType.I4)] int hClient, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 3)] OPCHDA_ATTRIBUTE[] pAttributeValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] phrErrors);

		// Token: 0x06000087 RID: 135
		void OnReadAnnotations([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int hrStatus, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 2)] OPCHDA_ANNOTATION[] pAnnotationValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phrErrors);

		// Token: 0x06000088 RID: 136
		void OnInsertAnnotations([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int hrStatus, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phClients, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phrErrors);

		// Token: 0x06000089 RID: 137
		void OnPlayback([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int hrStatus, [MarshalAs(UnmanagedType.I4)] int dwNumItems, IntPtr ppItemValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phrErrors);

		// Token: 0x0600008A RID: 138
		void OnUpdateComplete([MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] int hrStatus, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phClients, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phrErrors);

		// Token: 0x0600008B RID: 139
		void OnCancelComplete([MarshalAs(UnmanagedType.I4)] int dwCancelID);
	}
}
