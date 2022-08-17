using System;
using System.Collections;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcCom.Da.Wrapper
{
	// Token: 0x02000027 RID: 39
	[CLSCompliant(false)]
	public class EnumString : IEnumString
	{
		// Token: 0x0600019A RID: 410 RVA: 0x0001396C File Offset: 0x0001296C
		internal EnumString(ICollection strings)
		{
			if (strings != null)
			{
				foreach (object value in strings)
				{
					this.m_strings.Add(value);
				}
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000139D8 File Offset: 0x000129D8
		public void Skip(int celt)
		{
			lock (this)
			{
				try
				{
					this.m_index += celt;
					if (this.m_index > this.m_strings.Count)
					{
						this.m_index = this.m_strings.Count;
					}
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00013A50 File Offset: 0x00012A50
		public void Clone(out IEnumString ppenum)
		{
			lock (this)
			{
				try
				{
					ppenum = new EnumString(this.m_strings);
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00013AA4 File Offset: 0x00012AA4
		public void Reset()
		{
			lock (this)
			{
				try
				{
					this.m_index = 0;
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00013AF0 File Offset: 0x00012AF0
		public void RemoteNext(int celt, IntPtr rgelt, out int pceltFetched)
		{
			lock (this)
			{
				try
				{
					if (rgelt == IntPtr.Zero)
					{
						throw new ExternalException("E_INVALIDARG", -2147024809);
					}
					IntPtr[] array = new IntPtr[celt];
					pceltFetched = 0;
					if (this.m_index < this.m_strings.Count)
					{
						int num = 0;
						while (num < this.m_strings.Count - this.m_index && num < array.Length)
						{
							array[num] = Marshal.StringToCoTaskMemUni((string)this.m_strings[this.m_index + num]);
							pceltFetched++;
							num++;
						}
						this.m_index += pceltFetched;
						Marshal.Copy(array, 0, rgelt, pceltFetched);
					}
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x040000BD RID: 189
		private ArrayList m_strings = new ArrayList();

		// Token: 0x040000BE RID: 190
		private int m_index;
	}
}
