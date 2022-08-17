using System;
using System.Collections;

namespace Opc.Cpx
{
	// Token: 0x02000089 RID: 137
	public class BinaryWriter : BinaryStream
	{
		// Token: 0x060003EC RID: 1004 RVA: 0x0000AF8C File Offset: 0x00009F8C
		public byte[] Write(ComplexValue namedValue, TypeDictionary dictionary, string typeName)
		{
			if (namedValue == null)
			{
				throw new ArgumentNullException("namedValue");
			}
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			Context context = base.InitializeContext(null, dictionary, typeName);
			int num = this.WriteType(context, namedValue);
			if (num == 0)
			{
				throw new InvalidDataToWriteException("Could not write value into buffer.");
			}
			context.Buffer = new byte[num];
			int num2 = this.WriteType(context, namedValue);
			if (num2 != num)
			{
				throw new InvalidDataToWriteException("Could not write value into buffer.");
			}
			return context.Buffer;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000B010 File Offset: 0x0000A010
		private int WriteType(Context context, ComplexValue namedValue)
		{
			TypeDescription type = context.Type;
			int index = context.Index;
			if (namedValue.Value == null || namedValue.Value.GetType() != typeof(ComplexValue[]))
			{
				throw new InvalidDataToWriteException("Type instance does not contain field values.");
			}
			ComplexValue[] array = (ComplexValue[])namedValue.Value;
			if (array.Length != type.Field.Length)
			{
				throw new InvalidDataToWriteException("Type instance does not contain the correct number of fields.");
			}
			byte b = 0;
			for (int i = 0; i < type.Field.Length; i++)
			{
				FieldType fieldType = type.Field[i];
				ComplexValue complexValue = array[i];
				if (b != 0 && fieldType.GetType() != typeof(BitString))
				{
					context.Index++;
					b = 0;
				}
				int num;
				if (base.IsArrayField(fieldType))
				{
					num = this.WriteArrayField(context, fieldType, i, array, complexValue.Value);
				}
				else if (fieldType.GetType() == typeof(TypeReference))
				{
					num = this.WriteField(context, (TypeReference)fieldType, complexValue);
				}
				else
				{
					num = this.WriteField(context, fieldType, i, array, complexValue.Value, ref b);
				}
				if (num == 0 && b == 0)
				{
					throw new InvalidDataToWriteException(string.Concat(new string[]
					{
						"Could not write field '",
						fieldType.Name,
						"' in type '",
						type.TypeID,
						"'."
					}));
				}
				context.Index += num;
			}
			if (b != 0)
			{
				context.Index++;
			}
			return context.Index - index;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000B1B0 File Offset: 0x0000A1B0
		private int WriteField(Context context, FieldType field, int fieldIndex, ComplexValue[] fieldValues, object fieldValue, ref byte bitOffset)
		{
			System.Type type = field.GetType();
			if (type == typeof(Integer) || type.IsSubclassOf(typeof(Integer)))
			{
				return this.WriteField(context, (Integer)field, fieldValue);
			}
			if (type == typeof(FloatingPoint) || type.IsSubclassOf(typeof(FloatingPoint)))
			{
				return this.WriteField(context, (FloatingPoint)field, fieldValue);
			}
			if (type == typeof(CharString) || type.IsSubclassOf(typeof(CharString)))
			{
				return this.WriteField(context, (CharString)field, fieldIndex, fieldValues, fieldValue);
			}
			if (type == typeof(BitString) || type.IsSubclassOf(typeof(BitString)))
			{
				return this.WriteField(context, (BitString)field, fieldValue, ref bitOffset);
			}
			if (type == typeof(TypeReference) || type.IsSubclassOf(typeof(TypeReference)))
			{
				return this.WriteField(context, (TypeReference)field, fieldValue);
			}
			throw new NotImplementedException("Fields of type '" + type.ToString() + "' are not implemented yet.");
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000B2D0 File Offset: 0x0000A2D0
		private int WriteField(Context context, TypeReference field, object fieldValue)
		{
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
			if (fieldValue.GetType() != typeof(ComplexValue))
			{
				throw new InvalidDataToWriteException("Instance of type is not the correct type.");
			}
			return this.WriteType(context, (ComplexValue)fieldValue);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000B3BC File Offset: 0x0000A3BC
		private int WriteField(Context context, Integer field, object fieldValue)
		{
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
			if (buffer != null)
			{
				if (buffer.Length - context.Index < num)
				{
					throw new InvalidDataToWriteException("Unexpected end of buffer.");
				}
				byte[] array;
				if (flag)
				{
					int num2 = num;
					switch (num2)
					{
					case 1:
					{
						array = new byte[1];
						sbyte b = System.Convert.ToSByte(fieldValue);
						if (b < 0)
						{
							array[0] = (byte)(255 + (int)b + 1);
							goto IL_205;
						}
						array[0] = (byte)b;
						goto IL_205;
					}
					case 2:
						array = BitConverter.GetBytes(System.Convert.ToInt16(fieldValue));
						goto IL_205;
					case 3:
						break;
					case 4:
						array = BitConverter.GetBytes(System.Convert.ToInt32(fieldValue));
						goto IL_205;
					default:
						if (num2 == 8)
						{
							array = BitConverter.GetBytes(System.Convert.ToInt64(fieldValue));
							goto IL_205;
						}
						break;
					}
					array = (byte[])fieldValue;
				}
				else
				{
					int num3 = num;
					switch (num3)
					{
					case 1:
						array = new byte[]
						{
							 System.Convert.ToByte(fieldValue)
						};
						goto IL_205;
					case 2:
						array = BitConverter.GetBytes(System.Convert.ToUInt16(fieldValue));
						goto IL_205;
					case 3:
						break;
					case 4:
						array = BitConverter.GetBytes(System.Convert.ToUInt32(fieldValue));
						goto IL_205;
					default:
						if (num3 == 8)
						{
							array = BitConverter.GetBytes(System.Convert.ToUInt64(fieldValue));
							goto IL_205;
						}
						break;
					}
					array = (byte[])fieldValue;
				}
				IL_205:
				if (context.BigEndian)
				{
					base.SwapBytes(array, 0, num);
				}
				for (int i = 0; i < array.Length; i++)
				{
					buffer[context.Index + i] = array[i];
				}
			}
			return num;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000B604 File Offset: 0x0000A604
		private int WriteField(Context context, FloatingPoint field, object fieldValue)
		{
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
			if (buffer != null)
			{
				if (buffer.Length - context.Index < num)
				{
					throw new InvalidDataToWriteException("Unexpected end of buffer.");
				}
				byte[] array;
				if (a == "IEEE-754")
				{
					int num2 = num;
					if (num2 != 4)
					{
						if (num2 != 8)
						{
							array = (byte[])fieldValue;
						}
						else
						{
							array = BitConverter.GetBytes(System.Convert.ToDouble(fieldValue));
						}
					}
					else
					{
						array = BitConverter.GetBytes(System.Convert.ToSingle(fieldValue));
					}
				}
				else
				{
					array = (byte[])fieldValue;
				}
				for (int i = 0; i < array.Length; i++)
				{
					buffer[context.Index + i] = array[i];
				}
			}
			return num;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000B708 File Offset: 0x0000A708
		private int WriteField(Context context, CharString field, int fieldIndex, ComplexValue[] fieldValues, object fieldValue)
		{
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
			byte[] array = null;
			if (num2 == -1)
			{
				if (num > 2)
				{
					if (fieldValue.GetType() != typeof(byte[]))
					{
						throw new InvalidDataToWriteException("Field value is not a byte array.");
					}
					array = (byte[])fieldValue;
					num2 = array.Length / num;
				}
				else
				{
					if (fieldValue.GetType() != typeof(string))
					{
						throw new InvalidDataToWriteException("Field value is not a string.");
					}
					string text = (string)fieldValue;
					num2 = text.Length + 1;
					if (num == 1)
					{
						num2 = 1;
						foreach (char value in text)
						{
							num2++;
							byte[] bytes = BitConverter.GetBytes(value);
							if (bytes[1] != 0)
							{
								num2++;
							}
						}
					}
				}
			}
			if (field.CharCountRef != null)
			{
				this.WriteReference(context, field, fieldIndex, fieldValues, field.CharCountRef, num2);
			}
			if (buffer != null)
			{
				if (array == null)
				{
					string text3 = (string)fieldValue;
					array = new byte[num * num2];
					int num3 = 0;
					int num4 = 0;
					while (num4 < text3.Length && num3 < array.Length)
					{
						byte[] bytes2 = BitConverter.GetBytes(text3[num4]);
						array[num3++] = bytes2[0];
						if (num == 2 || bytes2[1] != 0)
						{
							array[num3++] = bytes2[1];
						}
						num4++;
					}
				}
				if (buffer.Length - context.Index < array.Length)
				{
					throw new InvalidDataToWriteException("Unexpected end of buffer.");
				}
				for (int j = 0; j < array.Length; j++)
				{
					buffer[context.Index + j] = array[j];
				}
				if (context.BigEndian && num > 1)
				{
					for (int k = 0; k < array.Length; k += num)
					{
						base.SwapBytes(buffer, context.Index + k, num);
					}
				}
			}
			return num2 * num;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000B91C File Offset: 0x0000A91C
		private int WriteField(Context context, BitString field, object fieldValue, ref byte bitOffset)
		{
			byte[] buffer = context.Buffer;
			int num = field.LengthSpecified ? field.Length : 8;
			int num2 = (num % 8 == 0) ? (num / 8) : (num / 8 + 1);
			if (fieldValue.GetType() != typeof(byte[]))
			{
				throw new InvalidDataToWriteException("Wrong data type to write.");
			}
			byte[] array = (byte[])fieldValue;
			if (buffer != null)
			{
				if (buffer.Length - context.Index < num2)
				{
					throw new InvalidDataToWriteException("Unexpected end of buffer.");
				}
				int num3 = num;
				byte b = (bitOffset == 0) ? byte.MaxValue : ((byte)((128 >> (int)(bitOffset - 1)) - 1));
				int num4 = 0;
				while (num3 >= 0 && num4 < num2)
				{
					byte[] array2 = buffer;
					int num5 = context.Index + num4;
					array2[num5] += (byte)(((int)b & (1 << num3) - 1 & (int)array[num4]) << (int)bitOffset);
					if (num3 + (int)bitOffset <= 8)
					{
						break;
					}
					if (context.Index + num4 + 1 >= buffer.Length)
					{
						throw new InvalidDataToWriteException("Unexpected end of buffer.");
					}
					byte[] array3 = buffer;
					int num6 = context.Index + num4 + 1;
					array3[num6] += (byte)(((int)(~(int)b) & (1 << num3) - 1 & (int)array[num4]) >> (int)(8 - bitOffset));
					if (num3 <= 8)
					{
						break;
					}
					num3 -= 8;
					num4++;
				}
			}
			num2 = (num + (int)bitOffset) / 8;
			bitOffset = (byte)((num + (int)bitOffset) % 8);
			return num2;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000BA88 File Offset: 0x0000AA88
		private int WriteArrayField(Context context, FieldType field, int fieldIndex, ComplexValue[] fieldValues, object fieldValue)
		{
			int index = context.Index;
			if (!fieldValue.GetType().IsArray)
			{
				throw new InvalidDataToWriteException("Array field value is not an array type.");
			}
			Array array = (Array)fieldValue;
			byte b = 0;
			if (field.ElementCountSpecified)
			{
				int num = 0;
				IEnumerator enumerator = array.GetEnumerator();
				//using (IEnumerator enumerator = array.GetEnumerator())
				//{
					while (enumerator.MoveNext())
					{
						object fieldValue2 = enumerator.Current;
						if (num == field.ElementCount)
						{
							break;
						}
						int num2 = this.WriteField(context, field, fieldIndex, fieldValues, fieldValue2, ref b);
						if (num2 == 0 && b == 0)
						{
							break;
						}
						context.Index += num2;
						num++;
					}
					goto IL_D0;
				//}
				IL_A2:
				int num3 = this.WriteField(context, field, fieldIndex, fieldValues, null, ref b);
				if (num3 == 0 && b == 0)
				{
					goto IL_217;
				}
				context.Index += num3;
				num++;
				IL_D0:
				if (num < field.ElementCount)
				{
					goto IL_A2;
				}
			}
			else if (field.ElementCountRef != null)
			{
				int num4 = 0;
				foreach (object fieldValue3 in array)
				{
					int num5 = this.WriteField(context, field, fieldIndex, fieldValues, fieldValue3, ref b);
					if (num5 == 0 && b == 0)
					{
						break;
					}
					context.Index += num5;
					num4++;
				}
				this.WriteReference(context, field, fieldIndex, fieldValues, field.ElementCountRef, num4);
			}
			else if (field.FieldTerminator != null)
			{
				foreach (object fieldValue4 in array)
				{
					int num6 = this.WriteField(context, field, fieldIndex, fieldValues, fieldValue4, ref b);
					if (num6 == 0 && b == 0)
					{
						break;
					}
					context.Index += num6;
				}
				byte[] terminator = base.GetTerminator(context, field);
				if (context.Buffer != null)
				{
					for (int i = 0; i < terminator.Length; i++)
					{
						context.Buffer[context.Index + i] = terminator[i];
					}
				}
				context.Index += terminator.Length;
			}
			IL_217:
			if (b != 0)
			{
				context.Index++;
			}
			return context.Index - index;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000BCF0 File Offset: 0x0000ACF0
		private void WriteReference(Context context, FieldType field, int fieldIndex, ComplexValue[] fieldValues, string fieldName, int count)
		{
			ComplexValue complexValue = null;
			if (fieldName.Length == 0)
			{
				if (fieldIndex > 0 && fieldIndex - 1 < fieldValues.Length)
				{
					complexValue = fieldValues[fieldIndex - 1];
				}
			}
			else
			{
				for (int i = 0; i < fieldIndex; i++)
				{
					complexValue = fieldValues[i];
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
			if (context.Buffer == null)
			{
				complexValue.Value = count;
			}
			if (!count.Equals(complexValue.Value))
			{
				throw new InvalidDataToWriteException("Reference field value and the actual array length are not equal.");
			}
		}
	}
}
