using System;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000041 RID: 65
	[Serializable]
	public class AlreadyConnectedException : ApplicationException
	{
		// Token: 0x0600018E RID: 398 RVA: 0x000060C2 File Offset: 0x000050C2
		public AlreadyConnectedException() : base("The remote server is already connected.")
		{
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000060CF File Offset: 0x000050CF
		public AlreadyConnectedException(string message) : base("The remote server is already connected.\r\n" + message)
		{
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000060E2 File Offset: 0x000050E2
		public AlreadyConnectedException(Exception e) : base("The remote server is already connected.", e)
		{
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000060F0 File Offset: 0x000050F0
		public AlreadyConnectedException(string message, Exception innerException) : base("The remote server is already connected.\r\n" + message, innerException)
		{
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006104 File Offset: 0x00005104
		protected AlreadyConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040000E3 RID: 227
		private const string Default = "The remote server is already connected.";
	}
}
