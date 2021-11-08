using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class XCommentWrapper : XObjectWrapper
	{
		private XComment Text => (XComment)base.WrappedNode;

		public override string Value
		{
			get
			{
				return Text.Value;
			}
			set
			{
				Text.Value = value;
			}
		}

		public override IXmlNode ParentNode
		{
			get
			{
				if (Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(Text.Parent);
			}
		}

		public XCommentWrapper(XComment text)
			: base(text)
		{
		}
	}
}
