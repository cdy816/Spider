using System;
using System.Xml.Serialization;

namespace Opc.Cpx
{
	// Token: 0x0200001A RID: 26
	[XmlInclude(typeof(Single))]
	[XmlType(Namespace = "http://opcfoundation.org/OPCBinary/1.0/")]
	[XmlInclude(typeof(Double))]
	public class FloatingPoint : FieldType
	{
		// Token: 0x04000037 RID: 55
		[XmlAttribute]
		public string FloatFormat;
	}
}
