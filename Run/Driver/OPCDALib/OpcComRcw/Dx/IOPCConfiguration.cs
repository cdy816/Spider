using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Dx
{
	// Token: 0x02000018 RID: 24
	[Guid("C130D281-F4AA-4779-8846-C2C4CB444F2A")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCConfiguration
	{
		// Token: 0x0600001D RID: 29
		void GetServers([MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppServers);

		// Token: 0x0600001E RID: 30
		void AddServers([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] SourceServer[] pServers, out DXGeneralResponse pResponse);

		// Token: 0x0600001F RID: 31
		void ModifyServers([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] SourceServer[] pServers, out DXGeneralResponse pResponse);

		// Token: 0x06000020 RID: 32
		void DeleteServers([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] ItemIdentifier[] pServers, out DXGeneralResponse pResponse);

		// Token: 0x06000021 RID: 33
		void CopyDefaultServerAttributes([MarshalAs(UnmanagedType.I4)] int bConfigToStatus, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] ItemIdentifier[] pServers, out DXGeneralResponse pResponse);

		// Token: 0x06000022 RID: 34
		void QueryDXConnections([MarshalAs(UnmanagedType.LPWStr)] string szBrowsePath, [MarshalAs(UnmanagedType.I4)] int dwNoOfMasks, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] DXConnection[] pDXConnectionMasks, [MarshalAs(UnmanagedType.I4)] int bRecursive, out IntPtr ppErrors, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppConnections);

		// Token: 0x06000023 RID: 35
		void AddDXConnections([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] DXConnection[] pConnections, out DXGeneralResponse pResponse);

		// Token: 0x06000024 RID: 36
		void UpdateDXConnections([MarshalAs(UnmanagedType.LPWStr)] string szBrowsePath, [MarshalAs(UnmanagedType.I4)] int dwNoOfMasks, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] DXConnection[] pDXConnectionMasks, [MarshalAs(UnmanagedType.I4)] int bRecursive, ref DXConnection pDXConnectionDefinition, [MarshalAs(UnmanagedType.I4)] out IntPtr ppErrors, out DXGeneralResponse pResponse);

		// Token: 0x06000025 RID: 37
		void ModifyDXConnections([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] DXConnection[] pDXConnectionDefinitions, out DXGeneralResponse pResponse);

		// Token: 0x06000026 RID: 38
		void DeleteDXConnections([MarshalAs(UnmanagedType.LPWStr)] string szBrowsePath, [MarshalAs(UnmanagedType.I4)] int dwNoOfMasks, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] DXConnection[] pDXConnectionMasks, [MarshalAs(UnmanagedType.I4)] int bRecursive, [MarshalAs(UnmanagedType.I4)] out IntPtr ppErrors, out DXGeneralResponse pResponse);

		// Token: 0x06000027 RID: 39
		void CopyDXConnectionDefaultAttributes([MarshalAs(UnmanagedType.I4)] int bConfigToStatus, [MarshalAs(UnmanagedType.LPWStr)] string szBrowsePath, [MarshalAs(UnmanagedType.I4)] int dwNoOfMasks, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 2)] DXConnection[] pDXConnectionMasks, [MarshalAs(UnmanagedType.I4)] int bRecursive, [MarshalAs(UnmanagedType.I4)] out IntPtr ppErrors, out DXGeneralResponse pResponse);

		// Token: 0x06000028 RID: 40
		void ResetConfiguration([MarshalAs(UnmanagedType.LPWStr)] string szConfigurationVersion, [MarshalAs(UnmanagedType.LPWStr)] out string pszConfigurationVersion);
	}
}
