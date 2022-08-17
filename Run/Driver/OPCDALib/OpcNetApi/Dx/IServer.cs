using System;
using Opc.Da;

namespace Opc.Dx
{
	// Token: 0x02000009 RID: 9
	public interface IServer :  IDisposable
	{
		// Token: 0x0600004E RID: 78
		SourceServer[] GetSourceServers();

		// Token: 0x0600004F RID: 79
		GeneralResponse AddSourceServers(SourceServer[] servers);

		// Token: 0x06000050 RID: 80
		GeneralResponse ModifySourceServers(SourceServer[] servers);

		// Token: 0x06000051 RID: 81
		GeneralResponse DeleteSourceServers(ItemIdentifier[] servers);

		// Token: 0x06000052 RID: 82
		GeneralResponse CopyDefaultSourceServerAttributes(bool configToStatus, ItemIdentifier[] servers);

		// Token: 0x06000053 RID: 83
		DXConnection[] QueryDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors);

		// Token: 0x06000054 RID: 84
		GeneralResponse AddDXConnections(DXConnection[] connections);

		// Token: 0x06000055 RID: 85
		GeneralResponse ModifyDXConnections(DXConnection[] connections);

		// Token: 0x06000056 RID: 86
		GeneralResponse UpdateDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, DXConnection connectionDefinition, out ResultID[] errors);

		// Token: 0x06000057 RID: 87
		GeneralResponse DeleteDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors);

		// Token: 0x06000058 RID: 88
		GeneralResponse CopyDXConnectionDefaultAttributes(bool configToStatus, string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors);

		// Token: 0x06000059 RID: 89
		string ResetConfiguration(string configurationVersion);
	}
}
