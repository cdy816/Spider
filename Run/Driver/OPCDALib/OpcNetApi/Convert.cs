using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace Opc
{
    // Token: 0x02000002 RID: 2
    public class Convert
    {
        // Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
        public static bool IsValid(Array array)
        {
            return array != null && array.Length > 0;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020E0 File Offset: 0x000010E0
        public static bool IsEmpty(Array array)
        {
            return array == null || array.Length == 0;
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000020F0 File Offset: 0x000010F0
        public static bool IsValid(string target)
        {
            return target != null && target.Length > 0;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002100 File Offset: 0x00001100
        public static bool IsEmpty(string target)
        {
            return target == null || target.Length == 0;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002110 File Offset: 0x00001110
        public static object Clone(object source)
        {
            if (source == null)
            {
                return null;
            }
            if (source.GetType().IsValueType)
            {
                return source;
            }
            if (source.GetType().IsArray || source.GetType() == typeof(Array))
            {
                Array array = (Array)((Array)source).Clone();
                for (int i = 0; i < array.Length; i++)
                {
                    array.SetValue(Convert.Clone(array.GetValue(i)), i);
                }
                return array;
            }
            object result;
            try
            {
                result = ((ICloneable)source).Clone();
            }
            catch
            {
                throw new NotSupportedException("Object cannot be cloned.");
            }
            return result;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000021B4 File Offset: 0x000011B4
        public static bool Compare(object a, object b)
        {
            if (a == null || b == null)
            {
                return a == null && b == null;
            }
            System.Type type = a.GetType();
            System.Type type2 = b.GetType();
            if (type != type2)
            {
                return false;
            }
            if (!type.IsArray || !type2.IsArray)
            {
                return a.Equals(b);
            }
            Array array = (Array)a;
            Array array2 = (Array)b;
            if (array.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (!Convert.Compare(array.GetValue(i), array2.GetValue(i)))
                {
                    return false;
                }
            }
            return true;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x0000224C File Offset: 0x0000124C
        public static object ChangeType(object source, System.Type newType)
        {
            if (source == null)
            {
                if (newType != null && newType.IsValueType)
                {
                    return Activator.CreateInstance(newType);
                }
                return null;
            }
            else
            {
                if (newType == null || newType == typeof(object) || newType == source.GetType())
                {
                    return Convert.Clone(source);
                }
                System.Type type = source.GetType();
                if (type.IsArray && newType.IsArray)
                {
                    ArrayList arrayList = new ArrayList(((Array)source).Length);
                    foreach (object source2 in ((Array)source))
                    {
                        arrayList.Add(Convert.ChangeType(source2, newType.GetElementType()));
                    }
                    return arrayList.ToArray(newType.GetElementType());
                }
                if (!type.IsArray && newType.IsArray)
                {
                    return new ArrayList(1)
                    {
                        Convert.ChangeType(source, newType.GetElementType())
                    }.ToArray(newType.GetElementType());
                }
                if (type.IsArray && !newType.IsArray && ((Array)source).Length == 1)
                {
                    return Convert.ChangeType(((Array)source).GetValue(0), newType);
                }
                if (type.IsArray && newType == typeof(string))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("{ ");
                    int num = 0;
                    foreach (object source3 in ((Array)source))
                    {
                        stringBuilder.AppendFormat("{0}", Convert.ChangeType(source3, typeof(string)));
                        num++;
                        if (num < ((Array)source).Length)
                        {
                            stringBuilder.Append(" | ");
                        }
                    }
                    stringBuilder.Append(" }");
                    return stringBuilder.ToString();
                }
                if (newType.IsEnum)
                {
                    if (type != typeof(string))
                    {
                        return Enum.ToObject(newType, source);
                    }
                    if (((string)source).Length > 0 && char.IsDigit((string)source, 0))
                    {
                        return Enum.ToObject(newType, System.Convert.ToInt32(source));
                    }
                    return Enum.Parse(newType, (string)source);
                }
                else
                {
                    if (newType == typeof(bool))
                    {
                        if (typeof(string).IsInstanceOfType(source))
                        {
                            string text = (string)source;
                            if (text.Length > 0 && (text[0] == '+' || text[0] == '-' || char.IsDigit(text, 0)))
                            {
                                return System.Convert.ToBoolean(System.Convert.ToInt32(source));
                            }
                        }
                        return System.Convert.ToBoolean(source);
                    }
                    return Convert.ChangeType(source, newType);
                }
            }
        }

        // Token: 0x06000008 RID: 8 RVA: 0x0000251C File Offset: 0x0000151C
        public static string ToString(object source)
        {
            if (source == null)
            {
                return "";
            }
            System.Type type = source.GetType();
            if (type == typeof(DateTime))
            {
                if ((DateTime)source == DateTime.MinValue)
                {
                    return string.Empty;
                }
                DateTime dateTime = (DateTime)source;
                if (dateTime.Millisecond > 0)
                {
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                if (type == typeof(XmlQualifiedName))
                {
                    return ((XmlQualifiedName)source).Name;
                }
                if (type.FullName == "System.RuntimeType")
                {
                    return ((System.Type)source).Name;
                }
                if (type == typeof(byte[]))
                {
                    byte[] array = (byte[])source;
                    StringBuilder stringBuilder = new StringBuilder(array.Length * 3);
                    for (int i = 0; i < array.Length; i++)
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                        stringBuilder.Append(" ");
                    }
                    return stringBuilder.ToString();
                }
                if (type.IsArray)
                {
                    return string.Format("{0}[{1}]", type.GetElementType().Name, ((Array)source).Length);
                }
                if (type == typeof(Array))
                {
                    return string.Format("Object[{0}]", ((Array)source).Length);
                }
                return source.ToString();
            }
        }

        // Token: 0x06000009 RID: 9 RVA: 0x0000267C File Offset: 0x0000167C
        public static bool Match(string target, string pattern, bool caseSensitive)
        {
            if (pattern == null || pattern.Length == 0)
            {
                return true;
            }
            if (target == null || target.Length == 0)
            {
                return false;
            }
            if (caseSensitive)
            {
                if (target == pattern)
                {
                    return true;
                }
            }
            else if (target.ToLower() == pattern.ToLower())
            {
                return true;
            }
            int i = 0;
            int j = 0;
        IL_273:
            while (j < target.Length && i < pattern.Length)
            {
                char c = Convert.ConvertCase(pattern[i++], caseSensitive);
                if (i > pattern.Length)
                {
                    return j >= target.Length;
                }
                char c2 = c;
                char c3;
                if (c2 <= '*')
                {
                    if (c2 != '#')
                    {
                        if (c2 == '*')
                        {
                            while (j < target.Length)
                            {
                                if (Convert.Match(target.Substring(j++), pattern.Substring(i), caseSensitive))
                                {
                                    return true;
                                }
                            }
                            return Convert.Match(target, pattern.Substring(i), caseSensitive);
                        }
                    }
                    else
                    {
                        c3 = target[j++];
                        if (!char.IsDigit(c3))
                        {
                            return false;
                        }
                        continue;
                    }
                }
                else if (c2 != '?')
                {
                    if (c2 == '[')
                    {
                        c3 = ConvertCase(target[j++], caseSensitive);
                        if (j > target.Length)
                        {
                            return false;
                        }
                        char c4 = '\0';
                        if (pattern[i] == '!')
                        {
                            i++;
                            c = ConvertCase(pattern[i++], caseSensitive);
                            while (i < pattern.Length)
                            {
                                if (c == ']')
                                {
                                    break;
                                }
                                if (c == '-')
                                {
                                    c = ConvertCase(pattern[i], caseSensitive);
                                    if (i > pattern.Length || c == ']')
                                    {
                                        return false;
                                    }
                                    if (c3 >= c4 && c3 <= c)
                                    {
                                        return false;
                                    }
                                }
                                c4 = c;
                                if (c3 == c)
                                {
                                    return false;
                                }
                                c = ConvertCase(pattern[i++], caseSensitive);
                            }
                            continue;
                        }
                        c = ConvertCase(pattern[i++], caseSensitive);
                        while (i < pattern.Length)
                        {
                            if (c == ']')
                            {
                                return false;
                            }
                            if (c != '-')
                            {
                                goto IL_1EB;
                            }
                            c = ConvertCase(pattern[i], caseSensitive);
                            if (i > pattern.Length || c == ']')
                            {
                                return false;
                            }
                            if (c3 < c4 || c3 > c)
                            {
                                goto IL_1EB;
                            }
                        IL_21A:
                            while (i < pattern.Length)
                            {
                                if (c == ']')
                                {
                                    break;
                                }
                                c = pattern[i++];
                            }
                            goto IL_273;
                        IL_1EB:
                            c4 = c;
                            if (c3 == c)
                            {
                                break;
                            }
                            c = ConvertCase(pattern[i++], caseSensitive);
                        }
                        //goto IL_21A;
                    }
                }
                else
                {
                    if (j >= target.Length)
                    {
                        return false;
                    }
                    if (i >= pattern.Length && j < target.Length - 1)
                    {
                        return false;
                    }
                    j++;
                    continue;
                }
                c3 = Convert.ConvertCase(target[j++], caseSensitive);
                if (c3 != c)
                {
                    return false;
                }
                if (i >= pattern.Length && j < target.Length - 1)
                {
                    return false;
                }
            }
            return true;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002913 File Offset: 0x00001913
        private static char ConvertCase(char c, bool caseSensitive)
        {
            if (!caseSensitive)
            {
                return char.ToUpper(c);
            }
            return c;
        }
    }
}
