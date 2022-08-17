using System;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000044 RID: 68
	[Serializable]
	public class AccessDeniedException : ResultIDException
	{
		// Token: 0x0600019D RID: 413 RVA: 0x000061BA File Offset: 0x000051BA
		public AccessDeniedException() : base(ResultID.E_ACCESS_DENIED, "The server refused the connection.")
		{
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000061CC File Offset: 0x000051CC
		public AccessDeniedException(string message) : base(ResultID.E_ACCESS_DENIED, "The server refused the connection.\r\n" + message)
		{
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000061E4 File Offset: 0x000051E4
		public AccessDeniedException(Exception e) : base(ResultID.E_ACCESS_DENIED, "The server refused the connection.", e)
		{
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000061F7 File Offset: 0x000051F7
		public AccessDeniedException(string message, Exception innerException) : base(ResultID.E_NETWORK_ERROR, "The server refused the connection.\r\n" + message, innerException)
		{
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006210 File Offset: 0x00005210
		protected AccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040000E6 RID: 230
		private const string Default = "The server refused the connection.";
	}
}
