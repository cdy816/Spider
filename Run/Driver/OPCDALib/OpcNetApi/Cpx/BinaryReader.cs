using System;
using System.Collections;

namespace Opc.Cpx
{
	// Token: 0x02000081 RID: 129
	public class BinaryReader : BinaryStream
	{
		// Token: 0x06000368 RID: 872 RVA: 0x0000964C File Offset: 0x0000864C
		public ComplexValue Read(byte[] buffer, TypeDictionary dictionary, string typeName)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			Context context = base.InitializeContext(buffer, dictionary, typeName);
			ComplexValue result = null;
			if (this.ReadType(context, out result) == 0)
			{
				throw new InvalidSchemaException("Type '" + typeName + "' not found in dictionary.");
			}
			return result;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x000096B4 File Offset: 0x000086B4
		private int ReadType(Context context, out ComplexValue complexValue)
		{
			complexValue = null;
			TypeDescription type = context.Type;
			int index = context.Index;
			byte b = 0;
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < type.Field.Length; i++)
			{
				FieldType fieldType = type.Field[i];
				ComplexValue complexValue2 = new ComplexValue();
				complexValue2.Name = ((fieldType.Name != null && fieldType.Name.Length != 0) ? fieldType.Name : ("[" + i.ToString() + "]"));
				complexValue2.Type = null;
				complexValue2.Value = null;
				if (b != 0 && fieldType.GetType() != typeof(BitString))
				{
					context.Index++;
					b = 0;
				}
				int num;
				if (base.IsArrayField(fieldType))
				{
					num = this.ReadArrayField(context, fieldType, i, arrayList, out complexValue2.Value);
				}
				else if (fieldType.GetType() == typeof(TypeReference))
				{
					object obj = null;
					num = this.ReadField(context, (TypeReference)fieldType, out obj);
					complexValue2.Name = fieldType.Name;
					complexValue2.Type = ((ComplexValue)obj).Type;
					complexValue2.Value = ((ComplexValue)obj).Value;
				}
				else
				{
					num = this.ReadField(context, fieldType, i, arrayList, out complexValue2.Value, ref b);
				}
				if (num == 0 && b == 0)
				{
					throw new InvalidDataInBufferException(string.Concat(new string[]
					{
						"Could not read field '",
						fieldType.Name,
						"' in type '",
						type.TypeID,
						"'."
					}));
				}
				context.Index += num;
				if (complexValue2.Type == null)
				{
					complexValue2.Type = Convert.ToString(complexValue2.Value.GetType());
				}
				arrayList.Add(complexValue2);
			}
			if (b != 0)
			{
				context.Index++;
			}
			complexValue = new ComplexValue();
			complexValue.Name = type.TypeID;
			complexValue.Type = type.TypeID;
			complexValue.Value = (ComplexValue[])arrayList.ToArray(typeof(ComplexValue));
			return context.Index - index;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000098F8 File Offset: 0x000088F8
		private int ReadField(Context context, FieldType field, int fieldIndex, ArrayList fieldValues, out object fieldValue, ref byte bitOffset)
		{
			fieldValue = null;
			System.Type type = field.GetType();
			if (type == typeof(Integer) || type.IsSubclassOf(typeof(Integer)))
			{
				return this.ReadField(context, (Integer)field, out fieldValue);
			}
			if (type == typeof(FloatingPoint) || type.IsSubclassOf(typeof(FloatingPoint)))
			{
				return this.ReadField(context, (FloatingPoint)field, out fieldValue);
			}
			if (type == typeof(CharString) || type.IsSubclassOf(typeof(CharString)))
			{
				return this.ReadField(context, (CharString)field, fieldIndex, fieldValues, out fieldValue);
			}
			if (type == typeof(BitString) || type.IsSubclassOf(typeof(BitString)))
			{
				return this.ReadField(context, (BitString)field, out fieldValue, ref bitOffset);
			}
			if (type == typeof(TypeReference))
			{
				return this.ReadField(context, (TypeReference)field, out fieldValue);
			}
			throw new NotImplementedException("Fields of type '" + type.ToString() + "' are not implemented yet.");
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00009A08 File Offset: 0x00008A08
		private int ReadField(Context context, TypeReference field, out object fieldValue)
		{
			fieldValue = null;
			TypeDescription[] typeDescription = context.Dictionary.TypeDescription;
			int i = 0;
			while (i < typeDescription.Length)
			{
				TypeDescription typeDescription2 = typeDescription[i];
				if (typeDescription2.TypeID == field.TypeID)
				{
					context.Type = typeDescription2;
					if (typeDescription2.DefaultBigEndianSpecified)
					{
						context.BigEndian = typeDescription2.DefaultBigEndian;
					}
					if (typeDescription2.DefaultCharWidthSpecified)
					{
						context.CharWidth = typeDescription2.DefaultCharWidth;
					}
					if (typeDescription2.DefaultStringEncoding != null)
					{
						context.StringEncoding = typeDescription2.DefaultStringEncoding;
					}
					if (typeDescription2.DefaultFloatFormat != null)
					{
						context.FloatFormat = typeDescription2.DefaultFloatFormat;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (context.Type == null)
			{
				throw new InvalidSchemaException("Reference type '" + field.TypeID + "' not found.");
			}
			ComplexValue complexValue = null;
			int num = this.ReadType(context, out complexValue);
			if (num == 0)
			{
				fieldValue = null;
			}
			fieldValue = complexValue;
			return num;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00009AEC File Offset: 0x00008AEC
		private int ReadField(Context context, Integer field, out object fieldValue)
		{
			fieldValue = null;
			byte[] buffer = context.Buffer;
			int num = field.LengthSpecified ? field.Length : 4;
			bool flag = field.Signed;
			if (field.GetType() == typeof(Int8))
			{
				num = 1;
				flag = true;
			}
			else if (field.GetType() == typeof(Int16))
			{
				num = 2;
				flag = true;
			}
			else if (field.GetType() == typeof(Int32))
			{
				num = 4;
				flag = true;
			}
			else if (field.GetType() == typeof(Int64))
			{
				num = 8;
				flag = true;
			}
			else if (field.GetType() == typeof(UInt8))
			{
				num = 1;
				flag = false;
			}
			else if (field.GetType() == typeof(UInt16))
			{
				num = 2;
				flag = false;
			}
			else if (field.GetType() == typeof(UInt32))
			{
				num = 4;
				flag = false;
			}
			else if (field.GetType() == typeof(UInt64))
			{
				num = 8;
				flag = false;
			}
			if (buffer.Length - context.Index < num)
			{
				throw new InvalidDataInBufferException("Unexpected end of buffer.");
			}
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = buffer[context.Index + i];
			}
			if (context.BigEndian)
			{
				base.SwapBytes(array, 0, num);
			}
			if (flag)
			{
				int num2 = num;
				switch (num2)
				{
				case 1:
					if (array[0] < 128)
					{
						fieldValue = (sbyte)array[0];
						return num;
					}
					fieldValue = (sbyte)(-(sbyte)array[0]);
					return num;
				case 2:
					fieldValue = BitConverter.ToInt16(array, 0);
					return num;
				case 3:
					break;
				case 4:
					fieldValue = BitConverter.ToInt32(array, 0);
					return num;
				default:
					if (num2 == 8)
					{
						fieldValue = BitConverter.ToInt64(array, 0);
						return num;
					}
					break;
				}
				fieldValue = array;
			}
			else
			{
				int num3 = num;
				switch (num3)
				{
				case 1:
					fieldValue = array[0];
					return num;
				case 2:
					fieldValue = BitConverter.ToUInt16(array, 0);
					return num;
				case 3:
					break;
				case 4:
					fieldValue = BitConverter.ToUInt32(array, 0);
					return num;
				default:
					if (num3 == 8)
					{
						fieldValue = BitConverter.ToUInt64(array, 0);
						return num;
					}
					break;
				}
				fieldValue = array;
			}
			return num;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00009D24 File Offset: 0x00008D24
		private int ReadField(Context context, FloatingPoint field, out object fieldValue)
		{
			fieldValue = null;
			byte[] buffer = context.Buffer;
			int num = field.LengthSpecified ? field.Length : 4;
			string a = (field.FloatFormat != null) ? field.FloatFormat : context.FloatFormat;
			if (field.GetType() == typeof(Single))
			{
				num = 4;
				a = "IEEE-754";
			}
			else if (field.GetType() == typeof(Double))
			{
				num = 8;
				a = "IEEE-754";
			}
			if (buffer.Length - context.Index < num)
			{
				throw new InvalidDataInBufferException("Unexpected end of buffer.");
			}
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = buffer[context.Index + i];
			}
			if (a == "IEEE-754")
			{
				int num2 = num;
				if (num2 != 4)
				{
					if (num2 != 8)
					{
						fieldValue = array;
					}
					else
					{
						fieldValue = BitConverter.ToDouble(array, 0);
					}
				}
				else
				{
					fieldValue = BitConverter.ToSingle(array, 0);
				}
			}
			else
			{
				fieldValue = array;
			}
			return num;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00009E24 File Offset: 0x00008E24
		private int ReadField(Context context, CharString field, int fieldIndex, ArrayList fieldValues, out object fieldValue)
		{
			fieldValue = null;
			byte[] buffer = context.Buffer;
			int num = field.CharWidthSpecified ? field.CharWidth : context.CharWidth;
			int num2 = field.LengthSpecified ? field.Length : -1;
			if (field.GetType() == typeof(Ascii))
			{
				num = 1;
			}
			else if (field.GetType() == typeof(Unicode))
			{
				num = 2;
			}
			if (field.CharCountRef != null)
			{
				num2 = this.ReadReference(context, field, fieldIndex, fieldValues, field.CharCountRef);
			}
			if (num2 == -1)
			{
				num2 = 0;
				for (int i = context.Index; i < context.Buffer.Length - num + 1; i += num)
				{
					num2++;
					bool flag = true;
					for (int j = 0; j < num; j++)
					{
						if (context.Buffer[i + j] != 0)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (buffer.Length - context.Index < num * num2)
			{
				throw new InvalidDataInBufferException("Unexpected end of buffer.");
			}
			if (num > 2)
			{
				byte[] array = new byte[num2 * num];
				for (int k = 0; k < num2 * num; k++)
				{
					array[k] = buffer[context.Index + k];
				}
				if (context.BigEndian)
				{
					for (int l = 0; l < array.Length; l += num)
					{
						base.SwapBytes(array, 0, num);
					}
				}
				fieldValue = array;
			}
			else
			{
				char[] array2 = new char[num2];
				for (int m = 0; m < num2; m++)
				{
					if (num == 1)
					{
						array2[m] = System.Convert.ToChar(buffer[context.Index + m]);
					}
					else
					{
						byte[] array3 = new byte[]
						{
							buffer[context.Index + 2 * m],
							buffer[context.Index + 2 * m + 1]
						};
						if (context.BigEndian)
						{
							base.SwapBytes(array3, 0, 2);
						}
						array2[m] = BitConverter.ToChar(array3, 0);
					}
				}
				string text = new string(array2);
				char[] trimChars = new char[1];
				fieldValue = text.TrimEnd(trimChars);
			}
			return num2 * num;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000A01C File Offset: 0x0000901C
		private int ReadField(Context context, BitString field, out object fieldValue, ref byte bitOffset)
		{
			fieldValue = null;
			byte[] buffer = context.Buffer;
			int num = field.LengthSpecified ? field.Length : 8;
			int num2 = (num % 8 == 0) ? (num / 8) : (num / 8 + 1);
			if (buffer.Length - context.Index < num2)
			{
				throw new InvalidDataInBufferException("Unexpected end of buffer.");
			}
			byte[] array = new byte[num2];
			int num3 = num;
			byte b = (byte)(~(byte)((1 << (int)bitOffset) - 1));
			int num4 = 0;
			while (num3 >= 0 && num4 < num2)
			{
				array[num4] = (byte)((b & buffer[context.Index + num4]) >> (int)bitOffset);
				if (num3 + (int)bitOffset <= 8)
				{
					byte[] array2 = array;
					int num5 = num4;
					array2[num5] &= (byte)((1 << num3) - 1);
					break;
				}
				if (context.Index + num4 + 1 >= buffer.Length)
				{
					throw new InvalidDataInBufferException("Unexpected end of buffer.");
				}
				byte[] array3 = array;
				int num6 = num4;
				array3[num6] += (byte)((~b & buffer[context.Index + num4 + 1]) << (int)(8 - bitOffset));
				if (num3 <= 8)
				{
					byte[] array4 = array;
					int num7 = num4;
					array4[num7] &= (byte)((1 << num3) - 1);
					break;
				}
				num3 -= 8;
				num4++;
			}
			fieldValue = array;
			num2 = (num + (int)bitOffset) / 8;
			bitOffset = (byte)((num + (int)bitOffset) % 8);
			return num2;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000A17C File Offset: 0x0000917C
		private int ReadArrayField(Context context, FieldType field, int fieldIndex, ArrayList fieldValues, out object fieldValue)
		{
			fieldValue = null;
			int index = context.Index;
			ArrayList arrayList = new ArrayList();
			object value = null;
			byte b = 0;
			if (field.ElementCountSpecified)
			{
				for (int i = 0; i < field.ElementCount; i++)
				{
					int num = this.ReadField(context, field, fieldIndex, fieldValues, out value, ref b);
					if (num == 0 && b == 0)
					{
						break;
					}
					arrayList.Add(value);
					context.Index += num;
				}
			}
			else if (field.ElementCountRef != null)
			{
				int num2 = this.ReadReference(context, field, fieldIndex, fieldValues, field.ElementCountRef);
				for (int j = 0; j < num2; j++)
				{
					int num3 = this.ReadField(context, field, fieldIndex, fieldValues, out value, ref b);
					if (num3 == 0 && b == 0)
					{
						break;
					}
					arrayList.Add(value);
					context.Index += num3;
				}
			}
			else if (field.FieldTerminator != null)
			{
				byte[] terminator = base.GetTerminator(context, field);
				while (context.Index < context.Buffer.Length)
				{
					bool flag = true;
					for (int k = 0; k < terminator.Length; k++)
					{
						if (terminator[k] != context.Buffer[context.Index + k])
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						context.Index += terminator.Length;
						break;
					}
					int num4 = this.ReadField(context, field, fieldIndex, fieldValues, out value, ref b);
					if (num4 == 0 && b == 0)
					{
						break;
					}
					arrayList.Add(value);
					context.Index += num4;
				}
			}
			if (b != 0)
			{
				context.Index++;
			}
			System.Type type = null;
			foreach (object obj in arrayList)
			{
				if (type == null)
				{
					type = obj.GetType();
				}
				else if (type != obj.GetType())
				{
					type = typeof(object);
					break;
				}
			}
			fieldValue = arrayList.ToArray(type);
			return context.Index - index;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000A394 File Offset: 0x00009394
		private int ReadReference(Context context, FieldType field, int fieldIndex, ArrayList fieldValues, string fieldName)
		{
			ComplexValue complexValue = null;
			if (fieldName.Length == 0)
			{
				if (fieldIndex > 0 && fieldIndex - 1 < fieldValues.Count)
				{
					complexValue = (ComplexValue)fieldValues[fieldIndex - 1];
				}
			}
			else
			{
				for (int i = 0; i < fieldIndex; i++)
				{
					complexValue = (ComplexValue)fieldValues[i];
					if (complexValue.Name == fieldName)
					{
						break;
					}
					complexValue = null;
				}
			}
			if (complexValue == null)
			{
				throw new InvalidSchemaException("Referenced field not found (" + fieldName + ").");
			}
			return System.Convert.ToInt32(complexValue.Value);
		}
	}
}
