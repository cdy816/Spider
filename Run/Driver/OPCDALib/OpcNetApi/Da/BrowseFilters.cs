using System;

namespace Opc.Da
{
	// Token: 0x020000EA RID: 234
	[Serializable]
	public class BrowseFilters : ICloneable
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x00012CEC File Offset: 0x00011CEC
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x00012CF4 File Offset: 0x00011CF4
		public int MaxElementsReturned
		{
			get
			{
				return this.m_maxElementsReturned;
			}
			set
			{
				this.m_maxElementsReturned = value;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00012CFD File Offset: 0x00011CFD
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x00012D05 File Offset: 0x00011D05
		public browseFilter BrowseFilter
		{
			get
			{
				return this.m_browseFilter;
			}
			set
			{
				this.m_browseFilter = value;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x00012D0E File Offset: 0x00011D0E
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x00012D16 File Offset: 0x00011D16
		public string ElementNameFilter
		{
			get
			{
				return this.m_elementNameFilter;
			}
			set
			{
				this.m_elementNameFilter = value;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00012D1F File Offset: 0x00011D1F
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x00012D27 File Offset: 0x00011D27
		public string VendorFilter
		{
			get
			{
				return this.m_vendorFilter;
			}
			set
			{
				this.m_vendorFilter = value;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00012D30 File Offset: 0x00011D30
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x00012D38 File Offset: 0x00011D38
		public bool ReturnAllProperties
		{
			get
			{
				return this.m_returnAllProperties;
			}
			set
			{
				this.m_returnAllProperties = value;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00012D41 File Offset: 0x00011D41
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x00012D49 File Offset: 0x00011D49
		public PropertyID[] PropertyIDs
		{
			get
			{
				return this.m_propertyIDs;
			}
			set
			{
				this.m_propertyIDs = value;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x00012D52 File Offset: 0x00011D52
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x00012D5A File Offset: 0x00011D5A
		public bool ReturnPropertyValues
		{
			get
			{
				return this.m_returnPropertyValues;
			}
			set
			{
				this.m_returnPropertyValues = value;
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00012D64 File Offset: 0x00011D64
		public virtual object Clone()
		{
			BrowseFilters browseFilters = (BrowseFilters)base.MemberwiseClone();
			browseFilters.PropertyIDs = (PropertyID[])((this.PropertyIDs != null) ? this.PropertyIDs.Clone() : null);
			return browseFilters;
		}

		// Token: 0x04000394 RID: 916
		private int m_maxElementsReturned;

		// Token: 0x04000395 RID: 917
		private browseFilter m_browseFilter;

		// Token: 0x04000396 RID: 918
		private string m_elementNameFilter;

		// Token: 0x04000397 RID: 919
		private string m_vendorFilter;

		// Token: 0x04000398 RID: 920
		private bool m_returnAllProperties;

		// Token: 0x04000399 RID: 921
		private PropertyID[] m_propertyIDs;

		// Token: 0x0400039A RID: 922
		private bool m_returnPropertyValues;
	}
}
