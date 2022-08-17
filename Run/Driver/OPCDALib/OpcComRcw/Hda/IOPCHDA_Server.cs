using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x02000042 RID: 66
	[Guid("1F1217B0-DEE0-11d2-A5E5-000086339399")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCHDA_Server
	{
		// Token: 0x06000058 RID: 88
		void GetItemAttributes([MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwAttrID, out IntPtr ppszAttrName, out IntPtr ppszAttrDesc, out IntPtr ppvtAttrDataType);

		// Token: 0x06000059 RID: 89
		void GetAggregates([MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwAggrID, out IntPtr ppszAggrName, out IntPtr ppszAggrDesc);

		// Token: 0x0600005A RID: 90
		void GetHistorianStatus(out OPCHDA_SERVERSTATUS pwStatus, out IntPtr pftCurrentTime, out IntPtr pftStartTime, [MarshalAs(UnmanagedType.I2)] out short pwMajorVersion, [MarshalAs(UnmanagedType.I2)] out short wMinorVersion, [MarshalAs(UnmanagedType.I2)] out short pwBuildNumber, [MarshalAs(UnmanagedType.I4)] out int pdwMaxReturnValues, [MarshalAs(UnmanagedType.LPWStr)] out string ppszStatusString, [MarshalAs(UnmanagedType.LPWStr)] out string ppszVendorInfo);

		// Token: 0x0600005B RID: 91
		void GetItemHandles([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszItemID, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phClient, out IntPtr pphServer, out IntPtr ppErrors);

		// Token: 0x0600005C RID: 92
		void ReleaseItemHandles([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, out IntPtr ppErrors);

		// Token: 0x0600005D RID: 93
		void ValidateItemIDs([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszItemID, out IntPtr ppErrors);

		// Token: 0x0600005E RID: 94
		void CreateBrowse([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwAttrID, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] OPCHDA_OPERATORCODES[] pOperator, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)] object[] vFilter, out IOPCHDA_Browser pphBrowser, out IntPtr ppErrors);
	}
}
