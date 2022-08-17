using System;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public class ConnectFailedException : ResultIDException
	{
		// Token: 0x06000198 RID: 408 RVA: 0x0000615A File Offset: 0x0000515A
		public ConnectFailedException() : base(ResultID.E_ACCESS_DENIED, "Could not connect to server.")
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000616C File Offset: 0x0000516C
		public ConnectFailedException(string message) : base(ResultID.E_NETWORK_ERROR, "Could not connect to server.\r\n" + message)
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006184 File Offset: 0x00005184
		public ConnectFailedException(Exception e) : base(ResultID.E_NETWORK_ERROR, "Could not connect to server.", e)
		{
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006197 File Offset: 0x00005197
		public ConnectFailedException(string message, Exception innerException) : base(ResultID.E_NETWORK_ERROR, "Could not connect to server.\r\n" + message, innerException)
		{
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000061B0 File Offset: 0x000051B0
		protected ConnectFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040000E5 RID: 229
		private const string Default = "Could not connect to server.";
	}
}
