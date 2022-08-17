using System;
using System.Collections;
using Opc.Da;

namespace Opc.Cpx
{
	// Token: 0x02000080 RID: 128
	public class ComplexTypeCache
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000360 RID: 864 RVA: 0x000093B0 File Offset: 0x000083B0
		// (set) Token: 0x06000361 RID: 865 RVA: 0x000093F0 File Offset: 0x000083F0
		public static Da.Server Server
		{
			get
			{
				Da.Server server;
				lock (typeof(ComplexTypeCache))
				{
					server = ComplexTypeCache.m_server;
				}
				return server;
			}
			set
			{
				lock (typeof(ComplexTypeCache))
				{
					ComplexTypeCache.m_server = value;
					ComplexTypeCache.m_items.Clear();
					ComplexTypeCache.m_dictionaries.Clear();
					ComplexTypeCache.m_descriptions.Clear();
				}
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000944C File Offset: 0x0000844C
		public static ComplexItem GetComplexItem(ItemIdentifier itemID)
		{
			if (itemID == null)
			{
				return null;
			}
			ComplexItem result;
			lock (typeof(ComplexTypeCache))
			{
				ComplexItem complexItem = new ComplexItem(itemID);
				try
				{
					complexItem.Update(ComplexTypeCache.m_server);
				}
				catch
				{
					complexItem = null;
				}
				ComplexTypeCache.m_items[itemID.Key] = complexItem;
				result = complexItem;
			}
			return result;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000094C4 File Offset: 0x000084C4
		public static ComplexItem GetComplexItem(BrowseElement element)
		{
			if (element == null)
			{
				return null;
			}
			ComplexItem complexItem;
			lock (typeof(ComplexTypeCache))
			{
				complexItem = ComplexTypeCache.GetComplexItem(new ItemIdentifier(element.ItemPath, element.ItemName));
			}
			return complexItem;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00009518 File Offset: 0x00008518
		public static string GetTypeDictionary(ItemIdentifier itemID)
		{
			if (itemID == null)
			{
				return null;
			}
			string result;
			lock (typeof(ComplexTypeCache))
			{
				string text = (string)ComplexTypeCache.m_dictionaries[itemID.Key];
				if (text != null)
				{
					result = text;
				}
				else
				{
					ComplexItem complexItem = ComplexTypeCache.GetComplexItem(itemID);
					if (complexItem != null)
					{
						text = complexItem.GetTypeDictionary(ComplexTypeCache.m_server);
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000958C File Offset: 0x0000858C
		public static string GetTypeDescription(ItemIdentifier itemID)
		{
			if (itemID == null)
			{
				return null;
			}
			string result;
			lock (typeof(ComplexTypeCache))
			{
				string text = null;
				ComplexItem complexItem = ComplexTypeCache.GetComplexItem(itemID);
				if (complexItem != null)
				{
					text = (string)ComplexTypeCache.m_descriptions[complexItem.TypeItemID.Key];
					if (text != null)
					{
						return text;
					}
					text = ((string)(m_descriptions[complexItem.TypeItemID.Key] = complexItem.GetTypeDescription(m_server)));
				}
				result = text;
			}
			return result;
		}

		// Token: 0x040001A1 RID: 417
		private static Da.Server m_server = null;

		// Token: 0x040001A2 RID: 418
		private static Hashtable m_items = new Hashtable();

		// Token: 0x040001A3 RID: 419
		private static Hashtable m_dictionaries = new Hashtable();

		// Token: 0x040001A4 RID: 420
		private static Hashtable m_descriptions = new Hashtable();
	}
}
