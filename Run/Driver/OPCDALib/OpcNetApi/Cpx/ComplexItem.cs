using System;
using System.Collections;
using System.IO;
using System.Xml;
using Opc.Da;

namespace Opc.Cpx
{
	// Token: 0x0200007F RID: 127
	public class ComplexItem : ItemIdentifier
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000895B File Offset: 0x0000795B
		public string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00008963 File Offset: 0x00007963
		public string TypeSystemID
		{
			get
			{
				return this.m_typeSystemID;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000896B File Offset: 0x0000796B
		public string DictionaryID
		{
			get
			{
				return this.m_dictionaryID;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00008973 File Offset: 0x00007973
		public string TypeID
		{
			get
			{
				return this.m_typeID;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000348 RID: 840 RVA: 0x0000897B File Offset: 0x0000797B
		public ItemIdentifier DictionaryItemID
		{
			get
			{
				return this.m_dictionaryItemID;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00008983 File Offset: 0x00007983
		public ItemIdentifier TypeItemID
		{
			get
			{
				return this.m_typeItemID;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000898B File Offset: 0x0000798B
		public ItemIdentifier UnconvertedItemID
		{
			get
			{
				return this.m_unconvertedItemID;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00008993 File Offset: 0x00007993
		public ItemIdentifier UnfilteredItemID
		{
			get
			{
				return this.m_unfilteredItemID;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000899B File Offset: 0x0000799B
		public ItemIdentifier DataFilterItem
		{
			get
			{
				return this.m_filterItem;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600034D RID: 845 RVA: 0x000089A3 File Offset: 0x000079A3
		// (set) Token: 0x0600034E RID: 846 RVA: 0x000089AB File Offset: 0x000079AB
		public string DataFilterValue
		{
			get
			{
				return this.m_filterValue;
			}
			set
			{
				this.m_filterValue = value;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x000089B4 File Offset: 0x000079B4
		public ComplexItem()
		{
		}

		// Token: 0x06000350 RID: 848 RVA: 0x000089BC File Offset: 0x000079BC
		public ComplexItem(ItemIdentifier itemID)
		{
			base.ItemPath = itemID.ItemPath;
			base.ItemName = itemID.ItemName;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x000089DC File Offset: 0x000079DC
		public override string ToString()
		{
			if (this.m_name != null || this.m_name.Length != 0)
			{
				return this.m_name;
			}
			return base.ItemName;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00008A00 File Offset: 0x00007A00
		public ComplexItem GetRootItem()
		{
			if (this.m_unconvertedItemID != null)
			{
				return ComplexTypeCache.GetComplexItem(this.m_unconvertedItemID);
			}
			if (this.m_unfilteredItemID != null)
			{
				return ComplexTypeCache.GetComplexItem(this.m_unfilteredItemID);
			}
			return this;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00008A2C File Offset: 0x00007A2C
		public void Update(Da.Server server)
		{
			this.Clear();
			ItemPropertyCollection[] properties = server.GetProperties(new ItemIdentifier[]
			{
				this
			}, ComplexItem.CPX_PROPERTIES, true);
			if (properties == null || properties.Length != 1)
			{
				throw new ApplicationException("Unexpected results returned from server.");
			}
			if (!this.Init((ItemProperty[])properties[0].ToArray(typeof(ItemProperty))))
			{
				throw new ApplicationException("Not a valid complex item.");
			}
			this.GetDataFilterItem(server);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00008AA0 File Offset: 0x00007AA0
		public ComplexItem[] GetTypeConversions(Da.Server server)
		{
			if (this.m_unconvertedItemID != null || this.m_unfilteredItemID != null)
			{
				return null;
			}
			BrowsePosition browsePosition = null;
			ComplexItem[] result;
			try
			{
				BrowseFilters browseFilters = new BrowseFilters();
				browseFilters.ElementNameFilter = "CPX";
				browseFilters.BrowseFilter = browseFilter.branch;
				browseFilters.ReturnAllProperties = false;
				browseFilters.PropertyIDs = null;
				browseFilters.ReturnPropertyValues = false;
				BrowseElement[] array = server.Browse(this, browseFilters, out browsePosition);
				if (array == null || array.Length == 0)
				{
					result = null;
				}
				else
				{
					if (browsePosition != null)
					{
						browsePosition.Dispose();
						browsePosition = null;
					}
					ItemIdentifier itemID = new ItemIdentifier(array[0].ItemPath, array[0].ItemName);
					browseFilters.ElementNameFilter = null;
					browseFilters.BrowseFilter = browseFilter.item;
					browseFilters.ReturnAllProperties = false;
					browseFilters.PropertyIDs = ComplexItem.CPX_PROPERTIES;
					browseFilters.ReturnPropertyValues = true;
					array = server.Browse(itemID, browseFilters, out browsePosition);
					if (array == null || array.Length == 0)
					{
						result = new ComplexItem[0];
					}
					else
					{
						ArrayList arrayList = new ArrayList(array.Length);
						foreach (BrowseElement browseElement in array)
						{
							if (browseElement.Name != "DataFilters")
							{
								ComplexItem complexItem = new ComplexItem();
								if (complexItem.Init(browseElement))
								{
									complexItem.GetDataFilterItem(server);
									arrayList.Add(complexItem);
								}
							}
						}
						result = (ComplexItem[])arrayList.ToArray(typeof(ComplexItem));
					}
				}
			}
			finally
			{
				if (browsePosition != null)
				{
					browsePosition.Dispose();
					browsePosition = null;
				}
			}
			return result;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00008C14 File Offset: 0x00007C14
		public ComplexItem[] GetDataFilters(Da.Server server)
		{
			if (this.m_unfilteredItemID != null)
			{
				return null;
			}
			if (this.m_filterItem == null)
			{
				return null;
			}
			BrowsePosition browsePosition = null;
			ComplexItem[] result;
			try
			{
				BrowseFilters browseFilters = new BrowseFilters();
				browseFilters.ElementNameFilter = null;
				browseFilters.BrowseFilter = browseFilter.item;
				browseFilters.ReturnAllProperties = false;
				browseFilters.PropertyIDs = ComplexItem.CPX_PROPERTIES;
				browseFilters.ReturnPropertyValues = true;
				BrowseElement[] array = server.Browse(this.m_filterItem, browseFilters, out browsePosition);
				if (array == null || array.Length == 0)
				{
					result = new ComplexItem[0];
				}
				else
				{
					ArrayList arrayList = new ArrayList(array.Length);
					foreach (BrowseElement element in array)
					{
						ComplexItem complexItem = new ComplexItem();
						if (complexItem.Init(element))
						{
							arrayList.Add(complexItem);
						}
					}
					result = (ComplexItem[])arrayList.ToArray(typeof(ComplexItem));
				}
			}
			finally
			{
				if (browsePosition != null)
				{
					browsePosition.Dispose();
					browsePosition = null;
				}
			}
			return result;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00008D00 File Offset: 0x00007D00
		public ComplexItem CreateDataFilter(Da.Server server, string filterName, string filterValue)
		{
			if (this.m_unfilteredItemID != null)
			{
				return null;
			}
			if (this.m_filterItem == null)
			{
				return null;
			}
			BrowsePosition browsePosition = null;
			ComplexItem result;
			try
			{
				ItemValue itemValue = new ItemValue(this.m_filterItem);
				StringWriter stringWriter = new StringWriter();
				XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
				xmlTextWriter.WriteStartElement("DataFilters");
				xmlTextWriter.WriteAttributeString("Name", filterName);
				xmlTextWriter.WriteString(filterValue);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.Close();
				itemValue.Value = stringWriter.ToString();
				itemValue.Quality = Quality.Bad;
				itemValue.QualitySpecified = false;
				itemValue.Timestamp = DateTime.MinValue;
				itemValue.TimestampSpecified = false;
				IdentifiedResult[] array = server.Write(new ItemValue[]
				{
					itemValue
				});
				if (array == null || array.Length == 0)
				{
					throw new ApplicationException("Unexpected result from server.");
				}
				if (array[0].ResultID.Failed())
				{
					throw new ApplicationException("Could not create new data filter.");
				}
				BrowseFilters browseFilters = new BrowseFilters();
				browseFilters.ElementNameFilter = filterName;
				browseFilters.BrowseFilter = browseFilter.item;
				browseFilters.ReturnAllProperties = false;
				browseFilters.PropertyIDs = ComplexItem.CPX_PROPERTIES;
				browseFilters.ReturnPropertyValues = true;
				BrowseElement[] array2 = server.Browse(this.m_filterItem, browseFilters, out browsePosition);
				if (array2 == null || array2.Length == 0)
				{
					throw new ApplicationException("Could not browse to new data filter.");
				}
				ComplexItem complexItem = new ComplexItem();
				if (!complexItem.Init(array2[0]))
				{
					throw new ApplicationException("Could not initialize to new data filter.");
				}
				result = complexItem;
			}
			finally
			{
				if (browsePosition != null)
				{
					browsePosition.Dispose();
					browsePosition = null;
				}
			}
			return result;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00008E8C File Offset: 0x00007E8C
		public void UpdateDataFilter(Da.Server server, string filterValue)
		{
			if (this.m_unfilteredItemID == null)
			{
				throw new ApplicationException("Cannot update the data filter for this item.");
			}
			IdentifiedResult[] array = server.Write(new ItemValue[]
			{
				new ItemValue(this)
				{
					Value = filterValue,
					Quality = Quality.Bad,
					QualitySpecified = false,
					Timestamp = DateTime.MinValue,
					TimestampSpecified = false
				}
			});
			if (array == null || array.Length == 0)
			{
				throw new ApplicationException("Unexpected result from server.");
			}
			if (array[0].ResultID.Failed())
			{
				throw new ApplicationException("Could not update data filter.");
			}
			this.m_filterValue = filterValue;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00008F28 File Offset: 0x00007F28
		public string GetTypeDictionary(Da.Server server)
		{
			ItemPropertyCollection[] properties = server.GetProperties(new ItemIdentifier[]
			{
				this.m_dictionaryItemID
			}, new PropertyID[]
			{
				Property.DICTIONARY
			}, true);
			if (properties == null || properties.Length == 0 || properties[0].Count == 0)
			{
				return null;
			}
			ItemProperty itemProperty = properties[0][0];
			if (!itemProperty.ResultID.Succeeded())
			{
				return null;
			}
			return (string)itemProperty.Value;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00008FA4 File Offset: 0x00007FA4
		public string GetTypeDescription(Da.Server server)
		{
			ItemPropertyCollection[] properties = server.GetProperties(new ItemIdentifier[]
			{
				this.m_typeItemID
			}, new PropertyID[]
			{
				Property.TYPE_DESCRIPTION
			}, true);
			if (properties == null || properties.Length == 0 || properties[0].Count == 0)
			{
				return null;
			}
			ItemProperty itemProperty = properties[0][0];
			if (!itemProperty.ResultID.Succeeded())
			{
				return null;
			}
			return (string)itemProperty.Value;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00009020 File Offset: 0x00008020
		public void GetDataFilterItem(Da.Server server)
		{
			this.m_filterItem = null;
			if (this.m_unfilteredItemID != null)
			{
				return;
			}
			BrowsePosition browsePosition = null;
			try
			{
				ItemIdentifier itemID = new ItemIdentifier(this);
				BrowseFilters browseFilters = new BrowseFilters();
				browseFilters.ElementNameFilter = "DataFilters";
				browseFilters.BrowseFilter = browseFilter.all;
				browseFilters.ReturnAllProperties = false;
				browseFilters.PropertyIDs = null;
				browseFilters.ReturnPropertyValues = false;
				BrowseElement[] array;
				if (this.m_unconvertedItemID == null)
				{
					browseFilters.ElementNameFilter = "CPX";
					array = server.Browse(itemID, browseFilters, out browsePosition);
					if (array == null || array.Length == 0)
					{
						return;
					}
					if (browsePosition != null)
					{
						browsePosition.Dispose();
						browsePosition = null;
					}
					itemID = new ItemIdentifier(array[0].ItemPath, array[0].ItemName);
					browseFilters.ElementNameFilter = "DataFilters";
				}
				array = server.Browse(itemID, browseFilters, out browsePosition);
				if (array != null && array.Length != 0)
				{
					this.m_filterItem = new ItemIdentifier(array[0].ItemPath, array[0].ItemName);
				}
			}
			finally
			{
				if (browsePosition != null)
				{
					browsePosition.Dispose();
					browsePosition = null;
				}
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00009118 File Offset: 0x00008118
		private void Clear()
		{
			this.m_typeSystemID = null;
			this.m_dictionaryID = null;
			this.m_typeID = null;
			this.m_dictionaryItemID = null;
			this.m_typeItemID = null;
			this.m_unconvertedItemID = null;
			this.m_unfilteredItemID = null;
			this.m_filterItem = null;
			this.m_filterValue = null;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00009164 File Offset: 0x00008164
		private bool Init(BrowseElement element)
		{
			base.ItemPath = element.ItemPath;
			base.ItemName = element.ItemName;
			this.m_name = element.Name;
			return this.Init(element.Properties);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00009198 File Offset: 0x00008198
		private bool Init(ItemProperty[] properties)
		{
			this.Clear();
			if (properties == null || properties.Length < 3)
			{
				return false;
			}
			foreach (ItemProperty itemProperty in properties)
			{
				if (itemProperty.ResultID.Succeeded())
				{
					if (itemProperty.ID == Property.TYPE_SYSTEM_ID)
					{
						this.m_typeSystemID = (string)itemProperty.Value;
					}
					else if (itemProperty.ID == Property.DICTIONARY_ID)
					{
						this.m_dictionaryID = (string)itemProperty.Value;
						this.m_dictionaryItemID = new ItemIdentifier(itemProperty.ItemPath, itemProperty.ItemName);
					}
					else if (itemProperty.ID == Property.TYPE_ID)
					{
						this.m_typeID = (string)itemProperty.Value;
						this.m_typeItemID = new ItemIdentifier(itemProperty.ItemPath, itemProperty.ItemName);
					}
					else if (itemProperty.ID == Property.UNCONVERTED_ITEM_ID)
					{
						this.m_unconvertedItemID = new ItemIdentifier(base.ItemPath, (string)itemProperty.Value);
					}
					else if (itemProperty.ID == Property.UNFILTERED_ITEM_ID)
					{
						this.m_unfilteredItemID = new ItemIdentifier(base.ItemPath, (string)itemProperty.Value);
					}
					else if (itemProperty.ID == Property.DATA_FILTER_VALUE)
					{
						this.m_filterValue = (string)itemProperty.Value;
					}
				}
			}
			return this.m_typeSystemID != null && this.m_dictionaryID != null && this.m_typeID != null;
		}

		// Token: 0x04000194 RID: 404
		public const string CPX_BRANCH = "CPX";

		// Token: 0x04000195 RID: 405
		public const string CPX_DATA_FILTERS = "DataFilters";

		// Token: 0x04000196 RID: 406
		public static readonly PropertyID[] CPX_PROPERTIES = new PropertyID[]
		{
			Property.TYPE_SYSTEM_ID,
			Property.DICTIONARY_ID,
			Property.TYPE_ID,
			Property.UNCONVERTED_ITEM_ID,
			Property.UNFILTERED_ITEM_ID,
			Property.DATA_FILTER_VALUE
		};

		// Token: 0x04000197 RID: 407
		private string m_name;

		// Token: 0x04000198 RID: 408
		private string m_typeSystemID;

		// Token: 0x04000199 RID: 409
		private string m_dictionaryID;

		// Token: 0x0400019A RID: 410
		private string m_typeID;

		// Token: 0x0400019B RID: 411
		private ItemIdentifier m_dictionaryItemID;

		// Token: 0x0400019C RID: 412
		private ItemIdentifier m_typeItemID;

		// Token: 0x0400019D RID: 413
		private ItemIdentifier m_unconvertedItemID;

		// Token: 0x0400019E RID: 414
		private ItemIdentifier m_unfilteredItemID;

		// Token: 0x0400019F RID: 415
		private ItemIdentifier m_filterItem;

		// Token: 0x040001A0 RID: 416
		private string m_filterValue;
	}
}
