using System;
using System.Runtime.InteropServices;

namespace OpcCom
{
	// Token: 0x0200001E RID: 30
	[Guid("50E8496C-FA60-46a4-AF72-512494C664C6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCWrappedServer
	{
		// Token: 0x06000129 RID: 297
		void Load([MarshalAs(UnmanagedType.LPStruct)] Guid clsid);

		// Token: 0x0600012A RID: 298
		void Unload();
	}
}
