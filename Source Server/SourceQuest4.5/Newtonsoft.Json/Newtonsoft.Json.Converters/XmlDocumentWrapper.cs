using System.Xml;

namespace Newtonsoft.Json.Converters
{
	internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument, IXmlNode
	{
		private readonly XmlDocument _document;

		public IXmlElement DocumentElement
		{
			get
			{
				if (_document.DocumentElement == null)
				{
					return null;
				}
				return new XmlElementWrapper(_document.DocumentElement);
			}
		}

		public XmlDocumentWrapper(XmlDocument document)
			: base(document)
		{
			_document = document;
		}

		public IXmlNode CreateComment(string data)
		{
			return new XmlNodeWrapper(_document.CreateComment(data));
		}

		public IXmlNode CreateTextNode(string text)
		{
			return new XmlNodeWrapper(_document.CreateTextNode(text));
		}

		public IXmlNode CreateCDataSection(string data)
		{
			return new XmlNodeWrapper(_document.CreateCDataSection(data));
		}

		public IXmlNode CreateWhitespace(string text)
		{
			return new XmlNodeWrapper(_document.CreateWhitespace(text));
		}

		public IXmlNode CreateSignificantWhitespace(string text)
		{
			return new XmlNodeWrapper(_document.CreateSignificantWhitespace(text));
		}

		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XmlDeclarationWrapper(_document.CreateXmlDeclaration(version, encoding, standalone));
		}

		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XmlDocumentTypeWrapper(_document.CreateDocumentType(name, publicId, systemId, null));
		}

		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XmlNodeWrapper(_document.CreateProcessingInstruction(target, data));
		}

		public IXmlElement CreateElement(string elementName)
		{
			return new XmlElementWrapper(_document.CreateElement(elementName));
		}

		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XmlElementWrapper(_document.CreateElement(qualifiedName, namespaceUri));
		}

		public IXmlNode CreateAttribute(string name, string value)
		{
			XmlNodeWrapper xmlNodeWrapper = new XmlNodeWrapper(_document.CreateAttribute(name));
			xmlNodeWrapper.Value = value;
			return xmlNodeWrapper;
		}

		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			XmlNodeWrapper xmlNodeWrapper = new XmlNodeWrapper(_document.CreateAttribute(qualifiedName, namespaceUri));
			xmlNodeWrapper.Value = value;
			return xmlNodeWrapper;
		}
	}
}
