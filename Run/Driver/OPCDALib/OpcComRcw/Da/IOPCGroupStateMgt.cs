using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200009B RID: 155
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("39c13a50-011e-11d0-9675-0020afd8adb3")]
	[ComImport]
	public interface IOPCGroupStateMgt
	{
		// Token: 0x0600011D RID: 285
		void GetState([MarshalAs(UnmanagedType.I4)] out int pUpdateRate, [MarshalAs(UnmanagedType.I4)] out int pActive, [MarshalAs(UnmanagedType.LPWStr)] out string ppName, [MarshalAs(UnmanagedType.I4)] out int pTimeBias, [MarshalAs(UnmanagedType.R4)] out float pPercentDeadband, [MarshalAs(UnmanagedType.I4)] out int pLCID, [MarshalAs(UnmanagedType.I4)] out int phClientGroup, [MarshalAs(UnmanagedType.I4)] out int phServerGroup);

		// Token: 0x0600011E RID: 286
		void SetState(IntPtr pRequestedUpdateRate, [MarshalAs(UnmanagedType.I4)] out int pRevisedUpdateRate, IntPtr pActive, IntPtr pTimeBias, IntPtr pPercentDeadband, IntPtr pLCID, IntPtr phClientGroup);

		// Token: 0x0600011F RID: 287
		void SetName([MarshalAs(UnmanagedType.LPWStr)] string szName);

		// Token: 0x06000120 RID: 288
		void CloneGroup([MarshalAs(UnmanagedType.LPWStr)] string szName, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 1)] out object ppUnk);
	}
}
