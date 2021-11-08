namespace Newtonsoft.Json.Converters
{
	internal interface IXmlElement : IXmlNode
	{
		bool IsEmpty
		{
			get;
		}

		void SetAttributeNode(IXmlNode attribute);

		string GetPrefixOfNamespace(string namespaceUri);
	}
}
