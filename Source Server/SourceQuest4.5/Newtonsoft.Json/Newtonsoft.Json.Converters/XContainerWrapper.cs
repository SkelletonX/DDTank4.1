using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class XContainerWrapper : XObjectWrapper
	{
		private IList<IXmlNode> _childNodes;

		private XContainer Container => (XContainer)base.WrappedNode;

		public override IList<IXmlNode> ChildNodes
		{
			get
			{
				if (_childNodes == null)
				{
					_childNodes = Container.Nodes().Select(WrapNode).ToList();
				}
				return _childNodes;
			}
		}

		public override IXmlNode ParentNode
		{
			get
			{
				if (Container.Parent == null)
				{
					return null;
				}
				return WrapNode(Container.Parent);
			}
		}

		public XContainerWrapper(XContainer container)
			: base(container)
		{
		}

		internal static IXmlNode WrapNode(XObject node)
		{
			if (node is XDocument)
			{
				return new XDocumentWrapper((XDocument)node);
			}
			if (node is XElement)
			{
				return new XElementWrapper((XElement)node);
			}
			if (node is XContainer)
			{
				return new XContainerWrapper((XContainer)node);
			}
			if (node is XProcessingInstruction)
			{
				return new XProcessingInstructionWrapper((XProcessingInstruction)node);
			}
			if (node is XText)
			{
				return new XTextWrapper((XText)node);
			}
			if (node is XComment)
			{
				return new XCommentWrapper((XComment)node);
			}
			if (node is XAttribute)
			{
				return new XAttributeWrapper((XAttribute)node);
			}
			if (node is XDocumentType)
			{
				return new XDocumentTypeWrapper((XDocumentType)node);
			}
			return new XObjectWrapper(node);
		}

		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			Container.Add(newChild.WrappedNode);
			_childNodes = null;
			return newChild;
		}
	}
}
