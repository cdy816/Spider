using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Ae
{
	// Token: 0x0200007A RID: 122
	[Guid("71BBE88E-9564-4bcd-BCFC-71C558D94F2D")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCEventServer2
	{
		// Token: 0x060000F0 RID: 240
		void GetStatus(out IntPtr ppEventServerStatus);

		// Token: 0x060000F1 RID: 241
		void CreateEventSubscription([MarshalAs(UnmanagedType.I4)] int bActive, [MarshalAs(UnmanagedType.I4)] int dwBufferTime, [MarshalAs(UnmanagedType.I4)] int dwMaxSize, [MarshalAs(UnmanagedType.I4)] int hClientSubscription, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 4)] out object ppUnk, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedBufferTime, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedMaxSize);

		// Token: 0x060000F2 RID: 242
		void QueryAvailableFilters([MarshalAs(UnmanagedType.I4)] out int pdwFilterMask);

		// Token: 0x060000F3 RID: 243
		void QueryEventCategories([MarshalAs(UnmanagedType.I4)] int dwEventType, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwEventCategories, out IntPtr ppszEventCategoryDescs);

		// Token: 0x060000F4 RID: 244
		[PreserveSig]
		int QueryConditionNames([MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppszConditionNames);

		// Token: 0x060000F5 RID: 245
		void QuerySubConditionNames([MarshalAs(UnmanagedType.LPWStr)] string szConditionName, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppszSubConditionNames);

		// Token: 0x060000F6 RID: 246
		void QuerySourceConditions([MarshalAs(UnmanagedType.LPWStr)] string szSource, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppszConditionNames);

		// Token: 0x060000F7 RID: 247
		void QueryEventAttributes([MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwAttrIDs, out IntPtr ppszAttrDescs, out IntPtr ppvtAttrTypes);

		// Token: 0x060000F8 RID: 248
		void TranslateToItemIDs([MarshalAs(UnmanagedType.LPWStr)] string szSource, [MarshalAs(UnmanagedType.I4)] int dwEventCategory, [MarshalAs(UnmanagedType.LPWStr)] string szConditionName, [MarshalAs(UnmanagedType.LPWStr)] string szSubconditionName, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] pdwAssocAttrIDs, out IntPtr ppszAttrItemIDs, out IntPtr ppszNodeNames, out IntPtr ppCLSIDs);

		// Token: 0x060000F9 RID: 249
		void GetConditionState([MarshalAs(UnmanagedType.LPWStr)] string szSource, [MarshalAs(UnmanagedType.LPWStr)] string szConditionName, [MarshalAs(UnmanagedType.I4)] int dwNumEventAttrs, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] pdwAttributeIDs, out IntPtr ppConditionState);

		// Token: 0x060000FA RID: 250
		void EnableConditionByArea([MarshalAs(UnmanagedType.I4)] int dwNumAreas, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszAreas);

		// Token: 0x060000FB RID: 251
		void EnableConditionBySource([MarshalAs(UnmanagedType.I4)] int dwNumSources, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszSources);

		// Token: 0x060000FC RID: 252
		void DisableConditionByArea([MarshalAs(UnmanagedType.I4)] int dwNumAreas, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszAreas);

		// Token: 0x060000FD RID: 253
		void DisableConditionBySource([MarshalAs(UnmanagedType.I4)] int dwNumSources, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszSources);

		// Token: 0x060000FE RID: 254
		void AckCondition([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPWStr)] string szAcknowledgerID, [MarshalAs(UnmanagedType.LPWStr)] string szComment, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszSource, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] szConditionName, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] FILETIME[] pftActiveTime, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwCookie, out IntPtr ppErrors);

		// Token: 0x060000FF RID: 255
		void CreateAreaBrowser(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 0)] out object ppUnk);

		// Token: 0x06000100 RID: 256
		void EnableConditionByArea2([MarshalAs(UnmanagedType.I4)] int dwNumAreas, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszAreas, out IntPtr ppErrors);

		// Token: 0x06000101 RID: 257
		void EnableConditionBySource2([MarshalAs(UnmanagedType.I4)] int dwNumSources, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszSources, out IntPtr ppErrors);

		// Token: 0x06000102 RID: 258
		void DisableConditionByArea2([MarshalAs(UnmanagedType.I4)] int dwNumAreas, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszAreas, out IntPtr ppErrors);

		// Token: 0x06000103 RID: 259
		void DisableConditionBySource2([MarshalAs(UnmanagedType.I4)] int dwNumSources, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszSources, out IntPtr ppErrors);

		// Token: 0x06000104 RID: 260
		void GetEnableStateByArea([MarshalAs(UnmanagedType.I4)] int dwNumAreas, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszAreas, out IntPtr pbEnabled, out IntPtr pbEffectivelyEnabled, out IntPtr ppErrors);

		// Token: 0x06000105 RID: 261
		void GetEnableStateBySource([MarshalAs(UnmanagedType.I4)] int dwNumSources, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszSources, out IntPtr pbEnabled, out IntPtr pbEffectivelyEnabled, out IntPtr ppErrors);
	}
}
