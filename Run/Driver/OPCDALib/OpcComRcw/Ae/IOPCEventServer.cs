using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Ae
{
	// Token: 0x02000076 RID: 118
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("65168851-5783-11D1-84A0-00608CB8A7E9")]
	[ComImport]
	public interface IOPCEventServer
	{
		// Token: 0x060000D3 RID: 211
		void GetStatus(out IntPtr ppEventServerStatus);

		// Token: 0x060000D4 RID: 212
		void CreateEventSubscription([MarshalAs(UnmanagedType.I4)] int bActive, [MarshalAs(UnmanagedType.I4)] int dwBufferTime, [MarshalAs(UnmanagedType.I4)] int dwMaxSize, [MarshalAs(UnmanagedType.I4)] int hClientSubscription, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 4)] out object ppUnk, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedBufferTime, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedMaxSize);

		// Token: 0x060000D5 RID: 213
		void QueryAvailableFilters([MarshalAs(UnmanagedType.I4)] out int pdwFilterMask);

		// Token: 0x060000D6 RID: 214
		void QueryEventCategories([MarshalAs(UnmanagedType.I4)] int dwEventType, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwEventCategories, out IntPtr ppszEventCategoryDescs);

		// Token: 0x060000D7 RID: 215
		[PreserveSig]
		int QueryConditionNames([MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppszConditionNames);

		// Token: 0x060000D8 RID: 216
		void QuerySubConditionNames([MarshalAs(UnmanagedType.LPWStr)] string szConditionName, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppszSubConditionNames);

		// Token: 0x060000D9 RID: 217
		void QuerySourceConditions([MarshalAs(UnmanagedType.LPWStr)] string szSource, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppszConditionNames);

		// Token: 0x060000DA RID: 218
		void QueryEventAttributes([MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwAttrIDs, out IntPtr ppszAttrDescs, out IntPtr ppvtAttrTypes);

		// Token: 0x060000DB RID: 219
		void TranslateToItemIDs([MarshalAs(UnmanagedType.LPWStr)] string szSource, [MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.LPWStr)] string szConditionName, [MarshalAs(UnmanagedType.LPWStr)] string szSubconditionName, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] pdwAssocAttrIDs, out IntPtr ppszAttrItemIDs, out IntPtr ppszNodeNames, out IntPtr ppCLSIDs);

		// Token: 0x060000DC RID: 220
		void GetConditionState([MarshalAs(UnmanagedType.LPWStr)] string szSource, [MarshalAs(UnmanagedType.LPWStr)] string szConditionName, [MarshalAs(UnmanagedType.I4)] int dwNumEventAttrs, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] pdwAttributeIDs, out IntPtr ppConditionState);

		// Token: 0x060000DD RID: 221
		void EnableConditionByArea([MarshalAs(UnmanagedType.I4)] int dwNumAreas, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszAreas);

		// Token: 0x060000DE RID: 222
		void EnableConditionBySource([MarshalAs(UnmanagedType.I4)] int dwNumSources, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszSources);

		// Token: 0x060000DF RID: 223
		void DisableConditionByArea([MarshalAs(UnmanagedType.I4)] int dwNumAreas, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszAreas);

		// Token: 0x060000E0 RID: 224
		void DisableConditionBySource([MarshalAs(UnmanagedType.I4)] int dwNumSources, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszSources);

		// Token: 0x060000E1 RID: 225
		void AckCondition([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPWStr)] string szAcknowledgerID, [MarshalAs(UnmanagedType.LPWStr)] string szComment, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszSource, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] szConditionName, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] FILETIME[] pftActiveTime, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwCookie, out IntPtr ppErrors);

		// Token: 0x060000E2 RID: 226
		void CreateAreaBrowser(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 0)] out object ppUnk);
	}
}
