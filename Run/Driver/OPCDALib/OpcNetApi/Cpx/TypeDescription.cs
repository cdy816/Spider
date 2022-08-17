using System;
using System.Xml.Serialization;

namespace Opc.Cpx
{
	// Token: 0x02000014 RID: 20
	[XmlType(Namespace = "http://opcfoundation.org/OPCBinary/1.0/")]
	public class TypeDescription
	{
		// Token: 0x04000022 RID: 34
		[XmlElement("Field")]
		public FieldType[] Field;

		// Token: 0x04000023 RID: 35
		[XmlAttribute]
		public string TypeID;

		// Token: 0x04000024 RID: 36
		[XmlAttribute]
		public bool DefaultBigEndian;

		// Token: 0x04000025 RID: 37
		[XmlIgnore]
		public bool DefaultBigEndianSpecified;

		// Token: 0x04000026 RID: 38
		[XmlAttribute]
		public string DefaultStringEncoding;

		// Token: 0x04000027 RID: 39
		[XmlAttribute]
		public int DefaultCharWidth;

		// Token: 0x04000028 RID: 40
		[XmlIgnore]
		public bool DefaultCharWidthSpecified;

		// Token: 0x04000029 RID: 41
		[XmlAttribute]
		public string DefaultFloatFormat;
	}
}
