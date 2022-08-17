using System;

namespace Opc.Ae
{
	// Token: 0x0200006E RID: 110
	[Serializable]
	public class AttributeDictionary : WriteableDictionary
	{
		// Token: 0x17000084 RID: 132
		public AttributeCollection this[int categoryID]
		{
			get
			{
				return (AttributeCollection)base[categoryID];
			}
			set
			{
				if (value != null)
				{
					base[categoryID] = value;
					return;
				}
				base[categoryID] = new AttributeCollection();
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00007940 File Offset: 0x00006940
		public virtual void Add(int key, int[] value)
		{
			if (value != null)
			{
				base.Add(key, new AttributeCollection(value));
				return;
			}
			base.Add(key, new AttributeCollection());
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00007969 File Offset: 0x00006969
		public AttributeDictionary() : base(null, typeof(int), typeof(AttributeCollection))
		{
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00007988 File Offset: 0x00006988
		public AttributeDictionary(int[] categoryIDs) : base(null, typeof(int), typeof(AttributeCollection))
		{
			for (int i = 0; i < categoryIDs.Length; i++)
			{
				this.Add(categoryIDs[i], null);
			}
		}
	}
}
