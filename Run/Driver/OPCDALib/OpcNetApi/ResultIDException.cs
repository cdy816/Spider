using System;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x0200002E RID: 46
	[Serializable]
	public class ResultIDException : ApplicationException
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005331 File Offset: 0x00004331
		public ResultID Result
		{
			get
			{
				return this.m_result;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005339 File Offset: 0x00004339
		public ResultIDException(ResultID result) : base(result.ToString())
		{
			this.m_result = result;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005360 File Offset: 0x00004360
		public ResultIDException(ResultID result, string message) : base(result.ToString() + "\r\n" + message)
		{
			this.m_result = result;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005392 File Offset: 0x00004392
		public ResultIDException(ResultID result, string message, Exception e) : base(result.ToString() + "\r\n" + message, e)
		{
			this.m_result = result;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000053C5 File Offset: 0x000043C5
		protected ResultIDException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040000A6 RID: 166
		private ResultID m_result = ResultID.E_FAIL;
	}
}
