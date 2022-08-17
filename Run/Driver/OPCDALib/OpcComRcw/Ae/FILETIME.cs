using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Ae
{
	// Token: 0x02000072 RID: 114
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct FILETIME
	{
		// Token: 0x04000352 RID: 850
		public int dwLowDateTime;

		// Token: 0x04000353 RID: 851
		public int dwHighDateTime;
	}
}
