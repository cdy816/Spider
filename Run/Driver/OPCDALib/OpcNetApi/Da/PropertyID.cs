using System;
using System.Runtime.Serialization;
using System.Xml;

namespace Opc.Da
{
	// Token: 0x0200008C RID: 140
	[Serializable]
	public struct PropertyID : ISerializable
	{
		// Token: 0x06000401 RID: 1025 RVA: 0x0000BE44 File Offset: 0x0000AE44
		private PropertyID(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			string name = "";
			string ns = "";
			enumerator.Reset();
			while (enumerator.MoveNext())
			{
				SerializationEntry serializationEntry = enumerator.Current;
				if (serializationEntry.Name.Equals("NA"))
				{
					SerializationEntry serializationEntry2 = enumerator.Current;
					name = (string)serializationEntry2.Value;
				}
				else
				{
					SerializationEntry serializationEntry3 = enumerator.Current;
					if (serializationEntry3.Name.Equals("NS"))
					{
						SerializationEntry serializationEntry4 = enumerator.Current;
						ns = (string)serializationEntry4.Value;
					}
				}
			}
			this.m_name = new XmlQualifiedName(name, ns);
			this.m_code = (int)info.GetValue("CO", typeof(int));
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000BF04 File Offset: 0x0000AF04
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (this.m_name != null)
			{
				info.AddValue("NA", this.m_name.Name);
				info.AddValue("NS", this.m_name.Namespace);
			}
			info.AddValue("CO", this.m_code);
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000BF5C File Offset: 0x0000AF5C
		public XmlQualifiedName Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000BF64 File Offset: 0x0000AF64
		public int Code
		{
			get
			{
				return this.m_code;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000BF6C File Offset: 0x0000AF6C
		public static bool operator ==(PropertyID a, PropertyID b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000BF81 File Offset: 0x0000AF81
		public static bool operator !=(PropertyID a, PropertyID b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000BF99 File Offset: 0x0000AF99
		public PropertyID(XmlQualifiedName name)
		{
			this.m_name = name;
			this.m_code = 0;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000BFA9 File Offset: 0x0000AFA9
		public PropertyID(int code)
		{
			this.m_name = null;
			this.m_code = code;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000BFB9 File Offset: 0x0000AFB9
		public PropertyID(string name, int code, string ns)
		{
			this.m_name = new XmlQualifiedName(name, ns);
			this.m_code = code;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000BFD0 File Offset: 0x0000AFD0
		public override bool Equals(object target)
		{
			if (target != null && target.GetType() == typeof(PropertyID))
			{
				PropertyID propertyID = (PropertyID)target;
				if (propertyID.Code != 0 && this.Code != 0)
				{
					return propertyID.Code == this.Code;
				}
				if (propertyID.Name != null && this.Name != null)
				{
					return propertyID.Name == this.Name;
				}
			}
			return false;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000C04C File Offset: 0x0000B04C
		public override int GetHashCode()
		{
			if (this.Code != 0)
			{
				return this.Code.GetHashCode();
			}
			if (this.Name != null)
			{
				return this.Name.GetHashCode();
			}
			return base.GetHashCode();
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000C09C File Offset: 0x0000B09C
		public override string ToString()
		{
			if (this.Name != null && this.Code != 0)
			{
				return string.Format("{0} ({1})", this.Name.Name, this.Code);
			}
			if (this.Name != null)
			{
				return this.Name.Name;
			}
			if (this.Code != 0)
			{
				return string.Format("{0}", this.Code);
			}
			return "";
		}

		// Token: 0x040001CD RID: 461
		private int m_code;

		// Token: 0x040001CE RID: 462
		private XmlQualifiedName m_name;

		// Token: 0x0200008D RID: 141
		private class Names
		{
			// Token: 0x040001CF RID: 463
			internal const string NAME = "NA";

			// Token: 0x040001D0 RID: 464
			internal const string NAMESPACE = "NS";

			// Token: 0x040001D1 RID: 465
			internal const string CODE = "CO";
		}
	}
}
