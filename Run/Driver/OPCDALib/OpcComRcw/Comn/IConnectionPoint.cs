using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x02000027 RID: 39
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IConnectionPoint
	{
		// Token: 0x0600002D RID: 45
		void GetConnectionInterface(out Guid pIID);

		// Token: 0x0600002E RID: 46
		void GetConnectionPointContainer(out IConnectionPointContainer ppCPC);

		// Token: 0x0600002F RID: 47
		void Advise([MarshalAs(UnmanagedType.IUnknown)] object pUnkSink, [MarshalAs(UnmanagedType.I4)] out int pdwCookie);

		// Token: 0x06000030 RID: 48
		void Unadvise([MarshalAs(UnmanagedType.I4)] int dwCookie);

		// Token: 0x06000031 RID: 49
		void EnumConnections(out IEnumConnections ppEnum);
	}
}
