using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
	{
		private XElement Element => (XElement)base.WrappedNode;

		public override IList<IXmlNode> Attributes => (from a in Element.Attributes()
			select new XAttributeWrapper(a)).Cast<IXmlNode>().ToList();

		public override string Value
		{
			get
			{
				return Element.Value;
			}
			set
			{
				Element.Value = value;
			}
		}

		public override string LocalName => Element.Name.LocalName;

		public override string NamespaceUri => Element.Name.NamespaceName;

		public bool IsEmpty => Element.IsEmpty;

		public XElementWrapper(XElement element)
			: base(element)
		{
		}

		public void SetAttributeNode(IXmlNode attribute)
		{
			XObjectWrapper xObjectWrapper = (XObjectWrapper)attribute;
			Element.Add(xObjectWrapper.WrappedNode);
		}

		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return Element.GetPrefixOfNamespace(namespaceUri);
		}
	}
}
