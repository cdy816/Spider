using System;

namespace Opc.Cpx
{
	// Token: 0x02000066 RID: 102
	public class BinaryStream
	{
		// Token: 0x06000278 RID: 632 RVA: 0x00006FE9 File Offset: 0x00005FE9
		protected BinaryStream()
		{
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00006FF4 File Offset: 0x00005FF4
		internal bool IsArrayField(FieldType field)
		{
			if (field.ElementCountSpecified)
			{
				if (field.ElementCountRef != null || field.FieldTerminator != null)
				{
					throw new InvalidSchemaException("Multiple array size attributes specified for field '" + field.Name + " '.");
				}
				return true;
			}
			else
			{
				if (field.ElementCountRef == null)
				{
					return field.FieldTerminator != null;
				}
				if (field.FieldTerminator != null)
				{
					throw new InvalidSchemaException("Multiple array size attributes specified for field '" + field.Name + " '.");
				}
				return true;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00007070 File Offset: 0x00006070
		internal byte[] GetTerminator(Context context, FieldType field)
		{
			if (field.FieldTerminator == null)
			{
				throw new InvalidSchemaException(field.Name + " is not a terminated group.");
			}
			string text = Convert.ToString(field.FieldTerminator).ToUpper();
			byte[] array = new byte[text.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = System.Convert.ToByte(text.Substring(i * 2, 2), 16);
			}
			return array;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000070E0 File Offset: 0x000060E0
		internal Context InitializeContext(byte[] buffer, TypeDictionary dictionary, string typeName)
		{
			Context result = new Context(buffer);
			result.Dictionary = dictionary;
			result.Type = null;
			result.BigEndian = dictionary.DefaultBigEndian;
			result.CharWidth = dictionary.DefaultCharWidth;
			result.StringEncoding = dictionary.DefaultStringEncoding;
			result.FloatFormat = dictionary.DefaultFloatFormat;
			TypeDescription[] typeDescription = dictionary.TypeDescription;
			int i = 0;
			while (i < typeDescription.Length)
			{
				TypeDescription typeDescription2 = typeDescription[i];
				if (typeDescription2.TypeID == typeName)
				{
					result.Type = typeDescription2;
					if (typeDescription2.DefaultBigEndianSpecified)
					{
						result.BigEndian = typeDescription2.DefaultBigEndian;
					}
					if (typeDescription2.DefaultCharWidthSpecified)
					{
						result.CharWidth = typeDescription2.DefaultCharWidth;
					}
					if (typeDescription2.DefaultStringEncoding != null)
					{
						result.StringEncoding = typeDescription2.DefaultStringEncoding;
					}
					if (typeDescription2.DefaultFloatFormat != null)
					{
						result.FloatFormat = typeDescription2.DefaultFloatFormat;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (result.Type == null)
			{
				throw new InvalidSchemaException("Type '" + typeName + "' not found in dictionary.");
			}
			return result;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000071E0 File Offset: 0x000061E0
		internal void SwapBytes(byte[] bytes, int index, int length)
		{
			for (int i = 0; i < length / 2; i++)
			{
				byte b = bytes[index + length - 1 - i];
				bytes[index + length - 1 - i] = bytes[index + i];
				bytes[index + i] = b;
			}
		}
	}
}
