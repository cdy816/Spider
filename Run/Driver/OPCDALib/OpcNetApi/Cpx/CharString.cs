using System;
using System.Xml.Serialization;

namespace Opc.Cpx
{
	// Token: 0x02000017 RID: 23
	[XmlInclude(typeof(Unicode))]
	[XmlType(Namespace = "http://opcfoundation.org/OPCBinary/1.0/")]
	[XmlInclude(typeof(Ascii))]
	public class CharString : FieldType
	{
		// Token: 0x04000033 RID: 51
		[XmlAttribute]
		public int CharWidth;

		// Token: 0x04000034 RID: 52
		[XmlIgnore]
		public bool CharWidthSpecified;

		// Token: 0x04000035 RID: 53
		[XmlAttribute]
		public string StringEncoding;

		// Token: 0x04000036 RID: 54
		[XmlAttribute]
		public string CharCountRef;
	}
}
