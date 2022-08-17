using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000AA RID: 170
	[Guid("8E368666-D72E-4f78-87ED-647611C61C9F")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCGroupStateMgt2
	{
		// Token: 0x0600015A RID: 346
		void GetState([MarshalAs(UnmanagedType.I4)] out int pUpdateRate, [MarshalAs(UnmanagedType.I4)] out int pActive, [MarshalAs(UnmanagedType.LPWStr)] out string ppName, [MarshalAs(UnmanagedType.I4)] out int pTimeBias, [MarshalAs(UnmanagedType.R4)] out float pPercentDeadband, [MarshalAs(UnmanagedType.I4)] out int pLCID, [MarshalAs(UnmanagedType.I4)] out int phClientGroup, [MarshalAs(UnmanagedType.I4)] out int phServerGroup);

		// Token: 0x0600015B RID: 347
		void SetState(IntPtr pRequestedUpdateRate, [MarshalAs(UnmanagedType.I4)] out int pRevisedUpdateRate, IntPtr pActive, IntPtr pTimeBias, IntPtr pPercentDeadband, IntPtr pLCID, IntPtr phClientGroup);

		// Token: 0x0600015C RID: 348
		void SetName([MarshalAs(UnmanagedType.LPWStr)] string szName);

		// Token: 0x0600015D RID: 349
		void CloneGroup([MarshalAs(UnmanagedType.LPWStr)] string szName, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 1)] out object ppUnk);

		// Token: 0x0600015E RID: 350
		void SetKeepAlive([MarshalAs(UnmanagedType.I4)] int dwKeepAliveTime, [MarshalAs(UnmanagedType.I4)] out int pdwRevisedKeepAliveTime);

		// Token: 0x0600015F RID: 351
		void GetKeepAlive([MarshalAs(UnmanagedType.I4)] out int pdwKeepAliveTime);
	}
}
