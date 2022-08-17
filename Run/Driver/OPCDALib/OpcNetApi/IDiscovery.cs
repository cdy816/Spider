using System;

namespace Opc
{
	// Token: 0x02000092 RID: 146
	public interface IDiscovery : IDisposable
	{
		// Token: 0x0600041B RID: 1051
		string[] EnumerateHosts();

		// Token: 0x0600041C RID: 1052
		Server[] GetAvailableServers(Specification specification);

		// Token: 0x0600041D RID: 1053
		Server[] GetAvailableServers(Specification specification, string host, ConnectData connectData);
	}
}
