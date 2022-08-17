using System;
using System.Runtime.Serialization;

namespace Opc.Cpx
{
	// Token: 0x02000067 RID: 103
	[Serializable]
	public class InvalidDataInBufferException : ApplicationException
	{
		// Token: 0x0600027D RID: 637 RVA: 0x00007219 File Offset: 0x00006219
		public InvalidDataInBufferException() : base("The data in the buffer cannot be read because it is not consistent with the schema.")
		{
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00007226 File Offset: 0x00006226
		public InvalidDataInBufferException(string message) : base("The data in the buffer cannot be read because it is not consistent with the schema.\r\n" + message)
		{
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00007239 File Offset: 0x00006239
		public InvalidDataInBufferException(Exception e) : base("The data in the buffer cannot be read because it is not consistent with the schema.", e)
		{
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00007247 File Offset: 0x00006247
		public InvalidDataInBufferException(string message, Exception innerException) : base("The data in the buffer cannot be read because it is not consistent with the schema.\r\n" + message, innerException)
		{
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000725B File Offset: 0x0000625B
		protected InvalidDataInBufferException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04000136 RID: 310
		private const string Default = "The data in the buffer cannot be read because it is not consistent with the schema.";
	}
}
