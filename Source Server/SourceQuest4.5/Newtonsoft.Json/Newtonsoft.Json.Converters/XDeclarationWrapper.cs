using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		internal XDeclaration Declaration
		{
			get;
			private set;
		}

		public override XmlNodeType NodeType => XmlNodeType.XmlDeclaration;

		public string Version => Declaration.Version;

		public string Encoding
		{
			get
			{
				return Declaration.Encoding;
			}
			set
			{
				Declaration.Encoding = value;
			}
		}

		public string Standalone
		{
			get
			{
				return Declaration.Standalone;
			}
			set
			{
				Declaration.Standalone = value;
			}
		}

		public XDeclarationWrapper(XDeclaration declaration)
			: base(null)
		{
			Declaration = declaration;
		}
	}
}
