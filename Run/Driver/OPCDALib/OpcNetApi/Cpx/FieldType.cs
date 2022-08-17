using System;
using System.Xml.Serialization;

namespace Opc.Cpx
{
	// Token: 0x02000015 RID: 21
	[XmlInclude(typeof(BitString))]
	[XmlInclude(typeof(UInt16))]
	[XmlInclude(typeof(Int64))]
	[XmlInclude(typeof(Int32))]
	[XmlInclude(typeof(Int16))]
	[XmlInclude(typeof(Int8))]
	[XmlInclude(typeof(TypeReference))]
	[XmlInclude(typeof(CharString))]
	[XmlInclude(typeof(UInt32))]
	[XmlInclude(typeof(UInt8))]
	[XmlType(Namespace = "http://opcfoundation.org/OPCBinary/1.0/")]
	[XmlInclude(typeof(FloatingPoint))]
	[XmlInclude(typeof(Unicode))]
	[XmlInclude(typeof(Ascii))]
	[XmlInclude(typeof(Single))]
	[XmlInclude(typeof(UInt64))]
	[XmlInclude(typeof(Double))]
	[XmlInclude(typeof(Integer))]
	public class FieldType
	{
		// Token: 0x0400002A RID: 42
		[XmlAttribute]
		public string Name;

		// Token: 0x0400002B RID: 43
		[XmlAttribute]
		public string Format;

		// Token: 0x0400002C RID: 44
		[XmlAttribute]
		public int Length;

		// Token: 0x0400002D RID: 45
		[XmlIgnore]
		public bool LengthSpecified;

		// Token: 0x0400002E RID: 46
		[XmlAttribute]
		public int ElementCount;

		// Token: 0x0400002F RID: 47
		[XmlIgnore]
		public bool ElementCountSpecified;

		// Token: 0x04000030 RID: 48
		[XmlAttribute]
		public string ElementCountRef;

		// Token: 0x04000031 RID: 49
		[XmlAttribute]
		public string FieldTerminator;
	}
}
