using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Ae
{
	// Token: 0x02000077 RID: 119
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("65168855-5783-11D1-84A0-00608CB8A7E9")]
	[ComImport]
	public interface IOPCEventSubscriptionMgt
	{
		// Token: 0x060000E3 RID: 227
		void SetFilter([MarshalAs(UnmanagedType.I4)] int dwEventType, [MarshalAs(UnmanagedType.I4)] int dwNumCategories, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] pdwEventCategories, [MarshalAs(UnmanagedType.I4)] int dwLowSeverity, [MarshalAs(UnmanagedType.I4)] int dwHighSeverity, [MarshalAs(UnmanagedType.I4)] int dwNumAreas, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 5)] string[] pszAreaList, [MarshalAs(UnmanagedType.I4)] int dwNumSources, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 7)] string[] pszSourceList);

		// Token: 0x060000E4 RID: 228
		void GetFilter([MarshalAs(UnmanagedType.I4)] out int pdwEventType, [MarshalAs(UnmanagedType.I4)] out int pdwNumCategories, out IntPtr ppdwEventCategories, [MarshalAs(UnmanagedType.I4)] out int pdwLowSeverity, [MarshalAs(UnmanagedType.I4)] out int pdwHighSeverity, [MarshalAs(UnmanagedType.I4)] out int pdwNumAreas, out IntPtr ppszAreaList, [MarshalAs(UnmanagedType.I4)] out int pdwNumSources, out IntPtr ppszSourceList);

		// Token: 0x060000E5 RID: 229
		void SelectReturnedAttributes([MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] dwAttributeIDs);

		// Token: 0x060000E6 RID: 230
		void GetReturnedAttributes([MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwAttributeIDs);

		// Token: 0x060000E7 RID: 231
		void Refresh([MarshalAs(UnmanagedType.I4)] int dwConnection);

		// Token: 0x060000E8 RID: 232
		void CancelRefresh([MarshalAs(UnmanagedType.I4)] int dwConnection);

		// Token: 0x060000E9 RID: 233
		void GetState([MarshalAs(UnmanagedType.I4)] out int pbActive, [MarshalAs(UnmanagedType.I4)] out int pdwBufferTime, [MarshalAs(UnmanagedType.I4)] out int pdwMaxSize, [MarshalAs(UnmanagedType.I4)] out int phClientSubscription);

		// Token: 0x060000EA RID: 234
		void SetState(IntPtr pbActive, IntPtr pdwBufferTime, IntPtr pdwMaxSize, [MarshalAs(UnmanagedType.I4)] int hClientSubscription, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedBufferTime, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedMaxSize);
	}
}
