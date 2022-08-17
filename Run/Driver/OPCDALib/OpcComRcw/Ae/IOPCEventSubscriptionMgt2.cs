using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Ae
{
	// Token: 0x0200007B RID: 123
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("94C955DC-3684-4ccb-AFAB-F898CE19AAC3")]
	[ComImport]
	public interface IOPCEventSubscriptionMgt2
	{
		// Token: 0x06000106 RID: 262
		void SetFilter([MarshalAs(UnmanagedType.I4)] int dwEventType, [MarshalAs(UnmanagedType.I4)] int dwNumCategories, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] pdwEventCategories, [MarshalAs(UnmanagedType.I4)] int dwLowSeverity, [MarshalAs(UnmanagedType.I4)] int dwHighSeverity, [MarshalAs(UnmanagedType.I4)] int dwNumAreas, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 5)] string[] pszAreaList, [MarshalAs(UnmanagedType.I4)] int dwNumSources, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 7)] string[] pszSourceList);

		// Token: 0x06000107 RID: 263
		void GetFilter([MarshalAs(UnmanagedType.I4)] out int pdwEventType, [MarshalAs(UnmanagedType.I4)] out int pdwNumCategories, out IntPtr ppdwEventCategories, [MarshalAs(UnmanagedType.I4)] out int pdwLowSeverity, [MarshalAs(UnmanagedType.I4)] out int pdwHighSeverity, [MarshalAs(UnmanagedType.I4)] out int pdwNumAreas, out IntPtr ppszAreaList, [MarshalAs(UnmanagedType.I4)] out int pdwNumSources, out IntPtr ppszSourceList);

		// Token: 0x06000108 RID: 264
		void SelectReturnedAttributes([MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] dwAttributeIDs);

		// Token: 0x06000109 RID: 265
		void GetReturnedAttributes([MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwAttributeIDs);

		// Token: 0x0600010A RID: 266
		void Refresh([MarshalAs(UnmanagedType.I4)] int dwConnection);

		// Token: 0x0600010B RID: 267
		void CancelRefresh([MarshalAs(UnmanagedType.I4)] int dwConnection);

		// Token: 0x0600010C RID: 268
		void GetState([MarshalAs(UnmanagedType.I4)] out int pbActive, [MarshalAs(UnmanagedType.I4)] out int pdwBufferTime, [MarshalAs(UnmanagedType.I4)] out int pdwMaxSize, [MarshalAs(UnmanagedType.I4)] out int phClientSubscription);

		// Token: 0x0600010D RID: 269
		void SetState(IntPtr pbActive, IntPtr pdwBufferTime, IntPtr pdwMaxSize, [MarshalAs(UnmanagedType.I4)] int hClientSubscription, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedBufferTime, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedMaxSize);

		// Token: 0x0600010E RID: 270
		void SetKeepAlive([MarshalAs(UnmanagedType.I4)] int dwKeepAliveTime, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedKeepAliveTime);

		// Token: 0x0600010F RID: 271
		void GetKeepAlive([MarshalAs(UnmanagedType.I4)] out int pdwKeepAliveTime);
	}
}
