using System;
using System.Collections;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcCom.Da.Wrapper
{
	// Token: 0x02000026 RID: 38
	[CLSCompliant(false)]
	public class EnumUnknown : IEnumUnknown
	{
		// Token: 0x06000195 RID: 405 RVA: 0x00013700 File Offset: 0x00012700
		internal EnumUnknown(ICollection unknowns)
		{
			if (unknowns != null)
			{
				foreach (object value in unknowns)
				{
					this.m_unknowns.Add(value);
				}
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0001376C File Offset: 0x0001276C
		public void Skip(int celt)
		{
			lock (this)
			{
				try
				{
					this.m_index += celt;
					if (this.m_index > this.m_unknowns.Count)
					{
						this.m_index = this.m_unknowns.Count;
					}
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000137E4 File Offset: 0x000127E4
		public void Clone(out IEnumUnknown ppenum)
		{
			lock (this)
			{
				try
				{
					ppenum = new EnumUnknown(this.m_unknowns);
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00013838 File Offset: 0x00012838
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

		// Token: 0x06000199 RID: 409 RVA: 0x00013884 File Offset: 0x00012884
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
					if (this.m_index < this.m_unknowns.Count)
					{
						int num = 0;
						while (num < this.m_unknowns.Count - this.m_index && num < array.Length)
						{
							array[num] = Marshal.GetIUnknownForObject(this.m_unknowns[this.m_index + num]);
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

		// Token: 0x040000BB RID: 187
		private ArrayList m_unknowns = new ArrayList();

		// Token: 0x040000BC RID: 188
		private int m_index;
	}
}
