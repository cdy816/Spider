using System;

namespace Opc.Ae
{
	// Token: 0x0200006F RID: 111
	[Serializable]
	public class AttributeCollection : WriteableCollection
	{
		// Token: 0x17000085 RID: 133
		public int this[int index]
		{
			get
			{
				return (int)this.Array[index];
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000079DB File Offset: 0x000069DB
		public new int[] ToArray()
		{
			return (int[])this.Array.ToArray(typeof(int));
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000079F7 File Offset: 0x000069F7
		internal AttributeCollection() : base(null, typeof(int))
		{
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00007A0A File Offset: 0x00006A0A
		internal AttributeCollection(int[] attributeIDs) : base(attributeIDs, typeof(int))
		{
		}
	}
}
