using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class XAttributeWrapper : XObjectWrapper
	{
		private XAttribute Attribute => (XAttribute)base.WrappedNode;

		public override string Value
		{
			get
			{
				return Attribute.Value;
			}
			set
			{
				Attribute.Value = value;
			}
		}

		public override string LocalName => Attribute.Name.LocalName;

		public override string NamespaceUri => Attribute.Name.NamespaceName;

		public override IXmlNode ParentNode
		{
			get
			{
				if (Attribute.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(Attribute.Parent);
			}
		}

		public XAttributeWrapper(XAttribute attribute)
			: base(attribute)
		{
		}
	}
}
