using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x02000029 RID: 41
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IConnectionPointContainer
	{
		// Token: 0x06000036 RID: 54
		void EnumConnectionPoints(out IEnumConnectionPoints ppEnum);

		// Token: 0x06000037 RID: 55
		void FindConnectionPoint(ref Guid riid, out IConnectionPoint ppCP);
	}
}
