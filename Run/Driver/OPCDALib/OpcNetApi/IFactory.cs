using System;

namespace Opc
{
	// Token: 0x020000D1 RID: 209
	public interface IFactory : IDisposable
	{
		// Token: 0x0600070B RID: 1803
		IServer CreateInstance(URL url, ConnectData connectData);
	}
}
