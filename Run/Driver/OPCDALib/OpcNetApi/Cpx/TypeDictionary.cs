using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Opc.Cpx
{
	// Token: 0x02000013 RID: 19
	[XmlType(Namespace = "http://opcfoundation.org/OPCBinary/1.0/")]
	[XmlRoot(Namespace = "http://opcfoundation.org/OPCBinary/1.0/", IsNullable = false)]
	public class TypeDictionary
	{
		// Token: 0x0400001C RID: 28
		[XmlElement("TypeDescription")]
		public TypeDescription[] TypeDescription;

		// Token: 0x0400001D RID: 29
		[XmlAttribute]
		public string DictionaryName;

		// Token: 0x0400001E RID: 30
		[XmlAttribute]
		[DefaultValue(true)]
		public bool DefaultBigEndian = true;

		// Token: 0x0400001F RID: 31
		[XmlAttribute]
		[DefaultValue("UCS-2")]
		public string DefaultStringEncoding = "UCS-2";

		// Token: 0x04000020 RID: 32
		[XmlAttribute]
		[DefaultValue(2)]
		public int DefaultCharWidth = 2;

		// Token: 0x04000021 RID: 33
		[XmlAttribute]
		[DefaultValue("IEEE-754")]
		public string DefaultFloatFormat = "IEEE-754";
	}
}
