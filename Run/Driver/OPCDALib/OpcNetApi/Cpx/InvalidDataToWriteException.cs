using System;
using System.Runtime.Serialization;

namespace Opc.Cpx
{
	// Token: 0x02000069 RID: 105
	[Serializable]
	public class InvalidDataToWriteException : ApplicationException
	{
		// Token: 0x06000287 RID: 647 RVA: 0x000072B1 File Offset: 0x000062B1
		public InvalidDataToWriteException() : base("The object cannot be written because it is not consistent with the schema.")
		{
		}

		// Token: 0x06000288 RID: 648 RVA: 0x000072BE File Offset: 0x000062BE
		public InvalidDataToWriteException(string message) : base("The object cannot be written because it is not consistent with the schema.\r\n" + message)
		{
		}

		// Token: 0x06000289 RID: 649 RVA: 0x000072D1 File Offset: 0x000062D1
		public InvalidDataToWriteException(Exception e) : base("The object cannot be written because it is not consistent with the schema.", e)
		{
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000072DF File Offset: 0x000062DF
		public InvalidDataToWriteException(string message, Exception innerException) : base("The object cannot be written because it is not consistent with the schema.\r\n" + message, innerException)
		{
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000072F3 File Offset: 0x000062F3
		protected InvalidDataToWriteException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04000138 RID: 312
		private const string Default = "The object cannot be written because it is not consistent with the schema.";
	}
}
