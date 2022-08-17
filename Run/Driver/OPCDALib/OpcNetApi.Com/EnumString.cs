using System;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcCom
{
	// Token: 0x02000003 RID: 3
	public class EnumString : IDisposable
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002635 File Offset: 0x00001635
		public EnumString(object enumerator)
		{
			this.m_enumerator = (IEnumString)enumerator;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002649 File Offset: 0x00001649
		public void Dispose()
		{
			Interop.ReleaseServer(this.m_enumerator);
			this.m_enumerator = null;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002660 File Offset: 0x00001660
		public string[] Next(int count)
		{
			string[] result;
			try
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)) * count);
				try
				{
					int num = 0;
					this.m_enumerator.RemoteNext(count, intPtr, out num);
					if (num == 0)
					{
						result = new string[0];
					}
					else
					{
						result = Interop.GetUnicodeStrings(ref intPtr, num, true);
					}
				}
				finally
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000026D8 File Offset: 0x000016D8
		public void Skip(int count)
		{
			this.m_enumerator.Skip(count);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000026E6 File Offset: 0x000016E6
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000026F4 File Offset: 0x000016F4
		public EnumString Clone()
		{
			IEnumString enumerator = null;
			this.m_enumerator.Clone(out enumerator);
			return new EnumString(enumerator);
		}

		// Token: 0x04000005 RID: 5
		private IEnumString m_enumerator;
	}
}
