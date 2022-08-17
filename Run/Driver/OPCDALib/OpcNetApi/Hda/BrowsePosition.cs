using System;

namespace Opc.Hda
{
	// Token: 0x0200004A RID: 74
	[Serializable]
	public class BrowsePosition : IBrowsePosition, IDisposable, ICloneable
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00006304 File Offset: 0x00005304
		~BrowsePosition()
		{
			this.Dispose(false);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00006334 File Offset: 0x00005334
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00006343 File Offset: 0x00005343
		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				this.m_disposed = true;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00006356 File Offset: 0x00005356
		public virtual object Clone()
		{
			return (BrowsePosition)base.MemberwiseClone();
		}

		// Token: 0x040000EA RID: 234
		private bool m_disposed;
	}
}
