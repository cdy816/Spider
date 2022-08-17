using System;
using System.Collections;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcCom.Da.Wrapper
{
	// Token: 0x02000023 RID: 35
	[CLSCompliant(false)]
	public class EnumConnectionPoints : IEnumConnectionPoints
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00013080 File Offset: 0x00012080
		internal EnumConnectionPoints(ICollection connectionPoints)
		{
			if (connectionPoints != null)
			{
				foreach (object obj in connectionPoints)
				{
					IConnectionPoint value = (IConnectionPoint)obj;
					this.m_connectionPoints.Add(value);
				}
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000130F0 File Offset: 0x000120F0
		public void Skip(int cConnections)
		{
			lock (this)
			{
				try
				{
					this.m_index += cConnections;
					if (this.m_index > this.m_connectionPoints.Count)
					{
						this.m_index = this.m_connectionPoints.Count;
					}
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00013168 File Offset: 0x00012168
		public void Clone(out IEnumConnectionPoints ppenum)
		{
			lock (this)
			{
				try
				{
					ppenum = new EnumConnectionPoints(this.m_connectionPoints);
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000131BC File Offset: 0x000121BC
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

		// Token: 0x0600018E RID: 398 RVA: 0x00013208 File Offset: 0x00012208
		public void RemoteNext(int cConnections, IntPtr ppCP, out int pcFetched)
		{
			lock (this)
			{
				try
				{
					if (ppCP == IntPtr.Zero)
					{
						throw new ExternalException("E_INVALIDARG", -2147024809);
					}
					IntPtr[] array = new IntPtr[cConnections];
					pcFetched = 0;
					if (this.m_index < this.m_connectionPoints.Count)
					{
						int num = 0;
						while (num < this.m_connectionPoints.Count - this.m_index && num < cConnections)
						{
							IConnectionPoint o = (IConnectionPoint)this.m_connectionPoints[this.m_index + num];
							array[num] = Marshal.GetComInterfaceForObject(o, typeof(IConnectionPoint));
							pcFetched++;
							num++;
						}
						this.m_index += pcFetched;
						Marshal.Copy(array, 0, ppCP, pcFetched);
					}
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x040000AB RID: 171
		private ArrayList m_connectionPoints = new ArrayList();

		// Token: 0x040000AC RID: 172
		private int m_index;
	}
}
