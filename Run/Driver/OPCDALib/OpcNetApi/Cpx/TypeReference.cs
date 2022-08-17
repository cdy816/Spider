using System;
using System.Xml.Serialization;

namespace Opc.Cpx
{
	// Token: 0x02000016 RID: 22
	[XmlType(Namespace = "http://opcfoundation.org/OPCBinary/1.0/")]
	public class TypeReference : FieldType
	{
		// Token: 0x04000032 RID: 50
		[XmlAttribute]
		public string TypeID;
	}
}
